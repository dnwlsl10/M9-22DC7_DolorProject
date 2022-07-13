using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Audio : MonoBehaviour
{
    public AudioSource source;
    WaitForSeconds ws = new WaitForSeconds(0.1f);
    public int audioType;

    private void Awake() {
        SceneManager.activeSceneChanged += (obj ,obj2) =>{
            this.gameObject.SetActive(false);
        };
        source = GetComponent<AudioSource>();     
    }

    public void Initialize(int audioType)
    {
        source.playOnAwake = false;
        this.audioType = audioType;
        switch(audioType)
        {
            case (int)AudioType.BGM:
            case (int)AudioType.Loop:
                source.loop = true;
                break;
            case (int)AudioType.OneShot:
                break;
        }
    }
    
    public void Play(AudioClip clip, float volume)
    {
        gameObject.SetActive(true);

        source.volume = volume;
        source.clip = clip;
        source.Play();

        if (audioType == 2)
            StartCoroutine(IEAudioEnd());
    }

    public void FadeOut()
    {
        StartCoroutine(IEFadeOut());
    }

    IEnumerator IEFadeOut()
    {
        float startVolume = source.volume;
        while(source.volume > 0.1f)
        {
            source.volume = source.volume - Time.deltaTime;
            print(source.volume);
            yield return null;
        }
        
        source.volume = startVolume;
        gameObject.SetActive(false);
    }

    IEnumerator IEAudioEnd()
    {
        while(source.isPlaying)
            yield return null;
        
        transform.parent = AudioPool.instance.transform;
        gameObject.SetActive(false);
    }

    private void OnDisable() {
        
        AudioPool.instance?.ReturnToPool(this);
    }

    private void OnDestroy() {
        Debug.LogWarning("AudioPool Object has been destroyed");
    }
}
