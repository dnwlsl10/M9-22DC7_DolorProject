using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeChangeValue : MonoBehaviour
{
    private MeshRenderer mr;
    private Material mat;

    private float _Shrink_Faces_Amplitude;
    private float _Animation_speed;
    private float _NormalPush;
    private Color _FrontFace_Color;
    private Color _Outline_Color;

    private void Start()
    {
        this.mr = this.GetComponent<MeshRenderer>();
        this.mat = this.mr.material;
    }

    bool isOpening;
    bool isStart;


    public void OnDefult()
    {
        this.mat.SetFloat("_Animation_speed", _Animation_speed = 0.5f);
        this.mat.SetFloat("_NormalPush", _NormalPush = 0.3f);
        this.mat.SetFloat("_Shrink_Faces_Amplitude", _Shrink_Faces_Amplitude = 3);
        this.mat.SetColor("_FrontFace_Color", _FrontFace_Color = Color.black);
        this.mat.SetColor("_Outline_Color", _Outline_Color = Color.white);
        this.mat.SetFloat("_Outline_Opacity", 30);
    }

    public IEnumerator OnStart()
    {
        if (this.gameObject.CompareTag("Left"))
        {
         
            float _r = 0;
            Color cr = new Color(_r, 0, 0, 1);
            this.mat.SetColor("_FrontFace_Color", cr);


            while (_NormalPush <= 0.5f)
            {
                this.mat.SetColor("_FrontFace_Color", cr);
                cr.r += 0.04f;
                this.mat.SetFloat("_NormalPush", _NormalPush += 0.01f);
                yield return null;
            }
        }
        else
        {
            float _g = 1;
            Color cr = new Color(0, _g, 0, 1);
            this.mat.SetColor("_FrontFace_Color", cr);


            while (_NormalPush <= 0.5f)
            {
                this.mat.SetColor("_FrontFace_Color", cr);
                cr.g += 0.04f;
                this.mat.SetFloat("_NormalPush", _NormalPush += 0.01f);
                yield return null;
            }
        }
       
    }

    public IEnumerator OnTigger()
    {
        this.mat.SetFloat("_Outline_Opacity", 60);
        yield return new WaitForSeconds(2f);
        this.mat.SetFloat("_Outline_Opacity", 30);
    }

    public void OnChangeRed()
    {
        this.mat.SetColor("_FrontFace_Color", _FrontFace_Color = Color.red);
    }

    public void OnChangeGreen()
    {
        this.mat.SetColor("_FrontFace_Color", _FrontFace_Color = Color.green);
    }
}
