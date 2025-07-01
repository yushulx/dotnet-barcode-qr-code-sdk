using Dynamsoft.Core;
using Dynamsoft.CVR;
using Dynamsoft.DBR;
using Dynamsoft.License;
using System.Drawing.Imaging;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;
using ZXing.Windows.Compatibility;

namespace BarcodeGenerator
{
    public partial class Form1 : Form
    {
        private TabControl tabControl;
        private TabPage generateTab;
        private TabPage readTab;
        private TabPage settingsTab;

        private TextBox inputTextBox;
        private ComboBox formatComboBox;
        private PictureBox barcodePictureBox;
        private Button generateButton;
        private Button saveButton;

        private Button uploadImageButton;
        private PictureBox inputImageBox;
        private TextBox resultTextBox;
        private Button decodeButton;

        private ComboBox sdkSelectorComboBox;


        public Form1()
        {
            InitializeComponent();

            MinimumSize = new Size(900, 900);

            Text = "Barcode Generator and Reader (ZXing.NET & Dynamsoft Barcode Reader)";
            Width = 800;
            Height = 600;

            tabControl = new TabControl { Dock = DockStyle.Fill };
            generateTab = new TabPage("Generate");
            readTab = new TabPage("Read");
            settingsTab = new TabPage("Settings");

            InitGenerateTab();
            InitReadTab();
            InitSettingsTab();
            InitSDK();

            tabControl.TabPages.Add(generateTab);
            tabControl.TabPages.Add(readTab);
            tabControl.TabPages.Add(settingsTab);

            Controls.Add(tabControl);
        }

        private void InitSDK()
        {
            // Initialize Dynamsoft Barcode Reader
            string errorMsg;
            int errorCode = LicenseManager.InitLicense("DLS2eyJoYW5kc2hha2VDb2RlIjoiMjAwMDAxLTE2NDk4Mjk3OTI2MzUiLCJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSIsInNlc3Npb25QYXNzd29yZCI6IndTcGR6Vm05WDJrcEQ5YUoifQ==", out errorMsg);
            if (errorCode != (int)EnumErrorCode.EC_OK)
                Console.WriteLine("License initialization error: " + errorMsg);
        }

        private void InitGenerateTab()
        {
            Label inputLabel = new Label { Text = "Text to Encode:", Top = 20, Left = 20 };
            inputTextBox = new TextBox { Top = 45, Left = 20, Width = 300 };

            Label formatLabel = new Label { Text = "Format:", Top = 80, Left = 20 };
            formatComboBox = new ComboBox { Top = 105, Left = 20, Width = 200 };
            formatComboBox.Items.AddRange(Enum.GetNames(typeof(BarcodeFormat)));
            formatComboBox.SelectedIndex = 0;

            generateButton = new Button { Text = "Generate", Top = 145, Left = 20 };
            generateButton.Click += GenerateButton_Click;

            saveButton = new Button { Text = "Save", Top = 145, Left = 120 };
            saveButton.Click += SaveButton_Click;

            barcodePictureBox = new PictureBox
            {
                Top = 200,
                Left = 20,
                Width = 600,
                Height = 400,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };

            generateTab.Controls.AddRange(new Control[]
            {
                inputLabel, inputTextBox, formatLabel, formatComboBox,
                generateButton, saveButton, barcodePictureBox
            });

        }

        private void InitReadTab()
        {
            uploadImageButton = new Button
            {
                Text = "Upload Image",
                Top = 20,
                Left = 20,
                Width = 120
            };
            uploadImageButton.Click += UploadImageButton_Click;

            decodeButton = new Button
            {
                Text = "Decode",
                Top = 20,
                Left = 160,
                Width = 100
            };
            decodeButton.Click += DecodeButton_Click;

            inputImageBox = new PictureBox
            {
                Top = 60,
                Left = 20,
                Width = 600,
                Height = 400,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };

            resultTextBox = new TextBox
            {
                Top = inputImageBox.Bottom + 20,
                Left = 20,
                Width = 600,
                Height = 100,
                Multiline = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                ScrollBars = ScrollBars.Vertical
            };

            readTab.Controls.AddRange(new Control[]
            {
                uploadImageButton,
                decodeButton,
                inputImageBox,
                resultTextBox
            });
        }

        private void InitSettingsTab()
        {
            Label sdkLabel = new Label { Text = "Barcode SDK", Top = 20, Left = 20 };
            sdkSelectorComboBox = new ComboBox
            {
                Top = 45,
                Left = 20,
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            sdkSelectorComboBox.Items.AddRange(new string[] { "ZXing", "Dynamsoft Barcode Reader" });
            sdkSelectorComboBox.SelectedIndex = 0;

            settingsTab.Controls.AddRange(new Control[] { sdkLabel, sdkSelectorComboBox });
        }


        private void GenerateButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(inputTextBox.Text))
            {
                inputTextBox.Text = GenerateRandomContent((BarcodeFormat)Enum.Parse(typeof(BarcodeFormat), formatComboBox.SelectedItem.ToString()));
            }

            var writer = new BarcodeWriterPixelData
            {
                Format = (BarcodeFormat)Enum.Parse(typeof(BarcodeFormat), formatComboBox.SelectedItem.ToString()),
                Options = new EncodingOptions
                {
                    Height = 400,
                    Width = 450,
                    Margin = 10
                }
            };

