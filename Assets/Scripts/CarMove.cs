using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour
{
    // Start is called before the first frame update

    SpriteRenderer SR;

    float speed;
    Vector2 dir;

    void Start()
    {
        speed = 4;
        dir = Vector2.right;
    }
    // Update is called once per frame
    void Update()
    {
        SR = this.GetComponent<SpriteRenderer>();
        if(dir == Vector2.right)
        {
            transform.Translate(dir * speed * Time.deltaTime);
        }  
        else if(dir == Vector2.left)
        {
            transform.Translate(dir * speed * Time.deltaTime);
        }
        Debug.Log(this.transform.position.x);
        DirChange();
    }

    void DirChange()
    {
        if (this.transform.position.x > 30)
        {
            SR.flipX = false;
            dir = Vector2.left;
        }
        else if (this.transform.position.x < 15)
        {
            SR.flipX = true;     
            dir = Vector2.right;
        }
    }



}
