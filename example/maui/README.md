# .NET MAUI Barcode and QR Code Reader
The sample demonstrates how to implement a [.NET MAUI](https://docs.microsoft.com/en-us/dotnet/maui/what-is-maui) app, which supports `Windows` and `Android`, to read barcode and QR code from image files using [Dynamsoft Barcode Reader SDK](https://www.dynamsoft.com/barcode-reader/overview/). 

## Dev Environment
- Visual Studio 2022 for Windows

## Dependencies
- [BarcodeQRCodeSDK](https://www.nuget.org/packages/BarcodeQRCodeSDK/)
- [Barcode.NET.Mobile](https://www.nuget.org/packages/Barcode.NET.Mobile)

## How to Use
1. Clone the repository.
2. Open the `BarcodeQRCode.sln` file in Visual Studio.
3. Apply for a [30-day trial license](https://www.dynamsoft.com/customer/license/trialLicense?product=dbr) and update following code in `ReaderPage.xaml.cs`:
    
    ```cs
    _barcodeQRCodeService.InitSDK("DLS2eyJoYW5kc2hha2VDb2RlIjoiMjAwMDAxLTE2NDk4Mjk3OTI2MzUiLCJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSIsInNlc3Npb25QYXNzd29yZCI6IndTcGR6Vm05WDJrcEQ5YUoifQ==");
    ``` 
4. Select a framework and build the project.

    - Android
        
        ![MAUI Android](https://www.dynamsoft.com/codepool/img/2022/05/maui-android.png)
    
    - Windows
        
        ![MAUI Windows](https://www.dynamsoft.com/codepool/img/2022/05/maui-windows.png)
    
    
        ![MAUI barcode and QR code reader](https://www.dynamsoft.com/codepool/img/2022/05/maui-barcode-qrcode-reader.png)
        
 ## Blog
 [How to Build a .NET MAUI Barcode and QR Code Reader for Windows and Android](https://www.dynamsoft.com/codepool/dotnet-maui-barcode-qr-code-reader.html)
