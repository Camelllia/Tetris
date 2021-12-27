using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTetromino : MonoBehaviour
{
    TetrisSpawner TetrisSpawner;
    TetrisBlock TetrisBlock;

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
        if(TetrisSpawner == null)
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
        Transform currentTetrominoTransform = TetrisSpawner.targetSpawn.transform;

        transform.position = currentTetrominoTransform.position;
        transform.rotation = currentTetrominoTransform.rotation;
    }

    void MoveDown()
    {
        while(CheckIsVaildPosition())
        {
            transform.position += new Vector3(0, -1, 0);
        }

        if(!CheckIsVaildPosition())
        {
            transform.position += new Vector3(0, 1, 0);
        }
    }

    bool CheckIsVaildPosition()
    {
        foreach(Transform mino in transform)
        {
            if (TetrisBlock.ValidMove() == false)
                return false;
            if (TetrisBlock.ValidLMove() == false)
                return false;
            if (TetrisBlock.ValidLMove() == false)
                return false;
        }
        return true;
    }
}
