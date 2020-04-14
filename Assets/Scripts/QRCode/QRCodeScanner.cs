using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using Microsoft.MixedReality.Toolkit.Utilities;

public class QRCodeScanner : MonoBehaviour {

    private UnityEngine.Windows.WebCam.PhotoCapture photoCaptureObject;
    private Texture2D screenCaptureTexture;
    private Texture2D croppedQR;
    private int cameraWidth;
    private int cameraHeight;
    private bool scanSuccess;
    public Text HeadsUpDisplay;
    public Text qrCodeHud;
    public Text HudValues;

    public OrderManager orderMan;
    public VoiceResponseManager soundManager;

    /// <summary>
    /// Initializes the camera component of the device and starts taking photos.
    /// </summary>
    void Start() {
        Resolution cameraResolution = UnityEngine.Windows.WebCam.PhotoCapture.SupportedResolutions.
            OrderByDescending((res) => res.width * res.height).First();

        cameraWidth = cameraResolution.width;
        cameraHeight = cameraResolution.height;

        screenCaptureTexture = new Texture2D(cameraWidth, cameraHeight);

        // Create a PhotoCapture object
        UnityEngine.Windows.WebCam.PhotoCapture.CreateAsync(
            false,
            delegate (UnityEngine.Windows.WebCam.PhotoCapture captureObject) {
                photoCaptureObject = captureObject;
                UnityEngine.Windows.WebCam.CameraParameters cameraParameters =
                    new UnityEngine.Windows.WebCam.CameraParameters();

                cameraParameters.hologramOpacity = 0.0f;
                cameraParameters.cameraResolutionWidth = cameraResolution.width;
                cameraParameters.cameraResolutionHeight = cameraResolution.height;
                cameraParameters.pixelFormat = UnityEngine.Windows.WebCam.CapturePixelFormat.BGRA32;

                // Activate the camera
                photoCaptureObject.StartPhotoModeAsync(
                    cameraParameters,
                    delegate (UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result) {
                        // Start a coroutine to keep taking photos
                        StartCoroutine(WaitAndCapture());
                    });
            });

        soundManager = FindObjectOfType<VoiceResponseManager>();
    }

    public void InitCamera() {
        Resolution cameraResolution = UnityEngine.Windows.WebCam.PhotoCapture.SupportedResolutions.
            OrderByDescending((res) => res.width * res.height).First();

        cameraWidth = cameraResolution.width;
        cameraHeight = cameraResolution.height;

        screenCaptureTexture = new Texture2D(cameraWidth, cameraHeight);

        UnityEngine.Windows.WebCam.PhotoCapture.CreateAsync(
            false,
            delegate (UnityEngine.Windows.WebCam.PhotoCapture captureObject) {
                photoCaptureObject = captureObject;
                UnityEngine.Windows.WebCam.CameraParameters cameraParameters =
                    new UnityEngine.Windows.WebCam.CameraParameters();

                cameraParameters.hologramOpacity = 0.0f;
                cameraParameters.cameraResolutionWidth = cameraResolution.width;
                cameraParameters.cameraResolutionHeight = cameraResolution.height;
                cameraParameters.pixelFormat = UnityEngine.Windows.WebCam.CapturePixelFormat.BGRA32;

                photoCaptureObject.StartPhotoModeAsync(
                    cameraParameters,
                    delegate (UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result) {
                        StartCoroutine(WaitAndCapture());
                    });
            });
    }

    /// <summary>
    /// Starts a coroutine to take photos. The camera must already be initialized by
    /// calling Start().
    /// </summary>
    public void StartCamera() {
        StartCoroutine(WaitAndCapture());
    }

    /// <summary>
    /// Takes a photo and decodes it.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitAndCapture() {
        scanSuccess = false;

        while (!scanSuccess) {
            Debug.Log("in wait and capture while");

            // Take a photo
            photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            soundManager.PlayCameraShutter();
            yield return new WaitForSeconds(0.5f);

            Color[] colors = screenCaptureTexture.GetPixels(0, 0, cameraWidth, cameraHeight);
            croppedQR = new Texture2D(cameraWidth, cameraHeight);
            croppedQR.SetPixels(colors);
            croppedQR.Apply();

            DecodeQR(croppedQR);
            yield return new WaitForSeconds(4f);
        }
    }

    /// <summary>
    /// Decodes a photo of a QR code.
    /// </summary>
    /// <param name="qrToDecode"></param>
    void DecodeQR(Texture2D qrToDecode) {
        if (qrToDecode != null) {
            string qrCodeContent = "";
            byte[] colorBytes = ProcessColor32(qrToDecode.GetPixels32());

#if !UNITY_EDITOR
            qrCodeContent = ZxingUwp.Barcode.Read(colorBytes, qrToDecode.width, qrToDecode.height);
#endif
            // Testing
            qrCodeContent = "authorized";

            switch (qrCodeContent) {
                case "null":
                    qrCodeHud.text = qrCodeContent;
                    break;
                case "authorized":
                    // Stops scanning
                    scanSuccess = true;
                    soundManager.PlaySuccess();
                    StartCoroutine(RemoveText());
                    qrCodeHud.text = "Login successful";
                    OrderManager go = Instantiate<OrderManager>(orderMan);
                    go.SetHud(HudValues);
                    break;
                default:
                    if (qrCodeContent == "") {
                        qrCodeHud.text = "no code scanned";
                    } else {
                        qrCodeHud.text = qrCodeContent;

                    }
                    if (qrCodeContent != "" && orderMan.getCurrentItem().ItemName == qrCodeContent) {
                        scanSuccess = true;
                        qrCodeHud.text = orderMan.getCurrentItem().ItemName + ": " + orderMan.getCurrentItem().QuantityRequired;
                        orderMan.activateVoice();
                        break;
                    }
                    qrCodeHud.text = "Please scan your employee card";
                    break;

            }
        }
    }

    IEnumerator RemoveText() {
        yield return new WaitForSeconds(5f);
        Destroy(qrCodeHud);
    }

    void OnCapturedPhotoToMemory(
        UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result,
        UnityEngine.Windows.WebCam.PhotoCaptureFrame photoCaptureFrame) {
        // Copy the raw image data into our target texture
        photoCaptureFrame.UploadImageDataToTexture(screenCaptureTexture);
    }

    private static byte[] ProcessColor32(Color32[] colors) {
        if (colors == null || colors.Length == 0) {
            return null;
        }

        int lengthOfColor32 = Marshal.SizeOf(typeof(Color32));
        int length = lengthOfColor32 * colors.Length;
        byte[] bytes = new byte[length];

        GCHandle handle = default;

        try {
            handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
            IntPtr ptr = handle.AddrOfPinnedObject();
            Marshal.Copy(ptr, bytes, 0, length);
        } catch (Exception e) {
            Debug.Log(e.ToString());
            return null;
        } finally {
            if (handle != default)
                handle.Free();
        }

        return bytes;
    }
    /// <summary>
    ///  Shutdown our photo capture resource.
    /// </summary>
    /// <param name="result"></param>
    void OnStoppedPhotoMode(UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result) {
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }

    void OnDestroy() {
        // Deactivate our camera
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    public void ScanOff() {
        scanSuccess = true;
        Debug.Log("scan off");
    }

}
