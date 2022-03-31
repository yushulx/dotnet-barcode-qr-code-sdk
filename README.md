# .NET 6 Barcode and QR Code Reader SDK
The .NET 6 Barcode and QR Code Reader SDK is a C++/CLI wrapper for [Dynamsoft C++ Barcode Reader SDK](https://www.dynamsoft.com/barcode-reader/sdk-desktop-server/).

## SDK Version
[v9.0](https://www.dynamsoft.com/barcode-reader/downloads//#desktop)

## License Activation
Click [here](https://www.dynamsoft.com/customer/license/trialLicense?product=dbr) to get a valid license key.

## Supported Platforms
- Windows (x64)
- Linux (x64)
- macOS (x64)

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

## Download .NET 6 SDK
* [Windows](https://dotnet.microsoft.com/en-us/download#windowscmd)
* [Linux](https://dotnet.microsoft.com/en-us/download#linuxubuntu)
* [Mac](https://dotnet.microsoft.com/en-us/download#macos)

## Methods
- `public static void InitLicense(string license)`
- `public static BarcodeQRCodeReader Create()`
- `public string[]? DecodeFile(string filename)`
- `public string[]? DecodeBuffer(IntPtr pBufferBytes, int width, int height, int stride, ImagePixelFormat format)`
- `public string[]? DecodeBase64(string base64string)`
- `public static string? GetVersionInfo()`
- `public void SetParameters(string parameters)`

## Usage
- Set the license key:
    
    ```csharp
    BarcodeQRCodeReader.InitLicense("LICENSE-KEY"); 
    ```
- Initialize the barcode and QR code reader object:
    
    ```csharp
    BarcodeQRCodeReader? reader = BarcodeQRCodeReader.Create();
    ```
- Decode barcode and QR code from an image file:

    ```csharp
    string[]? results = reader.DecodeFile(filename);
    ```    
- Decode barcode and QR code from a base64 string:
    
    ```csharp
    string[]? results = reader.DecodeBase64(base64string);
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

    string[]? results = reader.DecodeBuffer(bmpData.Scan0, bitmap.Width, bitmap.Height, bmpData.Stride, format);
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

## Quick Start

```csharp
using System;
using System.Runtime.InteropServices;
using Dynamsoft;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            BarcodeQRCodeReader.InitLicense("LICENSE-KEY"); // Get a license key from https://www.dynamsoft.com/customer/license/trialLicense?product=dbr
            BarcodeQRCodeReader? reader = null;
            try {
                reader = BarcodeQRCodeReader.Create();
                Console.WriteLine("Please enter an image file: ");
                string? filename = Console.ReadLine();
                if (filename != null) {
                    string[]? results = reader.DecodeFile(filename);
                    if (results != null) {
                        foreach (string result in results) {
                            Console.WriteLine(result);
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


## Example
- [command-line](https://github.com/yushulx/dotnet-barcode-qr-code-sdk/tree/main/example/command-line)
    
    ```bash
    dotnet restore
    dotnet run
    ```
    
    ![.NET 6 command-line barcode and QR code reader](https://camo.githubusercontent.com/5d949263555d882e444583cd6617b59e1c39f4fe259a89fb9b2164a7d3d79407/68747470733a2f2f7777772e64796e616d736f66742e636f6d2f636f6465706f6f6c2f696d672f323032322f30332f636f6d6d616e642d6c696e652d646f746e65742d626172636f64652d71722d636f64652d7265616465722e706e67)

- [desktop-gui](https://github.com/yushulx/dotnet-barcode-qr-code-sdk/tree/main/example/desktop-gui)
    
    ```bash
    dotnet restore
    dotnet run
    ```
    
    ![.NET 6 Desktop barcode and QR code reader](https://camo.githubusercontent.com/a75baf7c591eb0aad530ccaa0093d26fbf74f358b0c24674783ca42e8f241d6f/68747470733a2f2f7777772e64796e616d736f66742e636f6d2f636f6465706f6f6c2f696d672f323032322f30332f6465736b746f702d646f746e65742d626172636f64652d71722d636f64652d7265616465722e706e67)


## Building NuGet Package from Source Code

```bash
dotnet build --configuration Release
```