            PixelData pixelData;
            try
            {
                pixelData = writer.Write(inputTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating barcode: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.WriteOnly,
                bitmap.PixelFormat);

            try
            {
                System.Runtime.InteropServices.Marshal.Copy(
                    pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }

            barcodePictureBox.Image = bitmap;
            inputImageBox.Image = bitmap;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (barcodePictureBox.Image == null)
            {
                MessageBox.Show("Please generate a barcode first.", "No Image", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp",
                Title = "Save Barcode Image",
                FileName = "barcode"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var format = ImageFormat.Png;
                switch (Path.GetExtension(dialog.FileName).ToLower())
                {
                    case ".jpg": format = ImageFormat.Jpeg; break;
                    case ".bmp": format = ImageFormat.Bmp; break;
                }

                barcodePictureBox.Image.Save(dialog.FileName, format);
                MessageBox.Show("Image saved successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void UploadImageButton_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                inputImageBox.Image = Image.FromFile(dialog.FileName);
            }
        }

        private void DecodeButton_Click(object sender, EventArgs e)
        {
            if (inputImageBox.Image is not Bitmap bitmap) return;

            string selectedSdk = sdkSelectorComboBox.SelectedItem.ToString();
            string content = "";

            if (selectedSdk == "ZXing")
            {
                content += "ZXing Barcode Reader\r\n\r\n";

                var reader = new BarcodeReader();

                try
                {
                    var result = reader.DecodeMultiple(bitmap);

                    content += "Total Barcodes Found: " + (result?.Length ?? 0) + Environment.NewLine;

                    foreach (var item in result)
                    {
                        content += " Type: " + item.BarcodeFormat + ", " + "Content: " + item.Text + Environment.NewLine;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error decoding barcode: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (selectedSdk == "Dynamsoft Barcode Reader")
            {
                content += "Dynamsoft Barcode Reader\r\n\r\n";

                CaptureVisionRouter cvr = new CaptureVisionRouter();

                byte[] bytes;
                int width;
                int height;
                int stride;
                PixelFormat pixelFormat;
                GetBitmapData(bitmap, out bytes, out width, out height, out stride, out pixelFormat);

                EnumImagePixelFormat format = EnumImagePixelFormat.IPF_RGB_888;
                switch (pixelFormat)
                {
                    case PixelFormat.Format24bppRgb:
                        format = EnumImagePixelFormat.IPF_RGB_888;
                        break;
                    case PixelFormat.Format32bppArgb:
                    case PixelFormat.Format32bppRgb:
                        format = EnumImagePixelFormat.IPF_ARGB_8888;
                        break;
                    default:
                        MessageBox.Show("Unsupported pixel format.");
                        return;
                }


                ImageData data = new ImageData(bytes, width, height, stride, format);
                CapturedResult result = cvr.Capture(data, PresetTemplate.PT_READ_BARCODES);

                if (result != null && result.GetErrorCode() == 0)
                {
                    DecodedBarcodesResult barcodesResult = result.GetDecodedBarcodesResult();
                    if (barcodesResult != null)
                    {
                        BarcodeResultItem[] items = barcodesResult.GetItems();
                        content += "Total Barcodes Found: " + items.Length + Environment.NewLine;
                        foreach (BarcodeResultItem barcodeItem in items)
                        {
                            content += " Type: " + barcodeItem.GetFormatString() + ", " + "Content: " + barcodeItem.GetText() + Environment.NewLine;
                        }
                    }
                }
            }

            resultTextBox.Text = content != "" ? content : "No barcode found.";
        }

        private void GetBitmapData(Bitmap bitmap, out byte[] bytes, out int width, out int height, out int stride, out PixelFormat pixelFormat)
        {
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);

            IntPtr ptr = bmpData.Scan0;

            int bytesCount = Math.Abs(bmpData.Stride) * bitmap.Height;
            bytes = new byte[bytesCount];

            System.Runtime.InteropServices.Marshal.Copy(ptr, bytes, 0, bytesCount);

            width = bitmap.Width;
            height = bitmap.Height;
            stride = bmpData.Stride;
            pixelFormat = bitmap.PixelFormat;

            bitmap.UnlockBits(bmpData);
        }

        private string GenerateRandomContent(BarcodeFormat format)
        {
            Random rnd = new Random();

            switch (format)
            {
                case BarcodeFormat.EAN_13:
                    int part1 = rnd.Next(10, 99);
                    int part2 = rnd.Next(100000000, 999999999);
                    int part3 = rnd.Next(0, 9);
                    return $"{part1}{part2}{part3}";
                case BarcodeFormat.EAN_8:
                    return rnd.Next(1000000, 9999999).ToString();
                case BarcodeFormat.CODE_39:
                case BarcodeFormat.CODE_128:
                case BarcodeFormat.CODABAR:
                    return "ZX" + rnd.Next(100000, 999999);
                case BarcodeFormat.QR_CODE:
                    return "https://www.dynamsoft.com?id=" + rnd.Next(1000, 9999);
                case BarcodeFormat.UPC_A:
                    return rnd.NextInt64(10000000000L, 99999999999L).ToString();
                case BarcodeFormat.ITF:
                    return rnd.Next(100000000, 999999999).ToString();
                default:
                    return "TEST" + rnd.Next(1000, 9999);
            }
        }

    }
}
