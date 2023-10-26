using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    //Aボタンの押下を検知する
    //データの流れは StartPoint -> GameManager -> Title
    public GameManager gameManager;

    public GameObject title_logo;
    
    //モード選択
    //0:非選択　1:チュートリアル 2:そのままスタート
    private int select_mode = 0;

    public GameObject mode_select_obj;
    public RectTransform pointerImage;
    public GameObject selected_frame;
    public RectTransform selected_frame_pos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            select_mode = 1;
            selected_frame_pos.anchoredPosition = new Vector3(-450, -200, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            select_mode = 2;
            selected_frame_pos.anchoredPosition = new Vector3(450, -200, 0);
        }
    }

    public IEnumerator Generate()
    {
        //タイトルの表示
        title_logo.SetActive(true);

        //Aボタン入力を待機する
        while (true)
        {
            yield return new WaitForSeconds(0.03f);
            if (gameManager.ButtonDown_A == true)
            {
                gameManager.ButtonDown_A = false;
                break;
            }
        }

        //タイトルの非表示
        title_logo.SetActive(false);

        //効果音を鳴らす


        //人数選択を表示

        //チュートリアルの有無を選択
        mode_select_obj.SetActive(true);

        //Aボタン入力を待機する
        gameManager.ButtonDown_A = false;
        while (true)
        {
            yield return new WaitForSeconds(0.03f);
            if (gameManager.ButtonDown_A == true && select_mode != 0)
            {
                gameManager.ButtonDown_A = false;
                break;
            }
            else if(select_mode == 0)
            {
                gameManager.ButtonDown_A = false;
            }

            //X:50~850  Y:40~-460
            //ポインターカーソルの位置によって、選択を変える
            int posX = (int)pointerImage.anchoredPosition.x;
            int posY = (int)pointerImage.anchoredPosition.y;
            if (-460 < posY && posY < 40)
            {
                //左の選択
                if (-850 < posX && posX < -50)
                {
                    select_mode = 1;
                    //Debug.Log("チュートリアルを選択中");
                    //赤枠を移動させる
                    selected_frame_pos.anchoredPosition = new Vector3(-450, -200, 0);
                }
                //右の選択
                else if (50 < posX && posX < 850)
                {
                    select_mode = 2;
                    //Debug.Log("そのままスタートを選択中");
                    //赤枠を移動させる
                    selected_frame_pos.anchoredPosition = new Vector3(450, -200, 0);
                }
            }

            if(select_mode == 0)
            {
                selected_frame.SetActive(false);
            }
            else
            {
                selected_frame.SetActive(true);
            }
        }
        //チュートリアル選択情報をGameManagerに送る
        yield return StartCoroutine(gameManager.SetTutorial(select_mode));

        //初期状態にリセット
        select_mode = 0;
        selected_frame.SetActive(false);

        //チュートリアル選択UIを非表示
        mode_select_obj.SetActive(false);

        yield return 0;
    }
}
