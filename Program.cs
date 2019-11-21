using System;
using System.Runtime.InteropServices;

namespace DynamsoftBarcode
{
    class BarcodeManager
    {
        public const int OS_WIN = 0;
        public const int OS_LINUX = 1;
        public const int OS_MAC = 2;
    }

    // Dynamsoft Barcode Reader 6.3
    class WinBarcodeManager : BarcodeManager
    {
        // public enum BarcodeTextEncoding
        // {
        //     BTE_Default = 0,
        //     BTE_SHIFT_JIS_932 = 932,
        //     BTE_GB2312_936 = 936,
        //     BTE_KOREAN_949 = 949,
        //     BTE_BIG5_950 = 950,
        //     BTE_UTF16 = 1200,
        //     BTE_UTF16BE = 1201,
        //     BTE_UTF8 = 65001
        // }

        public enum ImagePixelFormat
        {
            /**0:Black, 1:White */
            IPF_BINARY,

            /**0:White, 1:Black */
            IPF_BINARYINVERTED,

            /**8bit gray */
            IPF_GRAYSCALED,

            /**NV21 */
            IPF_NV21,

            /**16bit */
            IPF_RGB_565,

            /**16bit */
            IPF_RGB_555,

            /**24bit */
            IPF_RGB_888,

            /**32bit */
            IPF_ARGB_8888,

            /**48bit */
            IPF_RGB_161616,

            /**64bit */
            IPF_ARGB_16161616
        }

        public enum BarcodeFormat_2
        {
            /**No barcode format in BarcodeFormat group 2*/
            BF2_NULL = 0x00,

            /**Nonstandard barcode */
            BF2_NONSTANDARD_BARCODE = 0x01
        }

        public enum BarcodeFormat
        {
            /**All supported formats */
            BF_ALL = -32505857,

            /**Combined value of BF_CODABAR, BF_CODE_128, BF_CODE_39, BF_CODE_39_Extended, BF_CODE_93, BF_EAN_13, BF_EAN_8, INDUSTRIAL_25, BF_ITF, BF_UPC_A, BF_UPC_E; */
            BF_ONED = 0x000007FF,

            /**Combined value of BF_GS1_DATABAR_OMNIDIRECTIONAL, BF_GS1_DATABAR_TRUNCATED, BF_GS1_DATABAR_STACKED, BF_GS1_DATABAR_STACKED_OMNIDIRECTIONAL, BF_GS1_DATABAR_EXPANDED, BF_GS1_DATABAR_EXPANDED_STACKED, BF_GS1_DATABAR_LIMITED*/
            BF_GS1_DATABAR = 0x0003F800,

            /**Combined value of BF_USPSINTELLIGENTMAIL, BF_POSTNET, BF_PLANET, BF_AUSTRALIANPOST, BF_UKROYALMAIL. Not supported yet. */
            BF_POSTALCODE = 0x01F00000,

            /**Code 39 */
            BF_CODE_39 = 0x1,

            /**Code 128 */
            BF_CODE_128 = 0x2,

            /**Code 93 */
            BF_CODE_93 = 0x4,

            /**Codabar */
            BF_CODABAR = 0x8,

            /**ITF */
            BF_ITF = 0x10,

            /**EAN-13 */
            BF_EAN_13 = 0x20,

            /**EAN-8 */
            BF_EAN_8 = 0x40,

            /**UPC-A */
            BF_UPC_A = 0x80,

            /**UPC-E */
            BF_UPC_E = 0x100,

            /**Industrial 2 of 5 */
            BF_INDUSTRIAL_25 = 0x200,

            /**CODE39 Extended */
            BF_CODE_39_EXTENDED = 0x400,

            /**GS1 Databar Omnidirectional*/
            BF_GS1_DATABAR_OMNIDIRECTIONAL = 0x800,

            /**GS1 Databar Truncated*/
            BF_GS1_DATABAR_TRUNCATED = 0x1000,

            /**GS1 Databar Stacked*/
            BF_GS1_DATABAR_STACKED = 0x2000,

            /**GS1 Databar Stacked Omnidirectional*/
            BF_GS1_DATABAR_STACKED_OMNIDIRECTIONAL = 0x4000,

