using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisSpawner : MonoBehaviour
{
    public GameObject[] Tetrominoes;
    public List<GameObject> ListTetrominoes;
    GameObject nextSpawn;
    GameObject targetSpawn;
    bool isFirst = true;
    
    void Start()
    {
        ListTetrominoes = new List<GameObject>();
        NewTetrominoes();
    }

    //���ο� ��Ʈ�ι̳� ����
    public void NewTetrominoes()
    {
        //0.5�� ���� ����
        Invoke("createTetrominoes", 0.5f);
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
            nextSpawn = Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], transform.position + new Vector3(5.6f, -0.2f, 0), Quaternion.identity);
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
            nextSpawn = Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], transform.position + new Vector3(4, 1, 0), Quaternion.identity);
            nextSpawn.GetComponent<TetrisBlock>().enabled = false;
            nextSpawn.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            destroyCheck();
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
 
}
