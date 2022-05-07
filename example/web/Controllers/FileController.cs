using Microsoft.AspNetCore.Mvc;
using Dynamsoft;

namespace MvcBarcodeQRCode.Controllers
{
    [ApiController]
    public class FileController : Controller
    {
        [HttpPost("/upload")]
        public async Task<IActionResult> Upload()
        {
            var files = Request.Form.Files;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Get a license key from https://www.dynamsoft.com/customer/license/trialLicense?product=dbr
            BarcodeQRCodeReader.InitLicense("DLS2eyJoYW5kc2hha2VDb2RlIjoiMjAwMDAxLTE2NDk4Mjk3OTI2MzUiLCJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSIsInNlc3Npb25QYXNzd29yZCI6IndTcGR6Vm05WDJrcEQ5YUoifQ==");
            BarcodeQRCodeReader? reader = BarcodeQRCodeReader.Create();
            // reader.SetParameters("{\"Version\":\"3.0\", \"ImageParameter\":{\"Name\":\"IP1\", \"BarcodeFormatIds\":[\"BF_QR_CODE\", \"BF_ONED\"], \"ExpectedBarcodesCount\":20}}");

            var output = "No barcode found.";
            foreach (var uploadFile in files)
            {
                var fileName = uploadFile.FileName;
                var filePath = Path.Combine(path, fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await uploadFile.CopyToAsync(stream);
                }
                if (reader != null)
                {
                    var results = reader.DecodeFile(filePath);
                    if (results != null)
                    {
                        output = "";
                        foreach (string result in results)
                        {
                            output += result + "\n";
                        }
                    }
                    else
                    {
                        output = "No barcode found.";
                    }
                }
            }

            if (reader != null)
            {
                reader.Destroy();
            }
            return Ok(output);
        }
    }
}