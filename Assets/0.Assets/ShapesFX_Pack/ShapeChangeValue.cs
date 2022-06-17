using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeChangeValue : MonoBehaviour
{
    private MeshRenderer mr;
    private Material mat;

    private float _Shrink_Faces_Amplitude;
    private float _Animation_speed;
    private float _NormalPush;
    private Color _FrontFace_Color;
    private Color _Outline_Color;
    public System.Action OnSelected;
    public bool isSelect;
    public bool isOpening;
    bool isStart;

    private void Start()
    {
        this.mr = this.GetComponent<MeshRenderer>();
        //None
        this.mat = this.mr.material;
        //Defult
        this.mat.SetFloat("_Animation_speed", _Animation_speed = 0.5f);
        this.mat.SetFloat("_NormalPush", _NormalPush = 0);
        this.mat.SetFloat("_Shrink_Faces_Amplitude", _Shrink_Faces_Amplitude = 0);
        this.mat.SetColor("_FrontFace_Color", _FrontFace_Color = Color.black);
        this.mat.SetColor("_Outline_Color", _Outline_Color = Color.white);
    }


    public void OnDefulte()
    {
        this.mat.SetFloat("_Animation_speed", _Animation_speed = 0.5f);
        this.mat.SetFloat("_NormalPush", _NormalPush = 0);
        this.mat.SetFloat("_Shrink_Faces_Amplitude", _Shrink_Faces_Amplitude = 0);
        this.mat.SetColor("_FrontFace_Color", _FrontFace_Color = Color.black);
        this.mat.SetColor("_Outline_Color", _Outline_Color = Color.white);
    }
    //_Animation_speed 0 => 0.15f
    //_NormalPush 0 => 0.5f

    public IEnumerator OnStart()
    {
        float _g = 1;
        Color cr = new Color(0, _g, 0, 1);
        this.mat.SetColor("_FrontFace_Color", cr);


        while (_NormalPush <= 0.5f)
        {
            this.mat.SetColor("_FrontFace_Color", cr);
            cr.g += 0.04f;
            this.mat.SetFloat("_NormalPush", _NormalPush += 0.1f);
            yield return null;
        }
    }


    IEnumerator OnOpening()
    {
        float _r = 0;
        float _g = 1;
        Color cr = new Color(_r, _g, 0, 1);
        this.mat.SetColor("_FrontFace_Color", cr);

        while (cr.r < 1 || _NormalPush > 0 || !isOpening)
        {
            this.mat.SetColor("_FrontFace_Color", cr);
            cr.r += 0.01f;
            cr.g -= 0.01f;
            this.mat.SetFloat("_NormalPush", _NormalPush -= 0.01f);
            if (cr.r >= 1 || cr.g <= 0 || _NormalPush <=0)
            {
                cr.r = 1;
                cr.g = 0;
                _NormalPush = 0;
            }


            yield return null;
        }

        this.mat.SetFloat("_Shrink_Faces_Amplitude", _Shrink_Faces_Amplitude = 0);
        this.mat.SetColor("_FrontFace_Color", _FrontFace_Color = Color.black);
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isSelect && other.gameObject.CompareTag("Player"))
        {
            isSelect = true;
            OnSelected();
            StopAllCoroutines();
            StartCoroutine(OnOpening());
        }
    }
}
