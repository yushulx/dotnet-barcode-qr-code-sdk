using Dynamsoft.CVR;
using Dynamsoft.DBR;
using Dynamsoft.License;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZXing;
using ZXing.Common;

namespace BarcodeSmokeTests
{
    /// <summary>
    /// Checks that the Dynamsoft Barcode Reader bundle is packaged correctly: the
    /// managed assembly resolves, the native modules load, and the licensing layer
    /// reports errors instead of crashing. None of these need a valid license, so
    /// they run on every push.
    /// </summary>
    [TestClass]
    public class RuntimeSmokeTests
    {
        [TestMethod]
        public void CaptureVisionRouter_CanBeConstructed()
        {
            // Constructing the router forces the native modules to load. A wrong or
            // missing native binary for the current RID fails right here.
            using (var router = new CaptureVisionRouter())
            {
                Assert.IsNotNull(router);
            }
        }

        [TestMethod]
        public void Capture_WithoutLicense_ReturnsErrorCodeInsteadOfThrowing()
        {
            int licenseCode = LicenseManager.InitLicense("", out _);

            using (var router = new CaptureVisionRouter())
            {
                // A raw pixel buffer keeps the test independent of image codecs.
                var image = SyntheticBarcodes.CreateQrCode("hello");

                CapturedResult result = router.Capture(image, PresetTemplate.PT_READ_BARCODES);

                Assert.IsNotNull(result, "Capture() should always return a result object.");

                if (licenseCode == 0)
                {
                    // A license was supplied through the environment, so decoding
                    // is expected to work here as well.
                    Assert.AreEqual(0, result.GetErrorCode(), result.GetErrorString());
                }
                else
                {
                    Assert.AreNotEqual(
                        0,
                        result.GetErrorCode(),
                        "Without a valid license Capture() must report a license error.");
                }
            }
        }

        [TestMethod]
        public void InitLicense_WithInvalidKey_IsRejected()
        {
            int code = LicenseManager.InitLicense("DLS2-not-a-real-license-key", out string message);

            Assert.AreNotEqual(0, code, "An invalid license key must not be accepted.");
            Assert.IsFalse(string.IsNullOrWhiteSpace(message), "A rejection reason is expected.");
        }
    }
}
