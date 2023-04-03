using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndroidX.AppCompat.App;
using Com.Dynamsoft.Dbr;
using Java.Interop;

namespace BarcodeQRCode.Platforms.Android
{
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

}
