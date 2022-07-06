using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType {BGM, Loop, OneShot}
public class AudioPool : MonoBehaviour
{
    public static AudioPool instance;
    public AudioClip[] clips;
    Dictionary<string, AudioClip> clipDictionary = new Dictionary<string, AudioClip>();
    Queue<GameObject>[] poolQueue = new Queue<GameObject>[3];

    private void Awake() {
        instance = this;
        transform.position = Vector3.zero;

        poolQueue[0] = new Queue<GameObject>();
        poolQueue[1] = new Queue<GameObject>();
        poolQueue[2] = new Queue<GameObject>();

        for (int i = 0; i < clips.Length; i++)
        {
            clipDictionary.Add(clips[i].name, clips[i]);
        }

        InitPool();
    }

    private void InitPool()
    {
        CreateNewSource(0);
        CreateNewSource(1);
        CreateNewSource(2);
    }

    private GameObject CreateNewSource(int audioType)
    {
        GameObject go = new GameObject(System.Enum.GetName(typeof(AudioType), audioType), typeof(AudioSource), typeof(Audio));
        go.transform.parent = transform;
        go.GetComponent<Audio>().Initialize(audioType);
        go.SetActive(false);
        return go;
    }

    public GameObject Play(string name, int type, Vector3 position)
    {
        if (clipDictionary.ContainsKey(name) == false)
        {
            Debug.LogWarning("NOMATCHNAME");
            return null;
        }

        if (poolQueue[type].Count <= 0)
        {
            CreateNewSource(type);
        }

        GameObject go = poolQueue[type].Dequeue();
        go.transform.position = position;
        go.GetComponent<Audio>().Play(clipDictionary[name]);

        return go;
    }
    public GameObject Play(string name, int type, Vector3 position, Transform parent)
    {
        GameObject go = Play(name, type, position);
        go?.transform.SetParent(parent);
        return go;
    }

    public void ReturnToPool(Audio a)
    {
        poolQueue[a.audioType].Enqueue(a.gameObject);
    }
}
