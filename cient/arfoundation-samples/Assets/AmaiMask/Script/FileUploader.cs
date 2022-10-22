using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.UI;

public class FileUploader : MonoBehaviour
{
    WebCamTexture webCam;
    public RawImage RawImage;

    

    void Start()
    {
        

        webCam = new WebCamTexture();
        // RawImageのテクスチャにWebCamTextureのインスタンスを設定
        
        //９０度回転
        Vector3 angles = RawImage.GetComponent<RectTransform>().eulerAngles;
        angles.z = -90;
        RawImage.GetComponent<RectTransform>().eulerAngles = angles;
        //

        webCam.Play();

        //StartCoroutine(UploadFile());
        StartCoroutine(Send(10));
    }

    //IEnumerator UploadFile()
    //{
    //    //string fileName = "hoge.jpg";
    //    //string filePath = Application.dataPath + "/" + fileName;
    //    // 画像ファイルをbyte配列に格納
    //    //byte[] img = File.ReadAllBytes(filePath);

    //    //
    //    webCam.Play();
    //    RawImage.texture = webCam;

    //    Texture2D texture = new Texture2D(webCam.width, webCam.height, TextureFormat.ARGB32, false);
    //    texture.SetPixels(webCam.GetPixels());
    //    byte[] img = texture.EncodeToJPG();
    //    Object.Destroy(texture);
 
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
    private IEnumerator Send(int frame)
    {
        while (frame > 0)
        {

            RawImage.texture = webCam;

            Texture2D texture = new Texture2D(webCam.width, webCam.height, TextureFormat.ARGB32, false);
            texture.SetPixels(webCam.GetPixels());
            byte[] img = texture.EncodeToJPG();
            Object.Destroy(texture);

            //byte[] img = File.ReadAllBytes(webCam.GetPixels());


            // formにバイナリデータを追加
            WWWForm form = new WWWForm();
            form.AddBinaryData("request_data", img, "file", "image/png");
            // HTTPリクエストを送る
            UnityWebRequest request = UnityWebRequest.Post("http://127.0.0.1:5000", form);
            
            Debug.Log("Send");
            yield return request.SendWebRequest();

            frame--;

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