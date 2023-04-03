using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using com.Dynamsoft.Dbr;

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

        //public class DBRLicenseVerificationListener : Java.Lang.Object, IDBRLicenseVerificationListener
        //{
        //    public void DBRLicenseVerificationCallback(bool isSuccess, Java.Lang.Exception error)
        //    {
        //        if (!isSuccess)
        //        {
        //            System.Console.WriteLine(error.Message);
        //        }
        //    }
        //}

        public static void InitLicense(string license)
        {
            //BarcodeReader.InitLicense(license, new DBRLicenseVerificationListener());
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

        //private Result[]? OutputResults(TextResult[]? results)
        //{
        //    Result[]? output = null;
        //    if (results != null && results.Length > 0)
        //    {
        //        output = new Result[results.Length];
        //        for (int i = 0; i < results.Length; ++i)
        //        {
        //            TextResult tmp = results[i];
        //            Result r = new Result();
        //            output[i] = r;
        //            r.Text = tmp.BarcodeText;
        //            r.Format1 = tmp.BarcodeFormatString;
        //            r.Format2 = tmp.BarcodeFormatString;
        //            if (tmp.LocalizationResult != null && tmp.LocalizationResult.ResultPoints != null)
        //            {
        //                IList<Point> points = tmp.LocalizationResult.ResultPoints;
        //                r.Points = new int[8] { points[0].X, points[0].Y, points[1].X, points[1].Y, points[2].X, points[2].Y, points[3].X, points[3].Y };
        //            }
        //            else
        //                r.Points = null;
        //        }
        //    }

        //    return output;
        //}

        //public Result[]? DecodeFile(string filename)
        //{
        //    if (reader == null) { return null; }

        //    TextResult[]? results = reader.DecodeFile(filename);
        //    return OutputResults(results);
        //}

        //public Result[]? DecodeBuffer(byte[] buffer, int width, int height, int stride, ImagePixelFormat format)
        //{
        //    if (reader == null) { return null; }

        //    TextResult[]? results = reader.DecodeBuffer(buffer, width, height, stride, (int)format);

        //    return OutputResults(results);
        //}

        //public Result[]? DecodeBase64(string base64string)
        //{
        //    if (reader == null) { return null; }
        //    TextResult[]? results = reader.DecodeBase64String(base64string);

        //    return OutputResults(results);
        //}

        //public void SetParameters(string parameters)
        //{
        //    if (reader == null) { return; }

        //    reader.InitRuntimeSettingsWithString(parameters, EnumConflictMode.CmOverwrite);
        //}
    }
}
