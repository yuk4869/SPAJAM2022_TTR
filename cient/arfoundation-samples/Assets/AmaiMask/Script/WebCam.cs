using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCam : MonoBehaviour
{
    public RawImage rawImage;
    WebCamTexture webCam;

    void Start()
    {
        // WebCamTextureのインスタンスを生成
        webCam = new WebCamTexture();
        // RawImageのテクスチャにWebCamTextureのインスタンスを設定
        rawImage.texture = webCam;
        Vector3 angles = rawImage.GetComponent<RectTransform>().eulerAngles;
        angles.z = -90;

        rawImage.GetComponent<RectTransform>().eulerAngles = angles;
        // カメラ表示開始
        webCam.Play();
    }
}