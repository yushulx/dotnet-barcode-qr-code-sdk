# .NET Barcode SDK: Generator and Reader

A Windows Forms application for generating and reading barcodes using [ZXing.NET](https://www.nuget.org/packages/ZXing.Net) and [Dynamsoft Barcode Reader](https://www.nuget.org/packages/Dynamsoft.DotNet.BarcodeReader.Bundle).

## Features

- Generate barcodes in various formats:
  - QR Code
  - EAN-13/EAN-8
  - CODE-39/CODE-128
  - UPC-A
  - ITF
  - And more
- Read barcodes from images
- Save generated barcodes as PNG, JPEG, or BMP
- Switch between different barcode SDKs (ZXing and Dynamsoft)

## Requirements

- .NET 8.0
- Windows OS
- [License Key for Dynamsoft Barcode Reader](https://www.dynamsoft.com/customer/license/trialLicense/?product=dcv&package=cross-platform)

## Installation

1. Clone this repository
2. Open the solution in Visual Studio
3. Build and run the project

## Usage

### Generate Barcodes
1. Select the barcode format from the dropdown
2. Enter the text to encode or leave it blank to generate a random text
3. Click "Generate" to create the barcode
4. Use "Save" button to export the barcode image

### Read Barcodes
1. Click "Upload Image" to select an image containing barcodes
2. Click "Decode" to scan for barcodes
3. View the decoded results in the text box

### Settings
- Switch between ZXing and Dynamsoft Barcode Reader SDKs

