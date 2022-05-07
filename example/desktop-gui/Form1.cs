using System.Drawing.Imaging;
using Dynamsoft;

namespace Test
{
    public partial class Form1 : Form
    {
        private BarcodeQRCodeReader reader;

        public Form1()
        {
            InitializeComponent();
            BarcodeQRCodeReader.InitLicense("DLS2eyJoYW5kc2hha2VDb2RlIjoiMjAwMDAxLTE2NDk4Mjk3OTI2MzUiLCJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSIsInNlc3Npb25QYXNzd29yZCI6IndTcGR6Vm05WDJrcEQ5YUoifQ=="); // Get a license key from https://www.dynamsoft.com/customer/license/trialLicense?product=dbr
            reader = BarcodeQRCodeReader.Create();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
                base.OnFormClosing(e);
                // Code
                reader.Destroy();
        } 

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image files (*.bmp, *.jpg, *.png) | *.bmp; *.jpg; *.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Bitmap? bitmap = null;
                    
                    try
                    {
                        bitmap = new Bitmap(dlg.FileName);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.ToString());
                        return;
                    }

                    pictureBox1.Image = bitmap;
                    textBox1.Clear();

                    this.Invoke((MethodInvoker)delegate
                    {
                        //Create a BitmapData and Lock all pixels to be written 
                        BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        ImageLockMode.ReadWrite, bitmap.PixelFormat);

                        BarcodeQRCodeReader.ImagePixelFormat format = BarcodeQRCodeReader.ImagePixelFormat.IPF_ARGB_8888;

                        switch (bitmap.PixelFormat)
                        {
                            case PixelFormat.Format24bppRgb:
                                format = BarcodeQRCodeReader.ImagePixelFormat.IPF_RGB_888;
                                break;
                            case PixelFormat.Format32bppArgb:
                                format = BarcodeQRCodeReader.ImagePixelFormat.IPF_ARGB_8888;
                                break;
                            case PixelFormat.Format16bppRgb565:
                                format = BarcodeQRCodeReader.ImagePixelFormat.IPF_RGB_565;
                                break;
                            case PixelFormat.Format16bppRgb555:
                                format = BarcodeQRCodeReader.ImagePixelFormat.IPF_RGB_555;
                                break;
                            case PixelFormat.Format8bppIndexed:
                                format = BarcodeQRCodeReader.ImagePixelFormat.IPF_GRAYSCALED;
                                break;
                        }

                        string[]? results = reader.DecodeBuffer(bmpData.Scan0, bitmap.Width, bitmap.Height, bmpData.Stride, format);
                        //Unlock the pixels
                        bitmap.UnlockBits(bmpData);

                        if (results != null)
                        {
                            foreach (string result in results)
                            {
                                textBox1.AppendText(result);
                                textBox1.AppendText(Environment.NewLine);
                            }
                        }
                        else
                        {
                            textBox1.AppendText("No barcode detected!");
                        }
                        
                    });
                }
            }
        }
    }
}
