using Dynamsoft;

namespace BarcodeQRCode.Services
{
    public partial class BarcodeQRCodeService
    {
        BarcodeQRCodeReader? reader = null;
        
        public partial void InitSDK(string license)
        {

            BarcodeQRCodeReader.InitLicense("DLS2eyJoYW5kc2hha2VDb2RlIjoiMjAwMDAxLTE2NDk4Mjk3OTI2MzUiLCJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSIsInNlc3Npb25QYXNzd29yZCI6IndTcGR6Vm05WDJrcEQ5YUoifQ=="); // Get a license key from https://www.dynamsoft.com/customer/license/trialLicense?product=dbr

            reader = BarcodeQRCodeReader.Create();
        }

        public partial string DecodeFile(string filePath)
        {
            if (reader == null)
                return "";

            string decodingResult = "";            
            try
            {

                BarcodeQRCodeReader.Result[]? results = reader.DecodeFile(filePath);
                if (results != null && results.Length > 0)
                {
                    int i = 1;
                    foreach (BarcodeQRCodeReader.Result result in results)
                    {
                        string barcodeFormat = result.Format1;
                        string message = "Barcode " + i + ": " + barcodeFormat + ", " + result.Text;
                        Console.WriteLine(message);
                        decodingResult += message;
                        i++;
                    }
                }
                else
                {
                    decodingResult = "No barcode found.";
                }
            }
            catch (Exception e)
            {
                decodingResult = e.Message;
            }

            return decodingResult;
        }
    }
}