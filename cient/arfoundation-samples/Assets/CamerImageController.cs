using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

using UnityEngine.Networking;
using System.IO;
using UnityEngine.UI;


public class CamerImageController : MonoBehaviour
{
    public ARCameraManager cameraManager;

    private Texture2D mTexture;
    private MeshRenderer mRenderer;

    private void Start()
    {
        mRenderer = GetComponent<MeshRenderer>();

        StartCoroutine(Send(0.3f));
    }

    void OnEnable()
    {
        cameraManager.frameReceived += OnCameraFrameReceived;
    }

    void OnDisable()
    {
        cameraManager.frameReceived -= OnCameraFrameReceived;
    }

    unsafe void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        XRCpuImage image;
        if (!cameraManager.TryGetLatestImage(out image))
            return;

        var conversionParams = new XRCpuImage.ConversionParams
        (
            image,
            TextureFormat.RGBA32,
            XRCpuImage.Transformation.None
        );

        if (mTexture == null || mTexture.width != image.width || mTexture.height != image.height)
        {
            mTexture = new Texture2D(conversionParams.outputDimensions.x,
                                     conversionParams.outputDimensions.y,
                                     conversionParams.outputFormat, false);
        }

        var buffer = mTexture.GetRawTextureData<byte>();
        image.Convert(conversionParams, new IntPtr(buffer.GetUnsafePtr()), buffer.Length);

        mTexture.Apply();
        mRenderer.material.mainTexture = mTexture;

        buffer.Dispose();
        image.Dispose();
    }
    IEnumerator Send(float frame)
    {
        while (true)
        {
            //RawImage.texture = ARCameraManager.s_Textures[0];


            Texture2D texture = new Texture2D(640, 480, TextureFormat.RGB565, false);

            texture.SetPixels32(mTexture.GetPixels32());
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