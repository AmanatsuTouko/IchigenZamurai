using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pattern_01: MonoBehaviour
{
    public GameObject gen1;
    private int count = 12; //作成する麻雀牌の個数

    public ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine("Generate");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //外部クラスから呼べるようにする
    public IEnumerator Generate()
    {
        this.transform.localRotation = Quaternion.Euler(0, 180, 0);

        float prePosX = 1.0f;
        float posX = 0.0f;

        for(int i=0; i<count; i++)
        {
            //同じX座標に麻雀牌が作成されないようにする
            while (true)
            {
                posX = (int)Random.Range(-3, 3) * 2.5f;
                if(posX != prePosX)
                {
                    prePosX = posX;
                    break;
                }
            }
            Vector3 vec = new Vector3(posX, 0, 0);

            GameObject gameObject = Instantiate(gen1, transform);
            gameObject.transform.localPosition = vec;

            //出現数のカウント
            scoreManager.AddCount_spawn(ScoreManager.HaiType.gen_1);

            yield return new WaitForSeconds(2.0f);
        }

        yield return new WaitForSeconds(10.0f);
        
        yield return 0;
    }
}
