using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Com.Dynamsoft.Dbr;

namespace Dynamsoft
{
    public class BarcodeQRCodeReader
    {
        private BarcodeReader reader;

        public class Result
        {
            public string? Text { get; set; }
            public int[]? Points { get; set; }
            public string? Format1 { get; set; }
            public string? Format2 { get; set; }
        }

        public class DBRLicenseVerificationListener : Java.Lang.Object, IDBRLicenseVerificationListener
        {
            public void DBRLicenseVerificationCallback(bool isSuccess, Java.Lang.Exception error)
            {
                if (!isSuccess)
                {
                    System.Console.WriteLine(error.Message);
                }
            }
        }

        public static void InitLicense(string license)
        {
            BarcodeReader.InitLicense(license, new DBRLicenseVerificationListener());
        }

        private BarcodeQRCodeReader()
        {
            reader = new BarcodeReader();
        }

        public static BarcodeQRCodeReader Create()
        {
            return new BarcodeQRCodeReader();
        }

        ~BarcodeQRCodeReader()
        {
            
        }

        public void Destroy()
        {
            
        }

        public static string? GetVersionInfo()
        {
            return BarcodeReader.Version;
        }
    }
}
