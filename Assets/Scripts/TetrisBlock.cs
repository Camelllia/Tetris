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
    public bool arrived;
    bool isgameover;



    TetrisSpawner TetrisSpawn;

    [SerializeField] GameObject starParticle;

    // Start is called before the first frame update
    void Start()
    {
        tag = "currentBlock";
        rotateCount = 15;
        AutorepeatSpeed = 0.05f;
        AutorepeatDelay = 0.17f;
        arrived = false;
        ListTetrominoes = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TetrisSpawn == null)
        {
            TetrisSpawn = FindObjectOfType<TetrisSpawner>();
        }
        //Ű���� �Է°��� ���� ȭ��ǥ�̸� moveLeft�Լ�, ������ ȭ��ǥ�̸� moveRight�Լ� ����
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

        for (int i = 0; i < 4; i++)
        {
            int childX = Mathf.RoundToInt(this.transform.GetChild(i).position.x);
            int childY = Mathf.RoundToInt(this.transform.GetChild(i).position.y);
            if (childY == 0)
            {
                arrived = true;
                break;
            }
            else if (grid[childX, childY - 1] != null)
            {
                arrived = true;
                break;
            }
            else
            {
                arrived = false;
            }
        }

        if (Time.time - previousTime > FallTime * LevelManager.Instance.speed && !arrived)
        {
            moveDown();
        }
        else if (Time.time - previousTime > FallTime * (35 + LevelManager.Instance.speed) / 50 && arrived)
        {
            moveDown();
        }

        if (Input.GetKey(KeyCode.DownArrow) && !arrived && Time.time - previousTime > FallTime / 14)
        {
            moveDown();
            ScoreManager.Instance.score++;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dropBlock();
        }

        
    }

    void moveLeft()
    {
        transform.position += new Vector3(-1, 0, 0);//�������� �̵�
        if (!ValidMove())
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
            AddToGrid();
            checkForLines(); // ���� ���� �� á���� Ȯ��
            this.enabled = false;    
            TetrisSpawn.CanHold = true; // �ؿ� ������Ƿ� Ȧ�� ��������
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
            if (arrived && rotateCount > 0)
            {
                previousTime = Time.time;
                rotateCount--;
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
            if (arrived && rotateCount > 0)
            {
                previousTime = Time.time;
                rotateCount--;
            }
        }
        else if (!ValidDown())
        {
            while (!ValidDown())
            {
                transform.position += new Vector3(0, 1, 0);
            }
            if (arrived && rotateCount > 0)
            {
                previousTime = Time.time;
                rotateCount--;
            }
            Debug.Log("down");
        }
    }

    void leftrotateBlock()
    {
        transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), 90);

        if(!ValidLMove() && !ValidRMove())
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
            if (arrived && rotateCount > 0)
            {
                previousTime = Time.time;
                rotateCount--;
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
            if (arrived && rotateCount > 0)
            {
                previousTime = Time.time;
                rotateCount--;
            }
        }
        else if (!ValidDown())
        {
            while (!ValidDown())
            {
                transform.position += new Vector3(0, 1, 0);
            }
            if (arrived && rotateCount > 0)
            {
                previousTime = Time.time;
                rotateCount--;
            }
        }
    }

    void dropBlock()
    {
        dropScore = 19;
        //�ڽĵ� x,y �Ѵ� ������
        //�� �ڽĵ��� y��ǥ�� �ٴ��� ����
        //���� ���� ���� ����
        //���� ���� ���̶�*2 ���ھ� �÷���
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
        ScoreManager.Instance.score += dropScore * 2;

       

        FallTime = 0;
    }


    void checkForLines()
    {
        cnt = 0;
        //cnt ����
        for (int i = Height - 1; i >= 0; i--) // ��Ʈ���� ���̸�ŭ �ݺ��ؼ�
        {
            if (HasLine(i))//���� �� ���ִٸ�
            {
                cnt++;//cnt ++���ְ�
                LevelManager.Instance.linecnt++;
                DeleteLine(i);//���� �����ϰ�
                RowDown(i);//�����ش�
            }
        }
        
        if (cnt == 0)
        {
            ScoreManager.Instance.combo = 0;
        }

        ScoreManager.Instance.CountScoreLine(cnt);

        if(cnt != 0)
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
            ListTetrominoes.Add(grid[j, i].gameObject); // ����Ʈ�� �߰����ְ�           
            foreach (GameObject value in ListTetrominoes)
            {
                for (int k = 0; k < ListTetrominoes.Count; k++)
                {
                    Instantiate(value, new Vector3(k, LevelManager.Instance.linecnt - 3, 1), Quaternion.identity);
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



    //�� ���� ������ �����̰� �ִ��� Ȯ���ϴ� �Լ�
    public bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < 0 || roundedX >= Width || roundedY < 0 || roundedY >= Height|| grid[roundedX, roundedY] != null)
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
                    //for (int i = 0; i < 4; i++)
                    //{
                        //int childX = Mathf.RoundToInt(transform.GetChild(i).position.x);
                        //int childY = Mathf.RoundToInt(transform.GetChild(i).position.y);
                        //if (roundedX > rightX)
                        //{
                        //    rightX = roundedX;
                        //}
                        //if (childX < Width && grid[childX + 1, childY] == null)
                        //{
                        //    rotateTrue = true;
                        //}
                        //else
                        //{
                        //    rotateTrue = false;
                        //}
                    //}
                    //if (grid[rightX + 1, roundedY] == null)
                    //{
                    //    transform.position += new Vector3(1, 0, 0);
                    //}
                    //if (rotateTrue)
                    //{
                    //    return true;
                    //}
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
                    //if (roundedX > 0 && grid[roundedX - 1, roundedY] == null)
                    //{
                    //    return true;
                    //}
                    return false;
                }
            }
        }
        return true;
    }

    public bool ValidDown()
    {
            Debug.Log("Down");
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            if (roundedY <  0)
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

    void gameOver()
    {
        isgameover = true;
        Debug.Log("gameover");
        GameObject.FindWithTag("GameOver").gameObject.transform.GetChild(4).gameObject.SetActive(true);
        ScoreManager.Instance.ScoreSet("", ScoreManager.Instance.score);
    }
}
