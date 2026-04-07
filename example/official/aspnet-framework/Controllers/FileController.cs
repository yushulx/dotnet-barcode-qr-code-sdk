using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Dynamsoft.DBR;
using Dynamsoft.License;
using Dynamsoft.CVR;
using Dynamsoft.Core;

namespace MvcBarcodeQRCodeFramework.Controllers
{
    public class FileController : Controller
    {
        // POST /upload
        [HttpPost]
        [Route("upload")]
        public ActionResult Upload()
        {
            var files = Request.Files;
            var uploadPath = Server.MapPath("~/Upload");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            // Get a license key from https://www.dynamsoft.com/customer/license/trialLicense?product=dbr
            string errorMsg;
            int errorCode = LicenseManager.InitLicense(
                "DLS2eyJoYW5kc2hha2VDb2RlIjoiMjAwMDAxLTE2NDk4Mjk3OTI2MzUiLCJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSIsInNlc3Npb25QYXNzd29yZCI6IndTcGR6Vm05WDJrcEQ5YUoifQ==",
                out errorMsg);

            if (errorCode != (int)EnumErrorCode.EC_OK &&
                errorCode != (int)EnumErrorCode.EC_LICENSE_CACHE_USED)
            {
                return Json(new { error = "License error: " + errorMsg });
            }

            var barcodes = new List<object>();

            using (var cvr = new CaptureVisionRouter())
            {
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    if (file == null || file.ContentLength == 0) continue;

                    string fileName = Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(uploadPath, fileName);
                    file.SaveAs(filePath);

                    CapturedResult result = cvr.Capture(filePath, PresetTemplate.PT_READ_BARCODES);
                    DecodedBarcodesResult barcodesResult = result.GetDecodedBarcodesResult();
                    if (barcodesResult == null) continue;

                    foreach (BarcodeResultItem item in barcodesResult.GetItems())
                    {
                        var loc = item.GetLocation();
                        barcodes.Add(new
                        {
                            text   = item.GetText(),
                            format = item.GetFormatString(),
                            points = new[]
                            {
                                new { x = loc.points[0][0], y = loc.points[0][1] },
                                new { x = loc.points[1][0], y = loc.points[1][1] },
                                new { x = loc.points[2][0], y = loc.points[2][1] },
                                new { x = loc.points[3][0], y = loc.points[3][1] },
                            }
                        });
                    }
                }
            }

            return Json(new { barcodes });
        }
    }
}
