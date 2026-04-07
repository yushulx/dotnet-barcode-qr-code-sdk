# Web Barcode and QR Code Reader in ASP.NET

This sample demonstrates how to use [Dynamsoft Barcode Reader](https://www.dynamsoft.com/barcode-reader/sdk-desktop-server/) to create a responsive web barcode and QR code reader with **ASP.NET Core** (.NET 8).

## Features

- **Auto-scan on image load** – drop or select an image and decoding starts immediately; no scan button needed.
- **Canvas overlay** – detected barcodes and QR codes are highlighted directly on the image with color-coded quadrilateral overlays.
- **Results panel** – each barcode's format and decoded text are listed below the image.
- **Responsive UI** – adapts to both desktop and mobile browsers.
- **Drag-and-drop upload** – in addition to the file picker.

## Prerequisites
- [.NET SDK 8.0+](https://dotnet.microsoft.com/en-us/download/dotnet/)

## Installation
The project uses the [Dynamsoft.DotNet.BarcodeReader.Bundle](https://www.nuget.org/packages/Dynamsoft.DotNet.BarcodeReader.Bundle) NuGet package:

```bash
dotnet add package Dynamsoft.DotNet.BarcodeReader.Bundle
```

## License Activation
Click [here](https://www.dynamsoft.com/customer/license/trialLicense/?product=dcv&package=cross-platform) to get a valid desktop license key.

## Usage
1. Set the license in `Controllers/FileController.cs`:
  
    ```cs
    int errorCode = LicenseManager.InitLicense("LICENSE-KEY", out errorMsg);
    ```
    
2. Build and run the project:

    ```bash
    dotnet restore
    dotnet run
    ```

3. Open the displayed URL in a browser, select or drag an image containing a barcode or QR code. The app will automatically scan the image, draw overlays on detected barcodes, and list the decoded text below.

## How It Works

### Backend (`Controllers/FileController.cs`)
- Accepts a `multipart/form-data` POST at `/upload`.
- Uses `CaptureVisionRouter.Capture()` to decode barcodes.
- Returns a JSON array: `{ barcodes: [{ text, format, points: [{x,y}×4] }] }`.

### Frontend (`wwwroot/js/site.js`)
- On file selection or drop, the image is previewed instantly via `FileReader`.
- The image is automatically POSTed to `/upload`.
- On response, each barcode's quadrilateral (`points`) is drawn on a `<canvas>` overlay scaled to the displayed image size.
- Results are listed in a panel below.

## Screenshot

![ASP.NET Barcode and QR Code Reader](https://www.dynamsoft.com/codepool/img/2022/04/asp-net-mobile-barcode-qr-code-reader.png)


## Blog
[How to Build a Barcode and QR Code Reader with HTML5 and ASP.NET](https://www.dynamsoft.com/codepool/mobile-barcode-qr-code-reader-html5-asp-net.html)

