using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;
    public GameObject mana;
    public GameObject xUI;

    bool isplaying;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = mana.GetComponent<AudioSource>();
        isplaying = true;
        audioSource.loop=true;
        audioSource.pitch = 1;
        audioSource.volume = 0.1f;


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
            xUI.SetActive(true);
        }
        else if(!isplaying)
        {
            audioSource.UnPause();
            xUI.SetActive(false);
        }

        isplaying = !isplaying;
    }
}
