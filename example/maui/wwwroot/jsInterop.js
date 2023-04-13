Dynamsoft.DBR.BarcodeReader.license = "DLS2eyJoYW5kc2hha2VDb2RlIjoiMjAwMDAxLTE2NDk4Mjk3OTI2MzUiLCJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSIsInNlc3Npb25QYXNzd29yZCI6IndTcGR6Vm05WDJrcEQ5YUoifQ==";

let reader = null;
let scanner = null;
let overlay = null;
let context = null;
let dotnetHelper = null;
let videoSelect = null;
let cameraInfo = {};

function initOverlay(ol) {
    overlay = ol;
    context = overlay.getContext('2d');
}

function updateOverlay(width, height) {
    if (overlay) {
        overlay.width = width;
        overlay.height = height;
        clearOverlay();
    }
}

function clearOverlay() {
    if (context) {
        context.clearRect(0, 0, overlay.width, overlay.height);
        context.strokeStyle = '#ff0000';
        context.lineWidth = 5;
    }
}

function drawOverlay(localization, text) {
    if (context) {
        context.beginPath();
        context.moveTo(localization.x1, localization.y1);
        context.lineTo(localization.x2, localization.y2);
        context.lineTo(localization.x3, localization.y3);
        context.lineTo(localization.x4, localization.y4);
        context.lineTo(localization.x1, localization.y1);
        context.stroke();

        context.font = '18px Verdana';
        context.fillStyle = '#ff0000';
        let x = [localization.x1, localization.x2, localization.x3, localization.x4];
        let y = [localization.y1, localization.y2, localization.y3, localization.y4];
        x.sort(function (a, b) {
            return a - b;
        });
        y.sort(function (a, b) {
            return b - a;
        });
        let left = x[0];
        let top = y[0];

        context.fillText(text, left, top + 50);
    }
}

function decodeImage(dotnetRef, url, data) {
    const img = new Image()
    img.onload = () => {
        updateOverlay(img.width, img.height);
        if (reader) {
            reader.decode(data).then(function (results) {
                try {
                    let localization;
                    if (results.length > 0) {
                        for (var i = 0; i < results.length; ++i) {
                            localization = results[i].localizationResult;
                            drawOverlay(localization, results[i].barcodeText);
                        }
                    }

                } catch (e) {
                    alert(e);
                }
                returnResultsAsString(dotnetRef, results);
            });

        }
    }
    img.src = url
}

function returnResultsAsString(dotnetRef, results) {
    let txts = [];
    try {
        for (let i = 0; i < results.length; ++i) {
            txts.push(results[i].barcodeText);
        }
        let barcoderesults = txts.join(', ');
        if (txts.length == 0) {
            barcoderesults = 'No barcode found';
        }

        if (dotnetRef) {
            dotnetRef.invokeMethodAsync('ReturnBarcodeResultsAsync', barcoderesults);
        }
    } catch (e) {
    }
}

function updateResolution() {
    if (scanner) {
        let resolution = scanner.getResolution();
        updateOverlay(resolution[0], resolution[1]);
    }
}

function listCameras(deviceInfos) {
    for (var i = deviceInfos.length - 1; i >= 0; --i) {
        var deviceInfo = deviceInfos[i];
        var option = document.createElement('option');
        option.value = deviceInfo.deviceId;
        option.text = deviceInfo.label;
        cameraInfo[deviceInfo.deviceId] = deviceInfo;
        videoSelect.appendChild(option);
    }
}

function showResults(results) {
    clearOverlay();

    let txts = [];
    try {
        let localization;
        if (results.length > 0) {
            for (var i = 0; i < results.length; ++i) {
                txts.push(results[i].barcodeText);
                localization = results[i].localizationResult;
                drawOverlay(localization, results[i].barcodeText);
            }

            if (dotnetHelper) {
                returnResultsAsString(dotnetHelper, results);
            }
        }

    } catch (e) {
        alert(e);
    }
}

async function openCamera() {
    clearOverlay();
    let deviceId = videoSelect.value;
    if (scanner) {
        await scanner.setCurrentCamera(cameraInfo[deviceId]);
    }
}

window.jsFunctions = {
    setImageUsingStreaming: async function (dotnetRef, overlayId, imageId, imageStream) {
        const arrayBuffer = await imageStream.arrayBuffer();
        const blob = new Blob([arrayBuffer]);
        const url = URL.createObjectURL(blob);
        document.getElementById(imageId).src = url;
        document.getElementById(imageId).style.display = 'block';
        initOverlay(document.getElementById(overlayId));
        if (reader) {
            reader.maxCvsSideLength = 9999
            decodeImage(dotnetRef, url, blob);
        }

    },
    initSDK: async function () {
        if (reader != null) {
            return true;
        }
        let result = true;
        try {
            reader = await Dynamsoft.DBR.BarcodeReader.createInstance();
            await reader.updateRuntimeSettings("balance");
        } catch (e) {
            console.log(e);
            result = false;
        }
        return result;
    },
    initScanner: async function(dotnetRef, videoId, selectId, overlayId) {
        let canvas = document.getElementById(overlayId);
        initOverlay(canvas);
        videoSelect = document.getElementById(selectId);
        videoSelect.onchange = openCamera;
        dotnetHelper = dotnetRef;

        try {
            scanner = await Dynamsoft.DBR.BarcodeScanner.createInstance();
            await scanner.setUIElement(document.getElementById(videoId));
            await scanner.updateRuntimeSettings("speed");

            let cameras = await scanner.getAllCameras();
            listCameras(cameras);
            await openCamera();
            scanner.onFrameRead = results => {
                showResults(results);
            };
            scanner.onUnduplicatedRead = (txt, result) => { };
            scanner.onPlayed = function () {
                updateResolution();
            }
            await scanner.show();

        } catch (e) {
            console.log(e);
            result = false;
        }
        return true;
    },
    selectFile: async function (dotnetRef, overlayId, imageId) {
        initOverlay(document.getElementById(overlayId));
        if (reader) {
            let input = document.createElement("input");
            input.type = "file";
            input.onchange = async function () {
                try {
                    let file = input.files[0];
                    var fr = new FileReader();
                    fr.onload = function () {
                        let image = document.getElementById(imageId);
                        image.src = fr.result;
                        decodeImage(dotnetRef, fr.result, file);
                    }
                    fr.readAsDataURL(file);

                } catch (ex) {
                    alert(ex.message);
                    throw ex;
                }
            };
            input.click();
        } else {
            alert("The barcode reader is still initializing.");
        }
    },
};

