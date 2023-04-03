# Barcode and QR Code Reader SDK
![Barcode and QR Code SDK](https://img.shields.io/nuget/v/BarcodeQRCodeSDK)

The Barcode and QR Code Reader SDK is a wrapper for [Dynamsoft Barcode Reader SDK](https://www.dynamsoft.com/barcode-reader/overview/), supporting .NET and C++ application development for 1D and 2D barcode recognition.

## Supported Platforms
- Windows (x64)
- Linux (x64)
- macOS (x64)
- Android

## License Activation
Click [here](https://www.dynamsoft.com/customer/license/trialLicense?product=dbr) to get a valid license key.

## C++ Development

### Quick Start
```cpp
#include <iostream>
#include <fstream>

#include "DynamsoftBarcodeReader.h"
#include "DynamsoftCommon.h"

using namespace dynamsoft::dbr;

typedef struct BarcodeFormatSet
{
	int barcodeFormatIds;
	int barcodeFormatIds_2;
}BarcodeFormatSet;

unsigned long GetTime()
{
#if defined(_WIN64) || defined(_WIN32)
	return GetTickCount64();
#else
	struct timeval timing;
	gettimeofday(&timing, NULL);
	return timing.tv_sec * 1000 + timing.tv_usec / 1000;
#endif
}

void ToHexString(unsigned char* pSrc, int iLen, char* pDest)
{
	const char HEXCHARS[16] = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

	int i;
	char* ptr = pDest;

	for (i = 0; i < iLen; ++i)
	{
		snprintf(ptr, 4, "%c%c ", HEXCHARS[(pSrc[i] & 0xF0) >> 4], HEXCHARS[(pSrc[i] & 0x0F) >> 0]);
		ptr += 3;
	}
}

void OutputResult(CBarcodeReader& reader, int errorcode, float time)
{
	char* pszTemp = NULL;
	char* pszTemp1 = NULL;
	char* pszTemp2 = NULL;
	int iRet = errorcode;
	pszTemp = (char*)malloc(4096);
	if (iRet != DBR_OK && iRet != DBRERR_MAXICODE_LICENSE_INVALID && iRet != DBRERR_AZTEC_LICENSE_INVALID && iRet != DBRERR_LICENSE_EXPIRED && iRet != DBRERR_QR_LICENSE_INVALID && iRet != DBRERR_GS1_COMPOSITE_LICENSE_INVALID &&
		iRet != DBRERR_1D_LICENSE_INVALID && iRet != DBRERR_PDF417_LICENSE_INVALID && iRet != DBRERR_DATAMATRIX_LICENSE_INVALID && iRet != DBRERR_GS1_DATABAR_LICENSE_INVALID && iRet != DBRERR_PATCHCODE_LICENSE_INVALID &&
		iRet != DBRERR_POSTALCODE_LICENSE_INVALID && iRet != DBRERR_DOTCODE_LICENSE_INVALID && iRet != DBRERR_DPM_LICENSE_INVALID && iRet != DBRERR_IRT_LICENSE_INVALID && iRet != DMERR_NO_LICENSE && iRet != DMERR_TRIAL_LICENSE)
	{
		snprintf(pszTemp, 4096, "Failed to read barcode: %s\r\n", CBarcodeReader::GetErrorString(iRet));
		printf("%s", pszTemp);
		free(pszTemp);
		return;
	}

	TextResultArray* paryResult = NULL;
	reader.GetAllTextResults(&paryResult);

	if (paryResult->resultsCount == 0)
	{
		snprintf(pszTemp, 4096, "No barcode found. Total time spent: %.3f seconds.\r\n", time);
		printf("%s", pszTemp);
		free(pszTemp);
		CBarcodeReader::FreeTextResults(&paryResult);
		return;
	}

	snprintf(pszTemp, 4096, "Total barcode(s) found: %d. Total time spent: %.3f seconds\r\n\r\n", paryResult->resultsCount, time);
	printf("%s", pszTemp);
	for (int iIndex = 0; iIndex < paryResult->resultsCount; iIndex++)
	{
		snprintf(pszTemp, 4096, "Barcode %d:\r\n", iIndex + 1);
		printf("%s", pszTemp);
		snprintf(pszTemp, 4096, "    Type: %s\r\n", paryResult->results[iIndex]->barcodeFormatString);

		printf("%s", pszTemp);
		snprintf(pszTemp, 4096, "    Value: %s\r\n", paryResult->results[iIndex]->barcodeText);
		printf("%s", pszTemp);

		pszTemp1 = (char*)malloc(paryResult->results[iIndex]->barcodeBytesLength * 3 + 1);
		pszTemp2 = (char*)malloc(paryResult->results[iIndex]->barcodeBytesLength * 3 + 100);
		ToHexString(paryResult->results[iIndex]->barcodeBytes, paryResult->results[iIndex]->barcodeBytesLength, pszTemp1);
		snprintf(pszTemp2, paryResult->results[iIndex]->barcodeBytesLength * 3 + 100, "    Hex Data: %s\r\n", pszTemp1);
		printf("%s", pszTemp2);
		free(pszTemp1);
		free(pszTemp2);
	}

	free(pszTemp);
	CBarcodeReader::FreeTextResults(&paryResult);
}

int main(int argc, const char* argv[])
{
	int iIndex = 0;
	int iRet = -1;
	unsigned long ullTimeBegin = 0;
	unsigned long ullTimeEnd = 0;

	char szErrorMsg[256];
	PublicRuntimeSettings runtimeSettings;

	printf("*************************************************\r\n");
	printf("Welcome to Dynamsoft Barcode Reader Demo\r\n");
	printf("*************************************************\r\n");
	printf("Hints: Please input 'Q' or 'q' to quit the application.\r\n");

	iRet = CBarcodeReader::InitLicense("DLS2eyJoYW5kc2hha2VDb2RlIjoiMjAwMDAxLTE2NDk4Mjk3OTI2MzUiLCJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSIsInNlc3Npb25QYXNzd29yZCI6IndTcGR6Vm05WDJrcEQ5YUoifQ==", szErrorMsg, 256);
	if (iRet != DBR_OK)
	{
		printf("InitLicense Failed: %s\n", szErrorMsg);
	}
	CBarcodeReader reader;


	while (1)
	{
		std::string input;
		std::cout << "\r\n>> Step 1: Input your image file's full path:\r\n";
		std::cin >> input;

		if (input._Equal("q") || input._Equal("Q"))
		{
			return true;
		}

		std::ifstream file(input);

		if (!file.good()) {
			std::cout << "Please input a valid path.\r\n" << std::endl;
			continue;
		}

		reader.InitRuntimeSettingsWithString("{\"ImageParameter\":{\"Name\":\"BestCoverage\",\"BarcodeFormatIds\": [\"BF_ALL\"],\"BarcodeFormatIds_2\": [\"BF2_POSTALCODE\", \"BF2_DOTCODE\"] , \"DeblurLevel\":9,\"ExpectedBarcodesCount\":512,\"ScaleDownThreshold\":100000,\"LocalizationModes\":[{\"Mode\":\"LM_CONNECTED_BLOCKS\"},{\"Mode\":\"LM_SCAN_DIRECTLY\"},{\"Mode\":\"LM_STATISTICS\"},{\"Mode\":\"LM_LINES\"},{\"Mode\":\"LM_STATISTICS_MARKS\"}],\"GrayscaleTransformationModes\":[{\"Mode\":\"GTM_ORIGINAL\"},{\"Mode\":\"GTM_INVERTED\"}]}}", CM_OVERWRITE, szErrorMsg, 256);

		reader.GetRuntimeSettings(&runtimeSettings);
		runtimeSettings.barcodeFormatIds = BF_ALL;
		runtimeSettings.barcodeFormatIds_2 = BF2_POSTALCODE | BF2_DOTCODE;
		iRet = reader.UpdateRuntimeSettings(&runtimeSettings, szErrorMsg, 256);
		if (iRet != DBR_OK)
		{
			printf("Error code: %d. Error message: %s\n", iRet, szErrorMsg);
			return -1;
		}

		ullTimeBegin = GetTime();
		iRet = reader.DecodeFile(input.c_str(), "");
		ullTimeEnd = GetTime();
		OutputResult(reader, iRet, (((float)(ullTimeEnd - ullTimeBegin)) / 1000));

	}

	return 0;
}
```

Please refer to [C++ Development](https://www.dynamsoft.com/barcode-reader/docs/server/programming/cplusplus/user-guide.html) for more information.

## .NET Development

### Quick Start

```csharp
using System;
using Dynamsoft;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            BarcodeQRCodeReader.InitLicense("DLS2eyJoYW5kc2hha2VDb2RlIjoiMjAwMDAxLTE2NDk4Mjk3OTI2MzUiLCJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSIsInNlc3Npb25QYXNzd29yZCI6IndTcGR6Vm05WDJrcEQ5YUoifQ=="); // Get a license key from https://www.dynamsoft.com/customer/license/trialLicense?product=dbr
            BarcodeQRCodeReader? reader = null;
            try {
                reader = BarcodeQRCodeReader.Create();
                Console.WriteLine("Please enter an image file: ");
                string? filename = Console.ReadLine();
                if (filename != null) {
                    Result[]? results = reader.DecodeFile(filename);
                    if (results != null) {
                        foreach (Result result in results) {
                            Console.WriteLine(result.Text);
                        }
                    }
                    else {
                        Console.WriteLine("No barcode found.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Destroy();
                }
            }
        }
    }
}
```

### Methods
- `public static void InitLicense(string license)`
- `public static BarcodeQRCodeReader Create()`
- `public Result[]? DecodeFile(string filename)`
- `public Result[]? DecodeBuffer(byte[] buffer, int width, int height, int stride, ImagePixelFormat format)`
- `public Result[]? DecodeBase64(string base64string)`
- `public static string? GetVersionInfo()`
- `public void SetParameters(string parameters)`

### Usage
- Set the license key:
    
    ```csharp
    BarcodeQRCodeReader.InitLicense("DLS2eyJoYW5kc2hha2VDb2RlIjoiMjAwMDAxLTE2NDk4Mjk3OTI2MzUiLCJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSIsInNlc3Npb25QYXNzd29yZCI6IndTcGR6Vm05WDJrcEQ5YUoifQ=="); 
    ```
- Initialize the barcode and QR code reader object:
    
    ```csharp
    BarcodeQRCodeReader? reader = BarcodeQRCodeReader.Create();
    ```
- Decode barcode and QR code from an image file:

    ```csharp
    Result[]? results = reader.DecodeFile(filename);
    ```    
- Decode barcode and QR code from a base64 string:
    
    ```csharp
    Result[]? results = reader.DecodeBase64(base64string);
    ```     
    
- Decode barcode and QR code from a buffer:
    
    ```csharp
    BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
    ImageLockMode.ReadWrite, bitmap.PixelFormat);

    BarcodeQRCodeReader.ImagePixelFormat format = BarcodeQRCodeReader.ImagePixelFormat.IPF_ARGB_8888;

    switch (bitmap.PixelFormat)
    {
        case PixelFormat.Format24bppRgb:
            format = BarcodeQRCodeReader.ImagePixelFormat.IPF_RGB_888;
            break;
        case PixelFormat.Format32bppArgb:
            format = BarcodeQRCodeReader.ImagePixelFormat.IPF_ARGB_8888;
            break;
        case PixelFormat.Format16bppRgb565:
            format = BarcodeQRCodeReader.ImagePixelFormat.IPF_RGB_565;
            break;
        case PixelFormat.Format16bppRgb555:
            format = BarcodeQRCodeReader.ImagePixelFormat.IPF_RGB_555;
            break;
        case PixelFormat.Format8bppIndexed:
            format = BarcodeQRCodeReader.ImagePixelFormat.IPF_GRAYSCALED;
            break;
    }

    int length = bitmap.Height * bmpData.Stride;
    byte[] bytes = new byte[length];
    Marshal.Copy(bmpData.Scan0, bytes, 0, length);

    Result[]? results = reader.DecodeBuffer(bytes, bitmap.Width, bitmap.Height, bmpData.Stride, format);
    bitmap.UnlockBits(bmpData);
    ```
- Get SDK version number:

    ```csharp
    string? version = BarcodeQRCodeReader.GetVersionInfo();
    ```
- Customize parameters:
    
    ```csharp
    // Refer to https://www.dynamsoft.com/barcode-reader/parameters/structure-and-interfaces-of-parameters.html?ver=latest
    reader.SetParameters("{\"Version\":\"3.0\", \"ImageParameter\":{\"Name\":\"IP1\", \"BarcodeFormatIds\":[\"BF_QR_CODE\", \"BF_ONED\"], \"ExpectedBarcodesCount\":20}}");
    ```

## Example
- [command-line](https://github.com/yushulx/dotnet-barcode-qr-code-sdk/tree/main/example/command-line)
    
    ```bash
    dotnet restore
    dotnet run
    ```
    
    ![.NET 6 command-line barcode and QR code reader](https://camo.githubusercontent.com/5d949263555d882e444583cd6617b59e1c39f4fe259a89fb9b2164a7d3d79407/68747470733a2f2f7777772e64796e616d736f66742e636f6d2f636f6465706f6f6c2f696d672f323032322f30332f636f6d6d616e642d6c696e652d646f746e65742d626172636f64652d71722d636f64652d7265616465722e706e67)

- [desktop-gui](https://github.com/yushulx/dotnet-barcode-qr-code-sdk/tree/main/example/desktop-gui) (**Windows Only**)
    
    ```bash
    dotnet restore
    dotnet run
    ```
    
    ![.NET 6 Desktop barcode and QR code reader](https://camo.githubusercontent.com/a75baf7c591eb0aad530ccaa0093d26fbf74f358b0c24674783ca42e8f241d6f/68747470733a2f2f7777772e64796e616d736f66742e636f6d2f636f6465706f6f6c2f696d672f323032322f30332f6465736b746f702d646f746e65742d626172636f64652d71722d636f64652d7265616465722e706e67) 

- [web](https://github.com/yushulx/dotnet-barcode-qr-code-sdk/tree/main/example/web)
  
    ```bash
    dotnet restore
    dotnet run
    ```
    
    ![ASP.NET online barcode and QR code reader](https://camo.githubusercontent.com/cf68ff6cadf563cb52b6d759d262ea44c5922d1bdc7c039027a70386e8763f10/68747470733a2f2f7777772e64796e616d736f66742e636f6d2f636f6465706f6f6c2f696d672f323032322f30342f6173702d6e65742d626172636f64652d71722d636f64652d7363616e2e706e67)
- [Android](https://github.com/yushulx/dotnet-barcode-qr-code-sdk/tree/main/example/android)

    ![MAUI Android barcode reader](https://www.dynamsoft.com/codepool/img/2023/04/maui-barcode-reader.jpg)

## Building NuGet Package from Source Code

```bash
dotnet build --configuration Release
nuget pack .\BarcodeQRCodeSDK.nuspec
```

## Supported Barcode Symbologies
- Linear Barcodes (1D)
  - Code 39 (including Code 39 Extended)
  - Code 93
  - Code 128
  - Codabar
  - Interleaved 2 of 5
  - EAN-8
  - EAN-13
  - UPC-A
  - UPC-E
  - Industrial 2 of 5

- 2D Barcodes
  - QR Code (including Micro QR Code and Model 1)
  - Data Matrix
  - PDF417 (including Micro PDF417)
  - Aztec Code
  - MaxiCode (mode 2-5)
  - DotCode

- Patch Code
- GS1 Composite Code
- GS1 DataBar
  - Omnidirectional,
  - Truncated, Stacked, Stacked
  - Omnidirectional, Limited,
  - Expanded, Expanded Stacked

- Postal Codes
  - USPS Intelligent Mail
  - Postnet
  - Planet
  - Australian Post
  - UK Royal Mail