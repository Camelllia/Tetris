using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClipKnock;
    public AudioClip audioClipApplause;

    public static SoundManager instance;


    void Awake() { if (SoundManager.instance == null) { SoundManager.instance = this; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void PlayKnockSound() { audioSource.PlayOneShot(audioClipKnock); }
    public void PlayApplauseSound() { audioSource.PlayOneShot(audioClipApplause); }
}
