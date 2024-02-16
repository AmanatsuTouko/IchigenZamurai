using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Result : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject resultPaper;

    public ScoreManager scoreManager;
    public List<TextMeshProUGUI> result_texts;
    /*
    0:一限
    1:二限
    2:三限
    3:GPA
    4:MaxCombo
    5:Score
    6:総合評価
    7;コメント
    */

    public GameObject pressAToNext;
    public GameObject pressAToTitle;

    //今回セーブする値
    //Rankingクラスで最初にセーブを行う
    public static int SCORE = 999;
    public static int MAX_COMBO = 999;
    public static int GPA = 999;

    public IEnumerator Generate()
    {
        float WaitTimes = 1.0f;

        //ボタン入力の初期化
        gameManager.ButtonDown_A = false;
        //コンボ数の清算
        scoreManager.StopComboUp();

        //成績通知表の表示
        resultPaper.SetActive(true);

        //値の代入
        yield return new WaitForSeconds(WaitTimes);
        SoundManager.Instance.Play(SoundManager.SE.Result_Pon);
        result_texts[0].text = scoreManager.Count_1gen + "/" + scoreManager.Count_1gen_spawn + " 単位";

        yield return new WaitForSeconds(WaitTimes);
        SoundManager.Instance.Play(SoundManager.SE.Result_Pon);
        result_texts[1].text = scoreManager.Count_2gen + "/" + scoreManager.Count_2gen_spawn + " 単位";

        yield return new WaitForSeconds(WaitTimes);
        SoundManager.Instance.Play(SoundManager.SE.Result_Pon);
        result_texts[2].text = scoreManager.Count_3gen + "/" + scoreManager.Count_3gen_spawn + " 単位";

        //GPA
        int SlashAll = scoreManager.Count_1gen + scoreManager.Count_2gen + scoreManager.Count_3gen;
        int RespawnAll = scoreManager.Count_1gen_spawn + scoreManager.Count_2gen_spawn + scoreManager.Count_3gen_spawn;
        float gpa = ((float)SlashAll / (float)RespawnAll) * 4.0f;
        int gpa_3 = (int)(gpa * 100);
        gpa = 4.0f - (float)gpa_3 / 100;
        yield return new WaitForSeconds(WaitTimes);
        SoundManager.Instance.Play(SoundManager.SE.Result_Pon);
        result_texts[3].text = gpa.ToString("f2");

        //COMBO
        yield return new WaitForSeconds(WaitTimes);
        SoundManager.Instance.Play(SoundManager.SE.Result_Pon);
        result_texts[4].text = scoreManager.comboMax.ToString();

        //SCORE
        yield return new WaitForSeconds(WaitTimes);
        SoundManager.Instance.Play(SoundManager.SE.Result_Pon);
        result_texts[5].text = scoreManager.score.ToString();

        //仮保存
        SCORE = scoreManager.score;
        MAX_COMBO = scoreManager.comboMax;
        GPA = (int)(gpa*100);

        //Aボタンで次に進むの表示
        //pressAToNext.SetActive(true);

        //Aボタンで次に進むを待機する
        //yield return StartCoroutine(WaitButtonDown_A());

        yield return new WaitForSeconds(2.0f);

        //効果音を鳴らす
        SoundManager.Instance.Play(SoundManager.SE.Result_Explosion);

        //pressAToNext.SetActive(false);

        //ここ調整の必要あり
        //総合評価の表示
        string evaluation = "";
        string comment = "";
        if(scoreManager.score > 120)
        {
            evaluation = "卒業";
            comment = "一限を切って効率よく単位を取得できたぞ！やったな！";
        }else if (scoreManager.score > 100)
        {
            evaluation = "休学";
            comment = "とても惜しい、修行してもう一回チャレンジだ！";
        }
        else if (scoreManager.score > 80)
        {
            evaluation = "留年";
            comment = "少し二限と三限を切りすぎてしまったな……";
        }
        else
        {
            evaluation = "退学";
            comment = "必要なものまで全て切り捨ててしまったようだ……";
        }

        //総合評価とコメントの表示
        result_texts[6].text = evaluation;
        result_texts[7].text = comment;

        //Aボタンでゲームを終了するの表示
        //pressAToTitle.SetActive(true);
        pressAToNext.SetActive(true);

        //Aボタンで次に進むを待機する
        yield return StartCoroutine(WaitButtonDown_A());


        //終了、タイトルに戻る前に初期化する
        //pressAToTitle.SetActive(false);
        pressAToNext.SetActive(false);

        for (int i=0; i<result_texts.Count; i++)
        {
            result_texts[i].text = "";
        }
        //成績通知表の非表示
        resultPaper.SetActive(false);

        yield return 0;
    }

    IEnumerator WaitButtonDown_A()
    {
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
        yield return 0;
    }
}
