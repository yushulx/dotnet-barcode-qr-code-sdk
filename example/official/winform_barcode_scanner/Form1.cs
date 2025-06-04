using System.Drawing.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = OpenCvSharp.Point;
using System.Text.Json.Nodes;
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;

using Dynamsoft.DBR;
using Dynamsoft.CVR;
using Dynamsoft.License;
using Dynamsoft.Core;

namespace Test
{
    public partial class Form1 : Form
    {
        private CaptureVisionRouter cvr;
        private VideoCapture capture;
        private bool isCapturing;
        private bool isCaptured;
        private Thread? thread;
        private Mat _mat = new Mat();
        private string? _currentFilename = "";

        public Form1()
        {
            string? assemblyPath = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location
            );

            string modelPath = assemblyPath == null ? "" : Path.Join(assemblyPath, "model");

            InitializeComponent();
            FormClosing += new FormClosingEventHandler(Form1_Closing);
            string license = "DLS2eyJoYW5kc2hha2VDb2RlIjoiMjAwMDAxLTE2NDk4Mjk3OTI2MzUiLCJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSIsInNlc3Npb25QYXNzd29yZCI6IndTcGR6Vm05WDJrcEQ5YUoifQ==";

            ActivateLicense(license);

            // Initialize camera
            capture = new VideoCapture(0);
            isCapturing = false;
            isCaptured = false;

            cvr = new CaptureVisionRouter();
        }

        // Get a license key from https://www.dynamsoft.com/customer/license/trialLicense
        private void ActivateLicense(string license)
        {
            try
            {
                string errorMsg;
                int errorCode = LicenseManager.InitLicense(license, out errorMsg);
                if (errorCode != (int)EnumErrorCode.EC_OK)
                    Console.WriteLine("License initialization error: " + errorMsg);
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ex.Message;
                return;
            }


            toolStripStatusLabel1.Text = "License is activated successfully.";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            // Code
            cvr.Dispose();
        }

        private void DetectFile(string filename)
        {
            richTextBoxInfo.Text = "";
            try
            {
                _mat = Cv2.ImRead(filename, ImreadModes.Color);
                Mat copy = new Mat(_mat.Rows, _mat.Cols, MatType.CV_8UC3);
                _mat.CopyTo(copy);
                pictureBoxSrc.Image = BitmapConverter.ToBitmap(copy);

                copy = DetectBarcode(_mat, copy);

                pictureBoxDest.Image = BitmapConverter.ToBitmap(copy);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private Mat DetectBarcode(Mat mat, Mat canvas)
        {
            int length = mat.Cols * mat.Rows * mat.ElemSize();
            byte[] bytes = new byte[length];
            Marshal.Copy(mat.Data, bytes, 0, length);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int width = mat.Cols;
            int height = mat.Rows;
            int stride = (int)mat.Step();

            ImageData data = new ImageData(bytes, width, height, stride, EnumImagePixelFormat.IPF_RGB_888);
            CapturedResult result = cvr.Capture(data, PresetTemplate.PT_READ_BARCODES);
            stopwatch.Stop();

            this.BeginInvoke((MethodInvoker)delegate
            {
                string elpasedTime = string.Format("Time elapsed (in ms): {0}", stopwatch.ElapsedMilliseconds);
                richTextBoxInfo.AppendText(elpasedTime);
                richTextBoxInfo.AppendText(Environment.NewLine);
            });

            DecodedBarcodesResult barcodesResult = result.GetDecodedBarcodesResult();
            if (barcodesResult != null)
            {
                BarcodeResultItem[] items = barcodesResult.GetItems();
                foreach (BarcodeResultItem barcodeItem in items)
                {
                    string output = "Text: " + barcodeItem.GetText() + Environment.NewLine + "Format: " + barcodeItem.GetFormatString() + Environment.NewLine;
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        richTextBoxInfo.AppendText(output);
                        richTextBoxInfo.AppendText(Environment.NewLine);
                    });

                    Dynamsoft.Core.Point[]? points = barcodeItem.GetLocation().points;
                    if (points != null)
                    {
                        OpenCvSharp.Point[] all = new OpenCvSharp.Point[4];
                        int xMin = points[0][0], yMax = points[0][1];
                        all[0] = new OpenCvSharp.Point(xMin, yMax);
                        for (int i = 1; i < 4; i++)
                        {
                            int x = points[i][0];
                            int y = points[i][1];
                            OpenCvSharp.Point p = new OpenCvSharp.Point(x, y);
                            xMin = x < xMin ? x : xMin;
                            yMax = y > yMax ? y : yMax;
                            all[i] = p;
                        }
                        OpenCvSharp.Point[][] contours = new OpenCvSharp.Point[][] { all };
                        Cv2.DrawContours(canvas, contours, 0, new Scalar(0, 255, 0), 2);
                        if (barcodeItem.GetText() != null) Cv2.PutText(canvas, barcodeItem.GetText(), new OpenCvSharp.Point(xMin, yMax), HersheyFonts.HersheySimplex, 1, new Scalar(0, 0, 255), 2);
                    }
                }
            }
            else
            {
                richTextBoxInfo.AppendText("No barcode detected!" + Environment.NewLine);
            }

            return canvas;
        }

