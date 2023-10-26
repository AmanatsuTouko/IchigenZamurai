using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO; //System.IO.FileInfo, System.IO.StreamReader, System.IO.StreamWriter
using System; //Exception
using System.Text; //Encoding
using TMPro;
using UnityEngine.UI;

using System.Linq;

public class Ranking : MonoBehaviour
{
    public GameManager gameManager;

    string writeFileScore = "score.txt";
    string writeFileCombo = "combo.txt";
    string writeFileDetail = "detail.txt";

    public GameObject rankings;
    public List<TextMeshProUGUI> scoreText = new List<TextMeshProUGUI>();
    //public List<TextMeshProUGUI> comboText = new List<TextMeshProUGUI>();

    public IEnumerator Generate()
    {
        yield return StartCoroutine(SaveAndDisplayTexts());

        //データの読み込みと反映
        UpdateScoreText();

        //Aボタン入力を待機する
        yield return StartCoroutine(WaitButtonDown_A());

        //非表示
        rankings.SetActive(false);

        yield return 0;
    }

    IEnumerator SaveAndDisplayTexts()
    {
        //今回の値のセーブ
        string writeLine = Result.SCORE.ToString() + "," + Result.MAX_COMBO.ToString() + "," + Result.GPA.ToString();
        WriteFile(writeFileScore, writeLine);

        //TMPro群の表示
        rankings.SetActive(true);
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

    // Start is called before the first frame update
    void Start()
    {
        //書き込み
        //WriteFile(writeFileScore, 250);

        //string writeText = "200, 24, 190, 210, 80";
        //WriteFile(writeFileDetail, writeText);

        //読み込んで、反映させる
        //UpdateScoreText();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //セーブロード処理とテキストへの反映処理

    //ファイルからデータを読み込んで画面に反映させる関数
    void UpdateScoreText()
    {
        try
        {
            string[] score;
            score = GetArrayFromFile(writeFileScore);

            //Textオブジェクトに反映
            for (int i = 0; i < score.Length; i++)
            {
                scoreText[i].text = score[i];
            }
        }
        catch (FileNotFoundException e)
        {
            throw;
        }
    }

    //ファイルへの書き込み関数
    void WriteFile(string FileName, string WriteText)
    {
        try
        {
            File.AppendAllText(GetReadPath(FileName), WriteText + "\n");
        }
        catch (FileNotFoundException e)
        {
            throw;
        }
    }

    //読み込むパスを返す関数
    string GetReadPath(string FileName)
    {
        //プラットフォームによって読み込むパスを変える。
        string FilePath = "";

        //MacOSのエディタの場合
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            FilePath = Application.dataPath + "/RankingData/" + FileName;
        }
        //MacOSのAppの場合
        else if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            FilePath = Application.dataPath + "/../../RankingData/" + FileName;
        }
        //WindowsOSのエディタの場合
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            FilePath = Application.dataPath + "/RankingData/" + FileName;
        }
        //WindowsOSのexeの場合
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            FilePath = Application.dataPath + "/RankingData/" + FileName;
        }
        return FilePath;
    }

    //読み込み関数
    //FileNameを引数にして、ソートした後の上から20個だけを返してくれる関数
    //引数：ファイルのパス(.txt)
    string[] GetArrayFromFile(string FileName)
    {
        try
        {
            List<int> scoreList = new List<int>();
            List<int> maxComboList = new List<int>();
            List<int> GPAList = new List<int>();

            //Stringでの読み込み
            string[] scoreStrings = File.ReadAllLines(GetReadPath(FileName));

            //,区切りで3つの配列に代入
            for(int i=0; i<scoreStrings.Length; i++)
            {
                List<string> tokens = scoreStrings[i].Split(',').ToList();
                scoreList.Add(Int32.Parse(tokens[0]));
                maxComboList.Add(Int32.Parse(tokens[1]));
                GPAList.Add(Int32.Parse(tokens[2]));
            }

            //昇順にソート
            bubbleSort_3(scoreList, maxComboList, GPAList, scoreStrings.Length);


            int ReturnArrayLength = 0;
            if (scoreStrings.Length < 20)
            {
                ReturnArrayLength = scoreStrings.Length;
            }
            else
            {
                ReturnArrayLength = 20;
            }

            //返り値となる文字列配列
            string[] returnArray = new string[20];
            //初期化
            for(int i=0; i<20; i++)
            {
                string ranksPadding = "";
                if (i < 9) ranksPadding = " ";
                returnArray[i] = (i+1).ToString() + "位" + ranksPadding + "         0              0　　　  0.00";
            }

            //大きい順に上から20個だけ持ってくる。
            for (int i = 0; i < ReturnArrayLength; i++)
            {
                float gpa = (float)GPAList[i] / 100;
                string gpaText = gpa.ToString("F");

                string scorePadding = "";
                if (scoreList[i].ToString().Length == 1) scorePadding = "  ";
                if (scoreList[i].ToString().Length == 2) scorePadding = " ";

                string comboPadding = "";
                if (maxComboList[i].ToString().Length == 1) comboPadding = "  ";
                if (maxComboList[i].ToString().Length == 2) comboPadding = " ";

                string returnTextLine = (i+1).ToString() + "位        " + scorePadding + scoreList[i] + "            " + comboPadding + maxComboList[i] + "　　　  " + gpaText;
                returnArray[i] = returnTextLine;
            }

            return returnArray;
        }
        catch (FileNotFoundException e)
        {
            //ダメだった場合は0を20個返す
            string[] returnArray = new string[20];
            for (int i = 0; i < 20; i++) returnArray[i] = "ERROR";

            return returnArray;
        }
    }

    //ソートする関数
    //ソート対象の配列、それ以外の配列2つ
    void bubbleSort_3(List<int> numbers, List<int> list1, List<int> list2, int array_size)
    {
        int i, j, temp;

        for (i = 0; i < (array_size - 1); i++)
        {
            for (j = (array_size - 1); j > i; j--)
            {
                if (numbers[j - 1] < numbers[j])
                {
                    temp = numbers[j - 1];
                    numbers[j - 1] = numbers[j];
                    numbers[j] = temp;

                    temp = list1[j - 1];
                    list1[j - 1] = list1[j];
                    list1[j] = temp;

                    temp = list2[j - 1];
                    list2[j - 1] = list2[j];
                    list2[j] = temp;
                }
            }
        }
    }
}
