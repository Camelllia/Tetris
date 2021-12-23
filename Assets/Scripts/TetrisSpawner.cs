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

    //���ο� ��Ʈ�ι̳� ����
    public void NewTetrominoes()
    {
        //0.5�� ���� ����
        Invoke("createTetrominoes", 0.1f);
    }

    //��Ʈ�ι̳� ����
    void createTetrominoes()
    {
        if(isFirst)
        {
            isFirst = false;
            targetSpawn = Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], transform.position, Quaternion.identity);
            ListTetrominoes.Add(targetSpawn);

            //���� ������ ��Ʈ�ι̳� �����ֱ�
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

    //��Ʈ�ι̳� ���� �� ����
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

        if(isHoldFirst) // Ȧ�尡 ó���϶�
        {
            isHoldFirst = false; //ù Ȧ�尡 ���� �ƴ�
            HoldBasket = targetSpawn; // ���� ��Ʈ�ι̳븦 Ȧ��ٱ��Ͽ� �־���
            HoldBasket.transform.position = new Vector3(-5, 16, 0); // Ȧ�� �ٱ��� ��ġ�� �̵�
            HoldBasket.GetComponent<TetrisBlock>().enabled = false; // Ȧ�� ���� ����� ����
            targetSpawn = nextSpawn; // ���� ��Ʈ�ι̳븦 ���� ��Ʈ�ι̳뿡�� ���ܿ�
            targetSpawn.transform.position = transform.position; // ���� ��ġ�� �̵�
            targetSpawn.transform.localScale = Vector3.one; // ������ �� ����
            targetSpawn.GetComponent<TetrisBlock>().enabled = true; // ��Ʈ�ι̳� ��� ���ֱ�
            createTetrominoes(); // �ٽ� ��Ʈ�ι̳� ����
            CanHold = false; // Ȧ�带 �� �� �����߱� ������ Ȧ�� ����
        }
        else
        {
            if(CanHold) // Ȧ�尡 ������ ��Ȳ�� ��
            {
                HoldBasket.transform.position = transform.position;
                HoldBasket.GetComponent<TetrisBlock>().enabled = true; 
                HoldBasket = targetSpawn;
                HoldBasket.transform.position = new Vector3(-5, 16,0);
                targetSpawn.GetComponent<TetrisBlock>().enabled = false;             
                HoldBasket.transform.position = targetSpawn.transform.position; 
                CanHold = false; // Ȧ�带 ���������� Ȧ�带 �ٽ� ���ϴ� ��Ȳ���� ����� ��
            }
        }
    }

}
