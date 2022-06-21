using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabControllerHighlight : MonoBehaviour
{
    [SerializeField]
    private Material originMat;
    public Material hightlight;
    private MeshRenderer mr;

    void Start(){
        this.mr = this.GetComponent<MeshRenderer>();
        this.originMat = mr.material;
    }

    public void OnGrabHightlight(){
        this.mr.sharedMaterial = hightlight;
    }

    public void OnRelease(){
        this.mr.sharedMaterial = originMat;
    }
}
