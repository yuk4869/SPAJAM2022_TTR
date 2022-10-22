using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR;




public class FileUploader : MonoBehaviour
{
    [SerializeField] private ARCameraManager _cameraManager = null;
    [SerializeField] private GameObject _target = null;
    //[SerializeField] private Texture2D _sampleTexture = null;
    //[SerializeField] private Material _transposeMaterial = null;
    //[SerializeField] private Text _text = null;

    private Texture2D _texture = null;
    private RenderTexture _previewTexture = null;
    private Renderer _renderer = null;
    private Material _material = null;

    private bool _needsRotate = true;

    WebCamTexture webCam;
    public RawImage RawImage;
    //public RawImage RawImage2;

    

    void Start()
    {
        _cameraManager.frameReceived += OnCameraFrameReceived;


        // RawImageのテクスチャにWebCamTextureのインスタンスを設定
        //WebCamDevice[] webCamDevice = WebCamTexture.devices;
        //webCam = new WebCamTexture(webCamDevice[1].name);


        //９０度回転
        Vector3 angles = RawImage.GetComponent<RectTransform>().eulerAngles;
        angles.z = -90;
        //RawImage.GetComponent<RectTransform>().eulerAngles = angles;
        //
        
        //webCam.Play();
        //WebCamDevice[] webCamDevice = WebCamTexture.devices;


        //StartCoroutine(UploadFile());
        StartCoroutine(Send(0.3f));
    }
    private void RefreshCameraFeedTexture()
    {
        // TryGetLatestImageで最新のイメージを取得します。
        // ただし、失敗の可能性があるため、falseが返された場合は無視します。
        if (!_cameraManager.TryGetLatestImage(out XRCpuImage cameraImage)) return;

        // 中略

        // デバイスの回転に応じてカメラの情報を変換するための情報を定義します。
        XRCpuImage.Transformation imageTransformation = (Input.deviceOrientation == DeviceOrientation.LandscapeRight)
            ? XRCpuImage.Transformation.MirrorY
            : XRCpuImage.Transformation.MirrorX;

        // カメライメージを取得するためのパラメータを設定します。
        XRCpuImage.ConversionParams conversionParams =
            new XRCpuImage.ConversionParams(cameraImage, TextureFormat.RGBA32, imageTransformation);

        // 生成済みのTexture2D（_texture）のネイティブのデータ配列の参照を得ます。
        NativeArray<byte> rawTextureData = _texture.GetRawTextureData<byte>();

        try
        {
            unsafe
            {
                // 前段で得たNativeArrayのポインタを渡し、直接データを流し込みます。
                cameraImage.Convert(conversionParams, new IntPtr(rawTextureData.GetUnsafePtr()), rawTextureData.Length);
            }
        }
        finally
        {
            cameraImage.Dispose();
        }

        // 取得したデータを適用します。
        _texture.Apply();

        // 後略
    }

    private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        RefreshCameraFeedTexture();
        //DisplayInfo();
    }

    //IEnumerator UploadFile()
    //{
        
    //{
    //        //string fileName = "hoge.jpg";
    //        //string filePath = Application.dataPath + "/" + fileName;
    //        // 画像ファイルをbyte配列に格納
    //        //byte[] img = File.ReadAllBytes(filePath);

    //        //
    //        //webCam.Play();
    //    //RawImage.texture = webCam;

    //    Texture2D texture = new Texture2D(webCam.width, webCam.height, TextureFormat.ARGB32, false);
    //    texture.SetPixels(_texture.GetPixels());
    //    byte[] img = texture.EncodeToJPG();
    //    UnityEngine.Object.Destroy(texture);

    //    //byte[] img = File.ReadAllBytes(webCam.GetPixels());


    //    // formにバイナリデータを追加
    //    WWWForm form = new WWWForm();
    //    form.AddBinaryData("request_data", img, "file", "image/png");
    //    // HTTPリクエストを送る
    //    UnityWebRequest request = UnityWebRequest.Post("http://127.0.0.1:5000", form);
    //    Debug.Log("Send");
    //    yield return request.SendWebRequest();

    //    if (request.isHttpError || request.isNetworkError)
    //    {
    //        // POSTに失敗した場合，エラーログを出力
    //        Debug.Log(request.error);
    //    }
    //    else
    //    {
    //        // POSTに成功した場合，レスポンスコードを出力
    //        Debug.Log(request.responseCode);
    //    }
    //}

    IEnumerator Send(float frame)
    {
        while (true)
        {
            //RawImage.texture = ARCameraManager.s_Textures[0];
            

            Texture2D texture = new Texture2D(640, 480, TextureFormat.RGB565, false);
            
            texture.SetPixels32(_texture.GetPixels32());
            byte[] img = texture.EncodeToJPG();
            Debug.Log(img);
            //RawImage2.texture = texture;
            
            //Object.Destroy(texture);

            //byte[] img = File.ReadAllBytes(webCam.GetPixels());


            // formにバイナリデータを追加
            WWWForm form = new WWWForm();
            form.AddBinaryData("request_data", img, "file", "image/png");
            // HTTPリクエストを送る
            UnityWebRequest request = UnityWebRequest.Post("http://127.0.0.1:5000", form);
            
            Debug.Log("Send");
            request.SendWebRequest();
            yield return new WaitForSeconds(frame);

            //frame--;

            if (request.isHttpError || request.isNetworkError)
            {
                // POSTに失敗した場合，エラーログを出力
                Debug.Log(request.error);
            }
            else
            {
                // POSTに成功した場合，レスポンスコードを出力
                Debug.Log(request.responseCode);
            }
            
        }

    }
}