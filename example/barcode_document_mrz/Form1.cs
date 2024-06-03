using System.Drawing.Imaging;
using Dynamsoft;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = OpenCvSharp.Point;
using static Dynamsoft.MrzScanner;
using static Dynamsoft.DocumentScanner;
using static Dynamsoft.BarcodeQRCodeReader;
using System.Text.Json.Nodes;
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

using MrzResult = Dynamsoft.MrzScanner.Result;
using DocResult = Dynamsoft.DocumentScanner.Result;
using BarcodeResult = Dynamsoft.BarcodeQRCodeReader.Result;
using System.Diagnostics;

namespace Test
{
    public partial class Form1 : Form
    {
        private MrzScanner mrzScanner;
        private DocumentScanner documentScanner;
        private BarcodeQRCodeReader barcodeScanner;
        private VideoCapture capture;
        private bool isCapturing;
        private Thread? thread;
        private Mat _mat = new Mat();
        private DocResult[]? _docResults;
        private MrzResult[]? _mrzResults;
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

            // Initialize MRZ scanner
            mrzScanner = MrzScanner.Create();

            // Initialize document scanner
            documentScanner = DocumentScanner.Create();
            documentScanner.SetParameters(DocumentScanner.Templates.color);

            // Initialize barcode scanner
            barcodeScanner = BarcodeQRCodeReader.Create();
        }

        // Get a license key from https://www.dynamsoft.com/customer/license/trialLicense
        private void ActivateLicense(string license)
        {
            try
            {
                MrzScanner.InitLicense(license);
                DocumentScanner.InitLicense(license);
                BarcodeQRCodeReader.InitLicense(license);
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
            mrzScanner.Destroy();
            documentScanner.Destroy();
            barcodeScanner.Destroy();
        }

        private void DetectFile(string filename)
        {
            richTextBoxInfo.Text = "";
            try
            {
                _mat = Cv2.ImRead(filename, ImreadModes.Color);
                Mat copy = new Mat(_mat.Rows, _mat.Cols, MatType.CV_8UC3);
                _mat.CopyTo(copy);
                if (checkBoxDdn.Checked)
                {
                    copy = DetectDocument(_mat, copy);
                    pictureBoxSrc.Image = BitmapConverter.ToBitmap(copy);
                    PreviewNormalizedImage();
                }
                else
                {
                    pictureBoxSrc.Image = BitmapConverter.ToBitmap(copy);

                    if (checkBoxDbr.Checked)
                    {
                        copy = DetectBarcode(_mat, copy);
                    }

                    if (checkBoxMrz.Checked)
                    {
                        copy = DetectMrz(_mat, copy);
                    }
                    pictureBoxDest.Image = BitmapConverter.ToBitmap(copy);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Mat DetectMrz(Mat mat, Mat canvas)
        {
            int length = mat.Cols * mat.Rows * mat.ElemSize();
            byte[] bytes = new byte[length];
            Marshal.Copy(mat.Data, bytes, 0, length);
            _mrzResults = mrzScanner.DetectBuffer(bytes, mat.Cols, mat.Rows, (int)mat.Step(), MrzScanner.ImagePixelFormat.IPF_RGB_888);
            if (_mrzResults != null)
            {
                string[] lines = new string[_mrzResults.Length];
                var index = 0;
                foreach (MrzResult result in _mrzResults)
                {
                    lines[index++] = result.Text;
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        richTextBoxInfo.Text += result.Text + Environment.NewLine;
                    });

                    if (result.Points != null)
                    {
                        Point[] points = new Point[4];
                        for (int i = 0; i < 4; i++)
                        {
                            points[i] = new Point(result.Points[i * 2], result.Points[i * 2 + 1]);
                        }
                        Cv2.DrawContours(canvas, new Point[][] { points }, 0, Scalar.Red, 2);
                    }
                }

                JsonNode? info = Parse(lines);
                if (info != null)
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        richTextBoxInfo.Text = info.ToString();
                    });

                }
            }

