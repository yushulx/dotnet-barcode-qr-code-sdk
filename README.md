# .NET 6 Barcode and QR Code Reader SDK
The .NET 6 Barcode and QR Code Reader SDK is a C++/CLI wrapper for [Dynamsoft C++ Barcode Reader SDK]((https://www.dynamsoft.com/barcode-reader/sdk-desktop-server/)).

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

## Example
- [command-line](example/command-line)
    
    ```bash
    dotnet restore
    dotnet run
    ```
- [desktop-gui](example/desktop-gui)
    
    ```bash
    dotnet restore
    dotnet run
    ```
    
    ![.NET 6 Desktop barcode and QR code reader](https://camo.githubusercontent.com/0218813daf4f27916d51d7104c56e89c0e3797eea8a6bc4e8b351654912d24ee/68747470733a2f2f7777772e636f6465706f6f6c2e62697a2f77702d636f6e74656e742f75706c6f6164732f323031392f31312f6e65742d636f72652d626172636f64652d7265616465722e706e67)


## Building NuGet Package from Source Code

```bash
dotnet build --configuration Release
```
