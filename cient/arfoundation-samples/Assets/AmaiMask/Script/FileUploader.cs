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


        // RawImage�̃e�N�X�`����WebCamTexture�̃C���X�^���X��ݒ�
        //WebCamDevice[] webCamDevice = WebCamTexture.devices;
        //webCam = new WebCamTexture(webCamDevice[1].name);


        //�X�O�x��]
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
        // TryGetLatestImage�ōŐV�̃C���[�W���擾���܂��B
        // �������A���s�̉\�������邽�߁Afalse���Ԃ��ꂽ�ꍇ�͖������܂��B
        if (!_cameraManager.TryGetLatestImage(out XRCpuImage cameraImage)) return;

        // ����

        // �f�o�C�X�̉�]�ɉ����ăJ�����̏���ϊ����邽�߂̏����`���܂��B
        XRCpuImage.Transformation imageTransformation = (Input.deviceOrientation == DeviceOrientation.LandscapeRight)
            ? XRCpuImage.Transformation.MirrorY
            : XRCpuImage.Transformation.MirrorX;

        // �J�����C���[�W���擾���邽�߂̃p�����[�^��ݒ肵�܂��B
        XRCpuImage.ConversionParams conversionParams =
            new XRCpuImage.ConversionParams(cameraImage, TextureFormat.RGBA32, imageTransformation);

        // �����ς݂�Texture2D�i_texture�j�̃l�C�e�B�u�̃f�[�^�z��̎Q�Ƃ𓾂܂��B
        NativeArray<byte> rawTextureData = _texture.GetRawTextureData<byte>();

        try
        {
            unsafe
            {
                // �O�i�œ���NativeArray�̃|�C���^��n���A���ڃf�[�^�𗬂����݂܂��B
                cameraImage.Convert(conversionParams, new IntPtr(rawTextureData.GetUnsafePtr()), rawTextureData.Length);
            }
        }
        finally
        {
            cameraImage.Dispose();
        }

        // �擾�����f�[�^��K�p���܂��B
        _texture.Apply();

        // �㗪
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
    //        // �摜�t�@�C����byte�z��Ɋi�[
    //        //byte[] img = File.ReadAllBytes(filePath);

    //        //
    //        //webCam.Play();
    //    //RawImage.texture = webCam;

    //    Texture2D texture = new Texture2D(webCam.width, webCam.height, TextureFormat.ARGB32, false);
    //    texture.SetPixels(_texture.GetPixels());
    //    byte[] img = texture.EncodeToJPG();
    //    UnityEngine.Object.Destroy(texture);

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