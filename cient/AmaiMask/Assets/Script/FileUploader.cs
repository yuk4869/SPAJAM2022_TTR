using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class FileUploader : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(UploadFile());
    }

    IEnumerator UploadFile()
    {
        string fileName = "hoge.jpg";
        string filePath = Application.dataPath + "/" + fileName;
        // 画像ファイルをbyte配列に格納
        byte[] img = File.ReadAllBytes(filePath);

        // formにバイナリデータを追加
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", img, fileName, "image/jpeg");
        // HTTPリクエストを送る
        UnityWebRequest request = UnityWebRequest.Post("http://192.168.0.10/upload.php", form);
        yield return request.SendWebRequest();

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