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

        //���C�u�����̏�����
        _javaClass = new AndroidJavaObject("com.example.getcam.GetCamParameter");

        // �v���r���[�T�C�Y���X�g�̎擾
        sizelist = _javaClass.Call<string>("GetPreviewSize");

        // XML�����X�g�ɕϊ�
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

        // �J�����̎��t�������擾
        orientation = _javaClass.Call<int[]>("GetOrientation");

        // Unity��WebCamTexture�ŃJ�������X�g�擾
        WebCamDevice[] devices = WebCamTexture.devices;
        // �J����0���N��������
        webCam = new WebCamTexture(devices[0].name);
        // RawImage�̃e�N�X�`����WebCamTexture�̃C���X�^���X��ݒ�
        RawImage.texture = webCam;
        // �c���̃T�C�Y��v���i_deviceList����orientation���l�����Č��肷��B�����ł͒��l�j
        webCam.requestedWidth = 4032;
        webCam.requestedHeight = 3024;
        // �J�����N��
        webCam.Play();
        // �N�������ď��߂�videoRotationAngle�Awidth�Aheight�ɒl������A
        // �A�X�y�N�g��A���x��]������ΐ������\������邩���킩��
        if ((webCam.videoRotationAngle == 90) || (webCam.videoRotationAngle == 270))
        {
            // �\������RawImage����]������
            Vector3 angles = RawImage.GetComponent<RectTransform>().eulerAngles;
            angles.z = -webCam.videoRotationAngle;
            RawImage.GetComponent<RectTransform>().eulerAngles = angles;
        }
        // ��]�ς݂Ȃ̂ŁAwidth��x�Aheight��y�ł��̂܂܃T�C�Y����
        // �S�̂�\��������悤�ɁA�傫�����̃T�C�Y��\���g�ɍ��킹��
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