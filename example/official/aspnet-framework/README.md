# Web Barcode and QR Code Reader in ASP.NET (.NET Framework 4.8)

This sample demonstrates how to use [Dynamsoft Barcode Reader](https://www.dynamsoft.com/barcode-reader/sdk-desktop-server/) to create a responsive web barcode and QR code reader with **ASP.NET MVC 5** targeting **.NET Framework 4.8**.

https://github.com/user-attachments/assets/70104d27-17a8-4d29-aa10-7cc663b57509

## Features

- **Auto-scan on image load** — drop or select an image and decoding starts immediately; no scan button needed.
- **Canvas overlay** — detected barcodes and QR codes are highlighted directly on the image with color-coded quadrilateral overlays.
- **Results panel** — each barcode's format and decoded text are listed below the image.
- **Responsive UI** — adapts to both desktop and mobile browsers.
- **Drag-and-drop upload** — in addition to the file picker.

## Prerequisites

- [.NET Framework 4.8 Developer Pack](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48)
- Visual Studio 2019 or later (with **ASP.NET and web development** workload)
- NuGet CLI or Visual Studio NuGet Package Manager

## Installation

1. Open `MvcBarcodeQRCodeFramework.csproj` in Visual Studio.
2. Restore NuGet packages (right-click solution → **Restore NuGet Packages**), or run:

    ```bash
    nuget restore MvcBarcodeQRCodeFramework.csproj
    ```

## License Activation

Set your license key in `Controllers/FileController.cs`:

```cs
int errorCode = LicenseManager.InitLicense("LICENSE-KEY", out errorMsg);
```

Get a free 30-day trial key from the [Dynamsoft trial page](https://www.dynamsoft.com/customer/license/trialLicense?product=dbr).

## Usage

1. Open the project in Visual Studio.
2. Set the license key as described above.
3. Build the project:

    ```bash
    nuget restore MvcBarcodeQRCodeFramework.csproj -PackagesDirectory packages
    msbuild MvcBarcodeQRCodeFramework.csproj /p:Configuration=Debug /t:Build
    ```

4. Run with IIS Express:

    ```bash
    "C:\Program Files\IIS Express\iisexpress.exe" /path:"%CD%" /port:62873
    ```

5. Open `http://localhost:62873/` in a browser, select or drag an image containing a barcode or QR code.
6. The app automatically scans, draws overlays on detected barcodes, and lists results below.

## How It Works

### Backend (`Controllers/FileController.cs`)
- Accepts a `multipart/form-data` POST at `/upload` via attribute routing (`[Route("upload")]`).
- Uses `CaptureVisionRouter.Capture()` to decode barcodes.
- Returns JSON: `{ barcodes: [{ text, format, points: [{x,y}×4] }] }` via `return Json(new { barcodes })`.

### Frontend (`Scripts/site.js`)
- On file selection or drop, the image is previewed via `FileReader`.
- The image is automatically POSTed to `/upload` (no button click needed).
- Each barcode's `points` quad is drawn on a `<canvas>` overlay, scaled to the displayed image size.
- Results are listed in a panel below.

## Project Structure

```
aspnet-framework/
├── App_Start/
│   ├── FilterConfig.cs
│   └── RouteConfig.cs          ← enables attribute routing
├── Content/
│   └── Site.css                ← responsive styles
├── Controllers/
│   ├── FileController.cs       ← POST /upload → JSON barcodes
│   └── HomeController.cs
├── Models/
│   └── ErrorViewModel.cs
├── Properties/
│   └── AssemblyInfo.cs
├── Scripts/
│   └── site.js                 ← auto-scan + canvas overlay
├── Views/
│   ├── Home/Index.cshtml
│   ├── Shared/_Layout.cshtml
│   ├── Shared/Error.cshtml
│   ├── _ViewStart.cshtml
│   └── Web.config
├── Global.asax
├── Global.asax.cs
├── MvcBarcodeQRCodeFramework.csproj
├── packages.config
└── Web.config
```

## Blog
[Decode Barcodes and QR Codes from Uploaded Images in ASP.NET MVC 5](https://www.dynamsoft.com/codepool/barcode-qr-code-scanner-web-app-asp-net-mvc.html)
