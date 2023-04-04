using System;
using ObjCRuntime;

namespace com.Dynamsoft.Dbr
{
	[Native]
	public enum EnumErrorCode : long
	{
		Ok = 0,
		Unknown = -10000,
		No_Memory = -10001,
		Null_Pointer = -10002,
		License_Invalid = -10003,
		License_Expired = -10004,
		File_Not_Found = -10005,
		Filetype_Not_Supported = -10006,
		BPP_Not_Supported = -10007,
		Index_Invalid = -10008,
		Barcode_Format_Invalid = -10009,
		Custom_Region_Invalid = -10010,
		Max_Barcode_Number_Invalid = -10011,
		Image_Read_Failed = -10012,
		TIFF_Read_Failed = -10013,
		QR_License_Invalid = -10016,
		EnumErrorCode_1D_License_Invalid = -10017,
		PDF417_License_Invalid = -10019,
		Datamatrix_License_Invalid = -10020,
		PDF_Read_Failed = -10021,
		PDF_DLL_Missing = -10022,
		Page_Number_Invalid = -10023,
		Custom_Size_Invalid = -10024,
		Custom_Modulesize_Invalid = -10025,
		Recognition_Timeout = -10026,
		Json_Parse_Failed = -10030,
		Json_Type_Invalid = -10031,
		Json_Key_Invalid = -10032,
		Json_Value_Invalid = -10033,
		Json_Name_Key_Missing = -10034,
		Json_Name_Value_Duplicated = -10035,
		Template_Name_Invalid = -10036,
		Json_Name_Reference_Invalid = -10037,
		Parameter_Value_Invalid = -10038,
		Domain_Not_Matched = -10039,
		ReservedInfo_Not_Matched = -10040,
		AZTEC_License_Invalid = -10041,
		License_Dll_Missing = -10042,
		Licensekey_Not_Matched = -10043,
		Requested_Failed = -10044,
		License_Init_Failed = -10045,
		Patchcode_License_Invalid = -10046,
		Postalcode_License_Invalid = -10047,
		DPM_License_Invalid = -10048,
		Frame_Decoding_Thread_Exists = -10049,
		Stop_Decoding_Thread_Failed = -10050,
		Set_Mode_Argument_Error = -10051,
		License_Content_Invalid = -10052,
		License_Key_Invalid = -10053,
		License_Device_Runs_Out = -10054,
		GetModeArgumentError = -10055,
		IrtLicenseInvalid = -10056,
		MaxicodeLicenseInvalid = -10057,
		Gs1DatabarLicenseInvalid = -10058,
		Gs1CompositeLicenseInvalid = -10059,
		PanoramaLicenseInvalid = -10060,
		DotcodeLicenseInvalid = -10061,
		PharmacodeLicenseInvalid = -10062,
		ImageOrientationInvalid = -10063,
		DirectoryInvalid = -10064,
		NoLicense = -20000,
		HandshakeCodeInvalid = -20001,
		LicenseBufferFailed = -20002,
		LicenseSyncFailed = -20003,
		DeviceNotMatch = -20004,
		BindDeviceFailed = -20005,
		LicenseInterfaceConflict = -20006,
		LicenseClientDllMissing = -20007,
		InstanceCountOverLimited = -20008,
		LicenseInitSequenceFailed = -20009,
		TrialLicense = -20010,
		FailedToReachDls = -20200
	}

	[Native]
	public enum DLSErrorCode : long
	{
		DLSErrorCode_CommonError = -20000
	}

	[Flags]
	[Native]
	public enum EnumBarcodeFormat_2 : long
	{
		Null = 0x0,
		Nonstandardbarcode = 0x1,
		PharmacodeOneTrack = 0x4,
		PharmacodeTwoTrack = 0x8,
		Pharmacode = 0xc,
		Dotcode = 0x2,
		Postalcode = 0x1f00000,
		Uspsintelligentmail = 0x100000,
		Postnet = 0x200000,
		Planet = 0x400000,
		Australianpost = 0x800000,
		Rm4scc = 0x1000000,
		All = 0xffffffffL
	}

