using System;
using System.Runtime.InteropServices;
using Dynamsoft;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check supported platforms
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine("Platform: Windows");
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Console.WriteLine("Platform: Linux");
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Console.WriteLine("Platform: macOS");
            }
            
            BarcodeQRCodeReader.InitLicense("LICENSE-KEY"); // Get a license key from https://www.dynamsoft.com/customer/license/trialLicense?product=dbr
            BarcodeQRCodeReader? reader = null;
            try {
                reader = BarcodeQRCodeReader.Create();
                Console.WriteLine("GetVersionInfo(): " + BarcodeQRCodeReader.GetVersionInfo());

                // Refer to https://www.dynamsoft.com/barcode-reader/parameters/structure-and-interfaces-of-parameters.html?ver=latest
                reader.SetParameters("{\"Version\":\"3.0\", \"ImageParameter\":{\"Name\":\"IP1\", \"BarcodeFormatIds\":[\"BF_QR_CODE\", \"BF_ONED\"], \"ExpectedBarcodesCount\":20}}");

                Console.WriteLine("Please enter an image file: ");
                string? filename = Console.ReadLine();
                if (filename != null) {
                    string[]? results = reader.DecodeFile(filename);
                    if (results != null) {
                        foreach (string result in results) {
                            Console.WriteLine(result);
                        }
                    }
                    else {
                        Console.WriteLine("No barcode found.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Destroy();
                }
            }
        }
    }
}
