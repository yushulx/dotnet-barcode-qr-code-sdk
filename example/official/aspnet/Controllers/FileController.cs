using Microsoft.AspNetCore.Mvc;
using Dynamsoft.DBR;
using Dynamsoft.License;
using Dynamsoft.CVR;
using Dynamsoft.Core;

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
            string errorMsg;
            int errorCode = LicenseManager.InitLicense("DLS2eyJoYW5kc2hha2VDb2RlIjoiMjAwMDAxLTE2NDk4Mjk3OTI2MzUiLCJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSIsInNlc3Npb25QYXNzd29yZCI6IndTcGR6Vm05WDJrcEQ5YUoifQ==", out errorMsg);
            if (errorCode != (int)EnumErrorCode.EC_OK)
            {
                return Ok("License error: " + errorMsg);
            }

            var output = "No barcode found.";
            using (CaptureVisionRouter cvr = new CaptureVisionRouter())
            {
                foreach (var uploadFile in files)
                {
                    var fileName = uploadFile.FileName;
                    var filePath = Path.Combine(path, fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await uploadFile.CopyToAsync(stream);
                    }

                    CapturedResult result = cvr.Capture(filePath, PresetTemplate.PT_READ_BARCODES);
                    DecodedBarcodesResult? barcodesResult = result.GetDecodedBarcodesResult();
                    if (barcodesResult != null)
                    {
                        BarcodeResultItem[] items = barcodesResult.GetItems();
                        if (items.Length > 0)
                        {
                            output = "";
                            foreach (BarcodeResultItem barcodeItem in items)
                            {
                                output += barcodeItem.GetText() + "\n";
                            }
                        }
                        else
                        {
                            output = "No barcode found.";
                        }
                    }
                    else
                    {
                        output = "No barcode found.";
                    }
                }
            }

            return Ok(output);
        }
    }
}