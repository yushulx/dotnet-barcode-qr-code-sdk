using Dynamsoft.CVR;
using Dynamsoft.DBR;
using Dynamsoft.License;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BarcodeSmokeTests
{
    /// <summary>
    /// End-to-end decode checks. They need an activated license, supplied through the
    /// <c>DYNAMSOFT_LICENSE_KEY</c> environment variable (a GitHub Actions secret in CI).
    /// Without one the tests report Inconclusive instead of passing silently.
    /// </summary>
    [TestClass]
    public class DecodeTests
    {
        private const string EnvVar = "DYNAMSOFT_LICENSE_KEY";

        private static string ResolveLicense()
        {
            string key = Environment.GetEnvironmentVariable(EnvVar);

            if (string.IsNullOrWhiteSpace(key))
            {
                Assert.Inconclusive(
                    $"{EnvVar} is not set, so the decode assertions were skipped. " +
                    "Provide a Dynamsoft trial license to run them.");
            }

            int code = LicenseManager.InitLicense(key.Trim(), out string message);
            if (code != 0 && code != (int)Dynamsoft.Core.EnumErrorCode.EC_LICENSE_WARNING)
            {
                Assert.Fail($"{EnvVar} was provided but activation failed ({code}): {message}");
            }

            return key;
        }

        [TestMethod]
        public void Decodes_QrCode_MatchingPayload()
        {
            ResolveLicense();

            const string payload = "https://www.dynamsoft.com/codepool/dotnet/";

            using (var router = new CaptureVisionRouter())
            {
                CapturedResult result = router.Capture(
                    SyntheticBarcodes.CreateQrCode(payload),
                    PresetTemplate.PT_READ_BARCODES);

                Assert.AreEqual(0, result.GetErrorCode(), result.GetErrorString());

                DecodedBarcodesResult barcodes = result.GetDecodedBarcodesResult();
                Assert.IsNotNull(barcodes, "Expected a decoded barcode result.");

                BarcodeResultItem[] items = barcodes.GetItems();
                Assert.AreEqual(1, items.Length, "Exactly one QR code is in the test image.");
                Assert.AreEqual(payload, items[0].GetText());
                Assert.AreEqual("QR_CODE", items[0].GetFormatString());
            }
        }

        [TestMethod]
        public void Decodes_MultipleBarcodes_InOneImage()
        {
            ResolveLicense();

            const string first = "DOTNET-SMOKE-0001";
            const string second = "DOTNET-SMOKE-0002";

            using (var router = new CaptureVisionRouter())
            {
                CapturedResult result = router.Capture(
                    SyntheticBarcodes.CreateQrCodePair(first, second),
                    PresetTemplate.PT_READ_BARCODES);

                Assert.AreEqual(0, result.GetErrorCode(), result.GetErrorString());

                BarcodeResultItem[] items = result.GetDecodedBarcodesResult().GetItems();
                var texts = items.Select(item => item.GetText()).OrderBy(t => t).ToArray();

                CollectionAssert.AreEquivalent(
                    new[] { first, second },
                    texts,
                    "Both QR codes should be decoded from the composite image.");
            }
        }
    }
}
