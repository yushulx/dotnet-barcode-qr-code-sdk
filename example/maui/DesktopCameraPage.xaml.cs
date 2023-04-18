using SkiaSharp;
using SkiaSharp.Views.Maui;
using OpenCvSharp;
using System.Runtime.InteropServices;
using Dynamsoft;
using System.Collections.Concurrent;

namespace BarcodeQrScanner;

public partial class DesktopCameraPage : ContentPage
{
    private BarcodeQRCodeReader reader;
    private Thread thread;
    private volatile bool isCapturing;
    private VideoCapture capture;

    private ConcurrentQueue<BarcodeQRCodeReader.Result[]> _queue = new ConcurrentQueue<BarcodeQRCodeReader.Result[]>();
    private ConcurrentQueue<SKBitmap> _bitmapQueue = new ConcurrentQueue<SKBitmap>();
    private SKBitmap _bitmap;

    private static object lockObject = new object();

    public DesktopCameraPage()
	{
		InitializeComponent();
        this.Disappearing += OnDisappearing;
        this.Appearing += OnAppearing;
        
        reader = BarcodeQRCodeReader.Create();
    }

    private void OnAppearing(object sender, EventArgs e)
    {
        Create();
    }
    
    private void OnDisappearing(object sender, EventArgs e)
    {
        Destroy();
    }

    private void Decode()
    {
        Mat mat = new Mat();
        capture.Read(mat);

        int length = mat.Cols * mat.Rows * mat.ElemSize();
        if (length == 0) return;
        byte[] bytes = new byte[length];
        Marshal.Copy(mat.Data, bytes, 0, length);

        BarcodeQRCodeReader.Result[] results = reader.DecodeBuffer(bytes, mat.Cols, mat.Rows, (int)mat.Step(), BarcodeQRCodeReader.ImagePixelFormat.IPF_RGB_888);
        if (results != null)
        {
            foreach (BarcodeQRCodeReader.Result result in results)
            {
                int[] points = result.Points;
                if (points != null)
                {
                    OpenCvSharp.Point[] all = new OpenCvSharp.Point[4];
                    int xMin = points[0], yMax = points[1];
                    all[0] = new OpenCvSharp.Point(xMin, yMax);
                    for (int i = 2; i < 7; i += 2)
                    {
                        int x = points[i];
                        int y = points[i + 1];
                        OpenCvSharp.Point p = new OpenCvSharp.Point(x, y);
                        xMin = x < xMin ? x : xMin;
                        yMax = y > yMax ? y : yMax;
                        all[i / 2] = p;
                    }
                    OpenCvSharp.Point[][] contours = new OpenCvSharp.Point[][] { all };
                    Cv2.DrawContours(mat, contours, 0, new Scalar(0, 0, 255), 2);
                    if (result.Text != null) Cv2.PutText(mat, result.Text, new OpenCvSharp.Point(xMin, yMax), HersheyFonts.HersheySimplex, 1, new Scalar(0, 0, 255), 2);
                }
            }
        }

        Mat newFrame = new Mat();
        Cv2.CvtColor(mat, newFrame, ColorConversionCodes.BGR2RGBA);
        SKBitmap bitmap = new SKBitmap(mat.Cols, mat.Rows, SKColorType.Rgba8888, SKAlphaType.Premul);
        bitmap.SetPixels(newFrame.Data);
        if (_bitmapQueue.Count == 2) ClearQueue();
        _bitmapQueue.Enqueue(bitmap);
        canvasView.InvalidateSurface();
    }

    private void FrameCallback()
    {
        while (isCapturing)
        {
            Decode();
        }
    }

    private void ClearQueue()
    {
        while (_bitmapQueue.Count > 0) {
            SKBitmap bitmap;
            _bitmapQueue.TryDequeue(out bitmap);
            bitmap.Dispose();
        }
    }

    private void Create()
    {
        lock (lockObject)
        {
            capture = new VideoCapture(0);

            if (capture.IsOpened())
            {
                isCapturing = true;
                thread = new Thread(new ThreadStart(FrameCallback));
                thread.Start();
            }
        }
    }
    private void Destroy()
    {
        lock (lockObject)
        {
            if (thread != null)
            {
                isCapturing = false;

                thread.Join();
                thread = null;
            }

            if (capture != null && capture.IsOpened())
            {
                capture.Release();
                capture = null;
            }

            ClearQueue();

            if (_bitmap != null)
            {
                _bitmap.Dispose();
                _bitmap = null;
            }
        }
    }
    
    ~DesktopCameraPage()
    {
        Destroy();
    }

    void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {
        lock (lockObject)
        {
            if (!isCapturing) return;

            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            _bitmapQueue.TryDequeue(out _bitmap);

            if (_bitmap != null)
            {
                try
                {
                    canvas.DrawBitmap(_bitmap, new SKPoint(0, 0));
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    _bitmap.Dispose();
                    _bitmap = null;
                }
            }
        }
           
    }
}