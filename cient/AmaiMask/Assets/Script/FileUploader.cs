using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.UI;
using System.Text;

public class FileUploader : MonoBehaviour
{
    public byte[] face;
    public byte[] emotionaValue;
    public Text ResultText_;
    public Text InputText_;
    void Start()
    {
        StartCoroutine(UploadFile());
    }

    IEnumerator UploadFile()
    {
        string fileName = "hoge.jpg";
        string filePath = Application.dataPath + "/" + fileName;
        byte[] img = File.ReadAllBytes(filePath);

        WWWForm form = new WWWForm();
        form.AddBinaryData("request_data", img, "file", "image/png");
        
        UnityWebRequest request = UnityWebRequest.Post("http://127.0.0.1:5000", form);
        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)
        {
            // POST�Ɏ��s�����ꍇ�C�G���[���O���o��
            Debug.Log(request.error);
        }
        else
        {
            // POST�ɐ��������ꍇ�C���X�|���X�R�[�h���o��
            Debug.Log(request.responseCode);
        }
    }
    private IEnumerator PostJason()
    {
        WWWForm form = new WWWForm();
        WWW www = new WWW("http://127.0.0.1:5000", form);

        if (www.error != null)
        {
            Debug.Log("HttpPost NG: " + www.error);
            //そもそも接続ができていないとき

        }
        else if (www.isDone)
        {
            //送られてきたデータをテキストに反映
            ResultText_.GetComponent<Text>().text = www.text;
        }
        yield return www.text;
    }
}