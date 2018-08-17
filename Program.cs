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
        public enum BarcodeTextEncoding
        {
            BTE_Default = 0,
            BTE_SHIFT_JIS_932 = 932,
            BTE_GB2312_936 = 936,
            BTE_KOREAN_949 = 949,
            BTE_BIG5_950 = 950,
            BTE_UTF16 = 1200,
            BTE_UTF16BE = 1201,
            BTE_UTF8 = 65001
        }

        public enum BarcodeFormat
        {
            BF_All = 0,
            BF_OneD = 0x3FF,

            BF_CODE_39 = 0x1,
            BF_CODE_128 = 0x2,
            BF_CODE_93 = 0x4,
            BF_CODABAR = 0x8,
            BF_ITF = 0x10,
            BF_EAN_13 = 0x20,
            BF_EAN_8 = 0x40,
            BF_UPC_A = 0x80,
            BF_UPC_E = 0x100,
            BF_INDUSTRIAL_25 = 0x200,
            BF_PDF417 = 0x2000000,
            BF_QR_CODE = 0x4000000,
            BF_DATAMATRIX = 0x8000000,
            BF_AZTEC = 0x10000000
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
        public static extern int DBR_FreeTextResults(ref IntPtr pBarcodeResultArray);

        [DllImport("DynamsoftBarcodeReader")]
        public static extern void DBR_GetAllTextResults(IntPtr hBarcode, ref IntPtr pBarcodeResultArray);

        [DllImport("DynamsoftBarcodeReader")]
        public static extern int DBR_GetTemplateSettings(IntPtr hBarcode, string pszTemplateName, ref PublicParameterSettings pBarcodeResultArray);

        [DllImport("DynamsoftBarcodeReader")]
        public static extern int DBR_SetTemplateSettings(IntPtr hBarcode, string pszTemplateName, ref PublicParameterSettings pBarcodeResultArray, char[] szErrorMsgBuffer, int nErrorMsgBufferLen);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct BarcodeResult
        {
            BarcodeFormat emBarcodeFormat;
            public string pszBarcodeFormatString;
            public string pszBarcodeText;
            public IntPtr pBarcodeBytes;
            public int nBarcodeBytesLength;
            public IntPtr pLocalizationResult;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct BarcodeResultArray
        {
            public int iBarcodeCount;
            public IntPtr ppBarcodes;
        }

        public enum TextFilterMode
        {
            TFM_Disable = 1,
            TFM_Enable = 2,
        }

        public enum RegionPredetectionMode
        {
            RPM_Disable = 1,
            RPM_Enable = 2,
        }

        public enum BarcodeInvertMode
        {
            BIM_DarkOnLight,
            BIM_LightOnDark,
        }

        public enum ColourImageConvertMode
        {
            CICM_Auto,
            CICM_Grayscale
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct PublicParameterSettings
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] mName;
            public int mTimeout;
            public int mPDFRasterDPI;
            public TextFilterMode mTextFilterMode;
            public RegionPredetectionMode mRegionPredetectionMode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public char[] mLocalizationAlgorithmPriority;
            public int mBarcodeFormatIds;
            public int mMaxAlgorithmThreadCount;
            public int mTextureDetectionSensitivity;
            public int mDeblurLevel;
            public int mAntiDamageLevel;
            public int mMaxDimOfFullImageAsBarcodeZone;
            public int mMaxBarcodesCount;
            public BarcodeInvertMode mBarcodeInvertMode;
            public int mScaleDownThreshold;
            public int mGrayEqualizationSensitivity;
            public int mEnableFillBinaryVacancy;
            public ColourImageConvertMode mColourImageConvertMode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 248)]
            public char[] mReserved;
            public int mExpectedBarcodesCount;
            public int mBinarizationBlockSize;
        }

        class Program
        {
            static void Main(string[] args)
            {
                Console.WriteLine("Please enter an image file: ");
                string filename = Console.ReadLine();
                IntPtr pBarcodeResultArray = IntPtr.Zero;
                int iFormat = 0x3FF | 0x2000000 | 0x4000000 | 0x8000000 | 0x10000000; // 1D, PDF417, QRCODE, DataMatrix, AZTEC
                string license = null;

                // Check supported platforms
                if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    license = "t0068NQAAAAjddDOuMLYaaTayn16mZ2Kfjyw3cZriG1f6s7AASS8ZWJGGJx6zi9iiebnAMNux5fJ2PGOVx70X+MUfVKGBs0g=";
                }
                else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    license = "t0068NQAAAGykzcdt3HeyfcKCCoT4mBuIi1TMkIQ5s7HAAOIOuyjtgpsVmPDSIb/9iAbGknChQMbpV9LCXquugQlL5qsfE3I=";
                }
                else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    license = "30771C7C2299A427B5765EB4250FC51B";
                }

                // Initialize Dynamsoft Barcode Reader
                IntPtr hBarcode = WinBarcodeManager.DBR_CreateInstance();

                // Set a valid license
                WinBarcodeManager.DBR_InitLicense(hBarcode, license);

                // Update DBR params
                WinBarcodeManager.PublicParameterSettings pSettings = new WinBarcodeManager.PublicParameterSettings();

                int ret = DBR_GetTemplateSettings(hBarcode, "", ref pSettings);
                pSettings.mBarcodeFormatIds = iFormat;
                char[] szErrorMsgBuffer = new char[256];
                ret = DBR_SetTemplateSettings(hBarcode, "", ref pSettings, szErrorMsgBuffer, 256);

                // Read barcodes
                ret = WinBarcodeManager.DBR_DecodeFile(hBarcode, filename, "");

                WinBarcodeManager.DBR_GetAllTextResults(hBarcode, ref pBarcodeResultArray);

                // Print barcode results
                if (pBarcodeResultArray != IntPtr.Zero)
                {
                    WinBarcodeManager.BarcodeResultArray results = (WinBarcodeManager.BarcodeResultArray)Marshal.PtrToStructure(pBarcodeResultArray, typeof(WinBarcodeManager.BarcodeResultArray));
                    int count = results.iBarcodeCount;
                    IntPtr[] barcodes = new IntPtr[count];
                    Marshal.Copy(results.ppBarcodes, barcodes, 0, count);

                    for (int i = 0; i < count; i++)
                    {
                        WinBarcodeManager.BarcodeResult result = (WinBarcodeManager.BarcodeResult)Marshal.PtrToStructure(barcodes[i], typeof(WinBarcodeManager.BarcodeResult));

                        Console.WriteLine("Value: " + result.pszBarcodeText);
                        Console.WriteLine("Format: " + result.pszBarcodeFormatString);
                        Console.WriteLine("-----------------------------");
                    }

                    // Release memory of barcode results
                    WinBarcodeManager.DBR_FreeTextResults(ref pBarcodeResultArray);
                }

                // Destroy Dynamsoft Barcode Reader
                WinBarcodeManager.DBR_DestroyInstance(hBarcode);
            }
        }
    }
}
