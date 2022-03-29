using System;
using System.Runtime.InteropServices;

namespace DynamsoftBarcode
{
    class Program
    {
        static void Main(string[] args)
        {
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

            BarcodeReaderManager barcodeReaderManager = new BarcodeReaderManager();
            Console.WriteLine("Please enter an image file: ");
            try {
                string? filename = Console.ReadLine();
                if (filename != null) barcodeReaderManager.DecodeFile(filename);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                barcodeReaderManager.Destroy();
            }
        }
    }
}
