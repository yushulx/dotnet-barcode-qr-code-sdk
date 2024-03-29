# .NET MAUI Barcode QR Code Scanner

The .NET MAUI project is ported from [Xamarin.Forms barcode QR code scanner](https://github.com/yushulx/xamarin-forms-barcode-qrcode-scanner). 

## Usage
1. Import the project to `Visual Studio 2022`.
2. Apply for a [30-day free trial license](https://www.dynamsoft.com/customer/license/trialLicense?product=dbr) and update the following code in `MainPage.xaml.cs`.

    ```csharp
    BarcodeQRCodeReader.InitLicense("DLS2eyJoYW5kc2hha2VDb2RlIjoiMjAwMDAxLTE2NDk4Mjk3OTI2MzUiLCJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSIsInNlc3Npb25QYXNzd29yZCI6IndTcGR6Vm05WDJrcEQ5YUoifQ==");
    ```
    
    To enable live camera scanning on a Windows desktop, the [Dynamsoft JavaScript barcode SDK](https://www.dynamsoft.com/barcode-reader/sdk-javascript/) is utilized. To activate this feature, ensure that you set the license key in the `wwwroot/jsInterop.js` file.
    
    ```js
    Dynamsoft.DBR.BarcodeReader.license = "DLS2eyJoYW5kc2hha2VDb2RlIjoiMjAwMDAxLTE2NDk4Mjk3OTI2MzUiLCJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSIsInNlc3Npb25QYXNzd29yZCI6IndTcGR6Vm05WDJrcEQ5YUoifQ==";
    ```
    
3. Build and run the project. 
    
    **Mobile**

    https://user-images.githubusercontent.com/2202306/238875171-0a4b6f5d-f477-4661-a2aa-06cb929e20fe.mp4
    
    **Desktop**

    https://github.com/yushulx/dotnet-barcode-qr-code-sdk/assets/2202306/ae54ad03-7387-4e87-97cf-7a648e7f7bf7


## Known Issues
- Due to the absence of native runtime libraries that support MacCatalyst, the .NET MAUI MacCatalyst app is currently limited to using web APIs for barcode scanning from image files.
- The live camera scanning cannot work on macOS, because `WKWebView` only supports `getUserMedia()` on **iPhone** and **iPad**.

    ![.NET MAUI blazor barcode scanner software](https://www.dynamsoft.com/codepool/img/2023/04/maui-macos-webview-camera-error.png)