            /**GS1 Databar Expanded*/
            BF_GS1_DATABAR_EXPANDED = 0x8000,

            /**GS1 Databar Expaned Stacked*/
            BF_GS1_DATABAR_EXPANDED_STACKED = 0x10000,

            /**GS1 Databar Limited*/
            BF_GS1_DATABAR_LIMITED = 0x20000,

            /**Patch code. */
            BF_PATCHCODE = 0x00040000,

            /**USPS Intelligent Mail. Not supported yet. */
            BF_USPSINTELLIGENTMAIL = 0x00100000,

            /**Postnet. Not supported yet. */
            BF_POSTNET = 0x00200000,

            /**Planet. Not supported yet. */
            BF_PLANET = 0x00400000,

            /**Australian Post. Not supported yet. */
            BF_AUSTRALIANPOST = 0x00800000,

            /**UK Royal Mail. Not supported yet. */
            BF_UKROYALMAIL = 0x01000000,

            /**PDF417 */
            BF_PDF417 = 0x02000000,

            /**QRCode */
            BF_QR_CODE = 0x04000000,

            /**DataMatrix */
            BF_DATAMATRIX = 0x08000000,

            /**AZTEC */
            BF_AZTEC = 0x10000000,

            /**MAXICODE */
            BF_MAXICODE = 0x20000000,

            /**Micro QR Code*/
            BF_MICRO_QR = 0x40000000,

            /**Micro PDF417*/
            BF_MICRO_PDF417 = 0x00080000,

            /**GS1 Composite Code*/
            BF_GS1_COMPOSITE = -2147483648,

            /**No barcode format in BarcodeFormat group 1*/
            BF_NULL = 0x00
        }

        [DllImport("DynamsoftBarcodeReader")]
        public static extern IntPtr DBR_CreateInstance();

        [DllImport("DynamsoftBarcodeReader")]
        public static extern void DBR_DestroyInstance(IntPtr hBarcode);

        [DllImport("DynamsoftBarcodeReader")]
        public static extern int DBR_InitLicense(IntPtr hBarcode, string license);

        [DllImport("DynamsoftBarcodeReader")]
        public static extern int DBR_DecodeFile(IntPtr hBarcode, string filename, string template);

        [DllImport("DynamsoftBarcodeReader")]
        public static extern int DBR_FreeTextResults(ref IntPtr pTextResultArray);

        [DllImport("DynamsoftBarcodeReader")]
        public static extern void DBR_GetAllTextResults(IntPtr hBarcode, ref IntPtr pTextResultArray);
        [DllImport("DynamsoftBarcodeReader")]
        public static extern void DBR_DecodeBuffer(IntPtr hBarcode, ref IntPtr pBufferBytes, int width, int height, int stride, ImagePixelFormat format, string template);

        // [DllImport("DynamsoftBarcodeReader")]
        // public static extern int DBR_GetTemplateSettings(IntPtr hBarcode, string pszTemplateName, ref PublicParameterSettings pTextResultArray);

        // [DllImport("DynamsoftBarcodeReader")]
        // public static extern int DBR_SetTemplateSettings(IntPtr hBarcode, string pszTemplateName, ref PublicParameterSettings pTextResultArray, char[] szErrorMsgBuffer, int nErrorMsgBufferLen);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct PTextResult
        {
            BarcodeFormat emBarcodeFormat;
            public string barcodeFormatString;
            BarcodeFormat_2 barcodeFormat_2;
            string barcodeFormatString_2;
            public string barcodeText;
            IntPtr barcodeBytes;
            int barcodeBytesLength;
            IntPtr localizationResult;
            IntPtr detailedResult;
            int resultsCount;
            IntPtr results;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 56)]
            char[] reserved;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TextResultArray
        {
            public int resultsCount;
            public IntPtr results;
        }

        // public enum TextFilterMode
        // {
        //     TFM_Disable = 1,
        //     TFM_Enable = 2,
        // }

        // public enum RegionPredetectionMode
        // {
        //     RPM_Disable = 1,
        //     RPM_Enable = 2,
        // }

