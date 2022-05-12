namespace Dynamsoft;

using System.Text;
using System.Runtime.InteropServices;

public class BarcodeQRCodeReader
{
    public class Result
    {
        public string? Text { get; set; }
        public int[]? Points { get; set; }
        public string? Format1 { get; set; }
        public string? Format2 { get; set; }
    }

    private IntPtr hBarcode;
    private static string? licenseKey;

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

    [DllImport("DynamsoftBarcodeReader")]
    static extern int DBR_DecodeBase64String(IntPtr hBarcode, string base64string, string template);

    [DllImport("DynamsoftBarcodeReader")]
    static extern IntPtr DBR_GetVersion();

    [DllImport("DynamsoftBarcodeReader")]
    static extern int DBR_InitRuntimeSettingsWithString(IntPtr barcodeReader, string content, ConflictMode conflictMode, [Out] byte[] errorMsg, int errorMsgSize);

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
    public enum ConflictMode
    {
        /**Ignores new settings and inherits the previous settings. */
        CM_IGNORE = 1,

        /**Overwrites the old settings with new settings. */
        CM_OVERWRITE = 2

    }

    public enum ResultCoordinateType
    {
        /**Returns the coordinate in pixel value. */
        RCT_PIXEL = 0x01,

        /**Returns the coordinate as a percentage. */
        RCT_PERCENTAGE = 0x02
    }

    public enum TerminatePhase
    {
        /**Exits the barcode reading algorithm after the region predetection is done. */
        TP_REGION_PREDETECTED = 0x00000001,

        /**Exits the barcode reading algorithm after the region predetection and image pre-processing is done. */
        TP_IMAGE_PREPROCESSED = 0x00000002,

        /**Exits the barcode reading algorithm after the region predetection, image pre-processing, and image binarization are done. */
        TP_IMAGE_BINARIZED = 0x00000004,

        /**Exits the barcode reading algorithm after the region predetection, image pre-processing, image binarization, and barcode localization are done. */
        TP_BARCODE_LOCALIZED = 0x00000008,

        /**Exits the barcode reading algorithm after the region predetection, image pre-processing, image binarization, barcode localization, and barcode type determining are done. */
        TP_BARCODE_TYPE_DETERMINED = 0x00000010,

