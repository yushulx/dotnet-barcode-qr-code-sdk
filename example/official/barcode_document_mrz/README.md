# Barcode, Document & MRZ Detection — .NET WinForms Demo

A Windows Forms desktop application built with C# and **.NET 8** that demonstrates real-time and file-based detection of **barcodes/QR codes**, **document edges with normalization**, and **MRZ (Machine Readable Zone)** using the [Dynamsoft Capture Vision Bundle](https://www.nuget.org/packages/Dynamsoft.DotNet.CaptureVision.Bundle/).

https://github.com/yushulx/dotnet-winform-document-barcode-mrz/assets/2202306/1ce1b1f9-df32-4db8-af74-0ce808768572

## Features

| Mode | What it does |
|------|-------------|
| **Barcode / QR Code (DBR)** | Reads 1D/2D barcodes from a camera stream or image file and overlays the result. |
| **MRZ** | Detects and parses Machine Readable Zone data from passports, visas, and ID cards. |
| **Document (DDN)** | Detects document edges, highlights the quadrilateral, and normalizes (deskews/crops) the document. |

## Requirements

- Windows 10/11
- [.NET SDK](https://dotnet.microsoft.com/download/dotnet)
- A webcam (optional — file mode works without one)

## NuGet Packages

```xml
<PackageReference Include="Dynamsoft.DotNet.CaptureVision.Bundle" Version="3.4.1000" />
<PackageReference Include="OpenCvSharp4" Version="4.6.0.20220608" />
<PackageReference Include="OpenCvSharp4.Extensions" Version="4.5.5.20211231" />
<PackageReference Include="OpenCvSharp4.runtime.win" Version="4.6.0.20220608" />
```

## Getting Started

1. Apply for a [free trial license](https://www.dynamsoft.com/customer/license/trialLicense/?product=dcv&package=cross-platform) and replace the license key in `Form1.cs`:

    ```csharp
    string license = "YOUR-LICENSE-KEY";
    int errorCode = LicenseManager.InitLicense(license, out string errorMsg);
    ```

2. Build and run:

    ```bash
    dotnet run
    ```

    Or open `barcode_document_mrz.sln` in Visual Studio 2022 and press **F5**.

   ![dotnet-document-mrz-barcode-recognition](https://github.com/yushulx/dotnet-winform-document-barcode-mrz/assets/2202306/9f393959-95fd-41a7-b8f3-6e365e987613)

## Usage

- Select a detection mode (**Barcode**, **MRZ**, or **Document**) using the radio buttons.
- Click **Camera Scan** to start live detection, or **Load File** to process an image.
- Drag and drop image files onto the file list for batch processing.
- Click **Save** to export the annotated image.

## Blog
[How to Build a Windows Desktop App for Document, Barcode, and MRZ Detection with C# and .NET WinForms](https://www.dynamsoft.com/codepool/dotnet-windows-document-barcode-mrz-detection.html)
