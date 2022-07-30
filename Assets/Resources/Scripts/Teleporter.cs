using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    public AudioClip hackSound;  
    private AudioSource _audioSource;
    public string sceneName = "Hacking";

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            _audioSource.PlayOneShot(hackSound);
            Initiate.Fade(sceneName, Color.white, 2f);
        }
    }
}
