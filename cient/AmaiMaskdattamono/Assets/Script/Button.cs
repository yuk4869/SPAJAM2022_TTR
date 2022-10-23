using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.IO;

public class Button : MonoBehaviour
{
    private AndroidJavaObject _javaClass = null;
    private List<Vector2> _prevSizeList = new List<Vector2>();
    private List<List<Vector2>> _deviceList = new List<List<Vector2>>();
    WebCamTexture webCam;
    public RawImage RawImage;

    void Start()
    {
        string sizelist;
        int[] orientation;
        float scale;

        //ライブラリの初期化
        _javaClass = new AndroidJavaObject("com.example.getcam.GetCamParameter");

        // プレビューサイズリストの取得
        sizelist = _javaClass.Call<string>("GetPreviewSize");

        // XMLをリストに変換
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(sizelist));
        XmlNode root = xmlDoc.FirstChild;
        XmlNodeList talkList = xmlDoc.GetElementsByTagName("device");
        Vector2 tmpsize;
        foreach (XmlNode devtmp in talkList)
        {
            _prevSizeList.Clear();
            XmlNodeList nodelist = devtmp.ChildNodes;
            foreach (XmlNode s in nodelist)
            {
                tmpsize.x = float.Parse(s.Attributes["Width"].Value);
                tmpsize.y = float.Parse(s.Attributes["Height"].Value);
                _prevSizeList.Add(tmpsize);
            }
            _deviceList.Add(_prevSizeList);
        }

        // カメラの取り付け向き取得
        orientation = _javaClass.Call<int[]>("GetOrientation");

        // UnityのWebCamTextureでカメラリスト取得
        WebCamDevice[] devices = WebCamTexture.devices;
        // カメラ0を起動させる
        webCam = new WebCamTexture(devices[0].name);
        // RawImageのテクスチャにWebCamTextureのインスタンスを設定
        RawImage.texture = webCam;
        // 縦横のサイズを要求（_deviceListからorientationを考慮して決定する。ここでは直値）
        webCam.requestedWidth = 4032;
        webCam.requestedHeight = 3024;
        // カメラ起動
        webCam.Play();
        // 起動させて初めてvideoRotationAngle、width、heightに値が入り、
        // アスペクト比、何度回転させれば正しく表示されるかがわかる
        if ((webCam.videoRotationAngle == 90) || (webCam.videoRotationAngle == 270))
        {
            // 表示するRawImageを回転させる
            Vector3 angles = RawImage.GetComponent<RectTransform>().eulerAngles;
            angles.z = -webCam.videoRotationAngle;
            RawImage.GetComponent<RectTransform>().eulerAngles = angles;
        }
        // 回転済みなので、widthはx、heightはyでそのままサイズ調整
        // 全体を表示させるように、大きい方のサイズを表示枠に合わせる
        Vector2 sizetmp = RawImage.GetComponent<RectTransform>().sizeDelta;
        if (webCam.width > webCam.height)
        {
            scale = sizetmp.x / webCam.width;
        }
        else
        {
            scale = sizetmp.y / webCam.height;
        }
        sizetmp.x = webCam.width * scale;
        sizetmp.y = webCam.height * scale;

        RawImage.GetComponent<RectTransform>().sizeDelta = sizetmp;
    }
}