using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public enum eOrbType
{
    OrbA, OrbB
}
public class OrbTrigger : MonoBehaviour
{
    public bool bGrabRightHand;
    public UIOrb uIOrb;
    private eOrbType orbType;
    private Coroutine coroutineholder;
    private MeshRenderer mr;
    private Material mat;
    public AudioClip onTouchSFX;
    private Color[] color = new Color[] {new Color(0.1136274f, 0.5188679f, 0.01330545f), new Color(0.129365f, 0.04142933f, 0.4622642f) }; 
    System.Action<int> a;
    private Collider orbCol;

    private void Awake()
    {
        orbCol = this.gameObject.GetComponent<Collider>();
        orbCol.enabled = false;
        mr = this.GetComponent<MeshRenderer>();
        orbType = eOrbType.OrbA;
        mat = mr.material;
        mat.SetColor("_Outline_Color", color[(int)orbType]);
        a += transform.root.GetComponentInChildren<OrbFire>().SetType;
    }
    private void ChangeColor(eOrbType ot)
    {
        mat.SetColor("_Outline_Color", color[(int)ot]);
        
        StartCoroutine(uIOrb.OnLerpUI((int)ot ,()=>{
            this.orbType = ot;
            a?.Invoke((int)ot);
            orbCol.enabled = true;
        }));
    }
    private void OnCollisionEnter(Collision other) 
    {
        if(other.collider.CompareTag("RightHand"))
        {
            if(bGrabRightHand) return;

            orbCol.enabled = false;
            AudioPool.instance.Play(onTouchSFX.name, 2, this.transform.position);
            switch(orbType)
            {
                case eOrbType.OrbA:
                    ChangeColor(eOrbType.OrbB);
                    break;
                case eOrbType.OrbB:
                    ChangeColor(eOrbType.OrbA);
                break;
            }
        }
    }
    public void OnGrabRight(){
        bGrabRightHand = true;
        this.gameObject.GetComponent<Collider>().enabled = true;
    } 
    public void OffGrabRight() => bGrabRightHand = false;
}
