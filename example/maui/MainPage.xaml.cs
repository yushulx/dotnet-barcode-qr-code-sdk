namespace BarcodeQRCode;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnReaderClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ReaderPage());
    }
}

