# Command-line .NET 6 Barcode and QR Code Reader
The sample demonstrates how to wrap [Dynamsoft C++ Barcode Reader SDK]((https://www.dynamsoft.com/barcode-reader/sdk-desktop-server/)) to build a **.NET 6** barcode and QR code reading application for **Windows**, **Linux**, and **macOS**.

## SDK Version
[v9.0](https://www.dynamsoft.com/barcode-reader/downloads//#desktop)

## License Activation
Click [here](https://www.dynamsoft.com/customer/license/trialLicense?product=dbr) to get a valid license key.

## Getting Started with .NET 6
* [Windows](https://dotnet.microsoft.com/en-us/download#windowscmd)
* [Linux](https://dotnet.microsoft.com/en-us/download#linuxubuntu)
* [Mac](https://dotnet.microsoft.com/en-us/download#macos)

## Usage
1. Download [Dynamsoft C++ Barcode Reader SDK](https://www.dynamsoft.com/barcode-reader/downloads//#desktop).
2. Copy libraries to the project root directory. Rename Windows `DynamsoftBarcodeReaderx64.dll` to `DynamsoftBarcodeReader.dll`. 
3. Set the license key in `BarcodeReaderManager.cs`:
    
    ```csharp
    DBR_InitLicense("LICENSE-KEY", errorMsg, 512);
    ```
4. Run the application:

    ```bash
    dotnet restore
    dotnet run
    ```    
    
## Blog
[.NET 6 Barcode and QR Code Reader for Windows, Linux & macOS](https://www.codepool.biz/net-core-barcode-app.html)

[0]:http://www.dynamsoft.com/Products/Dynamic-Barcode-Reader.aspx
[1]:https://www.dynamsoft.com/Downloads/Dynamic-Barcode-Reader-Download.aspx

