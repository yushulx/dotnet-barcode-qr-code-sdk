using System.Text;
using System.Runtime.InteropServices;

namespace DynamsoftBarcode
{
    class BarcodeReaderManager
    {
        private IntPtr hBarcode;

        public BarcodeReaderManager()
        {
            byte[] errorMsg = new byte[512];
            DBR_InitLicense("LICENSE-KEY", errorMsg, 512);
            Console.WriteLine(Encoding.ASCII.GetString(errorMsg) + "\n");
            hBarcode = DBR_CreateInstance();
        }

        public void Destroy()
        {
            if (hBarcode != IntPtr.Zero)
            {
                DBR_DestroyInstance(hBarcode);
                hBarcode = IntPtr.Zero;
            }
        }

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

            /**16bit with RGB channel order stored in memory from high to low address*/
            IPF_RGB_565,

            /**16bit with RGB channel order stored in memory from high to low address*/
            IPF_RGB_555,

            /**24bit with RGB channel order stored in memory from high to low address*/
            IPF_RGB_888,

            /**32bit with ARGB channel order stored in memory from high to low address*/
            IPF_ARGB_8888,

            /**48bit with RGB channel order stored in memory from high to low address*/
            IPF_RGB_161616,

            /**64bit with ARGB channel order stored in memory from high to low address*/
            IPF_ARGB_16161616,

            /**32bit with ABGR channel order stored in memory from high to low address*/
            IPF_ABGR_8888,

            /**64bit with ABGR channel order stored in memory from high to low address*/
            IPF_ABGR_16161616,

            /**24bit with BGR channel order stored in memory from high to low address*/
            IPF_BGR_888
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
            /**All supported formats in BarcodeFormat group 1*/
            BF_ALL = -29360129,

            /**Combined value of BF_CODABAR, BF_CODE_128, BF_CODE_39, BF_CODE_39_Extended, BF_CODE_93, BF_EAN_13, BF_EAN_8, INDUSTRIAL_25, BF_ITF, BF_UPC_A, BF_UPC_E, BF_MSI_CODE;  */
            BF_ONED = 0x003007FF,

            /**Combined value of BF_GS1_DATABAR_OMNIDIRECTIONAL, BF_GS1_DATABAR_TRUNCATED, BF_GS1_DATABAR_STACKED, BF_GS1_DATABAR_STACKED_OMNIDIRECTIONAL, BF_GS1_DATABAR_EXPANDED, BF_GS1_DATABAR_EXPANDED_STACKED, BF_GS1_DATABAR_LIMITED*/
            BF_GS1_DATABAR = 0x0003F800,

            /**Code 39 */
            BF_CODE_39 = 0x1,

            /**Code 128 */
            BF_CODE_128 = 0x2,

            /**Code 93 */
            BF_CODE_93 = 0x4,

            /**Codabar */
            BF_CODABAR = 0x8,

            /**Interleaved 2 of 5 */
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

            BF_GS1_COMPOSITE = -2147483648,

            /**MSI Code*/
            BF_MSI_CODE = 0x100000,

            /*Code 11*/
            BF_CODE_11 = 0x200000,

            /**No barcode format in BarcodeFormat group 1*/
            BF_NULL = 0x00
        }

        [DllImport("DynamsoftBarcodeReader")]
        static extern IntPtr DBR_CreateInstance();

        [DllImport("DynamsoftBarcodeReader")]
        static extern void DBR_DestroyInstance(IntPtr hBarcode);

        [DllImport("DynamsoftBarcodeReader")]
        static extern int DBR_InitLicense(string license, [Out] byte[] errorMsg, int errorMsgSize);

        [DllImport("DynamsoftBarcodeReader")]
        static extern int DBR_DecodeFile(IntPtr hBarcode, string filename, string template);

        [DllImport("DynamsoftBarcodeReader")]
        static extern int DBR_FreeTextResults(ref IntPtr pTextResultArray);

        [DllImport("DynamsoftBarcodeReader")]
        static extern void DBR_GetAllTextResults(IntPtr hBarcode, ref IntPtr pTextResultArray);
        [DllImport("DynamsoftBarcodeReader")]
        static extern int DBR_DecodeBuffer(IntPtr hBarcode, IntPtr pBufferBytes, int width, int height, int stride, ImagePixelFormat format, string template);

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

        public void DecodeFile(string filename)
        {
            // Read barcodes
            int ret = DBR_DecodeFile(hBarcode, filename, "");
            IntPtr pTextResultArray = IntPtr.Zero;
            DBR_GetAllTextResults(hBarcode, ref pTextResultArray);

            // Print barcode results
            if (pTextResultArray != IntPtr.Zero)
            {
                TextResultArray? results = (TextResultArray?)Marshal.PtrToStructure(pTextResultArray, typeof(TextResultArray));
                if (results != null)
                {
                    int count = results.Value.resultsCount;
                    IntPtr[] barcodes = new IntPtr[count];
                    Marshal.Copy(results.Value.results, barcodes, 0, count);

                    for (int i = 0; i < count; i++)
                    {
                        PTextResult? result = (PTextResult?)Marshal.PtrToStructure(barcodes[i], typeof(PTextResult));
                        if (result != null)
                        {
                            Console.WriteLine("Barcode format: " + result.Value.barcodeFormatString);
                            Console.WriteLine("Barcode text: " + result.Value.barcodeText);
                            Console.WriteLine("-----------------------------");
                        }
                    }
                }

                // Release memory of barcode results
                DBR_FreeTextResults(ref pTextResultArray);
            }
        }
    }
}
