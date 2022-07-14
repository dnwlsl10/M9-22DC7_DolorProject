using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : MonoBehaviour
{
    public System.Action onComplete;

    public void Init()
    {
        StartCoroutine(this.WaitForDisplayLogo());
    }
    private IEnumerator WaitForDisplayLogo()
    {
        // Debug.Log("1");
        yield return new WaitForSeconds(10f);
        // Debug.Log("2");
        this.onComplete();
    }
}
