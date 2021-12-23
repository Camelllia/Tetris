using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score;
    public int bestScore;
    public int lineScore;
    public int combo;
    private int level;
    private bool backtoback;
    public Text scoreText;
    public GameObject clearMesh;
    public GameObject scoreMesh;

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
        level = LevelManager.Instance.level;
        backtoback = false;
        combo = 0;
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
                combo = 0;
                lineScore = 0;
                backtoback = false;
                break;
            case 1:
                lineScore += 100;
                backtoback = false;
                Instantiate(clearMesh, clearMesh.transform.position, clearMesh.transform.rotation);
                Instantiate(scoreMesh, scoreMesh.transform.position, scoreMesh.transform.rotation);
                break;
            case 2:
                lineScore += 300;
                backtoback = false;
                //Instantiate(textmesh, ~, ~);
                break;
            case 3:
                lineScore += 500;
                backtoback = false;
                //Instantiate(textmesh, ~, ~);
                break;
            case 4:
                if (backtoback)
                {
                    lineScore += 1200;
                    //Instantiate(textmesh, ~, ~);
                }
                else
                {
                    lineScore += 800;
                    //Instantiate(textmesh, ~, ~);
                }
                backtoback = true;
                break;
        }
        score += (lineScore + combo * 50) * level;
        combo++;
    }
}
