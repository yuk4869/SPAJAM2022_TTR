using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Text;

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
}