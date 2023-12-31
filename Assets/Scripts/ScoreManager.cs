using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI ComboText;

    public TextMeshPro ScoreTextPlane;
    public TextMeshPro ComboTextPlane;

    public GameObject ComBoBonusTextObject;
    public TextMeshProUGUI ComboBonusText;

    public int Count_1gen = 0;
    public int Count_2gen = 0;
    public int Count_3gen = 0;

    public int score = 0;
    public int combo = 0;
    public int comboTotal = 0;
    public int comboBonus = 0;
    public int comboMax = 0;

    //リザルト用に出現した麻雀牌を記録しておく
    public int Count_1gen_spawn = 0;
    public int Count_2gen_spawn = 0;
    public int Count_3gen_spawn = 0;

    //被弾ボイス
    public AudioSource audioSource;
    public List<AudioClip> audioClips;

    //点滅
    public Image redFlash;

    public void ResetParam()
    {
        Count_1gen = 0;
        Count_2gen = 0;
        Count_3gen = 0;

        score = 0;
        combo = 0;
        comboTotal = 0;
        comboBonus = 0;
        comboMax = 0;

        Count_1gen_spawn = 0;
        Count_2gen_spawn = 0;
        Count_3gen_spawn = 0;
    }

    public IEnumerator ResetParamCoroutine()
    {
        ResetParam();
        ReloadText();
        yield return 0;
    }

    public enum HaiType
    {
        gen_1,
        gen_2,
        gen_3
    }

    private void ReloadText()
    {
        score = Count_1gen + comboTotal + comboBonus - (Count_2gen + Count_3gen)*3;
        ScoreText.text = "SCORE:" + score;
        ComboText.text = "COMBO:" + combo;


        ScoreTextPlane.text = "SCORE:" + score;
        ComboTextPlane.text = "COMBO:" + combo;
    }

    public void AddCount(HaiType haiType)
    {
        if (haiType == HaiType.gen_1)
        {
            Count_1gen++;
        }
        else if (haiType == HaiType.gen_2)
        {
            Count_2gen++;
            audioSource.clip = audioClips[Random.Range(0, 2)];
            audioSource.Play();
            StartCoroutine(Flash());
        }
        else if (haiType == HaiType.gen_3)
        {
            Count_3gen++;
            audioSource.clip = audioClips[Random.Range(0, 2)];
            audioSource.Play();
            StartCoroutine(Flash());
        }

        //Debug.Log(Count_1gen +" "+ Count_2gen + " " + Count_3gen);
        //Debug.Log(Count_1gen + "/" + Count_1gen_spawn + " " + Count_2gen + "/" + Count_2gen_spawn + " " + Count_3gen + "/" + Count_3gen_spawn);
    }

    public void AddCount_spawn(HaiType haiType)
    {
        if (haiType == HaiType.gen_1) Count_1gen_spawn++;
        else if (haiType == HaiType.gen_2) Count_2gen_spawn++;
        else if (haiType == HaiType.gen_3) Count_3gen_spawn++;

        //Debug.Log(Count_1gen + " " + Count_2gen + " " + Count_3gen);
    }

    public void AddComboCount()
    {
        combo += 1;
        Debug.Log(combo + "コンボ！");

        ReloadText();
    }

    //コンボが途絶えた際の処理
    public void StopComboUp()
    {
        //コンボ数のスコアへの加算
        comboTotal += combo;

        //コンボによるボーナスを追加
        int currentComboBonus = 0;
        if     (combo >= 100) currentComboBonus = (int)(combo * 1.00f);
        else if (combo >= 50) currentComboBonus = (int)(combo * 0.85f);
        else if (combo >= 25) currentComboBonus = (int)(combo * 0.75f);
        else if (combo >= 10) currentComboBonus = (int)(combo * 0.50f);
        comboBonus += currentComboBonus;

        //コンボボーナスの表示
        if ((currentComboBonus+combo) != 0)
        {
            StartCoroutine(DisplayComboBonus(currentComboBonus + combo));
        }

        //最大コンボ数の更新
        if(comboMax < combo)
        {
            comboMax = combo;
        }

        //コンボ数のリセット
        combo = 0;

        ReloadText();
    }

    IEnumerator DisplayComboBonus(int comboBonus)
    {
        ComboBonusText.text = "Bonus +" + comboBonus;
        ComBoBonusTextObject.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        ComBoBonusTextObject.SetActive(false);

        yield return 0;
    }

    //画面の点滅
    IEnumerator Flash()
    {
        redFlash.color = new Color32((byte)255, (byte)0, (byte)0, (byte)40);
        yield return new WaitForSeconds(0.1f);
        redFlash.color = new Color32((byte)255, (byte)0, (byte)0, (byte)0);
        yield return 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
