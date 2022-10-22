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
        // �摜�t�@�C����byte�z��Ɋi�[
        byte[] img = File.ReadAllBytes(filePath);

        // form�Ƀo�C�i���f�[�^��ǉ�
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", img, fileName, "image/jpeg");
        // HTTP���N�G�X�g�𑗂�
        UnityWebRequest request = UnityWebRequest.Post("http://192.168.0.10/upload.php", form);
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