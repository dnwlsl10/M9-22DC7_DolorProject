using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager2 : MonoBehaviour
{
    public static InGameManager2 instance;

    public event System.Action onGameStart;

    private void Awake() {
        instance = this;
    }

    IEnumerator Start() {
        yield return new WaitForSeconds(3);
        onGameStart?.Invoke();
    }
}
