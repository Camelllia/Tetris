using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTetromino : MonoBehaviour
{
    TetrisSpawner TetrisSpawner;
    TetrisBlock TetrisBlock;

    private int depth;

    void Start()
    {
        tag = "currentGhostTetromino";

        foreach (Transform mino in transform)
        {
            mino.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
        }
    }


    void Update()
    {
        if (TetrisSpawner == null)
        {
            TetrisSpawner = FindObjectOfType<TetrisSpawner>();
        }

        if (TetrisBlock == null)
        {
            TetrisBlock = FindObjectOfType<TetrisBlock>();
        }

        FollowActiveTetromino();
        MoveDown();
    }

    void FollowActiveTetromino()
    {
        Transform currentTetrominoTransform = GameObject.FindGameObjectWithTag("currentBlock").transform;

        transform.position = new Vector3(currentTetrominoTransform.position.x, transform.position.y, transform.position.z);
        transform.rotation = currentTetrominoTransform.rotation;

    }

    void MoveDown()
    {
        depth = 19;
        for (int i = 0; i < 4; i++)
        {
            for (int j = Mathf.RoundToInt(GameObject.FindGameObjectWithTag("currentBlock").transform.position.y) - 1; j >= 0; j--)
            {
                int childX = Mathf.RoundToInt(this.transform.GetChild(i).position.x);
                int childY = Mathf.RoundToInt(this.transform.GetChild(i).position.y);
                if (TetrisBlock.grid[childX, j] != null && depth > childY - j - 1)
                {
                    depth = childY - j - 1;
                    break;
                }
                else if (j == 0 && depth > childY)
                {
                    depth = childY;
                }
            }
        }
        transform.position += new Vector3(0, -depth, 0);
    }
}
