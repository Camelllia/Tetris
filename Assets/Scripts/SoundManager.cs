using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;
    public GameObject mana;

    bool isplaying;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = mana.GetComponent<AudioSource>();
        isplaying = true;
        audioSource.Play();
        audioSource.loop=true;
        audioSource.pitch = 1;
        audioSource.volume = 1;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Onclick()
    {
        if (isplaying)
        {
            audioSource.Pause();
        }
        else if(!isplaying)
        {
            audioSource.UnPause();
        }

        isplaying = !isplaying;
    }
}
