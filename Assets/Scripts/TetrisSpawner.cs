using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisSpawner : MonoBehaviour
{
    public GameObject[] Tetrominoes;
    public List<GameObject> ListTetrominoes;
    GameObject nextSpawn;
    GameObject targetSpawn;
    GameObject DropTetromino;
    bool isFirst = true;
    bool isHoldFirst = true;
    public bool CanHold = true;
    GameObject HoldBasket;

    TetrisBlock TetrisBlock;
    
    void Start()
    {
        ListTetrominoes = new List<GameObject>();
        NewTetrominoes();
    }

    void Update()
    {
        if(TetrisBlock == null)
        {
            TetrisBlock = FindObjectOfType<TetrisBlock>();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Hold();
        }

        if(!CanHold)
        {
            //Debug.Log("Cant Hold");
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
        if(isFirst)
        {
            isFirst = false;
            targetSpawn = Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], transform.position, Quaternion.identity);
            ListTetrominoes.Add(targetSpawn);

            //다음 스폰될 테트로미노 보여주기
            nextSpawn = Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], transform.position + new Vector3(6.4f, -0.5f, 0), Quaternion.identity);
            nextSpawn.GetComponent<TetrisBlock>().enabled = false;
            nextSpawn.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else
        {
            targetSpawn = nextSpawn;
            targetSpawn.transform.position = transform.position;
            targetSpawn.transform.localScale = Vector3.one;
            targetSpawn.GetComponent<TetrisBlock>().enabled = true;
            ListTetrominoes.Add(targetSpawn);
            nextSpawn = null;
            nextSpawn = Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], transform.position + new Vector3(6.4f, -0.5f, 0), Quaternion.identity);
            nextSpawn.GetComponent<TetrisBlock>().enabled = false;
            nextSpawn.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            //destroyCheck();
        }
    }

    //테트로미노 삭제 및 관리
    void destroyCheck()
    {
        if(ListTetrominoes.Count > 0)
        {
            for (int i = 0; i < ListTetrominoes.Count; i++)
            {
                if(ListTetrominoes[i].transform.childCount == 0)
                {
                    Destroy(ListTetrominoes[i]);
                    ListTetrominoes.RemoveAt(i);
                }
            }
        }
    }

    void Hold()
    {
        Debug.Log("Hold");

        if(isHoldFirst) // 홀드가 처음일때
        {
            isHoldFirst = false; //첫 홀드가 이제 아님
            HoldBasket = targetSpawn; // 현재 테트로미노를 홀드바구니에 넣어줌
            HoldBasket.transform.position = new Vector3(-5, 16, 0); // 홀드 바구니 위치로 이동
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
            if(CanHold) // 홀드가 가능한 상황일 때
            {
                HoldBasket.transform.position = transform.position;
                HoldBasket.GetComponent<TetrisBlock>().enabled = true; 
                HoldBasket = targetSpawn;
                HoldBasket.transform.position = new Vector3(-5, 16,0);
                targetSpawn.GetComponent<TetrisBlock>().enabled = false;             
                HoldBasket.transform.position = targetSpawn.transform.position; 
                CanHold = false; // 홀드를 실행했으니 홀드를 다시 못하는 상황으로 만들어 줌
            }
        }
    }

}
