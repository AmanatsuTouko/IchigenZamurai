using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressAButtonToNext : MonoBehaviour
{
    public Image image;
    private bool upAlpha = true;
    private float coefficient = 150.0f;
    float a = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //“_–Å‚³‚¹‚é
        if (upAlpha)
        {
            a += Time.deltaTime * coefficient;
            if (a > 255)
            {
                a = 255;
                upAlpha = false;
            }
        }
        else
        {
            a -= Time.deltaTime * coefficient;
            if (a < 0)
            {
                a = 0;
                upAlpha = true;
            }
        }
        image.color = new Color32((byte)255, (byte)255, (byte)255, (byte)a);
    }
}