        private void buttonFile_Click(object sender, EventArgs e)
        {
            StopScan();
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image files (*.bmp, *.jpg, *.png) | *.bmp; *.jpg; *.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    listBox1.Items.Add(dlg.FileName);
                    _currentFilename = dlg.FileName;
                    DetectFile(dlg.FileName);
                }
            }
        }

        private void buttonCamera_Click(object sender, EventArgs e)
        {
            if (!capture.IsOpened())
            {
                MessageBox.Show("Failed to open video stream or file");
                return;
            }

            if (buttonCamera.Text == "Camera Scan")
            {
                richTextBoxInfo.Text = "";
                StartScan();
            }
            else
            {
                StopScan();
            }
        }

        private void StartScan()
        {
            buttonCamera.Text = "Stop";
            isCapturing = true;
            thread = new Thread(new ThreadStart(FrameCallback));
            thread.Start();
        }

        private void StopScan()
        {
            buttonCamera.Text = "Camera Scan";
            isCapturing = false;
            if (thread != null) thread.Join();
        }

        private void FrameCallback()
        {
            while (isCapturing)
            {
                capture.Read(_mat);
                Mat copy = new Mat(_mat.Rows, _mat.Cols, MatType.CV_8UC3);
                _mat.CopyTo(copy);

                copy = DetectBarcode(_mat, copy);

                pictureBoxSrc.Image = BitmapConverter.ToBitmap(copy);

                if (isCaptured)
                {
                    isCaptured = false;
                    pictureBoxDest.Image = pictureBoxSrc.Image;
                }
            }

            //pictureBoxDest.Image = pictureBoxSrc.Image;
        }

        private void Form1_Closing(object? sender, FormClosingEventArgs e)
        {
            StopScan();
        }

        private void enterLicenseKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string license = InputBox("Enter License Key", "", "");
            if (license != null && license != "")
            {
                ActivateLicense(license);
            }
        }

        public static string InputBox(string title, string promptText, string value)
        {
            Form form = new Form();
            RichTextBox textBox = new RichTextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(60, 72, 80, 30);
            buttonCancel.SetBounds(260, 72, 80, 30);

            form.ClientSize = new System.Drawing.Size(400, 120);
            form.Controls.AddRange(new Control[] { textBox, buttonOk, buttonCancel });
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            return textBox.Text;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentFilename = listBox1.SelectedItem.ToString();
            if (_currentFilename != null && _currentFilename != "")
            {
                DetectFile(_currentFilename);
            }
        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    listBox1.Items.Add(file);
                }
            }
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (isCapturing) return;
            if (_currentFilename != null && _currentFilename != "")
            {
                DetectFile(_currentFilename);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string path = Path.Join(folderBrowserDialog.SelectedPath, DateTime.Now.ToFileTimeUtc() + ".jpg");
                pictureBoxDest.Image.Save(path, ImageFormat.Jpeg);
                MessageBox.Show("Saved to " + folderBrowserDialog.SelectedPath);
            }
        }

        private void dBRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string template = InputBox("Set DBR Template", "", "");
            if (template != null && template != "")
            {
                cvr.InitSettings(template);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isCaptured = true;
        }
    }
}
