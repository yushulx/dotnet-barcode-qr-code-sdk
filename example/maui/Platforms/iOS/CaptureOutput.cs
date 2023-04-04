using AVFoundation;
using BarcodeQrScanner.Services;
using CoreFoundation;
using CoreMedia;
using CoreVideo;
using Foundation;
using SkiaSharp;
using Dynamsoft;
using static CoreFoundation.DispatchSource;

namespace BarcodeQrScanner.Platforms.iOS
{
    class CaptureOutput : AVCaptureVideoDataOutputSampleBufferDelegate
    {
        public BarcodeQRCodeReader reader;
        public Action update;
        private bool ready = true;
        private DispatchQueue queue = new DispatchQueue("ReadTask", true);
        private NSError errorr;
        private nint bpr;
        public nint width;
        public nint height;
        private NSData buffer;
        public string result = "";
        private BarcodeQRCodeReader.Result[] results;
        CameraPreview cameraPreview;
        public BarcodeQrData[] output = null;

        public CaptureOutput(CameraPreview preview)
        {
            cameraPreview = preview;
        }

        public override void DidOutputSampleBuffer(AVCaptureOutput captureOutput, CMSampleBuffer sampleBuffer, AVCaptureConnection connection)
        {
            if (ready)
            {
                ready = false;
                CVPixelBuffer cVPixelBuffer = (CVPixelBuffer)sampleBuffer.GetImageBuffer();

                cVPixelBuffer.Lock(CVPixelBufferLock.ReadOnly);
                nint dataSize = cVPixelBuffer.DataSize;
                width = cVPixelBuffer.Width;
                height = cVPixelBuffer.Height;
                IntPtr baseAddress = cVPixelBuffer.BaseAddress;
                bpr = cVPixelBuffer.BytesPerRow;
                cVPixelBuffer.Unlock(CVPixelBufferLock.ReadOnly);
                buffer = NSData.FromBytes(baseAddress, (nuint)dataSize);
                cVPixelBuffer.Dispose();
                queue.DispatchAsync(ReadTask);
            }
            sampleBuffer.Dispose();
        }

        private void ReadTask()
        {
            output = null;
            if (reader != null)
            {
                byte[] bytearray = new byte[buffer.Length];
                System.Runtime.InteropServices.Marshal.Copy(buffer.Bytes, bytearray, 0, Convert.ToInt32(buffer.Length));
                results = reader.DecodeBuffer(bytearray,
                                            (int)width,
                                            (int)height,
                                            (int)bpr,
                                            BarcodeQRCodeReader.ImagePixelFormat.IPF_ARGB_8888);

                if (results != null && results.Length > 0)
                {
                    output = BarcodeQrData.Convert(results);
                }
                else
                {
                    result = "";
                }
            }

            DispatchQueue.MainQueue.DispatchAsync(update);
            ready = true;
        }
    }
}
