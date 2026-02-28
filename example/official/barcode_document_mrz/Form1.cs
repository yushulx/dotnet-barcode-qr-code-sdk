using System.Drawing.Imaging;
using Dynamsoft.CVR;
using Dynamsoft.Core;
using Dynamsoft.DBR;
using Dynamsoft.DDN;
using Dynamsoft.DCP;
using Dynamsoft.DLR;
using Dynamsoft.License;
using Dynamsoft.Utility;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = OpenCvSharp.Point;
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;

namespace Test
{
    public partial class Form1 : Form
    {
        private CaptureVisionRouter cvRouter;
        private VideoCapture capture;
        private bool isCapturing;
        private Thread? thread;
        private Mat _mat = new Mat();
        private string? _currentFilename = "";

        // Detection mode enum
        private enum DetectionMode
        {
            DBR,  // Barcode
            MRZ,  // MRZ
            DDN   // Document
        }
        private DetectionMode currentMode = DetectionMode.DBR;

        public Form1()
        {
            InitializeComponent();
            FormClosing += new FormClosingEventHandler(Form1_Closing);

            // Initialize license
            string license = "DLS2eyJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSJ9";
            int errorCode = LicenseManager.InitLicense(license, out string errorMsg);
            if (errorCode != (int)EnumErrorCode.EC_OK && errorCode != (int)EnumErrorCode.EC_LICENSE_WARNING)
            {
                toolStripStatusLabel1.Text = $"License error: {errorMsg}";
            }
            else
            {
                toolStripStatusLabel1.Text = "License activated successfully.";
            }

            // Initialize CaptureVisionRouter
            cvRouter = new CaptureVisionRouter();

            // Initialize camera
            capture = new VideoCapture(0);
            isCapturing = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            cvRouter?.Dispose();
        }

        private byte[] GetJpegBytes(Mat mat)
        {
            // Encode Mat to JPEG bytes for Capture method
            Cv2.ImEncode(".jpg", mat, out byte[] jpegBytes);
            return jpegBytes;
        }

        private void DetectFile(string filename)
        {
            richTextBoxInfo.Text = "";
            try
            {
                _mat = Cv2.ImRead(filename, ImreadModes.Color);
                ProcessFile(filename, _mat);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ProcessFile(string filename, Mat mat)
        {
            Mat canvas = new Mat();
            mat.CopyTo(canvas);

            string template = GetTemplate();
            CapturedResult[] results = cvRouter.CaptureMultiPages(filename, template);

            // For single image, we use the first result
            if (results != null && results.Length > 0)
            {
                CapturedResult result = results[0];
                ProcessResult(result, mat, canvas, true);
            }

            pictureBoxSrc.Image = BitmapConverter.ToBitmap(canvas);
        }

        private void ProcessResult(CapturedResult result, Mat originalMat, Mat canvas, bool isFileMode)
        {
            if (result.GetErrorCode() != (int)EnumErrorCode.EC_OK && 
                result.GetErrorCode() != (int)EnumErrorCode.EC_UNSUPPORTED_JSON_KEY_WARNING)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    richTextBoxInfo.Text = $"Error: {result.GetErrorCode()}, {result.GetErrorString()}";
                });
                return;
            }

            switch (currentMode)
            {
                case DetectionMode.DBR:
                    ProcessBarcodeResult(result, canvas, isFileMode);
                    break;
                case DetectionMode.MRZ:
                    ProcessMrzResult(result, canvas, isFileMode);
                    break;
                case DetectionMode.DDN:
                    ProcessDocumentResult(result, originalMat, canvas, isFileMode);
                    break;
            }
        }

