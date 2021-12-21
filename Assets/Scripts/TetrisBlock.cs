using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public Vector3 RotationPoint;
    private float previousTime, previousTimeLeft, previousTimeRight;
    public float FallTime = 0.8f;
    public static int Height = 20;
    public static int Width = 10;
    private static Transform[,] grid = new Transform[Width, Height];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Ű���� �Է°��� ���� ȭ��ǥ�̸� moveLeft�Լ�, ������ ȭ��ǥ�̸� moveRight�Լ� ����
        if((Input.GetKey(KeyCode.LeftArrow) && Time.time - previousTimeLeft > (Input.GetKey(KeyCode.LeftArrow) ? FallTime / 8 : FallTime)))
        {
            moveLeft();
        }
        else if ((Input.GetKey(KeyCode.RightArrow) && Time.time - previousTimeRight > (Input.GetKey(KeyCode.RightArrow) ? FallTime / 8 : FallTime)))
        {
            moveRight();
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            rotateBlock();
        }


        if(Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? FallTime/14 : FallTime))
        {
            moveDown();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dropBlock();
        }
    }

    void moveLeft()
    {
        transform.position += new Vector3(-1, 0, 0);//�������� �̵�
        if(!ValidMove())
        {
            transform.position -= new Vector3(-1, 0, 0);
        }
        previousTimeLeft = Time.time;
    }

    void moveRight()
    {
        transform.position += new Vector3(1, 0, 0);//���������� �̵�
        if (!ValidMove())
        {
            transform.position -= new Vector3(1, 0, 0);
        }
        previousTimeRight = Time.time;
    }


    void moveDown()
    {
        transform.position += new Vector3(0, -1, 0);
        if (!ValidMove())
        {
            transform.position -= new Vector3(0, -1, 0);
        }
        previousTime = Time.time;
    }

    void rotateBlock()
    {
        transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), 90);
        if (!ValidMove())
        {
            transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), -90);
        }
    }

    void dropBlock()
    {
        FallTime = 0.003f;
    }



    //�� ���� ������ �����̰� �ִ��� Ȯ���ϴ� �Լ�
    bool ValidMove()
    {
        foreach(Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if(roundedX<0||roundedX>=Width||roundedY<0||roundedY>=Height)
            {
                return false;
            }
            /*if(grid[roundedX,roundedY]!=null)
            {
                return false;
            }*/
        }
        return true;
    }
}
