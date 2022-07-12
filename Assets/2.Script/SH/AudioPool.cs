using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType {BGM, Loop, OneShot}
public class AudioPool : MonoBehaviour
{
    public static AudioPool instance;
    public AudioClip[] clips;
    Dictionary<string, AudioClip> clipDictionary = new Dictionary<string, AudioClip>();
    Queue<Audio>[] poolQueue = new Queue<Audio>[3];

    public GameObject audiosourcePrefab;

    private void Awake() {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        transform.position = Vector3.zero;

        poolQueue[0] = new Queue<Audio>();
        poolQueue[1] = new Queue<Audio>();
        poolQueue[2] = new Queue<Audio>();

        for (int i = 0; i < clips.Length; i++)
            clipDictionary.Add(clips[i].name, clips[i]);
        
        InitPool();
       // DontDestroyOnLoad(gameObject);
    }

    private void InitPool()
    {
        CreateNewSource(0);
        CreateNewSource(1);
        CreateNewSource(2);
    }

    private void CreateNewSource(int audioType)
    {
        GameObject go = Instantiate(audiosourcePrefab);
        go.transform.parent = transform;
        go.GetComponent<Audio>().Initialize(audioType);
        go.SetActive(false);
    }

    public Audio Play(string name, int type, Vector3 position, float volume=1)
    {
        if (name == null) return null;
        
        if (clipDictionary.ContainsKey(name) == false)
        {
            AudioClip clip = Resources.Load<AudioClip>("Audio/"+name);
            if (clip) clipDictionary.Add(name, clip);
            else
            {
                Debug.LogWarning("Audio name " + name + " not found");
                return null;
            }
        }

        if (poolQueue[type].Count <= 0)
            CreateNewSource(type);
        
        Audio audio = poolQueue[type].Dequeue();
        audio.transform.position = position;
        audio.Play(clipDictionary[name], volume);

        return audio;
    }

    public Audio Play(string name, int type, Vector3 position, Transform parent, float volume=1)
    {
        if (name == null) return null;

        Audio audio = Play(name, type, position);
        audio?.transform.SetParent(parent);
        return audio;
    }

    public void ReturnToPool(Audio a)
    {
        poolQueue[a.audioType].Enqueue(a);
        if (a.transform.parent != transform)
            StartCoroutine(SetParent(a.transform));
    }

    IEnumerator SetParent(Transform t)
    {
        yield return null;
        t.parent = transform;
    }
}
