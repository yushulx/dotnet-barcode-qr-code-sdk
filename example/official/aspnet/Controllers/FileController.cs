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
            if (errorCode != (int)EnumErrorCode.EC_OK && errorCode != (int)EnumErrorCode.EC_LICENSE_CACHE_USED)
            {
                return Ok(new { error = "License error: " + errorMsg });
            }

            var barcodes = new List<object>();
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
                        foreach (BarcodeResultItem barcodeItem in items)
                        {
                            var location = barcodeItem.GetLocation();
                            barcodes.Add(new
                            {
                                text = barcodeItem.GetText(),
                                format = barcodeItem.GetFormatString(),
                                points = new[]
                                {
                                    new { x = location.points[0][0], y = location.points[0][1] },
                                    new { x = location.points[1][0], y = location.points[1][1] },
                                    new { x = location.points[2][0], y = location.points[2][1] },
                                    new { x = location.points[3][0], y = location.points[3][1] },
                                }
                            });
                        }
                    }
                }
            }

            return Ok(new { barcodes });
        }
    }
}