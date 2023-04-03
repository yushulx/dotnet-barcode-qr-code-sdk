namespace BarcodeQRCode;

using BarcodeQRCode.Services;

public partial class ReaderPage : ContentPage
{
    BarcodeQRCodeService _barcodeQRCodeService;

    public ReaderPage()
    {
        InitializeComponent();

        InitService();
    }

    private async void InitService()
    {
        await Task.Run(() =>
        {
            _barcodeQRCodeService = new BarcodeQRCodeService();

            try
            {
                _barcodeQRCodeService.InitSDK("DLS2eyJoYW5kc2hha2VDb2RlIjoiMjAwMDAxLTE2NDk4Mjk3OTI2MzUiLCJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSIsInNlc3Npb25QYXNzd29yZCI6IndTcGR6Vm05WDJrcEQ5YUoifQ==");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "OK");
            }

            return Task.CompletedTask;
        });
    }

    private async void OnFilePickerClicked(object sender, EventArgs e)
    {
        FileResult file;
        try
        {
            file = await FilePicker.PickAsync(PickOptions.Images);

            if (file == null) return;

            FileLabel.Text = $"File picked: {file.FileName}";

            Image.Source = ImageSource.FromFile(file.FullPath);

            var result = _barcodeQRCodeService.DecodeFile(file.FullPath);
            ResultLabel.Text = result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}