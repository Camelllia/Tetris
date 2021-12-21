using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    public float LifeTime = 0.5f;
    
    void Start()
    {
        Invoke("particleDestroy", LifeTime);
    }

    void particleDestroy()
    {
        Destroy(this.gameObject);
    }
    
}
