using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cong : MonoBehaviour
{
    private GameObject im;
    Image img;
    public GameObject am;
    // Start is called before the first frame update
    void Start()
    {

        im = GameObject.Find("Image");
        img = im.GetComponent<Image>();
        am.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(img.fillAmount > 0.8)
        {
            am.gameObject.SetActive(true);
        }
    }
}
