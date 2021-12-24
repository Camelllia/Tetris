using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    AudioSource audioSource;
    public GameObject mana;
    bool isplaying;

    // Start is called before the first frame update
    void Start()
    {
        audioSource=mana.gameObject.GetComponent<AudioSource>();
        audioSource.Play();
        audioSource.mute = false;
        audioSource.loop = true;
        audioSource.pitch = 1;
        audioSource.volume = 1;
        isplaying = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic()
    {
        if (isplaying)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.UnPause();
        }
        isplaying = !isplaying;
    }

}
