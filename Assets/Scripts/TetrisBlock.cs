using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public Vector3 RotationPoint;
    private float previousTime, previousTimeLeft, previousTimeRight;
    public float AutorepeatDelay, AutorepeatSpeed;
    public float FallTime = 0.8f;
    public static int Height = 20;
    public static int Width = 10;
    public static Transform[,] grid = new Transform[Width, Height];
    List<GameObject> ListTetrominoes;

    private int dropScore;
    private int cnt;
    private int rotateCount;
    bool isgameover;

    bool isdelay;
    float time;

    TetrisSpawner TetrisSpawn;

    [SerializeField] GameObject starParticle;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        isdelay = true;
        tag = "currentBlock";
        rotateCount = 15;
        AutorepeatSpeed = 0.05f;
        AutorepeatDelay = 0.17f;//0.17
        ListTetrominoes = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TetrisSpawn == null)
        {
            TetrisSpawn = FindObjectOfType<TetrisSpawner>();
        }

        if ((Input.GetKeyDown(KeyCode.LeftArrow) && Time.time - previousTimeLeft + AutorepeatSpeed > (Input.GetKeyDown(KeyCode.LeftArrow) ? FallTime / 8 : FallTime)) && isdelay)
        {
            StartCoroutine("DelayL");
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            isdelay = true;
        }
        if ((Input.GetKeyDown(KeyCode.RightArrow) && Time.time - previousTimeRight + AutorepeatSpeed > (Input.GetKeyDown(KeyCode.RightArrow) ? FallTime / 8 : FallTime)) && isdelay)
        {
            StartCoroutine("DelayR");
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            isdelay = true;
        }

        //키보드 입력값이 왼쪽 화살표이면 moveLeft함수, 오른쪽 화살표이면 moveRight함수 실행
        if ((Input.GetKey(KeyCode.LeftArrow) && Time.time - previousTimeLeft + AutorepeatSpeed > (Input.GetKey(KeyCode.LeftArrow) ? FallTime / 8 : FallTime)) && !isdelay)
        {
            moveLeft();
        }
        else if ((Input.GetKey(KeyCode.RightArrow) && Time.time - previousTimeRight + AutorepeatSpeed > (Input.GetKey(KeyCode.RightArrow) ? FallTime / 8 : FallTime)) && !isdelay)
        {
            moveRight();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.X))
        {
            rotateBlock();
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            leftrotateBlock();
        }

        if (Time.time - previousTime > FallTime * LevelManager.Instance.speed && !Arrived())
        {
            moveDown();
        }
        else if (Time.time - previousTime > FallTime * (30 + LevelManager.Instance.speed * 5) / 50 && Arrived())
        {
            moveDown();
        }

        if (Input.GetKey(KeyCode.DownArrow) && Time.time - previousTime > FallTime / (15 + LevelManager.Instance.level * 10) && !Arrived())
        {
            moveDown();
            ScoreManager.Instance.score++;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dropBlock();
        }
    }

    IEnumerator DelayL()
    {
        moveLeft();
        yield return new WaitForSeconds(AutorepeatDelay);

        isdelay = false;
    }
    IEnumerator DelayR()
    {
        moveRight();
        yield return new WaitForSeconds(AutorepeatDelay);

        isdelay = false;
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
            TetrisSpawn.CanHold = true; // 밑에 닿았으므로 홀드 가능해짐
            tag = "Untagged";
            Destroy(GameObject.FindGameObjectWithTag("currentGhostTetromino"));
            if (!isgameover)
            {
                TetrisSpawn.NewTetrominoes();
            }
        }
        previousTime = Time.time;
    }

    void rotateBlock()
    {
        transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), -90);

        if (!ValidLMove() && !ValidRMove())
        {
            transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), 90);
        }
        else if (!ValidRMove())
        {
            while (!ValidRMove())
            {
                transform.position += new Vector3(-1, 0, 0);
                if (!ValidLMove())
                {
                    transform.position += new Vector3(1, 0, 0);
                    transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), 90);
                    break;
                }
            }
        }
        else if (!ValidLMove())
        {
            while (!ValidLMove())
            {
                transform.position += new Vector3(1, 0, 0);
                if (!ValidRMove())
                {
                    transform.position += new Vector3(-1, 0, 0);
                    transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), 90);
                    break;
                }
            }
        }
        else if (!ValidDown())
        {
            while (!ValidDown())
            {
                transform.position += new Vector3(0, 1, 0);
            }
        }

        if (Arrived() && rotateCount > 0)
        {
            previousTime = Time.time;
            rotateCount--;
        }
    }

    void leftrotateBlock()
    {
        transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), 90);

        if (!ValidLMove() && !ValidRMove())
        {
            transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), -90);
        }
        else if (!ValidLMove())
        {
            while (!ValidLMove())
            {
                transform.position += new Vector3(1, 0, 0);
                if (!ValidRMove())
                {
                    transform.position += new Vector3(-1, 0, 0);
                    transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), -90);
                    break;
                }
            }
        }
        else if (!ValidRMove())
        {
            while (!ValidRMove())
            {
                transform.position += new Vector3(-1, 0, 0);
                if (!ValidLMove())
                {
                    transform.position += new Vector3(1, 0, 0);
                    transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), -90);
                    break;
                }
            }
        }
        else if (!ValidDown())
        {
            while (!ValidDown())
            {
                transform.position += new Vector3(0, 1, 0);
            }
        }

        if (Arrived() && rotateCount > 0)
        {
            previousTime = Time.time;
            rotateCount--;
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
                if (grid[childX, j] != null && dropScore > childY - j - 1)
                {
                    dropScore = childY - j - 1;
                    break;
                }
                else if (j == 0 && dropScore > childY)
                {
                    dropScore = childY;
                }
            }
        }
        transform.position += new Vector3(0, -dropScore, 0);

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
                LevelManager.Instance.linecnt++;
                Debug.Log(ListTetrominoes.Count);
                DeleteLine(i);//줄을 삭제하고
                RowDown(i);//내려준다
                ListTetrominoes.Clear();
            }
        }

        if (cnt == 0)
        {
            ScoreManager.Instance.combo = 0;
        }

        ScoreManager.Instance.CountScoreLine(cnt);

        if (cnt != 0)
        {
            ScoreManager.Instance.combo++;
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
            ListTetrominoes.Add(grid[j, i].gameObject); // 리스트에 추가해주고
            if (ListTetrominoes.Count == 10)
            {
                for (int k = 12; k < ListTetrominoes.Count + 12; k++)
                {
                    Instantiate(ListTetrominoes[0], new Vector3(k, LevelManager.Instance.linecnt + (LevelManager.Instance.level - 1) * 10, 1), Quaternion.identity);
                }
            }
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

            if (roundedY < 19)
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
    public bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < 0 || roundedX >= Width || roundedY < 0 || roundedY >= Height || grid[roundedX, roundedY] != null)
            {
                return false;
            }
        }
        return true;
    }

    public bool ValidRMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX >= Width)
            {
                return false;
            }
            else if (roundedX > 0 && roundedY > 0)
            {
                if (grid[roundedX, roundedY] != null && roundedX > transform.position.x)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool ValidLMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            if (roundedX < 0)
            {
                return false;
            }
            else if (roundedX < Width && roundedY > 0)
            {
                if (grid[roundedX, roundedY] != null && roundedX < transform.position.x)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool ValidDown()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            if (roundedY < 0)
            {
                return false;
            }
            else if (grid[roundedX, roundedY] != null && roundedY < transform.position.y)
            {
                return false;
            }
        }
        return true;
    }

    public bool Arrived()
    {
        for (int i = 0; i < 4; i++)
        {
            int childX = Mathf.RoundToInt(this.transform.GetChild(i).position.x);
            int childY = Mathf.RoundToInt(this.transform.GetChild(i).position.y);
            if (childY == 0)
            {
                return true;
            }
            else if (grid[childX, childY - 1] != null)
            {
                return true;
            }
        }
        return false;
    }

    void gameOver()
    {
        isgameover = true;
        Debug.Log("gameover");
        GameObject.FindWithTag("GameOver").gameObject.transform.GetChild(4).gameObject.SetActive(true);
        ScoreManager.Instance.ScoreSet();
    }
}
