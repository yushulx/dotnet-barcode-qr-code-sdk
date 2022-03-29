using System;
using System.Runtime.InteropServices;
using Dynamsoft;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            BarcodeQRCodeReader.InitLicense("LICENSE-KEY"); // Get a license key from https://www.dynamsoft.com/customer/license/trialLicense?product=dbr

            // Check supported platforms
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {

            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {

            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {

            }
            
            BarcodeQRCodeReader? reader = null;
            try {
                reader = BarcodeQRCodeReader.Create();
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
