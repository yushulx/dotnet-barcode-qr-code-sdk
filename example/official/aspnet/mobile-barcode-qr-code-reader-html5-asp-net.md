---
layout: post
status: publish
published: true
title: "Read Barcodes and QR Codes Server-Side with HTML5 and ASP.NET in C#"
author:
  display_name: Xiao Ling
  email: xiao@dynamsoft.com
date: 2022-04-05 10:00:51 +0800
date_gmt: 2022-04-05 18:00:51 +0800
categories:
  - Web Barcode Scanner
tags:
  - Barcode
  - QR Code
  - Mobile
  - HTML5
  - ASP.NET
  - C#
  - .NET
  - Server-Side Barcode Decoding
  - JavaScript
  - Dynamsoft Barcode Reader
description: "Step-by-step guide to building a modern, responsive server-side barcode and QR code reader with HTML5, ASP.NET Core MVC, and Dynamsoft Barcode Reader SDK in C#. Covers drag-and-drop image upload, CaptureVisionRouter integration, canvas quad overlay, and responsive UI design."
image: ./img/2022/04/asp-net-mobile-barcode-qr-code-reader.png
last_modified: 2026-04-07 10:00:00 +0800

---

HTML5 is universally supported across all modern browsers — Chrome, Edge, Safari, Firefox, and Opera — on both desktop and mobile, as well as WebView components used in native Android and iOS apps. This cross-platform reach means a single ASP.NET web application can serve all users without maintaining separate native codebases.

