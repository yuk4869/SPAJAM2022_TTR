using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.UI;

public class FileUploader : MonoBehaviour
{
    WebCamTexture webCam;
    public RawImage RawImage;
    //public RawImage RawImage2;

    

    void Start()
    {
        

        webCam = new WebCamTexture();
        // RawImage�̃e�N�X�`����WebCamTexture�̃C���X�^���X��ݒ�
        
        //�X�O�x��]
        Vector3 angles = RawImage.GetComponent<RectTransform>().eulerAngles;
        angles.z = -90;
        //RawImage.GetComponent<RectTransform>().eulerAngles = angles;
        //

        webCam.Play();

        //StartCoroutine(UploadFile());
        StartCoroutine(Send(0.3f));
    }

    //IEnumerator UploadFile()
    //{
    //    //string fileName = "hoge.jpg";
    //    //string filePath = Application.dataPath + "/" + fileName;
    //    // �摜�t�@�C����byte�z��Ɋi�[
    //    //byte[] img = File.ReadAllBytes(filePath);

    //    //
    //    webCam.Play();
    //    RawImage.texture = webCam;

    //    Texture2D texture = new Texture2D(webCam.width, webCam.height, TextureFormat.ARGB32, false);
    //    texture.SetPixels(webCam.GetPixels());
    //    byte[] img = texture.EncodeToJPG();
    //    Object.Destroy(texture);

    //    //byte[] img = File.ReadAllBytes(webCam.GetPixels());


    //    // form�Ƀo�C�i���f�[�^��ǉ�
    //    WWWForm form = new WWWForm();
    //    form.AddBinaryData("request_data", img, "file", "image/png");
    //    // HTTP���N�G�X�g�𑗂�
    //    UnityWebRequest request = UnityWebRequest.Post("http://127.0.0.1:5000", form);
    //    Debug.Log("Send");
    //    yield return request.SendWebRequest();

    //    if (request.isHttpError || request.isNetworkError)
    //    {
    //        // POST�Ɏ��s�����ꍇ�C�G���[���O���o��
    //        Debug.Log(request.error);
    //    }
    //    else
    //    {
    //        // POST�ɐ��������ꍇ�C���X�|���X�R�[�h���o��
    //        Debug.Log(request.responseCode);
    //    }
    //}
    
    private IEnumerator Send(float frame)
    {
        while (true)
        {
            RawImage.texture = webCam;
            

            Texture2D texture = new Texture2D(640, 480, TextureFormat.RGB565, false);
            
            texture.SetPixels32(webCam.GetPixels32());
            byte[] img = texture.EncodeToJPG();
            Debug.Log(img);
            //RawImage2.texture = texture;
            
            Object.Destroy(texture);

            //byte[] img = File.ReadAllBytes(webCam.GetPixels());


            // form�Ƀo�C�i���f�[�^��ǉ�
            WWWForm form = new WWWForm();
            form.AddBinaryData("request_data", img, "file", "image/png");
            // HTTP���N�G�X�g�𑗂�
            UnityWebRequest request = UnityWebRequest.Post("http://127.0.0.1:5000", form);
            
            Debug.Log("Send");
            request.SendWebRequest();
            yield return new WaitForSeconds(frame);

            //frame--;

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
}