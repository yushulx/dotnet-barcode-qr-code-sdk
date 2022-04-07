// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.getElementById("file").addEventListener("change", function () {
    let file = this.files[0];
    let image = document.getElementById('image');
    let reader = new FileReader();

    reader.addEventListener("load", function () {
        image.src = reader.result;
    }, false);

    if (file) {
        reader.readAsDataURL(file);
    }
});

function upload() {
    const host = location.hostname
    const protocol = location.protocol
    const uploadPath = '/upload'
    let xhr = new XMLHttpRequest();
    let formData = new FormData();
    let file = document.getElementById('file').files[0];
    formData.append('barcodeImage', file, file.name);
    xhr.open('POST', uploadPath, true);
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            document.getElementById("results").innerHTML = "Detection Results: " + xhr.responseText;
        }
    }
}