using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayLevelText : MonoBehaviour
{
    public GameObject textBase;
    public Image blackImg;
    public TextMeshProUGUI text;

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
        float betweenSec = 1.0f;
        // �I������
        if(displayText == "�I���I")
        {
            SoundManager.Instance.Play(SoundManager.SE.DisplayLevel_Kakaxtu);
            yield return new WaitForSeconds(betweenSec);
            SoundManager.Instance.Play(SoundManager.SE.DisplayLevel_ChakiEnd);
        }
        // �J�n����
        else
        {
            SoundManager.Instance.Play(SoundManager.SE.DisplayLevel_Dodon);
            yield return new WaitForSeconds(betweenSec);
            SoundManager.Instance.Play(SoundManager.SE.DisplayLevel_Chaki);
        }

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
