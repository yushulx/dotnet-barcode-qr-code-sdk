using SkiaSharp;
using Dynamsoft;

namespace BarcodeQrScanner.Services
{
    public class BarcodeQrData
    {
        public string text;
        public string format;
        public SKPoint[] points;

        public static BarcodeQrData[] Convert(BarcodeQRCodeReader.Result[] results)
        {
            BarcodeQrData[] output = null;
            if (results != null && results.Length > 0)
            {
                output = new BarcodeQrData[results.Length];
                for (int index = 0; index < results.Length; ++index)
                {
                    BarcodeQRCodeReader.Result result = results[index];
                    BarcodeQrData data = new BarcodeQrData
                    {
                        text = result.Text,
                        format = result.Format1
                    };
                    int[] coordinates = result.Points;
                    if (coordinates != null && coordinates.Length == 8)
                    {
                        data.points = new SKPoint[4];

                        for (int i = 0; i < 4; ++i)
                        {
                            SKPoint p = new SKPoint();
                            p.X = coordinates[i * 2];
                            p.Y = coordinates[i * 2 + 1];
                            data.points[i] = p;
                        }
                    }


                    output[index] = data;
                }
            }
            return output;
        }
    }
}