        // public enum BarcodeInvertMode
        // {
        //     BIM_DarkOnLight,
        //     BIM_LightOnDark,
        // }

        // public enum ColourImageConvertMode
        // {
        //     CICM_Auto,
        //     CICM_Grayscale
        // }

        // [StructLayout(LayoutKind.Sequential, Pack = 1)]
        // internal struct PublicParameterSettings
        // {
        //     [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        //     public char[] mName;
        //     public int mTimeout;
        //     public int mPDFRasterDPI;
        //     public TextFilterMode mTextFilterMode;
        //     public RegionPredetectionMode mRegionPredetectionMode;
        //     [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        //     public char[] mLocalizationAlgorithmPriority;
        //     public int mBarcodeFormatIds;
        //     public int mMaxAlgorithmThreadCount;
        //     public int mTextureDetectionSensitivity;
        //     public int mDeblurLevel;
        //     public int mAntiDamageLevel;
        //     public int mMaxDimOfFullImageAsBarcodeZone;
        //     public int mMaxBarcodesCount;
        //     public BarcodeInvertMode mBarcodeInvertMode;
        //     public int mScaleDownThreshold;
        //     public int mGrayEqualizationSensitivity;
        //     public int mEnableFillBinaryVacancy;
        //     public ColourImageConvertMode mColourImageConvertMode;
        //     [MarshalAs(UnmanagedType.ByValArray, SizeConst = 248)]
        //     public char[] mReserved;
        //     public int mExpectedBarcodesCount;
        //     public int mBinarizationBlockSize;
        // }

        class Program
        {
            static void Main(string[] args)
            {
                Console.WriteLine("Please enter an image file: ");
                string filename = Console.ReadLine();
                IntPtr pTextResultArray = IntPtr.Zero;
                //int iFormat = 0x3FF | 0x2000000 | 0x4000000 | 0x8000000 | 0x10000000; // 1D, PDF417, QRCODE, DataMatrix, AZTEC
                string license = "LICENSE-KEY";

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

                // Initialize Dynamsoft Barcode Reader
                IntPtr hBarcode = WinBarcodeManager.DBR_CreateInstance();

                // Set a valid license
                WinBarcodeManager.DBR_InitLicense(hBarcode, license);

                // // Update DBR params
                // WinBarcodeManager.PublicParameterSettings pSettings = new WinBarcodeManager.PublicParameterSettings();

                // int ret = DBR_GetTemplateSettings(hBarcode, "", ref pSettings);
                // pSettings.mBarcodeFormatIds = iFormat;
                // char[] szErrorMsgBuffer = new char[256];
                // ret = DBR_SetTemplateSettings(hBarcode, "", ref pSettings, szErrorMsgBuffer, 256);

                // Read barcodes
                int ret = WinBarcodeManager.DBR_DecodeFile(hBarcode, filename, "");

                WinBarcodeManager.DBR_GetAllTextResults(hBarcode, ref pTextResultArray);

                // Print barcode results
                if (pTextResultArray != IntPtr.Zero)
                {
                    WinBarcodeManager.TextResultArray results = (WinBarcodeManager.TextResultArray)Marshal.PtrToStructure(pTextResultArray, typeof(WinBarcodeManager.TextResultArray));
                    int count = results.resultsCount;
                    IntPtr[] barcodes = new IntPtr[count];
                    Marshal.Copy(results.results, barcodes, 0, count);

                    for (int i = 0; i < count; i++)
                    {
                        WinBarcodeManager.PTextResult result = (WinBarcodeManager.PTextResult)Marshal.PtrToStructure(barcodes[i], typeof(WinBarcodeManager.PTextResult));

                        Console.WriteLine("Value: " + result.barcodeText);
                        Console.WriteLine("Format: " + result.barcodeFormatString);
                        Console.WriteLine("-----------------------------");
                    }

                    // Release memory of barcode results
                    WinBarcodeManager.DBR_FreeTextResults(ref pTextResultArray);
                }

                // Destroy Dynamsoft Barcode Reader
                WinBarcodeManager.DBR_DestroyInstance(hBarcode);
            }
        }
    }
}
