namespace BarcodeQRCode;

using Dynamsoft;

public partial class ReaderPage : ContentPage
{
    BarcodeQRCodeReader reader;

    public ReaderPage()
    {
        InitializeComponent();

        InitService();
    }

    private async void InitService()
    {
        await Task.Run(() =>
        {
           
            try
            {
                BarcodeQRCodeReader.InitLicense("DLS2eyJoYW5kc2hha2VDb2RlIjoiMjAwMDAxLTE2NDk4Mjk3OTI2MzUiLCJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSIsInNlc3Npb25QYXNzd29yZCI6IndTcGR6Vm05WDJrcEQ5YUoifQ==");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "OK");
            }

            reader = BarcodeQRCodeReader.Create();

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

            String decodingResult = "";
            try
            {

                BarcodeQRCodeReader.Result[]? results = reader.DecodeFile(file.FullPath);
                if (results != null && results.Length > 0)
                {
                    int i = 1;
                    foreach (BarcodeQRCodeReader.Result result in results)
                    {
                        string barcodeFormat = result.Format1;
                        string message = "Barcode " + i + ": " + barcodeFormat + ", " + result.Text;
                        Console.WriteLine(message);
                        decodingResult += message;
                        i++;
                    }
                }
                else
                {
                    decodingResult = "No barcode found.";
                }
            }
            catch (Exception exception)
            {
                decodingResult = exception.Message.ToString();
            }
            ResultLabel.Text = decodingResult;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}