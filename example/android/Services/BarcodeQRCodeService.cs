namespace BarcodeQRCode.Services
{
    public partial class BarcodeQRCodeService
    {
        public partial void InitSDK(string license);
        public partial string DecodeFile(string filePath);
    }
}