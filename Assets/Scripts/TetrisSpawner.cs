using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisSpawner : MonoBehaviour
{
    public GameObject[] Tetrominoes;
    public List<GameObject> ListTetrominoes;
    private List<GameObject> bag;
    private HashSet<int> deck = new HashSet<int>();
    private int next;
    GameObject nextSpawn;
    GameObject targetSpawn;
    GameObject HoldBasket;
    bool isFirst = true;
    public bool CanHold = true;

    TetrisBlock TetrisBlock;
    
    void Start()
    {
        ListTetrominoes = new List<GameObject>();
        bag = new List<GameObject>();
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

            if (deck.Count == Tetrominoes.Length)
            {
                deck.Clear();
            }


            do
            {
                next = Random.Range(0, Tetrominoes.Length);
            } while (deck.Contains(next));

            deck.Add(next);

            //���� ������ ��Ʈ�ι̳� �����ֱ�
            nextSpawn = Instantiate(Tetrominoes[next], transform.position + new Vector3(6.4f, -0.5f, 0), Quaternion.identity);
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
            nextSpawn = Instantiate(Tetrominoes[next], transform.position + new Vector3(6.4f, -0.5f, 0), Quaternion.identity);
            nextSpawn.GetComponent<TetrisBlock>().enabled = false;
            nextSpawn.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            
        }
    }
    

    void Hold()
    { 

        if(HoldBasket==null) // Ȧ�尡 ó���϶�
        {
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
