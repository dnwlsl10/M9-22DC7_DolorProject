using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    AudioSource source;
    WaitForSeconds ws = new WaitForSeconds(0.1f);
    public int audioType;

    private void Awake() {
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
    
    public void Play(AudioClip clip)
    {
        gameObject.SetActive(true);

        if (audioType != 2)
        {
            source.clip = clip;
            source.Play();
        }
        else
        {
            source.PlayOneShot(clip);
        }

        StartCoroutine(IEAudioEnd());
    }

    IEnumerator IEAudioEnd()
    {
        while(source.isPlaying)
            yield return ws;
        
        transform.parent = AudioPool.instance.transform;
        gameObject.SetActive(false);
    }

    private void OnDisable() {
        AudioPool.instance.ReturnToPool(this);
    }
}
