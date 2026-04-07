// Barcode & QR Code Reader – auto-scan on image load, canvas overlay

const fileInput = document.getElementById('file');
const imageEl   = document.getElementById('image');
const canvas    = document.getElementById('overlay-canvas');
const ctx       = canvas.getContext('2d');
const resultsDiv   = document.getElementById('results');
const previewArea  = document.getElementById('preview-area');
const resultsArea  = document.getElementById('results-area');
const spinnerEl    = document.getElementById('spinner');
const uploadZone   = document.getElementById('upload-zone');

let lastBarcodes = [];

// Open file dialog when the upload zone is clicked
uploadZone.addEventListener('click', () => fileInput.click());

// Drag-and-drop support
uploadZone.addEventListener('dragover', (e) => {
    e.preventDefault();
    uploadZone.classList.add('drag-over');
});
uploadZone.addEventListener('dragleave', () => uploadZone.classList.remove('drag-over'));
uploadZone.addEventListener('drop', (e) => {
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
        // Show preview area and clear previous state
        previewArea.style.display = 'flex';
        resultsArea.style.display = 'none';
        resultsDiv.innerHTML = '';
        lastBarcodes = [];
        clearCanvas();

        imageEl.src = reader.result;
        imageEl.onload = () => {
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

function clearCanvas() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
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
            try {
                const data = JSON.parse(xhr.responseText);
                showResults(data);
            } catch (e) {
                showError('Failed to parse server response.');
            }
        } else {
            showError('Upload failed (HTTP ' + xhr.status + ').');
        }
    };
    xhr.send(formData);
}

function showResults(data) {
    resultsArea.style.display = 'block';
    syncCanvas();
    clearCanvas();

    if (data.error) {
        showError(data.error);
        return;
    }

    lastBarcodes = (data.barcodes && data.barcodes.length > 0) ? data.barcodes : [];

    if (lastBarcodes.length === 0) {
        resultsDiv.innerHTML = '<div class="no-result">No barcodes or QR codes detected.</div>';
        return;
    }

    const scaleX = imageEl.offsetWidth  / imageEl.naturalWidth;
    const scaleY = imageEl.offsetHeight / imageEl.naturalHeight;

    let html = '';
    lastBarcodes.forEach((item, index) => {
        html += `
            <div class="result-item">
                <span class="result-index">${index + 1}</span>
                <span class="result-format">${escapeHtml(item.format)}</span>
                <span class="result-text">${escapeHtml(item.text)}</span>
            </div>`;
        drawQuad(item.points, scaleX, scaleY, index);
    });
    resultsDiv.innerHTML = html;
}

const PALETTE = ['#00b4d8', '#ff6b6b', '#51cf66', '#fcc419', '#cc5de8', '#ff922b'];

function drawQuad(points, scaleX, scaleY, index) {
    const color = PALETTE[index % PALETTE.length];
    ctx.save();

    ctx.strokeStyle = color;
    ctx.lineWidth   = 2.5;
    ctx.fillStyle   = color + '30';

    ctx.beginPath();
    ctx.moveTo(points[0].x * scaleX, points[0].y * scaleY);
    for (let i = 1; i < points.length; i++) {
        ctx.lineTo(points[i].x * scaleX, points[i].y * scaleY);
    }
    ctx.closePath();
    ctx.fill();
    ctx.stroke();

    // Draw index label at centroid
    const cx = points.reduce((s, p) => s + p.x, 0) / points.length * scaleX;
    const cy = points.reduce((s, p) => s + p.y, 0) / points.length * scaleY;
    const label = String(index + 1);
    const radius = 12;

    ctx.fillStyle = color;
    ctx.beginPath();
    ctx.arc(cx, cy, radius, 0, Math.PI * 2);
    ctx.fill();

    ctx.fillStyle = '#fff';
    ctx.font = `bold 12px sans-serif`;
    ctx.textAlign = 'center';
    ctx.textBaseline = 'middle';
    ctx.fillText(label, cx, cy);

    ctx.restore();
}

function showError(msg) {
    resultsArea.style.display = 'block';
    resultsDiv.innerHTML = `<div class="no-result error">${escapeHtml(msg)}</div>`;
}

function escapeHtml(text) {
    const div = document.createElement('div');
    div.appendChild(document.createTextNode(text));
    return div.innerHTML;
}

// Redraw overlays on window resize
window.addEventListener('resize', () => {
    if (!imageEl.src || !imageEl.naturalWidth) return;
    syncCanvas();
    clearCanvas();
    if (lastBarcodes.length === 0) return;
    const scaleX = imageEl.offsetWidth  / imageEl.naturalWidth;
    const scaleY = imageEl.offsetHeight / imageEl.naturalHeight;
    lastBarcodes.forEach((item, index) => drawQuad(item.points, scaleX, scaleY, index));
});