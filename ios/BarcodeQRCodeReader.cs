using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using com.Dynamsoft.Dbr;
using System.Runtime.InteropServices;
using Foundation;

namespace Dynamsoft
{
    public class BarcodeQRCodeReader
    {
       
        private DynamsoftBarcodeReader? reader;

        public class Result
        {
            public string? Text { get; set; }
            public int[]? Points { get; set; }
            public string? Format1 { get; set; }
            public string? Format2 { get; set; }
        }

        public enum ImagePixelFormat
        {
            /**0:Black, 1:White */
            IPF_BINARY,

            /**0:White, 1:Black */
            IPF_BINARYINVERTED,

            /**8bit gray */
            IPF_GRAYSCALED,

            /**NV21 */
            IPF_NV21,

            /**16bit with RGB channel order stored in memory from high to low address*/
            IPF_RGB_565,

            /**16bit with RGB channel order stored in memory from high to low address*/
            IPF_RGB_555,

            /**24bit with RGB channel order stored in memory from high to low address*/
            IPF_RGB_888,

            /**32bit with ARGB channel order stored in memory from high to low address*/
            IPF_ARGB_8888,

            /**48bit with RGB channel order stored in memory from high to low address*/
            IPF_RGB_161616,

            /**64bit with ARGB channel order stored in memory from high to low address*/
            IPF_ARGB_16161616,

            /**32bit with ABGR channel order stored in memory from high to low address*/
            IPF_ABGR_8888,

            /**64bit with ABGR channel order stored in memory from high to low address*/
            IPF_ABGR_16161616,

            /**24bit with BGR channel order stored in memory from high to low address*/
            IPF_BGR_888
        }

        public class Listener : DBRLicenseVerificationListener
        {
            public void DBRLicenseVerificationCallback(bool isSuccess, NSError error)
            {
                if (error != null)
                {
                    System.Console.WriteLine(error.UserInfo);
                }
            }
        }

        public static void InitLicense(string license)
        {
            DynamsoftBarcodeReader.InitLicense(license, new Listener());
        }

        private BarcodeQRCodeReader()
        {
            reader = new DynamsoftBarcodeReader();
        }

        public static BarcodeQRCodeReader Create()
        {
            return new BarcodeQRCodeReader();
        }

        ~BarcodeQRCodeReader()
        {
            Destroy();
        }

        public void Destroy()
        {
            reader = null;
        }

        public static string? GetVersionInfo()
        {
            return DynamsoftBarcodeReader.Version;
        }

        private Result[]? OutputResults(iTextResult[]? results)
        {
            Result[]? output = null;
            if (results != null && results.Length > 0)
            {
                output = new Result[results.Length];
                for (int i = 0; i < results.Length; ++i)
                {
                    iTextResult tmp = results[i];
                    Result r = new Result();
                    output[i] = r;
                    r.Text = tmp.BarcodeText;
                    r.Format1 = tmp.BarcodeFormatString;
                    r.Format2 = tmp.BarcodeFormatString;
                    if (tmp.LocalizationResult != null && tmp.LocalizationResult.ResultPoints != null)
                    {
                        NSObject[] points = tmp.LocalizationResult.ResultPoints;
                        r.Points = new int[8] { (int)((NSValue)points[0]).CGPointValue.X, (int)((NSValue)points[0]).CGPointValue.Y, (int)((NSValue)points[1]).CGPointValue.X, (int)((NSValue)points[1]).CGPointValue.Y, (int)((NSValue)points[2]).CGPointValue.X, (int)((NSValue)points[2]).CGPointValue.Y, (int)((NSValue)points[3]).CGPointValue.X, (int)((NSValue)points[3]).CGPointValue.Y };
                    }
                    else
                        r.Points = null;
                }
            }

            return output;
        }

        public Result[]? DecodeFile(string filename)
        {
            if (reader == null) { return null; }

            NSError error;
            iTextResult[]? results = reader.DecodeFileWithName(filename, out error);
            return OutputResults(results);
        }

        public Result[]? DecodeBuffer(byte[] myBytes, int width, int height, int stride, ImagePixelFormat format)
        {
            if (reader == null) { return null; }

            NSError error;
            IntPtr buffer = Marshal.AllocHGlobal(myBytes.Length);
            Marshal.Copy(myBytes, 0, buffer, myBytes.Length);
            NSData data = NSData.FromBytes(buffer, (nuint)myBytes.Length);
            
            iTextResult[]? results = reader.DecodeBuffer(data, width, height, stride, (EnumImagePixelFormat)format, out error);
            Marshal.FreeHGlobal(buffer);

            return OutputResults(results);
        }

        public Result[]? DecodeBase64(string base64string)
        {
            if (reader == null) { return null; }

            NSError error;
            iTextResult[]? results = reader.DecodeBase64(base64string, out error);

            return OutputResults(results);
        }

        public void SetParameters(string parameters)
        {
            if (reader == null) { return; }

            NSError error;
            reader.InitRuntimeSettingsWithString(parameters, EnumConflictMode.Overwrite, out error);
        }
    }
}
