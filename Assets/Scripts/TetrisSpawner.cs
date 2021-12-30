using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisSpawner : MonoBehaviour
{
    public GameObject[] Tetrominoes;
    private HashSet<int> deck = new HashSet<int>();
    private int next;
    public GameObject nextSpawn;
    public GameObject targetSpawn;
    public GameObject HoldBasket;
    GameObject ghostTetromino;

    bool isFirst = true;
    public bool CanHold = true;
    TetrisBlock TetrisBlock;
    public bool isHolding;

    void Start()
    {
        isHolding = false;
        NewTetrominoes();
    }

    void Update()
    {
        if (TetrisBlock == null)
        {
            TetrisBlock = FindObjectOfType<TetrisBlock>();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Hold();
        }


    }


    //새로운 테트로미노 생성
    public void NewTetrominoes()
    {
        //0.5초 마다 생성
        Invoke("createTetrominoes", 0.1f);

    }

    //테트로미노 생성
    void createTetrominoes()
    {


        if (isFirst)
        {
            isFirst = false;

            if (deck.Count == Tetrominoes.Length)
            {
                deck.Clear();
            }


            do
            {
                next = Random.Range(0, Tetrominoes.Length);
            } while (deck.Contains(next));

            deck.Add(next);


            targetSpawn = Instantiate(Tetrominoes[next], transform.position, Quaternion.identity);

            SpawnGhostTetromino(targetSpawn);

            if (deck.Count == Tetrominoes.Length)
            {
                deck.Clear();
            }


            do
            {
                next = Random.Range(0, Tetrominoes.Length);
            } while (deck.Contains(next));

            deck.Add(next);

            //다음 스폰될 테트로미노 보여주기
            nextSpawn = Instantiate(Tetrominoes[next], transform.position + new Vector3(-9.6f, -1f, 0), Quaternion.identity);
            nextSpawn.GetComponent<TetrisBlock>().enabled = false;
            nextSpawn.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        }
        else
        {
            targetSpawn = nextSpawn;
            targetSpawn.transform.position = transform.position;
            targetSpawn.transform.localScale = Vector3.one;
            targetSpawn.GetComponent<TetrisBlock>().enabled = true;

            if (deck.Count == Tetrominoes.Length)
            {
                deck.Clear();
            }


            do
            {
                next = Random.Range(0, Tetrominoes.Length);
            } while (deck.Contains(next));

            deck.Add(next);

            nextSpawn = null;
            nextSpawn = Instantiate(Tetrominoes[next], transform.position + new Vector3(-9.6f, -1f, 0), Quaternion.identity);
            nextSpawn.GetComponent<TetrisBlock>().enabled = false;
            nextSpawn.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            SpawnGhostTetromino(targetSpawn);

        }
    }


    void Hold()
    {

        if (HoldBasket == null) // 홀드가 처음일때
        {
            targetSpawn.tag = "Untagged";
            HoldBasket = targetSpawn; // 현재 테트로미노를 홀드바구니에 넣어줌
            Destroy(ghostTetromino);
            HoldBasket.transform.position = new Vector3(-10, 16.8f, 0); // 홀드 바구니 위치로 이동
            HoldBasket.transform.rotation = Quaternion.Euler(0, 0, 0);
            HoldBasket.GetComponent<TetrisBlock>().enabled = false; // 홀드 값의 기능을 꺼줌
            targetSpawn = nextSpawn; // 현재 테트로미노를 다음 테트로미노에서 땡겨옴
            targetSpawn.transform.position = transform.position; // 스폰 위치로 이동
            targetSpawn.transform.localScale = Vector3.one; // 스케일 값 조정
            targetSpawn.GetComponent<TetrisBlock>().enabled = true; // 테트로미노 기능 켜주기
            createTetrominoes(); // 다시 테트로미노 생성
            CanHold = false; // 홀드를 한 번 실행했기 때문에 홀드 못함
        }
        else
        {
            if (CanHold) // 홀드가 가능한 상황일 때
            {
                targetSpawn.tag = "Untagged";
                Destroy(ghostTetromino);
                //TempGhost = ghostTetromino;
                //Destroy(ghostTetromino);
                //SpawnGhostTetromino(HoldGhost);

                // HoldGhost = TempGhost;

                HoldBasket.transform.position = transform.position;
                HoldBasket.tag = "currentBlock";
                SpawnGhostTetromino(HoldBasket);
                HoldBasket.GetComponent<TetrisBlock>().enabled = true;
                isHolding = true;
                HoldBasket = targetSpawn;
                HoldBasket.transform.position = new Vector3(-10, 16.8f, 0);
                HoldBasket.transform.rotation = Quaternion.Euler(0, 0, 0);
                targetSpawn.GetComponent<TetrisBlock>().enabled = false;
                HoldBasket.transform.position = targetSpawn.transform.position;
                CanHold = false; // 홀드를 실행했으니 홀드를 다시 못하는 상황으로 만들어 줌
            }
        }
    }

    public void SpawnGhostTetromino(GameObject ga)
    {
        ghostTetromino = Instantiate(ga, transform.position, Quaternion.identity);

        Destroy(ghostTetromino.GetComponent<TetrisBlock>());
        ghostTetromino.AddComponent<GhostTetromino>();
    }


}
