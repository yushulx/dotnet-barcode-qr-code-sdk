using System.Drawing.Imaging;
using Dynamsoft;
using Result = Dynamsoft.BarcodeQRCodeReader.Result;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace Test
{
    public partial class Form1 : Form
    {
        private BarcodeQRCodeReader reader;
        private VideoCapture capture;
        private bool isCapturing;
        private Thread? thread;

        public Form1()
        {
            InitializeComponent();
            FormClosing += new FormClosingEventHandler(Form1_Closing);
            BarcodeQRCodeReader.InitLicense("DLS2eyJoYW5kc2hha2VDb2RlIjoiMjAwMDAxLTE2NDk4Mjk3OTI2MzUiLCJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSIsInNlc3Npb25QYXNzd29yZCI6IndTcGR6Vm05WDJrcEQ5YUoifQ=="); // Get a license key from https://www.dynamsoft.com/customer/license/trialLicense?product=dbr
            reader = BarcodeQRCodeReader.Create();
            capture = new VideoCapture(0);
            isCapturing = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            // Code
            reader.Destroy();
        }

        private void ShowResults(Result[] results)
        {
            if (results == null)
                return;
        }
        private Bitmap DecodeMat(Mat mat)
        {
            Result[]? results = reader.DecodeBuffer(mat.Data, mat.Cols, mat.Rows, (int)mat.Step(), BarcodeQRCodeReader.ImagePixelFormat.IPF_RGB_888);
            if (results != null)
            {
                foreach (Result result in results)
                {
                    string output = "Text: " + result.Text + Environment.NewLine + "Format: " + result.Format1 + Environment.NewLine;
                    textBox1.AppendText(output);
                    textBox1.AppendText(Environment.NewLine);
                    int[]? points = result.Points;
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
            else
            {
                textBox1.AppendText("No barcode detected!" + Environment.NewLine);
            }

            Bitmap bitmap = BitmapConverter.ToBitmap(mat);
            return bitmap;
        }

        private void DecodeBitmap(Bitmap bitmap)
        {
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        ImageLockMode.ReadWrite, bitmap.PixelFormat);

            BarcodeQRCodeReader.ImagePixelFormat format = BarcodeQRCodeReader.ImagePixelFormat.IPF_ARGB_8888;

            switch (bitmap.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    format = BarcodeQRCodeReader.ImagePixelFormat.IPF_RGB_888;
                    break;
                case PixelFormat.Format32bppArgb:
                    format = BarcodeQRCodeReader.ImagePixelFormat.IPF_ARGB_8888;
                    break;
                case PixelFormat.Format16bppRgb565:
                    format = BarcodeQRCodeReader.ImagePixelFormat.IPF_RGB_565;
                    break;
                case PixelFormat.Format16bppRgb555:
                    format = BarcodeQRCodeReader.ImagePixelFormat.IPF_RGB_555;
                    break;
                case PixelFormat.Format8bppIndexed:
                    format = BarcodeQRCodeReader.ImagePixelFormat.IPF_GRAYSCALED;
                    break;
            }

            Result[]? results = reader.DecodeBuffer(bmpData.Scan0, bitmap.Width, bitmap.Height, bmpData.Stride, format);
            //Unlock the pixels
            bitmap.UnlockBits(bmpData);

            if (results != null)
            {
                foreach (Result result in results)
                {
                    string output = "Text: " + result.Text + Environment.NewLine + "Format: " + result.Format1 + Environment.NewLine;
                    textBox1.AppendText(output);
                }
            }
            else
            {
                textBox1.AppendText("No barcode detected!" + Environment.NewLine);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            StopScan();
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image files (*.bmp, *.jpg, *.png) | *.bmp; *.jpg; *.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Clear();
                    try
                    {
                        Mat mat = Cv2.ImRead(dlg.FileName, ImreadModes.Color);
                        pictureBox1.Image = DecodeMat(mat);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (!capture.IsOpened())
            {
                MessageBox.Show("Failed to open video stream or file");
                return;
            }

            if (button2.Text == "Camera Scan")
            {
                StartScan();
            }
            else {
                StopScan();
            }
        }

        private void StartScan() {
            button2.Text = "Stop";
            isCapturing = true;
            // Task.Run(() => {
            //     while (isCapturing)
            //     {
            //         Mat frame = new Mat();
            //         capture.Read(frame);

            //         // Update UI
            //         this.BeginInvoke((Action)(() => {
            //             pictureBox1.Image = DecodeMat(frame);}));
            //     }
            // });

            thread = new Thread(new ThreadStart(FrameCallback));
            thread.Start();
        }

        private void StopScan() {
            button2.Text = "Camera Scan";
            isCapturing = false;
            if (thread != null) thread.Join();
        }

        private void FrameCallback() {
            while (isCapturing) {
                Mat frame = new Mat();
                capture.Read(frame);
                // pictureBox1.Image = BitmapConverter.ToBitmap(frame);
                pictureBox1.Image = DecodeMat(frame);
            }
        }

        private void Form1_Closing(object? sender, FormClosingEventArgs e)
        {
            StopScan();
        }
    }
}