        private void DrawQuadrilateral(Mat canvas, Quadrilateral? quad, Scalar color, string label = "")
        {
            if (quad == null) return;
            try
            {
                var pts = quad.points;
                if (pts != null && pts.Length >= 4)
                {
                    Point[] contourPoints = new Point[4];
                    for (int i = 0; i < 4; i++)
                    {
                        contourPoints[i] = new Point((int)pts[i][0], (int)pts[i][1]);
                    }
                    Cv2.DrawContours(canvas, new Point[][] { contourPoints }, 0, color, 2);
                    
                    if (!string.IsNullOrEmpty(label))
                    {
                        int minX = Math.Min(Math.Min(contourPoints[0].X, contourPoints[1].X), 
                                          Math.Min(contourPoints[2].X, contourPoints[3].X));
                        int minY = Math.Min(Math.Min(contourPoints[0].Y, contourPoints[1].Y), 
                                          Math.Min(contourPoints[2].Y, contourPoints[3].Y));
                        Cv2.PutText(canvas, label, new Point(minX, minY - 5),
                            HersheyFonts.HersheySimplex, 0.7, color, 2);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DrawQuadrilateral error: {ex.Message}");
            }
        }

        private void ProcessBarcodeResult(CapturedResult result, Mat canvas, bool isFileMode)
        {
            DecodedBarcodesResult? barcodesResult = result.GetDecodedBarcodesResult();
            BarcodeResultItem[]? items = barcodesResult?.GetItems();

            if (items == null || items.Length == 0)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    richTextBoxInfo.Text = "No barcode detected.";
                });
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Detected {items.Length} barcode(s):\n");

            foreach (BarcodeResultItem item in items)
            {
                string text = item.GetText();
                string format = item.GetFormatString();

                sb.AppendLine($"Format: {format}");
                sb.AppendLine($"Text: {text}\n");

                // Draw barcode overlay
                DrawQuadrilateral(canvas, item.GetLocation(), new Scalar(0, 255, 0), text);
            }

            this.BeginInvoke((MethodInvoker)delegate
            {
                richTextBoxInfo.Text = sb.ToString();
            });
        }

