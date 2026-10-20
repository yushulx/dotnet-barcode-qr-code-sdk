using Dynamsoft.Core;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;

namespace BarcodeSmokeTests
{
    /// <summary>
    /// Builds barcode images in memory with ZXing.NET, so the tests do not depend on
    /// checked-in fixtures and behave the same on Windows and Linux.
    /// </summary>
    internal static class SyntheticBarcodes
    {
        /// <summary>
        /// Renders <paramref name="content"/> as a QR code and returns it as the
        /// 24-bit RGB pixel buffer that Dynamsoft's <c>ImageData</c> expects.
        /// </summary>
        internal static ImageData CreateQrCode(string content, int size = 300)
        {
            PixelData pixels = Render(content, size);
            return ToImageData(pixels);
        }

        /// <summary>
        /// Renders two QR codes side by side in a single image, so multi-barcode
        /// detection can be exercised as well.
        /// </summary>
        internal static ImageData CreateQrCodePair(string first, string second, int size = 300, int gap = 40)
        {
            PixelData left = Render(first, size);
            PixelData right = Render(second, size);

            int width = size * 2 + gap;
            int height = size;
            int stride = width * 3;
            var rgb = new byte[stride * height];

            // White background keeps the quiet zones clean.
            Array.Fill(rgb, (byte)0xFF);

            Blit(left, rgb, stride, 0);
            Blit(right, rgb, stride, size + gap);

            return new ImageData(rgb, width, height, stride, EnumImagePixelFormat.IPF_RGB_888);
        }

        private static PixelData Render(string content, int size)
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Width = size,
                    Height = size,
                    Margin = 4,
                    PureBarcode = false,
                },
            };

            return writer.Write(content);
        }

        private static void Blit(PixelData source, byte[] target, int targetStride, int offsetX)
        {
            for (int y = 0; y < source.Height; y++)
            {
                int src = y * source.Width * 4;
                int dst = y * targetStride + offsetX * 3;
                for (int x = 0; x < source.Width; x++)
                {
                    target[dst + x * 3 + 0] = source.Pixels[src + x * 4 + 2]; // R
                    target[dst + x * 3 + 1] = source.Pixels[src + x * 4 + 1]; // G
                    target[dst + x * 3 + 2] = source.Pixels[src + x * 4 + 0]; // B
                }
            }
        }

        private static ImageData ToImageData(PixelData pixels)
        {
            int width = pixels.Width;
            int height = pixels.Height;
            int stride = width * 3;
            var rgb = new byte[stride * height];

            for (int y = 0; y < height; y++)
            {
                int src = y * width * 4;
                int dst = y * stride;
                for (int x = 0; x < width; x++)
                {
                    rgb[dst + x * 3 + 0] = pixels.Pixels[src + x * 4 + 2]; // R
                    rgb[dst + x * 3 + 1] = pixels.Pixels[src + x * 4 + 1]; // G
                    rgb[dst + x * 3 + 2] = pixels.Pixels[src + x * 4 + 0]; // B
                }
            }

            return new ImageData(rgb, width, height, stride, EnumImagePixelFormat.IPF_RGB_888);
        }
    }
}
