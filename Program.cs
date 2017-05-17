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

    // Dynamsoft Barcode Reader 5.1
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
            BF_DATAMATRIX = 0x8000000
        }

        [DllImport("DynamsoftBarcodeReader")]
        public static extern IntPtr DBR_CreateInstance();

        [DllImport("DynamsoftBarcodeReader")]
        public static extern void DBR_DestroyInstance(IntPtr hBarcode);

        [DllImport("DynamsoftBarcodeReader")]
        public static extern int DBR_InitLicenseEx(IntPtr hBarcode, string license);

        [DllImport("DynamsoftBarcodeReader")]
        public static extern int DBR_DecodeFileEx(IntPtr hBarcode, string filename, ref IntPtr pBarcodeResultArray);

        [DllImport("DynamsoftBarcodeReader")]
        public static extern int DBR_SetBarcodeFormats(IntPtr hBarcode, int iFormat);

        [DllImport("DynamsoftBarcodeReader")]
        public static extern int DBR_SetMaxBarcodesNumPerPage(IntPtr hBarcode, int iMaxCount);

        [DllImport("DynamsoftBarcodeReader")]
        public static extern int DBR_FreeBarcodeResults(ref IntPtr pBarcodeResultArray);

        [DllImport("DynamsoftBarcodeReader")]
        public static extern void DBR_SetBarcodeTextEncoding(IntPtr hBarcode, BarcodeTextEncoding emEncoding);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct BarcodeResult
        {
            public int emBarcodeFormat;
            public string pBarcodeData;
            public int iBarcodeDataLength;
            public int iLeft;
            public int iTop;
            public int iWidth;
            public int iHeight;
            public int iX1;
            public int iY1;
            public int iX2;
            public int iY2;
            public int iX3;
            public int iY3;
            public int iX4;
            public int iY4;
            public int iPageNum;
            public IntPtr pBarcodeText;
            public int iAngle;
            public int iModuleSize;
            public int bIsUnrecognized;
            public string pBarcodeFormatString;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct BarcodeResultArray
        {
            public int iBarcodeCount;
            public IntPtr ppBarcodes;
        }
    }

    // Dynamsoft Barcode Reader 4.2
    class LinuxMacBarcodeManager : BarcodeManager
    {
        [DllImport("DynamsoftBarcodeReader")]
        public static extern int DBR_InitLicense(string license);

        [DllImport("DynamsoftBarcodeReader")]
        public static extern int DBR_DecodeFile(string filename, IntPtr opt, ref IntPtr pBarcodeResultArray);

        [DllImport("DynamsoftBarcodeReader")]
        public static extern int DBR_FreeBarcodeResults(ref IntPtr pBarcodeResultArray);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct BarcodeResult
        {
            public Int64 llFormat;
            public string pBarcodeData;
            public int iBarcodeDataLength;
            public int iLeft;
            public int iTop;
            public int iWidth;
            public int iHeight;
            public int iX1;
            public int iY1;
            public int iX2;
            public int iY2;
            public int iX3;
            public int iY3;
            public int iX4;
            public int iY4;
            public int iPageNum;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct BarcodeResultArray
        {
            public int iBarcodeCount;
            public IntPtr ppBarcodes;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ReaderOptions
        {
            public int iMaxBarcodesNumPerPage;
            public long llBarcodeFormat;
        }

        // Supported barcode formats
        public static string GetFormatStr(long format)
        {
            if (format == CODE_39)
                return "CODE_39";
            if (format == CODE_128)
                return "CODE_128";
            if (format == CODE_93)
                return "CODE_93";
            if (format == CODABAR)
                return "CODABAR";
            if (format == ITF)
                return "ITF";
            if (format == UPC_A)
                return "UPC_A";
            if (format == UPC_E)
                return "UPC_E";
            if (format == EAN_13)
                return "EAN_13";
            if (format == EAN_8)
                return "EAN_8";
            if (format == INDUSTRIAL_25)
                return "INDUSTRIAL_25";
            if (format == QR_CODE)
                return "QR_CODE";
            if (format == PDF417)
                return "PDF417";
            if (format == DATAMATRIX)
                return "DATAMATRIX";

            return "UNKNOWN";
        }

        const long OneD = 0x3FFL;

        const long CODE_39 = 0x1L;
        const long CODE_128 = 0x2L;
        const long CODE_93 = 0x4L;
        const long CODABAR = 0x8L;
        const long ITF = 0x10L;
        const long EAN_13 = 0x20L;
        const long EAN_8 = 0x40L;
        const long UPC_A = 0x80L;
        const long UPC_E = 0x100L;
        const long INDUSTRIAL_25 = 0x200L;

        const long PDF417 = 0x2000000L;
        const long DATAMATRIX = 0x8000000L;
        const long QR_CODE = 0x4000000L;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter an image file: ");
            string filename = Console.ReadLine();

            IntPtr pBarcodeResultArray = IntPtr.Zero;
            int iFormat = 0x3FF | 0x2000000 | 0x8000000 | 0x4000000; // 1D, QRCODE, PDF417, DataMatrix
            int iMaxCount = 0x7FFFFFFF;
            string license = null;

            int iOS = -1;
            // Check supported platforms
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                iOS = BarcodeManager.OS_WIN;
                license = "t0260NQAAALGw+aCAePXdOS3p1xkqT5hesExKVpEe7NiIhkdlUz/Jvx8km3ItI0ykUcmeP67BYVlJ2PDW++bjSYmDLmyMgOmmvc0mdvhlSy500kqnLoBAL+TybcdAP42b5p5WehK9Gsmweqi+ydK6B0KaUNQMDJZ1DrnhDXZ209pfpJoVybPk/CMcDKXaF2oRLKEOYVscXTF6mbiWUnMP5lj4OdTvFa0eVRcE0q9BckiqYgUZLK4L6DVgRXWRL5nRPtvEtd+qZe6psu0JZ7HEPhsbodfAVH2G436z1QahLGJXdQCoQv8UQ/quGQP2wCWemfueeKJ4Y6WsvEvmkUpizbTOE3Njjaw=";
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                iOS = BarcodeManager.OS_LINUX;
                license = "30771C7C2299A4271A84011B981A3901";
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                iOS = BarcodeManager.OS_MAC;
                license = "30771C7C2299A427B5765EB4250FC51B";
            }

            switch (iOS)
            {
                case BarcodeManager.OS_WIN:
                    {
                        // Initialize Dynamsoft Barcode Reader
                        IntPtr hBarcode = WinBarcodeManager.DBR_CreateInstance();

                        // Set a valid license
                        WinBarcodeManager.DBR_InitLicenseEx(hBarcode, license);

                        // Set an encoding type
                        WinBarcodeManager.DBR_SetBarcodeTextEncoding(hBarcode, WinBarcodeManager.BarcodeTextEncoding.BTE_UTF8);

                        // Set barcode formats
                        WinBarcodeManager.DBR_SetBarcodeFormats(hBarcode, iFormat);
                        WinBarcodeManager.DBR_SetMaxBarcodesNumPerPage(hBarcode, iMaxCount);

                        // Read barcodes
                        int ret = WinBarcodeManager.DBR_DecodeFileEx(hBarcode, filename, ref pBarcodeResultArray);

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

                                Console.WriteLine("Value: " + Marshal.PtrToStringUni(result.pBarcodeText));
                                Console.WriteLine("Format: " + result.pBarcodeFormatString);
                                Console.WriteLine("-----------------------------");
                            }

                            // Release memory of barcode results
                            WinBarcodeManager.DBR_FreeBarcodeResults(ref pBarcodeResultArray);
                        }

                        // Destroy Dynamsoft Barcode Reader
                        WinBarcodeManager.DBR_DestroyInstance(hBarcode);
                    }
                    break;
                case BarcodeManager.OS_LINUX:
                case BarcodeManager.OS_MAC:
                    {
                        // Set a valid license
                        LinuxMacBarcodeManager.DBR_InitLicense(license);
                        LinuxMacBarcodeManager.ReaderOptions ro = new LinuxMacBarcodeManager.ReaderOptions();
                        ro.llBarcodeFormat = iFormat;
                        ro.iMaxBarcodesNumPerPage = iMaxCount;

                        // Copy the struct to unmanaged memory.
                        IntPtr opt = Marshal.AllocHGlobal(Marshal.SizeOf(ro));
                        Marshal.StructureToPtr(ro, opt, false);

                        // Read barcodes
                        int ret = LinuxMacBarcodeManager.DBR_DecodeFile(filename, opt, ref pBarcodeResultArray);

                        // Print barcode results
                        if (pBarcodeResultArray != IntPtr.Zero)
                        {
                            LinuxMacBarcodeManager.BarcodeResultArray results = (LinuxMacBarcodeManager.BarcodeResultArray)Marshal.PtrToStructure(pBarcodeResultArray, typeof(LinuxMacBarcodeManager.BarcodeResultArray));
                            int count = results.iBarcodeCount;
                            IntPtr[] barcodes = new IntPtr[count];
                            Marshal.Copy(results.ppBarcodes, barcodes, 0, count);

                            for (int i = 0; i < count; i++)
                            {
                                LinuxMacBarcodeManager.BarcodeResult result = (LinuxMacBarcodeManager.BarcodeResult)Marshal.PtrToStructure(barcodes[i], typeof(LinuxMacBarcodeManager.BarcodeResult));

                                Console.WriteLine("Value: " + result.pBarcodeData);
                                Console.WriteLine("Format: " + LinuxMacBarcodeManager.GetFormatStr(result.llFormat));
                                Console.WriteLine("-----------------------------");
                            }

                            // Release memory of barcode results
                            LinuxMacBarcodeManager.DBR_FreeBarcodeResults(ref pBarcodeResultArray);
                        }
                    }
                    break;
            }
        }
    }
}
