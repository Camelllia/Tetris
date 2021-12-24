using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMeshManager : MonoBehaviour
{
    public TextMesh lvlup;
    public TextMesh clear;
    public TextMesh score;

    public static TextMeshManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TextMeshManager>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("TextMeshManager");
                    instance = instanceContainer.AddComponent<TextMeshManager>();
                }
            }
            return instance;
        }
    }
    private static TextMeshManager instance;

    public void CallLvlup()
    {
        Instantiate(lvlup, transform);
    }

    public void CallClear(int i, int j, bool back)
    {
        switch (i)
        {
            case 0:
                clear.text = "";
                break;
            case 1:
                clear.text = "SINGLE";
                break;
            case 2:
                clear.text = "DOUBLE";
                break;
            case 3:
                clear.text = "TRIPLE";
                break;
            case 4:
                clear.text = "TETRIS";
                if (back)
                {
                    clear.text = "BACK-TO-BACK\n" + clear.text;
                }
                break;
        }

        if (j > 0)
        {
            clear.text += "\nCOMBO " + j;
        }
        Instantiate(clear, transform);
    }

    public void CallScore(int i, int j)
    {
        score.text = "+" + (i + j * 50) * LevelManager.Instance.level;
        Instantiate(score, transform);
    }
}
