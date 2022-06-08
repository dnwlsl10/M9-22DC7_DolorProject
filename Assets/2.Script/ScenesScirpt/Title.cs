using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    public System.Action OnClick;

    public void Update()
    {

        if (Input.anyKeyDown)
        {
            this.OnClick();
        }
    }
}