        private void ProcessMrzResult(CapturedResult result, Mat canvas, bool isFileMode)
        {
            // Draw MRZ text-line location quads on the canvas
            RecognizedTextLinesResult? textLinesResult = result.GetRecognizedTextLinesResult();
            if (textLinesResult != null)
            {
                TextLineResultItem[] textLineItems = textLinesResult.GetItems();
                if (textLineItems != null)
                {
                    foreach (var textItem in textLineItems)
                    {
                        DrawQuadrilateral(canvas, textItem.GetLocation(), new Scalar(0, 255, 0));
                    }
                }
            }

            ParsedResult? parsedResult = result.GetParsedResult();
            ParsedResultItem[]? items = parsedResult?.GetItems();

            if (items == null || items.Length == 0)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    richTextBoxInfo.Text = "No MRZ detected.";
                });
                return;
            }

            StringBuilder sb = new StringBuilder();

            foreach (ParsedResultItem item in items)
            {
                string docType = item.GetCodeType();

                sb.AppendLine($"Document Type: {docType}");

                // Get raw text lines
                string? line1 = item.GetFieldValue("line1");
                string? line2 = item.GetFieldValue("line2");
                string? line3 = item.GetFieldValue("line3");

                sb.AppendLine("\nRaw Text:");
                if (line1 != null) sb.AppendLine($"  Line 1: {line1}");
                if (line2 != null) sb.AppendLine($"  Line 2: {line2}");
                if (line3 != null) sb.AppendLine($"  Line 3: {line3}");

                // Get parsed information
                string? docId = docType == "MRTD_TD3_PASSPORT" ? 
                    item.GetFieldValue("passportNumber") : item.GetFieldValue("documentNumber");
                string? surname = item.GetFieldValue("primaryIdentifier");
                string? givenname = item.GetFieldValue("secondaryIdentifier");
                string? nationality = item.GetFieldValue("nationality");
                string? issuer = item.GetFieldValue("issuingState");
                string? dob = item.GetFieldValue("dateOfBirth");
                string? expiry = item.GetFieldValue("dateOfExpiry");
                string? gender = item.GetFieldValue("sex");

                sb.AppendLine("\nParsed Information:");
                if (docId != null) sb.AppendLine($"  Document ID: {docId}");
                if (surname != null) sb.AppendLine($"  Surname: {surname}");
                if (givenname != null) sb.AppendLine($"  Given Name: {givenname}");
                if (nationality != null) sb.AppendLine($"  Nationality: {nationality}");
                if (issuer != null) sb.AppendLine($"  Issuing Country: {issuer}");
                if (gender != null) sb.AppendLine($"  Gender: {gender}");
                if (dob != null) sb.AppendLine($"  Date of Birth: {dob}");
                if (expiry != null) sb.AppendLine($"  Expiration Date: {expiry}");
            }

            this.BeginInvoke((MethodInvoker)delegate
            {
                richTextBoxInfo.Text = sb.ToString();
            });
        }

        private void ProcessDocumentResult(CapturedResult result, Mat originalMat, Mat canvas, bool isFileMode)
        {
            ProcessedDocumentResult? docResult = result.GetProcessedDocumentResult();

            // Draw detected document-edge quads on the canvas
            DetectedQuadResultItem[]? detectedItems = docResult?.GetDetectedQuadResultItems();
            if (detectedItems != null)
            {
                foreach (var quadItem in detectedItems)
                {
                    DrawQuadrilateral(canvas, quadItem.GetLocation(), new Scalar(0, 0, 255), "Document");
                }
            }

            EnhancedImageResultItem[]? enhancedItems = docResult?.GetEnhancedImageResultItems();

            StringBuilder sb = new StringBuilder();

            if ((detectedItems == null || detectedItems.Length == 0) &&
                (enhancedItems == null || enhancedItems.Length == 0))
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    richTextBoxInfo.Text = "No document detected.";
                });
                return;
            }

            if (detectedItems != null && detectedItems.Length > 0)
            {
                sb.AppendLine($"Detected {detectedItems.Length} document edge(s).");
            }

            if (enhancedItems != null && enhancedItems.Length > 0)
            {
                sb.AppendLine($"Normalized {enhancedItems.Length} document(s):");
                for (int i = 0; i < enhancedItems.Length; i++)
                {
                    ImageData? imageData = enhancedItems[i].GetImageData();
                    if (imageData != null)
                    {
                        sb.AppendLine($"  Document {i + 1}: {imageData.GetWidth()}x{imageData.GetHeight()}");
                    }
                }
            }

            this.BeginInvoke((MethodInvoker)delegate
            {
                richTextBoxInfo.Text = sb.ToString();
            });
        }

        private string GetTemplate()
        {
            return currentMode switch
            {
                DetectionMode.DBR => PresetTemplate.PT_READ_BARCODES,
                DetectionMode.MRZ => "ReadPassportAndId",
                DetectionMode.DDN => PresetTemplate.PT_DETECT_AND_NORMALIZE_DOCUMENT,
                _ => PresetTemplate.PT_READ_BARCODES
            };
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
                MessageBox.Show("Failed to open camera");
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
                if (_mat.Empty()) continue;

                Mat canvas = new Mat();
                _mat.CopyTo(canvas);

                try
                {
                    // Convert Mat to JPEG bytes for Capture method
                    byte[] jpegBytes = GetJpegBytes(_mat);
                    CapturedResult result = cvRouter.Capture(jpegBytes, GetTemplate());
                    ProcessResult(result, _mat, canvas, false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error: {ex.Message}");
                }

                if (!canvas.Empty())
                {
                    Bitmap bmp = BitmapConverter.ToBitmap(canvas);
                    pictureBoxSrc.Image = bmp;
                }
            }
        }

        private void Form1_Closing(object? sender, FormClosingEventArgs e)
        {
            StopScan();
        }

        private void enterLicenseKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string license = InputBox("Enter License Key", "", "");
            if (!string.IsNullOrEmpty(license))
            {
                int errorCode = LicenseManager.InitLicense(license, out string errorMsg);
                if (errorCode != (int)EnumErrorCode.EC_OK && errorCode != (int)EnumErrorCode.EC_LICENSE_WARNING)
                {
                    toolStripStatusLabel1.Text = $"License error: {errorMsg}";
                }
                else
                {
                    toolStripStatusLabel1.Text = "License activated successfully.";
                }
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
            if (listBox1.SelectedItem != null)
            {
                _currentFilename = listBox1.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(_currentFilename))
                {
                    DetectFile(_currentFilename);
                }
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
                string[]? files = (string[]?)e.Data.GetData(DataFormats.FileDrop);
                if (files != null)
                {
                    foreach (string file in files)
                    {
                        listBox1.Items.Add(file);
                    }
                }
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton? rb = sender as RadioButton;
            if (rb != null && rb.Checked)
            {
                if (rb == radioButtonDbr)
                    currentMode = DetectionMode.DBR;
                else if (rb == radioButtonMrz)
                    currentMode = DetectionMode.MRZ;
                else if (rb == radioButtonDdn)
                    currentMode = DetectionMode.DDN;

                // Re-detect with new mode if file is loaded
                if (!isCapturing && !string.IsNullOrEmpty(_currentFilename))
                {
                    DetectFile(_currentFilename);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (pictureBoxSrc.Image == null)
            {
                MessageBox.Show("No image to save.");
                return;
            }

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string path = Path.Join(folderBrowserDialog.SelectedPath, DateTime.Now.ToFileTimeUtc() + ".jpg");
                pictureBoxSrc.Image.Save(path, ImageFormat.Jpeg);
                MessageBox.Show("Saved to " + folderBrowserDialog.SelectedPath);
            }
        }
    }
}
