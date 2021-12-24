using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score;
    public int bestScore;
<<<<<<< Updated upstream
    public int lineScore;
    public int combo;
    private bool backtoback;
    public Text scoreText;

=======
    public Text scoreText;
>>>>>>> Stashed changes

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
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
<<<<<<< Updated upstream
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
=======
>>>>>>> Stashed changes
    }
}
