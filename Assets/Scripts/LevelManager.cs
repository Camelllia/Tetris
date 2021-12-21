using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int level;
    public int linecnt;
    public float speed;

    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelManager>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("LevelManager");
                    instance = instanceContainer.AddComponent<LevelManager>();
                }
            }
            return instance;
        }
    }
    private static LevelManager instance;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        linecnt = 0;
        speed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (linecnt >= 10 && level < 26)
        {
            level++;
            linecnt -= 10;
            speed *= 0.8f;
        }
    }
}