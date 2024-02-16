using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Title title;
    public Tutorial tutorial;
    public Tutorial_Explain tutorial_explain;
    public ScreenTransiton screenTransiton;
    public DisplayLevelText displayLevelText;
    public pattern_01 pattern_1;
    public pattern_02 pattern_2;
    public pattern_03 pattern_3;
    public Result result;
    public Ranking ranking;

    public bool ButtonDown_A = false;
    private bool _isPlaytutorial = false;

    public ScoreManager scoreManager;
    public GameObject ScoreWindow;
    public GameObject ScoreWindowPlane;

    void Start()
    {
        StartCoroutine(Generate());
    }

    IEnumerator Generate()
    {
        while (true)
        {
            SoundManager.Instance.Play(SoundManager.SE.DisplayLevel_ChakiEnd);
            SoundManager.Instance.Play(SoundManager.BGM.Title);
            yield return StartCoroutine(title.Generate());

            SoundManager.Instance.StopBGM();

            // チュートリアルを選択した場合
            if (_isPlaytutorial)
            {
                SoundManager.Instance.Play(SoundManager.BGM.Tutorial);
                yield return StartCoroutine(tutorial_explain.Generate());
                SoundManager.Instance.StopBGM();
                // スコア・コンボのリセット
                yield return StartCoroutine(scoreManager.ResetParamCoroutine());
            }

            yield return StartCoroutine(screenTransiton.Generate());

            ScoreWindow.SetActive(true);
            ScoreWindowPlane.SetActive(true);

            // LEVEL 1
            yield return StartCoroutine(displayLevelText.Generate("レベル１"));
            SoundManager.Instance.Play(SoundManager.BGM.Level1);
            yield return StartCoroutine(pattern_1.Generate());
            SoundManager.Instance.StopBGM();

            // LEVEL 2
            yield return StartCoroutine(displayLevelText.Generate("レベル２"));
            SoundManager.Instance.Play(SoundManager.BGM.Level2);
            yield return StartCoroutine(pattern_2.Generate());
            SoundManager.Instance.StopBGM();

            // LEVEL FINAL
            yield return StartCoroutine(displayLevelText.Generate("ファイナルレベル"));
            SoundManager.Instance.Play(SoundManager.BGM.Level3);
            yield return StartCoroutine(pattern_3.Generate());
            SoundManager.Instance.StopBGM();

            yield return StartCoroutine(displayLevelText.Generate("終了！"));

            ScoreWindow.SetActive(false);
            ScoreWindowPlane.SetActive(false);

            yield return StartCoroutine(screenTransiton.Generate());

            // 結果表示
            yield return StartCoroutine(result.Generate());

            // ランキングの表示
            yield return StartCoroutine(ranking.Generate());

            // 各パラメータと牌オブジェクトの削除
            yield return StartCoroutine(scoreManager.ResetParamCoroutine());
            yield return StartCoroutine(DestroyAllHais());
        }

        yield return 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ButtonDown_A = true;
        }

        // Rボタンでゲームのリセットを行う
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene loadScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(loadScene.name);
        }
    }

    public IEnumerator SetTutorial(int select_mode)
    {
        if (select_mode == 1) _isPlaytutorial = true;
        if (select_mode == 2) _isPlaytutorial = false;
        yield return 0;
    }

    private IEnumerator DestroyAllHais()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("gen1");
        foreach (GameObject ball in objects)
        {
            Destroy(ball);
        }
        objects = GameObject.FindGameObjectsWithTag("gen2");
        foreach (GameObject ball in objects)
        {
            Destroy(ball);
        }
        objects = GameObject.FindGameObjectsWithTag("gen3");
        foreach (GameObject ball in objects)
        {
            Destroy(ball);
        }

        yield return 0;
    }

}
