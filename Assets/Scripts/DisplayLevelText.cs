using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayLevelText : MonoBehaviour
{
    public GameObject textBase;
    public Image blackImg;
    public TextMeshProUGUI text;

    //���ʉ���炷
    public AudioSource audioSource_1;
    public AudioSource audioSource_2;
    public AudioSource audioSource_3;
    public AudioSource audioSource_4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Generate(string displayText)
    {
        float a = 0.0f;
        float coefficient = 2400.0f;

        textBase.SetActive(true);
        text.text = displayText;

        //���X�ɔZ������
        while (true)
        {
            yield return new WaitForSeconds(0.05f);

            a += Time.deltaTime * coefficient;

            if (a > 255)
            {
                a = 255;
                blackImg.color = new Color32((byte)0, (byte)0, (byte)0, (byte)a);
                text.color = new Color32((byte)255, (byte)255, (byte)255, (byte)a);
                break;
            }
            blackImg.color = new Color32((byte)0, (byte)0, (byte)0, (byte)a);
            text.color = new Color32((byte)255, (byte)255, (byte)255, (byte)a);
        }

        //���ʉ���炷
        if (displayText == "�I���I") { audioSource_3.Play(); }
        else { audioSource_1.Play(); }

        yield return new WaitForSeconds(1.0f);

        //���ʉ���炷
        if (displayText == "�I���I") { audioSource_4.Play(); }
        else { audioSource_2.Play(); }

        //���X�ɔ�������
        while (true)
        {
            yield return new WaitForSeconds(0.05f);

            a -= Time.deltaTime * coefficient;

            if (a < 0)
            {
                a = 0;
                blackImg.color = new Color32((byte)0, (byte)0, (byte)0, (byte)a);
                text.color = new Color32((byte)255, (byte)255, (byte)255, (byte)a);
                break;
            }
            blackImg.color = new Color32((byte)0, (byte)0, (byte)0, (byte)a);
            text.color = new Color32((byte)255, (byte)255, (byte)255, (byte)a);
        }


        textBase.SetActive(false);
        text.text = "";

        yield return 0;
    }
}
