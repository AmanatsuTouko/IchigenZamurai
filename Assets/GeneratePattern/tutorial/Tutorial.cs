using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject gen;

    Vector3[,] Pos = new Vector3[5, 3]; //出現位置の配列
    private bool[,] alreadyPos = new bool[5, 3]; //既に出現している位置の配列

    public GameManager gameManager;

    //4秒長押しでチュートリアルを終えるようにするためのカウント
    private int buttonADownTimes = 0;

    //長押しのカウント表示用のUI
    public List<Image> timesImages;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator ResetPos()
    {
        buttonADownTimes = 0;

        this.transform.localRotation = Quaternion.Euler(0, 180, 0);

        //配置をListに保存しておく
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                float posX = (i - 2) * -3.5f;
                float posY = (j - 1) * -3.5f;
                if (i == 1 || i == 3)
                {
                    if (j == 0) posY = 2.0f;
                    if (j == 1) posY = -2.0f;
                    if (j == 2) continue;
                }
                Pos[i, j] = new Vector3(posX, posY, 0);

                /*
                GameObject gameObject = Instantiate(gens[0], transform);
                gameObject.transform.localPosition = Pos[i, j];
                alreadyPos[i, j] = true;
                */
            }
        }
        Pos[1, 2] = new Vector3(0, 0, -5);
        Pos[3, 2] = new Vector3(0, 0, -5);

        //初期値の設定
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                alreadyPos[i, j] = false;
            }
        }

        yield return 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Generate()
    {
        //初期化
        yield return StartCoroutine(ResetPos());

        bool generate = true;
        int generateCount = 0;

        //切られた位置のみ復活する
        while (true)
        {
            //生成のみ2秒おきにする。コルーチンの回る秒数は短くする
            if (generate)
            {
                //埋まっていない場所を求めて生成する
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if ((i == 1 || i == 3) && j == 2) continue;

                        if (alreadyPos[i, j] == false)
                        {
                            alreadyPos[i, j] = true;

                            //生成処理
                            GameObject gameObject = Instantiate(gen, transform);
                            gameObject.transform.localPosition = Pos[i, j];
                            gameObject.transform.localScale = new Vector3(0, 0, 0);

                            //牌自身にどこにいるかの情報を持たせる
                            Tutoria_hai hai = gameObject.GetComponent<Tutoria_hai>();
                            hai.pos = new Vector2Int(i, j);
                            hai.tutorial = this;
                        }
                    }
                }
                generate = false;
            }

            generateCount += 1;
            if(generateCount > 3)
            {
                generateCount = 0;
                generate = true;
            }

            //2秒おきに復活する
            //yield return new WaitForSeconds(2.0f);

            yield return new WaitForSeconds(0.5f);


            //終了処理
            if (gameManager.ButtonDown_A == true)
            {
                buttonADownTimes += 1;
            }
            else
            {
                buttonADownTimes = 0;
            }

            // Debug.Log(buttonADownTimes);

            //秒数に応じて長押しの色を変える
            if(buttonADownTimes == 1)
            {
                timesImages[0].color = new Color32((byte)0, (byte)0, (byte)0, (byte)255);
                timesImages[1].color = new Color32((byte)120, (byte)120, (byte)120, (byte)255);
                timesImages[2].color = new Color32((byte)120, (byte)120, (byte)120, (byte)255);
            }
            else if (buttonADownTimes == 2)
            {
                timesImages[0].color = new Color32((byte)0, (byte)0, (byte)0, (byte)255);
                timesImages[1].color = new Color32((byte)0, (byte)0, (byte)0, (byte)255);
                timesImages[2].color = new Color32((byte)120, (byte)120, (byte)120, (byte)255);
            }
            else if(buttonADownTimes == 3)
            {
                timesImages[0].color = new Color32((byte)0, (byte)0, (byte)0, (byte)255);
                timesImages[1].color = new Color32((byte)0, (byte)0, (byte)0, (byte)255);
                timesImages[2].color = new Color32((byte)0, (byte)0, (byte)0, (byte)255);
            }
            else if (buttonADownTimes == 0)
            {
                timesImages[0].color = new Color32((byte)120, (byte)120, (byte)120, (byte)255);
                timesImages[1].color = new Color32((byte)120, (byte)120, (byte)120, (byte)255);
                timesImages[2].color = new Color32((byte)120, (byte)120, (byte)120, (byte)255);
            }
            
            //終了条件
            if (buttonADownTimes > 3)
            {
                gameManager.ButtonDown_A = false;
                DestroyAllHai();
                //yield return new WaitForSeconds(0.5f);
                break;
            }

        }
        yield return 0;
    }

    private void DestroyAllHai()
    {
        foreach (Transform n in this.gameObject.transform)
        {
            GameObject.Destroy(n.gameObject);
        }
    }

    public void deleteAlreadyPos(Vector2Int pos)
    {
        alreadyPos[pos.x, pos.y] = false;
    }
}
