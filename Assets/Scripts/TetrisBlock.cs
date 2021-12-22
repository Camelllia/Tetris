using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public Vector3 RotationPoint;
    private float previousTime, previousTimeLeft, previousTimeRight;
    private float timer;
    public float AutorepeatDelay, AutorepeatSpeed;
    public float FallTime = 0.8f;
    public static int Height = 20;
    public static int Width = 10;
    private static Transform[,] grid = new Transform[Width, Height];

    private int dropScore;
    private int cnt;
    private bool backtoback;
    bool isgameover;

    TetrisSpawner TetrisSpawn;

    [SerializeField] GameObject starParticle;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        AutorepeatSpeed = 0.05f;
        AutorepeatDelay = 0.17f;
        backtoback = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (TetrisSpawn == null)
        {
            TetrisSpawn = FindObjectOfType<TetrisSpawner>();
        }
        timer += Time.deltaTime;
        //키보드 입력값이 왼쪽 화살표이면 moveLeft함수, 오른쪽 화살표이면 moveRight함수 실행
        if ((Input.GetKey(KeyCode.LeftArrow) && Time.time - previousTimeLeft + AutorepeatSpeed > (Input.GetKey(KeyCode.LeftArrow) ? FallTime / 8 : FallTime)))
        {
            moveLeft();
        }
        else if ((Input.GetKey(KeyCode.RightArrow) && Time.time - previousTimeRight + AutorepeatSpeed > (Input.GetKey(KeyCode.RightArrow) ? FallTime / 8 : FallTime)))
        {
            moveRight();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.X))
        {
            rotateBlock();
        }
        else if(Input.GetKeyDown(KeyCode.Z))
        {
            leftrotateBlock();
        }


        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? FallTime / 14 : FallTime * LevelManager.Instance.speed))
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
        transform.position += new Vector3(-1, 0, 0);//왼쪽으로 이동
        if (!ValidMove())
        {
            transform.position -= new Vector3(-1, 0, 0);
        }
        previousTimeLeft = Time.time;
    }

    void moveRight()
    {
        transform.position += new Vector3(1, 0, 0);//오른쪽으로 이동
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
            AddToGrid();
            checkForLines(); // 가로 줄이 꽉 찼는지 확인
            this.enabled = false;
            if (!isgameover)
            {
                TetrisSpawn.NewTetrominoes();
            }
        }
        previousTime = Time.time;
    }

    void rotateBlock()
    {
        if (!ValidMove())
        {
            transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), -90);
        }
        else
        {
            transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), -90);
        }
    }

    void leftrotateBlock()
    {

        if (!ValidMove())
        {
            transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), 90);
        }
        else
        {
            transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), 90);
        }
    }

    void dropBlock()
    {
        dropScore = 19;
        //자식들 x,y 둘다 가져와
        //각 자식들의 y좌표와 바닥의 차이
        //가장 작은 값을 구해
        //가장 작은 값이랑*2 스코어 플러스
        for (int i = 0; i < 4; i++)
        {
            for (int j = Height - 1; j >= 0; j--)
            {
                int childX = Mathf.RoundToInt(this.transform.GetChild(i).position.x);
                int childY = Mathf.RoundToInt(this.transform.GetChild(i).position.y);
                if (grid[childX, j] != null && dropScore > childY - j)
                {
                    dropScore = childY - j;
                    break;
                }
                else if (j == 0 && dropScore > childY)
                {
                    dropScore = childY;
                }
            }
            Debug.Log(dropScore);
        }
        ScoreManager.Instance.score += dropScore * 2;

       

        FallTime = 0;
    }


    void checkForLines()
    {
        cnt = 0;
        //cnt 선언
        for (int i = Height - 1; i >= 0; i--) // 테트리스 높이만큼 반복해서
        {
            if (HasLine(i))//줄이 꽉 차있다면 
            {
                cnt++;//cnt ++해주고 
                DeleteLine(i);//줄을 삭제하고
                RowDown(i);//내려준다
            }
        }
        //여기서 cnt값에 따라 점수 추가
        if (cnt == 1)
        {
            ScoreManager.Instance.score += 100;
            LevelManager.Instance.linecnt += 1;
            backtoback = false;
        }
        else if (cnt == 2)
        {
            ScoreManager.Instance.score += 300;
            LevelManager.Instance.linecnt += 2;
            backtoback = false;
        }
        else if (cnt == 3)
        {
            ScoreManager.Instance.score += 500;
            LevelManager.Instance.linecnt += 3;
            backtoback = false;
        }
        else if (cnt == 4)
        {
            if (backtoback)
            {
                ScoreManager.Instance.score += 1200;
            }
            else
            {
                ScoreManager.Instance.score += 800;
            }
            LevelManager.Instance.linecnt += 4;
            backtoback = true;
        }
    }

    bool HasLine(int i)
    {
        for (int j = 0; j < Width; j++)
        {
            if (grid[j, i] == null)
            {
                return false;
            }
        }
        return true;
    }

    void DeleteLine(int i)
    {
        for (int j = 0; j < Width; j++)
        {
            Instantiate(starParticle, grid[j, i].gameObject.transform.position, Quaternion.identity);
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    void RowDown(int i)
    {
        for (int y = i; y < Height; y++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }


    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedY < 20)
            {
                grid[roundedX, roundedY] = children;
            }
            else
            {
                gameOver();
            }
        }
    }



    //맵 범위 내에서 움직이고 있는지 확인하는 함수
    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < 0 || roundedX >= Width || roundedY < 0 || roundedY >= Height)
            {
                return false;
            }
            if (grid[roundedX, roundedY] != null)
            {
                return false;
            }
        }
        return true;
    }

    void gameOver()
    {
        isgameover = true;
        Debug.Log("gameover");
        GameObject.FindWithTag("GameOver").gameObject.transform.GetChild(3).gameObject.SetActive(true);
    }



}
