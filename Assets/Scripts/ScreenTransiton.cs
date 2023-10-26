using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransiton : MonoBehaviour
{
    public Image blackScreen;
    private float coefficient = 2400.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Generate()
    {
        //トランジションのアニメーション用の移動
        //徐々にスピードを減速する
        float a = 0.0f;
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            
            a += Time.deltaTime * coefficient;
            
            if(a > 255)
            {
                a = 255;
                blackScreen.color = new Color32((byte)0, (byte)0, (byte)0, (byte)a);
                break;
            }
            blackScreen.color = new Color32((byte)0, (byte)0, (byte)0, (byte)a);
        }

        yield return new WaitForSeconds(1.0f);

        while (true)
        {
            yield return new WaitForSeconds(0.05f);

            a -= Time.deltaTime * coefficient;

            if (a < 0)
            {
                a = 0;
                blackScreen.color = new Color32((byte)0, (byte)0, (byte)0, (byte)a);
                break;
            }
            blackScreen.color = new Color32((byte)0, (byte)0, (byte)0, (byte)a);
        }

        yield return 0;
    }
}