This guide shows how to build a **barcode and QR code reader** using **HTML5**, **ASP.NET Core (C#)**, and the [Dynamsoft Barcode Reader SDK](https://www.dynamsoft.com/barcode-reader/overview/). The architecture follows a **server-side decoding model**: the browser captures an image, uploads it via HTTP, and the ASP.NET backend performs barcode and QR code recognition using Dynamsoft's high-accuracy engine.

The finished application features:

- **Auto-scan on image load** — no scan button; decoding starts as soon as an image is selected or dropped.
- **Canvas overlay** — detected barcodes are highlighted on the image with color-coded quadrilateral outlines and index labels.
- **Results panel** — decoded text and format for each barcode are listed below the image.
- **Responsive layout** — adapts cleanly from desktop to mobile with drag-and-drop support.

> **Prefer client-side decoding?** Dynamsoft also supports fully in-browser barcode scanning via JavaScript and WebAssembly. See [Build a Barcode Scanner Using JavaScript and HTML5](https://www.dynamsoft.com/codepool/html5-barcode-reader-javascript-webassembly.html) for that approach.

## Key Takeaways

- An ASP.NET MVC controller can accept image uploads and decode 30+ barcode formats server-side using Dynamsoft's `CaptureVisionRouter` in just a few lines of C#.
- The backend now returns structured JSON — including each barcode's **text**, **format**, and **quad coordinates** — which the browser uses to draw precise overlays on the image via the HTML5 Canvas API.
- Removing the scan button and triggering upload automatically on `<input type="file">` change provides a frictionless UX on both desktop and mobile.
- A `<canvas>` element positioned absolutely over the image scales quad points using the ratio of displayed size to natural (pixel) size, keeping overlays pixel-accurate at any viewport width.

## Common Developer Questions

- How do I decode barcodes and QR codes from uploaded images in ASP.NET C#?
- How do I draw a bounding-box overlay on a detected barcode in an HTML5 app?
- How do I auto-scan without a button in a web barcode reader?
- What is the best .NET library for server-side barcode recognition with high accuracy?

## Why Choose Server-Side Barcode Decoding?

| Factor | Server-Side (.NET) | Client-Side (JS/WASM) |
|---|---|---|
| **Processing power** | Full server CPU/GPU | Limited to device hardware |
| **SDK update control** | Centralized, no client update needed | Depends on browser cache / CDN |
| **Sensitive data handling** | Image stays on server | Image processed on device |
| **Low-end device support** | Offloads compute from weak devices | May struggle on older phones |
| **Bundle size** | No JS payload overhead | WASM adds ~2 MB to client |

Server-side decoding is well-suited for **enterprise workflows**, **document intake pipelines**, and applications where **accuracy and control** outweigh the need for offline or real-time scanning.

## Prerequisites

- [.NET SDK 8.0+](https://dotnet.microsoft.com/en-us/download/dotnet/)
- [Get a 30-day free trial license](https://www.dynamsoft.com/customer/license/trialLicense/?product=dcv&package=cross-platform) for Dynamsoft Capture Vision

## Step-by-Step: Build a Responsive Server-Side Barcode and QR Code Reader

### Step 1: Create a New ASP.NET MVC Project

```bash
dotnet new mvc -o MvcBarcodeQRCode
```

### Step 2: Add the Dynamsoft Barcode Reader NuGet Package

```bash
dotnet add package Dynamsoft.DotNet.BarcodeReader.Bundle
```

### Step 3: Build the Responsive HTML5 Upload UI

Open `Views/Home/Index.cshtml` and replace the default content with a drag-and-drop upload zone, an image preview container with a `<canvas>` overlay, and a results panel:

```html
@{
    ViewData["Title"] = "Barcode & QR Code Reader";
}

<div class="scanner-app">
    <div class="scanner-header">
        <h1>Barcode &amp; QR Code Reader</h1>
        <p class="subtitle">Upload an image to instantly decode barcodes and QR codes</p>
    </div>

    <div class="upload-zone" id="upload-zone">
        <input type="file" id="file" accept="image/*" name="barcodeImage" />
        <div class="upload-prompt">
            <svg width="48" height="48" viewBox="0 0 24 24" fill="none"
                 stroke="currentColor" stroke-width="1.5">
                <path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4" />
                <polyline points="17 8 12 3 7 8" />
                <line x1="12" y1="3" x2="12" y2="15" />
            </svg>
            <span>Click or drag an image here</span>
            <small>Supports JPG, PNG, BMP, TIFF, and more</small>
        </div>
    </div>

    <div class="preview-area" id="preview-area" style="display:none">
        <div class="image-container">
            <img id="image" alt="Uploaded image" />
            <canvas id="overlay-canvas"></canvas>
        </div>
        <div id="spinner" class="spinner" style="display:none">
            <div class="spinner-ring"></div>
            <span>Scanning&hellip;</span>
        </div>
    </div>

    <div class="results-area" id="results-area" style="display:none">
        <h2 class="results-title">Scan Results</h2>
        <div id="results"></div>
    </div>
</div>
```

Key points:

- The `<canvas id="overlay-canvas">` is positioned **absolutely** over the image via CSS, so it overlaps the image exactly.
- The file `<input>` is hidden; the entire upload zone acts as the click target.
- The spinner is shown while the server processes the image and hidden once results arrive.

### Step 4: Add Responsive CSS

In `wwwroot/css/site.css`, define styles that work on any screen size. Key rules:

```css
/* Upload zone with drag-over feedback */
.upload-zone {
    border: 2px dashed #c9d6e3;
    border-radius: 14px;
    padding: 2.5rem 1.5rem;
    cursor: pointer;
    transition: border-color 0.2s ease, background 0.2s ease;
    background: #fff;
}
.upload-zone:hover, .upload-zone.drag-over {
    border-color: #0d6efd;
    background: #f0f5ff;
}
.upload-zone input[type="file"] { display: none; }

/* Image + canvas stacking */
.image-container {
    position: relative;   /* anchor for the absolutely-positioned canvas */
    display: inline-block;
}
.image-container img {
    display: block;
    max-width: 100%;
    max-height: 65vh;     /* keep image within viewport on any screen */
}
.image-container canvas {
    position: absolute;
    top: 0; left: 0;
    pointer-events: none;
}

/* Animated spinner */
.spinner-ring {
    width: 34px; height: 34px;
    border: 3px solid #dee2e6;
    border-top-color: #0d6efd;
    border-radius: 50%;
    animation: spin 0.75s linear infinite;
}
@keyframes spin { to { transform: rotate(360deg); } }

/* Result rows */
.result-item {
    display: flex;
    align-items: flex-start;
    gap: .75rem;
    padding: .7rem .9rem;
    border-radius: 8px;
    background: #f8f9fa;
    margin-bottom: .5rem;
    flex-wrap: wrap;
}
.result-format {
    background: #e9ecef;
    border-radius: 4px;
    padding: 2px 8px;
    font-size: .75rem;
    font-weight: 600;
}
.result-text {
    font-family: 'Courier New', monospace;
    font-size: .92rem;
    word-break: break-all;
    flex: 1;
}
```

### Step 5: Auto-Scan JavaScript with Canvas Overlay

Replace `wwwroot/js/site.js` with the following. The logic:

1. Previews the selected image locally with `FileReader` (no upload yet).
2. After the image loads, automatically POSTs it to `/upload` — no button click needed.
3. Parses the JSON response and draws a scaled quadrilateral on the canvas for each detected barcode.

```javascript
const fileInput  = document.getElementById('file');
const imageEl    = document.getElementById('image');
const canvas     = document.getElementById('overlay-canvas');
const ctx        = canvas.getContext('2d');
const previewArea = document.getElementById('preview-area');
const resultsArea = document.getElementById('results-area');
const spinnerEl   = document.getElementById('spinner');
const uploadZone  = document.getElementById('upload-zone');

let lastBarcodes = [];

uploadZone.addEventListener('click', () => fileInput.click());

uploadZone.addEventListener('dragover', e => {
    e.preventDefault();
    uploadZone.classList.add('drag-over');
});
uploadZone.addEventListener('dragleave', () => uploadZone.classList.remove('drag-over'));
uploadZone.addEventListener('drop', e => {
    e.preventDefault();
    uploadZone.classList.remove('drag-over');
    const file = e.dataTransfer.files[0];
    if (file && file.type.startsWith('image/')) handleFile(file);
});

fileInput.addEventListener('change', function () {
    if (this.files[0]) handleFile(this.files[0]);
});

function handleFile(file) {
    const reader = new FileReader();
    reader.addEventListener('load', function () {
        previewArea.style.display = 'flex';
        resultsArea.style.display = 'none';
        document.getElementById('results').innerHTML = '';
        lastBarcodes = [];
        ctx.clearRect(0, 0, canvas.width, canvas.height);

        imageEl.src = reader.result;
        imageEl.onload = () => {
            syncCanvas();   // size canvas to match rendered image
            upload(file);   // auto-trigger scan
        };
    });
    reader.readAsDataURL(file);
}

function syncCanvas() {
    canvas.width  = imageEl.offsetWidth;
    canvas.height = imageEl.offsetHeight;
}

function upload(file) {
    spinnerEl.style.display = 'flex';
    const formData = new FormData();
    formData.append('barcodeImage', file, file.name);

    const xhr = new XMLHttpRequest();
    xhr.open('POST', '/upload', true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState !== 4) return;
        spinnerEl.style.display = 'none';
        if (xhr.status === 200) {
            showResults(JSON.parse(xhr.responseText));
        }
    };
    xhr.send(formData);
}

const PALETTE = ['#00b4d8', '#ff6b6b', '#51cf66', '#fcc419', '#cc5de8', '#ff922b'];

function showResults(data) {
    resultsArea.style.display = 'block';
    syncCanvas();
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    lastBarcodes = data.barcodes || [];

    const div = document.getElementById('results');
    if (lastBarcodes.length === 0) {
        div.innerHTML = '<div class="no-result">No barcodes detected.</div>';
        return;
    }

    // Scale factor: SDK returns coords in original image pixels
    const sx = imageEl.offsetWidth  / imageEl.naturalWidth;
    const sy = imageEl.offsetHeight / imageEl.naturalHeight;

    div.innerHTML = lastBarcodes.map((item, i) => `
        <div class="result-item">
            <span class="result-index">${i + 1}</span>
            <span class="result-format">${item.format}</span>
            <span class="result-text">${item.text}</span>
        </div>`).join('');

    lastBarcodes.forEach((item, i) => drawQuad(item.points, sx, sy, i));
}

function drawQuad(points, sx, sy, index) {
    const color = PALETTE[index % PALETTE.length];
    ctx.save();
    ctx.strokeStyle = color;
    ctx.lineWidth   = 2.5;
    ctx.fillStyle   = color + '30';   // semi-transparent fill
    ctx.beginPath();
    ctx.moveTo(points[0].x * sx, points[0].y * sy);
    for (let i = 1; i < points.length; i++)
        ctx.lineTo(points[i].x * sx, points[i].y * sy);
    ctx.closePath();
    ctx.fill();
    ctx.stroke();

    // Draw numbered circle at centroid
    const cx = points.reduce((s, p) => s + p.x, 0) / 4 * sx;
    const cy = points.reduce((s, p) => s + p.y, 0) / 4 * sy;
    ctx.fillStyle = color;
    ctx.beginPath();
    ctx.arc(cx, cy, 12, 0, Math.PI * 2);
    ctx.fill();
    ctx.fillStyle = '#fff';
    ctx.font = 'bold 12px sans-serif';
    ctx.textAlign = 'center';
    ctx.textBaseline = 'middle';
    ctx.fillText(index + 1, cx, cy);
    ctx.restore();
}

// Redraw on window resize
window.addEventListener('resize', () => {
    if (!imageEl.naturalWidth || !lastBarcodes.length) return;
    syncCanvas();
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    const sx = imageEl.offsetWidth  / imageEl.naturalWidth;
    const sy = imageEl.offsetHeight / imageEl.naturalHeight;
    lastBarcodes.forEach((item, i) => drawQuad(item.points, sx, sy, i));
});
```

The critical detail is **`syncCanvas()`** — the canvas `width`/`height` attributes must equal the image's rendered pixel dimensions (not CSS dimensions). This ensures the canvas coordinate system matches what the user sees. Because the image is displayed responsively, its rendered size changes with viewport width, so `syncCanvas()` is called on initial load, after results arrive, and on every `resize` event.

### Step 6: Return JSON with Quad Coordinates from ASP.NET

The upload controller previously returned plain text. Update `Controllers/FileController.cs` to return structured JSON that includes each barcode's text, format string, and the four corner points of its bounding quad:

```cs
using Microsoft.AspNetCore.Mvc;
using Dynamsoft.DBR;
using Dynamsoft.License;
using Dynamsoft.CVR;
using Dynamsoft.Core;

namespace MvcBarcodeQRCode.Controllers
{
    [ApiController]
    public class FileController : Controller
    {
        [HttpPost("/upload")]
        public async Task<IActionResult> Upload()
        {
            var files = Request.Form.Files;
            var path  = Path.Combine(Directory.GetCurrentDirectory(), "Upload");
            Directory.CreateDirectory(path);

            string errorMsg;
            int errorCode = LicenseManager.InitLicense("LICENSE-KEY", out errorMsg);
            if (errorCode != (int)EnumErrorCode.EC_OK &&
                errorCode != (int)EnumErrorCode.EC_LICENSE_CACHE_USED)
            {
                return Ok(new { error = "License error: " + errorMsg });
            }

            var barcodes = new List<object>();
            using (CaptureVisionRouter cvr = new CaptureVisionRouter())
            {
                foreach (var uploadFile in files)
                {
                    var filePath = Path.Combine(path, uploadFile.FileName);
                    using (var stream = System.IO.File.Create(filePath))
                        await uploadFile.CopyToAsync(stream);

                    var result        = cvr.Capture(filePath, PresetTemplate.PT_READ_BARCODES);
                    var barcodesResult = result.GetDecodedBarcodesResult();
                    if (barcodesResult == null) continue;

                    foreach (var item in barcodesResult.GetItems())
                    {
                        var loc = item.GetLocation();
                        barcodes.Add(new {
                            text   = item.GetText(),
                            format = item.GetFormatString(),
                            points = new[] {
                                new { x = loc.Points[0].X, y = loc.Points[0].Y },
                                new { x = loc.Points[1].X, y = loc.Points[1].Y },
                                new { x = loc.Points[2].X, y = loc.Points[2].Y },
                                new { x = loc.Points[3].X, y = loc.Points[3].Y },
                            }
                        });
                    }
                }
            }

            return Ok(new { barcodes });
        }
    }
}
```

`item.GetLocation()` returns a `Quadrilateral` from `Dynamsoft.Core`. Its `Points[0..3]` are the four corner coordinates of the detected barcode region in the **original image's pixel space**. The JavaScript then scales these to the rendered image size.

### Step 7: Run and Test

```bash
dotnet restore
dotnet run
```

Open the URL shown in the terminal. Drag an image onto the upload zone (or click to browse). The app:

1. Previews the image instantly in the browser.
2. Sends it to the server automatically.
3. Returns decoded barcodes with quad locations.
4. Draws color-coded overlays and lists results below the image.

### Step 8: Publish for Production

```bash
dotnet publish --configuration Release
```

## Performance and Supported Barcode Formats

- Dynamsoft Barcode Reader supports **30+ barcode formats** including Code 128, Code 39, EAN/UPC, QR Code, PDF417, DataMatrix, and Aztec.
- The server-side `CaptureVisionRouter` engine uses multi-threaded processing optimized for high-throughput batch scenarios.
- For low-latency use cases (real-time scanning), consider the [JavaScript SDK](https://www.dynamsoft.com/barcode-reader/sdk-javascript/) which runs entirely in-browser via WebAssembly.

## Common Issues and Edge Cases

- **License initialization fails:** Call `LicenseManager.InitLicense()` before creating a `CaptureVisionRouter`. An expired or invalid key returns a non-`EC_OK` error code — check `errorMsg` for details.
- **Large image uploads time out:** ASP.NET Core limits request body size to ~30 MB by default. Increase `MaxRequestBodySize` in `Program.cs` or apply `[RequestSizeLimit]` to the controller action for larger files.
- **Overlays appear misaligned:** Ensure `syncCanvas()` is called after the image fully loads (`imageEl.onload`) and on `window.resize`. Misalignment usually means the canvas size was measured before layout was complete.
- **No barcode detected on a valid image:** Low-resolution or blurry images reduce recognition accuracy. Ensure uploaded images are at least 640×480 pixels and well-lit.

## Source Code

[https://github.com/yushulx/dotnet-barcode-qr-code-sdk/tree/main/example/official/aspnet](https://github.com/yushulx/dotnet-barcode-qr-code-sdk/tree/main/example/official/aspnet)

