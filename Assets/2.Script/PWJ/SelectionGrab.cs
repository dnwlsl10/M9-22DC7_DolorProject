using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionGrab : MonoBehaviour
{
    public enum eHand
    {
        Right,
        Left
    }
    public eHand hand;
    public System.Action onRight;
    public System.Action onLeft;
    public bool isRightExistence;
    public bool isLeftExistence;
    private CubeChangeValue cubeChange;
    private void Start()
    {
        isRightExistence = true;
        isLeftExistence = false;
        this.cubeChange = this.GetComponentInChildren<CubeChangeValue>();
        OnDefultValue();
    }

    public void OnDefultValue()
    {
        this.cubeChange.OnDefult();
    }

    public void OnStartValue()
    {
        StartCoroutine(this.cubeChange.OnStart());
    }

    public void OnChangeRed()
    {
        this.cubeChange.OnChangeRed();
    }

    public void OnChangeGreen()
    {
        this.cubeChange.OnChangeGreen();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (isRightExistence && hand == eHand.Right && other.gameObject.CompareTag("Player"))
        {
            onRight();
            StartCoroutine(this.cubeChange.OnTigger());
        }
        else if (isLeftExistence && hand == eHand.Left && other.gameObject.CompareTag("Player")) 
        {
            onLeft();
            StartCoroutine(this.cubeChange.OnTigger());
        }
    }


}
