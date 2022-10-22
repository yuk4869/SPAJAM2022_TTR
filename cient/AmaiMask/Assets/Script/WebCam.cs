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
        // WebCamTexture�̃C���X�^���X�𐶐�
        webCam = new WebCamTexture();
        // RawImage�̃e�N�X�`����WebCamTexture�̃C���X�^���X��ݒ�
        rawImage.texture = webCam;
        Vector3 angles = rawImage.GetComponent<RectTransform>().eulerAngles;
        angles.z = -90;

        rawImage.GetComponent<RectTransform>().eulerAngles = angles;
        // �J�����\���J�n
        webCam.Play();
    }
}