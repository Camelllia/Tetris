using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score;
    public int lineScore;
    public int combo;
    private bool backtoback;
    public InputField newName;
    public Text upperText;
    public Text scoreText;
    public Text currentScoreText;
    public string[] bestName = new string[5];
    public int[] bestScore = new int[5];
    public Text highNameText0;
    public Text highNameText1;
    public Text highNameText2;
    public Text highNameText3;
    public Text highNameText4;
    public Text highScoreText0;
    public Text highScoreText1;
    public Text highScoreText2;
    public Text highScoreText3;
    public Text highScoreText4;

    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScoreManager>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("ScoreManager");
                    instance = instanceContainer.AddComponent<ScoreManager>();
                }
            }
            return instance;
        }
    }
    private static ScoreManager instance;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        backtoback = false;
        combo = 0;
        ScoreSet();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
        currentScoreText.text = "NEW HIGH SCORE!\n" + score + "\nENTER YOUR INITIALS:";
        newName.characterLimit = 3;
        upperText.text = newName.text.ToUpper();
    }

    public void CountScoreLine(int i)
    {
        lineScore = 0;
        switch (i)
        {
            case 0:
                break;
            case 1:
                lineScore += 100;
                backtoback = false;
                break;
            case 2:
                lineScore += 300;
                backtoback = false;
                break;
            case 3:
                lineScore += 500;
                backtoback = false;
                break;
            case 4:
                if (backtoback)
                {
                    lineScore += 1200;
                }
                else
                {
                    lineScore += 800;
                }
                break;
        }

        TextMeshManager.Instance.CallClear(i, combo, backtoback);

        if (lineScore > 0)
        {
            TextMeshManager.Instance.CallScore(lineScore, combo);
        }
        score += (lineScore + combo * 50) * LevelManager.Instance.level;

        if (i == 4)
        {
            backtoback = true;
        }
    }

    public void ScoreSet()
    {
        string currentName = upperText.text;
        int currentScore = score;

        PlayerPrefs.SetString("CurrentPlayerName", currentName);
        PlayerPrefs.SetInt("CurrentPlayerScore", currentScore);

        string tmpName = "";
        int tmpScore = 0;

        for (int i = 0; i < 5; i++)
        {
            bestScore[i] = PlayerPrefs.GetInt(i + "BestScore");
            bestName[i] = PlayerPrefs.GetString(i + "BestName");

            while (bestScore[i] < currentScore)
            {
                //자리바꾸기
                tmpName = bestName[i];
                tmpScore = bestScore[i];
                bestName[i] = currentName;
                bestScore[i] = currentScore;

                //랭킹에 저장
                PlayerPrefs.SetString(i + "BestName", currentName);
                PlayerPrefs.SetInt(i + "BestScore", currentScore);

                //다음 반복을 위한 준비
                currentName = tmpName;
                currentScore = tmpScore;
            }
        }
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetString(i + "BestName", bestName[i]);
            PlayerPrefs.SetInt(i + "BestScore", bestScore[i]);
            highNameText0.text = "" + bestName[0];
            highNameText1.text = "" + bestName[1];
            highNameText2.text = "" + bestName[2];
            highNameText3.text = "" + bestName[3];
            highNameText4.text = "" + bestName[4];
            highScoreText0.text = "" + bestScore[0];
            highScoreText1.text = "" + bestScore[1];
            highScoreText2.text = "" + bestScore[2];
            highScoreText3.text = "" + bestScore[3];
            highScoreText4.text = "" + bestScore[4];
        }
    }
}