            return canvas;
        }

        private Mat DetectBarcode(Mat mat, Mat canvas)
        {
            int length = mat.Cols * mat.Rows * mat.ElemSize();
            byte[] bytes = new byte[length];
            Marshal.Copy(mat.Data, bytes, 0, length);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            BarcodeResult[]? results = barcodeScanner.DecodeBuffer(bytes, mat.Cols, mat.Rows, (int)mat.Step(), BarcodeQRCodeReader.ImagePixelFormat.IPF_RGB_888);
            stopwatch.Stop();

            this.BeginInvoke((MethodInvoker)delegate
            {
                String elpasedTime = String.Format("Time elapsed (in ms): {0}", stopwatch.ElapsedMilliseconds);
                richTextBoxInfo.AppendText(elpasedTime);
                richTextBoxInfo.AppendText(Environment.NewLine);
            });

            if (results != null)
            {
                foreach (BarcodeResult result in results)
                {
                    string output = "Text: " + result.Text + Environment.NewLine + "Format: " + result.Format1 + Environment.NewLine;
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        richTextBoxInfo.AppendText(output);
                        richTextBoxInfo.AppendText(Environment.NewLine);
                    });

                    int[]? points = result.Points;
                    if (points != null)
                    {
                        OpenCvSharp.Point[] all = new OpenCvSharp.Point[4];
                        int xMin = points[0], yMax = points[1];
                        all[0] = new OpenCvSharp.Point(xMin, yMax);
                        for (int i = 2; i < 7; i += 2)
                        {
                            int x = points[i];
                            int y = points[i + 1];
                            OpenCvSharp.Point p = new OpenCvSharp.Point(x, y);
                            xMin = x < xMin ? x : xMin;
                            yMax = y > yMax ? y : yMax;
                            all[i / 2] = p;
                        }
                        OpenCvSharp.Point[][] contours = new OpenCvSharp.Point[][] { all };
                        Cv2.DrawContours(canvas, contours, 0, new Scalar(0, 255, 0), 2);
                        if (result.Text != null) Cv2.PutText(canvas, result.Text, new OpenCvSharp.Point(xMin, yMax), HersheyFonts.HersheySimplex, 1, new Scalar(0, 0, 255), 2);
                    }
                }
            }
            // else
            // {
            //     richTextBoxInfo.AppendText("No barcode detected!" + Environment.NewLine);
            // }
            return canvas;
        }

        private Mat DetectDocument(Mat mat, Mat canvas)
        {
            int length = mat.Cols * mat.Rows * mat.ElemSize();
            byte[] bytes = new byte[length];
            Marshal.Copy(mat.Data, bytes, 0, length);

            _docResults = documentScanner.DetectBuffer(bytes, mat.Cols, mat.Rows, (int)mat.Step(), DocumentScanner.ImagePixelFormat.IPF_RGB_888);
            if (_docResults != null)
            {
                DocResult result = _docResults[0];
                if (result.Points != null)
                {
                    Point[] points = new Point[4];
                    for (int i = 0; i < 4; i++)
                    {
                        points[i] = new Point(result.Points[i * 2], result.Points[i * 2 + 1]);
                    }
                    Cv2.DrawContours(canvas, new Point[][] { points }, 0, Scalar.Blue, 2);
                }
            }
            return canvas;
        }

        private void PreviewNormalizedImage()
        {
            if (_docResults != null)
            {
                DocResult result = _docResults[0];
                int length = _mat.Cols * _mat.Rows * _mat.ElemSize();
                byte[] bytes = new byte[length];
                Marshal.Copy(_mat.Data, bytes, 0, length);

                NormalizedImage image = documentScanner.NormalizeBuffer(bytes, _mat.Cols, _mat.Rows, (int)_mat.Step(), DocumentScanner.ImagePixelFormat.IPF_RGB_888, result.Points);
                if (image != null && image.Data != null)
                {
                    Mat newMat;
                    if (image.Stride < image.Width)
                    {
                        // binary
                        byte[] data = image.Binary2Grayscale();
                        newMat = new Mat(image.Height, image.Width, MatType.CV_8UC1, data);
                    }
                    else if (image.Stride >= image.Width * 3)
                    {
                        // color
                        newMat = new Mat(image.Height, image.Width, MatType.CV_8UC3, image.Data);
                    }
                    else
                    {
                        // grayscale
                        newMat = new Mat(image.Height, image.Stride, MatType.CV_8UC1, image.Data);
                    }

                    Mat copy = new Mat(_mat.Rows, _mat.Cols, MatType.CV_8UC3);
                    newMat.CopyTo(copy);

                    if (checkBoxDbr.Checked)
                    {
                        copy = DetectBarcode(newMat, copy);
                    }

                    if (checkBoxMrz.Checked)
                    {
                        copy = DetectMrz(newMat, copy);
                    }
                    pictureBoxDest.Image = BitmapConverter.ToBitmap(copy);
                }
            }
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

                if (checkBoxDdn.Checked)
                {
                    copy = DetectDocument(_mat, copy);
                }

                if (checkBoxDbr.Checked)
                {
                    copy = DetectBarcode(_mat, copy);
                }

                if (checkBoxMrz.Checked)
                {
                    copy = DetectMrz(_mat, copy);
                }

                pictureBoxSrc.Image = BitmapConverter.ToBitmap(copy);
            }

            if (checkBoxDdn.Checked)
            {
                PreviewNormalizedImage();
            }
            else
            {
                pictureBoxDest.Image = pictureBoxSrc.Image;
            }
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

        private void dDNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string template = InputBox("Set DDN Template", "", "");
            if (template != null && template != "")
            {
                documentScanner.SetParameters(template);
            }
        }

        private void dBRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string template = InputBox("Set DBR Template", "", "");
            if (template != null && template != "")
            {
                barcodeScanner.SetParameters(template);
            }
        }
    }
}