	[Flags]
	[Native]
	public enum EnumBarcodeFormat : long
	{
		Null = 0x0,
		Code39 = 0x1,
		Code128 = 0x2,
		Code93 = 0x4,
		Codabar = 0x8,
		Itf = 0x10,
		Ean13 = 0x20,
		Ean8 = 0x40,
		Upca = 0x80,
		Upce = 0x100,
		Industrial = 0x200,
		Code39extended = 0x400,
		Msicode = 0x100000,
		Gs1databaromnidirectional = 0x800,
		Gs1databartruncated = 0x1000,
		Gs1databarstacked = 0x2000,
		Gs1databarstackedomnidirectional = 0x4000,
		Gs1databarexpanded = 0x8000,
		Gs1databarexpandedstacked = 0x10000,
		Gs1databarlimited = 0x20000,
		Patchcode = 0x40000,
		Code11 = 0x200000,
		Pdf417 = 0x2000000,
		Qrcode = 0x4000000,
		Datamatrix = 0x8000000,
		Aztec = 0x10000000,
		Maxicode = 0x20000000,
		Microqr = 0x40000000,
		Micropdf417 = 0x80000,
		Gs1composite = -0x80000000L,
		Oned = 0x3007ff,
		Gs1databar = 0x3f800,
		All = -0x1c00001
	}

	[Native]
	public enum EnumImagePixelFormat : long
	{
		Binary = 0,
		BinaryInverted = 1,
		GrayScaled = 2,
		Nv21 = 3,
		Rgb565 = 4,
		Rgb555 = 5,
		Rgb888 = 6,
		Argb8888 = 7,
		Rgb161616 = 8,
		Argb16161616 = 9,
		Abgr8888 = 10,
		Abgr16161616 = 11,
		Bgr888 = 12
	}

	[Native]
	public enum EnumBarcodeComplementMode : long
	{
		Auto = 1,
		General = 2,
		Skip = 0,
		Rev = -2147483648L
	}

	[Native]
	public enum EnumBarcodeColourMode : long
	{
		DarkOnLight = 1,
		LightOnDark = 2,
		DarkOnDark = 4,
		LightOnLight = 8,
		DarkLightMixed = 16,
		DarkOnLightDarkSurrounding = 32,
		Skip = 0,
		Rev = -2147483648L
	}

	[Native]
	public enum EnumBinarizationMode : long
	{
		Auto = 1,
		LocalBlock = 2,
		Threshold = 4,
		Skip = 0,
		Rev = -2147483648L
	}

	[Native]
	public enum EnumColourClusteringMode : long
	{
		Auto = 1,
		GeneralHSV = 2,
		Skip = 0,
		Rev = -2147483648L
	}

	[Native]
	public enum EnumColourConversionMode : long
	{
		General = 1,
		Skip = 0,
		Rev = -2147483648L
	}

	[Native]
	public enum EnumDPMCodeReadingMode : long
	{
		Auto = 1,
		General = 2,
		Skip = 0,
		Rev = -2147483648L
	}

	[Native]
	public enum EnumConflictMode : long
	{
		Ignore = 1,
		Overwrite = 2
	}

	[Native]
	public enum EnumImagePreprocessingMode : long
	{
		Auto = 1,
		General = 2,
		GrayEqualize = 4,
		GraySmooth = 8,
		SharpenSmooth = 16,
		Morphology = 32,
		Skip = 0,
		Rev = -2147483648L
	}

	[Native]
	public enum EnumIntermediateResultType : long
	{
		NoResult = 0,
		OriginalImage = 1,
		ColourClusteredImage = 2,
		ColourConvertedGrayScaleImage = 4,
		TransformedGrayScaleImage = 8,
		PredetectedRegion = 16,
		PreprocessedImage = 32,
		BinarizedImage = 64,
		TextZone = 128,
		Contour = 256,
		LineSegment = 512,
		Form = 1024,
		SegmentationBlock = 2048,
		TypedBarcodeZone = 4096,
		PredetectedQuadrilateral = 8192
	}

	[Native]
	public enum EnumLocalizationMode : long
	{
		Auto = 1,
		ConnectedBlocks = 2,
		Statistics = 4,
		Lines = 8,
		ScanDirectly = 16,
		StatisticsMarks = 32,
		StatisticsPostalCode = 64,
		Centre = 128,
		OneDFastScan = 256,
		Skip = 0,
		Rev = -2147483648L
	}

	[Native]
	public enum EnumQRCodeErrorCorrectionLevel : long
	{
		H = 0,
		L = 1,
		M = 2,
		Q = 3
	}

	[Native]
	public enum EnumRegionPredetectionMode : long
	{
		Auto = 1,
		General = 2,
		GeneralRGBContrast = 4,
		GeneralGrayContrast = 8,
		GeneralHSVContrast = 16,
		Skip = 0,
		Rev = -2147483648L
	}

	[Native]
	public enum EnumDeformationResistingMode : long
	{
		Auto = 1,
		General = 2,
		BroadWrap = 4,
		LocalReference = 8,
		Dewrinkle = 16,
		Skip = 0,
		Rev = -2147483648L
	}