        /**Exits the barcode reading algorithm after the region predetection, image pre-processing, image binarization, barcode localization, barcode type determining, and barcode recognition are done. */
        TP_BARCODE_RECOGNIZED = 0x00000020

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PTextResult
    {
        BarcodeFormat emBarcodeFormat;
        public string barcodeFormatString;
        BarcodeFormat_2 barcodeFormat_2;
        public string barcodeFormatString_2;
        public string barcodeText;
        IntPtr barcodeBytes;
        int barcodeBytesLength;
        public IntPtr localizationResult;
        IntPtr detailedResult;
        int resultsCount;
        IntPtr results;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 56)]
        char[] reserved;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct LocalizationResult
    {
        /**The terminate phase of localization result. */
        TerminatePhase terminatePhase;

        /**Barcode type in BarcodeFormat group 1 */
        BarcodeFormat barcodeFormat;

        /**Barcode type in BarcodeFormat group 1 as string */
        public string barcodeFormatString;

        /**Barcode type in BarcodeFormat group 2*/
        BarcodeFormat_2 barcodeFormat_2;

        /**Barcode type in BarcodeFormat group 2 as string */
        public string barcodeFormatString_2;

        /**The X coordinate of the left-most point */
        public int x1;

        /**The Y coordinate of the left-most point */
        public int y1;

        /**The X coordinate of the second point in a clockwise direction */
        public int x2;

        /**The Y coordinate of the second point in a clockwise direction */
        public int y2;

        /**The X coordinate of the third point in a clockwise direction */
        public int x3;

        /**The Y coordinate of the third point in a clockwise direction */
        public int y3;

        /**The X coordinate of the fourth point in a clockwise direction */
        public int x4;

        /**The Y coordinate of the fourth point in a clockwise direction */
        public int y4;

        /**The angle of a barcode. Values range is from 0 to 360. */
        int angle;

        /**The barcode module size (the minimum bar width in pixel) */
        int moduleSize;

        /**The page number the barcode located in. The index is 0-based. */
        int pageNumber;

        /**The region name the barcode located in. */
        public string regionName;

        /**The document name */
        public string documentName;

        /**The coordinate type */
        ResultCoordinateType resultCoordinateType;

        /**The accompanying text content in a byte array */
        IntPtr accompanyingTextBytes;

        /**The length of the accompanying text byte array */
        int accompanyingTextBytesLength;

        /**The confidence of the localization result*/
        int confidence;

        /**Reserved memory for the struct. The length of this array indicates the size of the memory reserved for this struct. */

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 52)]
        char[] reserved;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TextResultArray
    {
        public int resultsCount;
        public IntPtr results;
    }

    public static void InitLicense(string license)
    {
        byte[] errorMsg = new byte[512];
        licenseKey = license;
        DBR_InitLicense(license, errorMsg, 512);
        Console.WriteLine("InitLicense(): " + Encoding.ASCII.GetString(errorMsg));
    }

    private BarcodeQRCodeReader()
    {
        hBarcode = DBR_CreateInstance();
    }

    public static BarcodeQRCodeReader Create()
    {
        if (licenseKey == null)
        {
            throw new Exception("Please call InitLicense first.");
        }
        return new BarcodeQRCodeReader();
    }

    ~BarcodeQRCodeReader()
    {
        if (hBarcode != IntPtr.Zero)
        {
            DBR_DestroyInstance(hBarcode);
            hBarcode = IntPtr.Zero;
        }
    }

    public void Destroy()
    {
        if (hBarcode != IntPtr.Zero)
        {
            DBR_DestroyInstance(hBarcode);
            hBarcode = IntPtr.Zero;
        }
    }

    public static string? GetVersionInfo()
    {
        return Marshal.PtrToStringUTF8(DBR_GetVersion());
    }

    private Result[]? OutputResults()
    {
        IntPtr pTextResultArray = IntPtr.Zero;

        DBR_GetAllTextResults(hBarcode, ref pTextResultArray);

        if (pTextResultArray != IntPtr.Zero)
        {
            Result[]? resultArray = null;
            TextResultArray? results = (TextResultArray?)Marshal.PtrToStructure(pTextResultArray, typeof(TextResultArray));
            if (results != null)
            {
                int count = results.Value.resultsCount;
                if (count > 0)
                {
                    IntPtr[] barcodes = new IntPtr[count];
                    Marshal.Copy(results.Value.results, barcodes, 0, count);
                    resultArray = new Result[count];

                    for (int i = 0; i < count; i++)
                    {
                        PTextResult? result = (PTextResult?)Marshal.PtrToStructure(barcodes[i], typeof(PTextResult));
                        if (result != null)
                        {
                            Result r = new Result();
                            resultArray[i] = r;
                            r.Text = result.Value.barcodeText;
                            r.Format1 = result.Value.barcodeFormatString;
                            r.Format2 = result.Value.barcodeFormatString_2;
                            LocalizationResult? localizationResult = (LocalizationResult?)Marshal.PtrToStructure(result.Value.localizationResult, typeof(LocalizationResult));
                            if (localizationResult != null)
                            {
                                r.Points = new int[8] { localizationResult.Value.x1, localizationResult.Value.y1, localizationResult.Value.x2, localizationResult.Value.y2, localizationResult.Value.x3, localizationResult.Value.y3, localizationResult.Value.x4, localizationResult.Value.y4 };
                            }
                        }
                    }
                }
            }

            // Release memory of barcode results
            DBR_FreeTextResults(ref pTextResultArray);

            return resultArray;
        }

        return null;
    }

    public Result[]? DecodeFile(string filename)
    {
        if (hBarcode == IntPtr.Zero) return null;

        int ret = DBR_DecodeFile(hBarcode, filename, "");
        return OutputResults();
    }

    public Result[]? DecodeBuffer(IntPtr pBufferBytes, int width, int height, int stride, ImagePixelFormat format)
    {
        if (hBarcode == IntPtr.Zero) return null;

        int ret = DBR_DecodeBuffer(hBarcode, pBufferBytes, width, height, stride, format, "");
        return OutputResults();
    }

    public Result[]? DecodeBase64(string base64string)
    {
        if (hBarcode == IntPtr.Zero) return null;

        int ret = DBR_DecodeBase64String(hBarcode, base64string, "");
        return OutputResults();
    }

    public void SetParameters(string parameters)
    {
        if (hBarcode == IntPtr.Zero) return;

        byte[] errorMsg = new byte[512];
        DBR_InitRuntimeSettingsWithString(hBarcode, parameters, ConflictMode.CM_OVERWRITE, errorMsg, 512);
        Console.WriteLine("SetParameters(): " + Encoding.ASCII.GetString(errorMsg) + "\n");
    }
}
