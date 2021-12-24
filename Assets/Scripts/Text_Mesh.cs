using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_Mesh : MonoBehaviour
{
    public TextMesh textMesh;
    public float targetY;
    public float degree;
    public float radian;
    public float scale;

    // Start is called before the first frame update
    void Start()
    {
        degree = 0;
        radian = 0;
        scale = 1;
        targetY = transform.position.y + 3f;
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        degree += 150f * Time.deltaTime;
        radian = degree * Mathf.PI / 180;
        transform.position = new Vector3(transform.position.x, targetY - 3f + Mathf.Sin(radian) * 3f, transform.position.z);
        if (degree < 60)
        {
            scale = 0.5f + Mathf.Sin(radian * 2f) * 0.4f;
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