	[Native]
	public enum EnumResultType : long
	{
		StandardText = 0,
		RawText = 1,
		CandidateText = 2,
		PartialText = 3
	}

	[Native]
	public enum EnumTerminatePhase : long
	{
		Predetected = 1,
		Preprocecessed = 2,
		Binarized = 4,
		Localized = 8,
		Determined = 16,
		Recognized = 32
	}

	[Native]
	public enum EnumTextAssistedCorrectionMode : long
	{
		Auto = 1,
		Verifying = 2,
		VerifyingPatching = 4,
		Skip = 0,
		Rev = -2147483648L
	}

	[Native]
	public enum EnumTextFilterMode : long
	{
		Auto = 1,
		GeneralContour = 2,
		Skip = 0,
		Rev = -2147483648L
	}

	[Native]
	public enum EnumTextResultOrderMode : long
	{
		Confidence = 1,
		Position = 2,
		Format = 4,
		Skip = 0,
		Rev = -2147483648L
	}

	[Native]
	public enum EnumTextureDetectionMode : long
	{
		Auto = 1,
		GeneralWidthConcentration = 2,
		Skip = 0,
		Rev = -2147483648L
	}

	[Native]
	public enum EnumGrayscaleTransformationMode : long
	{
		Inverted = 1,
		Original = 2,
		Skip = 0,
		Rev = -2147483648L
	}

	[Native]
	public enum EnumResultCoordinateType : long
	{
		ixel = 1,
		ercentage = 2
	}

	[Native]
	public enum EnumIMResultDataType : long
	{
		Image = 1,
		Contour = 2,
		LineSegment = 4,
		LocalizationResult = 8,
		RegionOfInterest = 16,
		Quadrilateral = 32,
		Reference = 64
	}

	[Native]
	public enum EnumIntermediateResultSavingMode : long
	{
		Memory = 1,
		FileSystem = 2,
		Both = 4,
		ReferenceMemory = 8
	}

	[Native]
	public enum EnumAccompanyingTextRecognitionMode : long
	{
		General = 1,
		Skip = 0,
		Rev = -2147483648L
	}

	[Native]
	public enum EnumScaleUpMode : long
	{
		Auto = 1,
		LinearInterpolation = 2,
		NearestNeighbourInterpolation = 4,
		Skip = 0,
		Rev = -2147483648L
	}

	[Native]
	public enum EnumPDFReadingMode : long
	{
		Auto = 1,
		Vector = 2,
		Raster = 4,
		Rev = -2147483648L
	}

	[Native]
	public enum EnumDeblurMode : long
	{
		DirectBinarization = 1,
		ThresholdBinarization = 2,
		GrayEqualization = 4,
		Smoothing = 8,
		Morphing = 16,
		DeepAnalysis = 32,
		Sharpening = 64,
		BasedOnLocBin = 128,
		SharpeningSmoothing = 256,
		Rev = -2147483648L,
		Skip = 0
	}

	[Native]
	public enum EnumDMUUIDGenerationMethod : long
	{
		Random = 1,
		Hardware = 2
	}

	[Native]
	public enum EnumDMLicenseModule : long
	{
		Oned = 1,
		Qrcode = 2,
		Pdf417 = 3,
		Datamatrix = 4,
		Aztec = 5,
		Maxicode = 6,
		PatchCode = 7,
		Gs1databar = 8,
		Gs1composite = 9,
		Postalcode = 10,
		Dotcode = 11,
		Intermediateresult = 12,
		Dpm = 13,
		Nonstandardbarcode = 16
	}

	[Native]
	public enum EnumDMChargeWay : long
	{
		Auto = 0,
		DeviceCount = 1,
		ScanCount = 2,
		ConcurrentDeviceCount = 3,
		AppDomainCount = 6,
		ActiveDeviceCount = 8,
		InstanceCount = 9,
		ConcurrentInstanceCount = 10
	}

	[Native]
	public enum EnumProduct : long
	{
		Dbr = 1,
		Dlr = 2,
		Dwt = 4,
		Dce = 8,
		Dps = 16,
		All = 65535
	}

	[Native]
	public enum EnumPresetTemplate : long
	{
		Default = 0,
		VideoSingleBarcode = 1,
		VideoSpeedFirst = 2,
		VideoReadRateFirst = 3,
		ImageSpeedFirst = 4,
		ImageReadRateFirst = 5,
		ImageDefault = 6
	}

	[Native]
	public enum EnumLogMode : long
	{
		Off = 0,
		Image = 1,
		Text = 2
	}


}
