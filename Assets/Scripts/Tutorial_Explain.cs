using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial_Explain : MonoBehaviour
{
    public GameObject SamuraiTextWindow;
    public TextMeshProUGUI textWindow;

    private List<string> textList = new List<string>();
    private int nowTextNum = 0;

    //A?{?^??????
    public GameManager gameManager;

    //????????
    public GameObject explainImg_1;
    public GameObject explainImg_2;
    public GameObject explainImg_3;

    public GameObject pressAButton;

    public GameObject pressAButtonToFinishTestSlash;

    public Tutorial tutorial;

    // Start is called before the first frame update
    void Start()
    {
        textWindow.text = "";
        textList.Add("私は一限侍。\n一限を切ることを生業としている。");
        textList.Add("人生は麻雀なり。\n何を取捨選択するかがカギとなる。");
        textList.Add("む、貴様は新入りだな？\n早速練習を始めるぞ！！");

        textList.Add("心得は身に着けたな？満足するまで\n試し斬りしていくといい！");

        textList.Add("む、今日も一限に苦しむ\n大学生の声が聞こえる！");
        textList.Add("……いざ、参らん！！！");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetParam()
    {
        //textWindow.text = "";
        nowTextNum = 0;
    }

    public IEnumerator Generate()
    {
        //?e?L?X?g?E?B???h?E???\??
        SamuraiTextWindow.SetActive(true);

        //?e?L?X?g???\??
        while(nowTextNum < 3)
        {
            //N???????e?L?X?g?\??
            yield return StartCoroutine(textShow());
            //A?{?^???????????@
            yield return StartCoroutine(WaitButtonDown_A());
        }

        //?e?L?X?g?E?B???h?E?????\??
        SamuraiTextWindow.SetActive(false);

        //A?{?^?????????i?????\??
        pressAButton.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        //???????????????\??
        explainImg_1.SetActive(true);
        yield return StartCoroutine(WaitButtonDown_A());
        explainImg_1.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        explainImg_2.SetActive(true);
        yield return StartCoroutine(WaitButtonDown_A());
        explainImg_2.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        explainImg_3.SetActive(true);
        yield return StartCoroutine(WaitButtonDown_A());
        explainImg_3.SetActive(false);

        //A?{?^?????????i???????\??
        pressAButton.SetActive(false);


        //?e?L?X?g?E?B???h?E???\??
        SamuraiTextWindow.SetActive(true);
        textWindow.text = "";


        yield return StartCoroutine(textShow());
        yield return StartCoroutine(WaitButtonDown_A());

        //?e?L?X?g??????
        textWindow.text = "";
        //?e?L?X?g?E?B???h?E?????\??
        SamuraiTextWindow.SetActive(false);
        //?{?^?????I?t
        gameManager.ButtonDown_A = false;

        //A?{?^?????????a?????I???????\??
        pressAButtonToFinishTestSlash.SetActive(true);
        //?`???[?g???A???J?n
        yield return StartCoroutine(tutorial.Generate());
        pressAButtonToFinishTestSlash.SetActive(false);

        //?e?L?X?g?\??
        SamuraiTextWindow.SetActive(true);
        yield return StartCoroutine(textShow());
        yield return StartCoroutine(WaitButtonDown_A());
        yield return StartCoroutine(textShow());
        yield return StartCoroutine(WaitButtonDown_A());
        textWindow.text = "";
        SamuraiTextWindow.SetActive(false);
        gameManager.ButtonDown_A = false;

        //??????
        ResetParam();

        yield return 0;
    }

    IEnumerator textShow()
    {
        int MaxLength = textList[nowTextNum].Length;
        int index = 0;

        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            index += 1;
            textWindow.text = textList[nowTextNum].Substring(0, index);
            if(index >= MaxLength)
            {
                break;
            }
        }
        gameManager.ButtonDown_A = false;
        nowTextNum += 1;
        yield return 0;
    }

    IEnumerator WaitButtonDown_A()
    {
        //A?{?^???????????@????
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
