using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicTrigger : MonoBehaviour
{
    public AudioClip track;
    [Range(0f, 1f)]
    public float volumeTarget;
    public float volumeChangeDuration = 0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            var audioSource = GameObject.Find("AudioManager").GetComponent<AudioSource>();

            // Dont replay the same track if it's already playing
            if(track && track.name != audioSource.clip.name){
                audioSource.clip = track; 
                audioSource.time = 0; 
                audioSource.Stop(); 
                audioSource.Play(); 
            }

            StartCoroutine(StartFade(audioSource, volumeChangeDuration, volumeTarget));
        }
    }

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
