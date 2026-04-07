// Barcode & QR Code Reader (.NET Framework) – auto-scan on image load, canvas overlay

var fileInput  = document.getElementById('file');
var imageEl    = document.getElementById('image');
var canvas     = document.getElementById('overlay-canvas');
var ctx        = canvas.getContext('2d');
var previewArea = document.getElementById('preview-area');
var resultsArea = document.getElementById('results-area');
var spinnerEl   = document.getElementById('spinner');
var uploadZone  = document.getElementById('upload-zone');

var lastBarcodes = [];

// Open file dialog when the upload zone is clicked
uploadZone.addEventListener('click', function () { fileInput.click(); });

// Drag-and-drop support
uploadZone.addEventListener('dragover', function (e) {
    e.preventDefault();
    uploadZone.classList.add('drag-over');
});
uploadZone.addEventListener('dragleave', function () {
    uploadZone.classList.remove('drag-over');
});
uploadZone.addEventListener('drop', function (e) {
    e.preventDefault();
    uploadZone.classList.remove('drag-over');
    var file = e.dataTransfer.files[0];
    if (file && file.type.indexOf('image/') === 0) handleFile(file);
});

fileInput.addEventListener('change', function () {
    if (this.files[0]) handleFile(this.files[0]);
});

function handleFile(file) {
    var reader = new FileReader();
    reader.addEventListener('load', function () {
        previewArea.style.display = 'flex';
        resultsArea.style.display = 'none';
        document.getElementById('results').innerHTML = '';
        lastBarcodes = [];
        ctx.clearRect(0, 0, canvas.width, canvas.height);

        imageEl.src = reader.result;
        imageEl.onload = function () {
            syncCanvas();
            upload(file);
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
    var formData = new FormData();
    formData.append('barcodeImage', file, file.name);

    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/upload', true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState !== 4) return;
        spinnerEl.style.display = 'none';
        if (xhr.status === 200) {
            try {
                showResults(JSON.parse(xhr.responseText));
            } catch (e) {
                showError('Failed to parse server response.');
            }
        } else {
            showError('Upload failed (HTTP ' + xhr.status + ').');
        }
    };
    xhr.send(formData);
}

var PALETTE = ['#00b4d8', '#ff6b6b', '#51cf66', '#fcc419', '#cc5de8', '#ff922b'];

function showResults(data) {
    resultsArea.style.display = 'block';
    syncCanvas();
    ctx.clearRect(0, 0, canvas.width, canvas.height);

    if (data.error) { showError(data.error); return; }

    lastBarcodes = (data.barcodes && data.barcodes.length > 0) ? data.barcodes : [];
    var div = document.getElementById('results');

    if (lastBarcodes.length === 0) {
        div.innerHTML = '<div class="no-result">No barcodes or QR codes detected.</div>';
        return;
    }

    var sx = imageEl.offsetWidth  / imageEl.naturalWidth;
    var sy = imageEl.offsetHeight / imageEl.naturalHeight;

    var html = '';
    for (var i = 0; i < lastBarcodes.length; i++) {
        var item = lastBarcodes[i];
        html += '<div class="result-item">'
              + '<span class="result-index">' + (i + 1) + '</span>'
              + '<span class="result-format">' + escapeHtml(item.format) + '</span>'
              + '<span class="result-text">'   + escapeHtml(item.text)   + '</span>'
              + '</div>';
        drawQuad(item.points, sx, sy, i);
    }
    div.innerHTML = html;
}

function drawQuad(points, sx, sy, index) {
    var color = PALETTE[index % PALETTE.length];
    ctx.save();
    ctx.strokeStyle = color;
    ctx.lineWidth   = 2.5;
    ctx.fillStyle   = color + '30';
    ctx.beginPath();
    ctx.moveTo(points[0].x * sx, points[0].y * sy);
    for (var i = 1; i < points.length; i++)
        ctx.lineTo(points[i].x * sx, points[i].y * sy);
    ctx.closePath();
    ctx.fill();
    ctx.stroke();

    var cx = (points[0].x + points[1].x + points[2].x + points[3].x) / 4 * sx;
    var cy = (points[0].y + points[1].y + points[2].y + points[3].y) / 4 * sy;
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

function showError(msg) {
    resultsArea.style.display = 'block';
    document.getElementById('results').innerHTML =
        '<div class="no-result error">' + escapeHtml(msg) + '</div>';
}

function escapeHtml(text) {
    var div = document.createElement('div');
    div.appendChild(document.createTextNode(text));
    return div.innerHTML;
}

window.addEventListener('resize', function () {
    if (!imageEl.naturalWidth || !lastBarcodes.length) return;
    syncCanvas();
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    var sx = imageEl.offsetWidth  / imageEl.naturalWidth;
    var sy = imageEl.offsetHeight / imageEl.naturalHeight;
    for (var i = 0; i < lastBarcodes.length; i++)
        drawQuad(lastBarcodes[i].points, sx, sy, i);
});
