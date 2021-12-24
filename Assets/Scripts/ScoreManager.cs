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
    public Text scoreText;
    public Text highScoreText1;
    public Text highScoreText2;
    public Text highScoreText3;
    public Text highScoreText4;
    public Text highScoreText5;
    private int[] bestScore = new int[5];
    private string[] bestName = new string[5];

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
        ScoreSet("", bestScore[0]);
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
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

    public void ScoreSet(string currentName, int currentScore)
    {
        PlayerPrefs.SetString("CurrentPlayerName", currentName);
        PlayerPrefs.SetInt("CurrentPlayerScore", currentScore);

        int tmpScore = 0;
        string tmpName = "";

        for (int i = 0; i < 5; i++)
        {
            bestScore[i] = PlayerPrefs.GetInt(i + "BestScore");
            bestName[i] = PlayerPrefs.GetString(i + "BestName");

            while (bestScore[i] < currentScore)
            {
                //자리바꾸기!
                tmpScore = bestScore[i];
                tmpName = bestName[i];
                bestScore[i] = currentScore;
                bestName[i] = currentName;

                //랭킹에 저장
                PlayerPrefs.SetInt(i + "BestScore", currentScore);
                PlayerPrefs.SetString(i.ToString() + "BestName", currentName);

                //다음 반복을 위한 준비
                currentScore = tmpScore;
                currentName = tmpName;
            }
        }
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt(i + "BestScore", bestScore[i]);
            PlayerPrefs.SetString(i.ToString() + "BestName", bestName[i]);
            highScoreText1.text = "" + bestScore[0];
            highScoreText2.text = "" + bestScore[1];
            highScoreText3.text = "" + bestScore[2];
            highScoreText4.text = "" + bestScore[3];
            highScoreText5.text = "" + bestScore[4];
        }
    }
}
