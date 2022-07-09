using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public enum eOrbType
{
    OrbA, OrbB, OrbC
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
    //new Color(0.1136274f,0.5188679f, 0.01330545f)
    //new Color(0.129365f ,0.04142933f, 0.4622642f)
    //new Color(0.957f, 0.4659617f, 0.1058823f)
    private Color[] color = new Color[] {Color.green, Color.black , new Color(0.957f, 0.4659617f, 0.1058823f) }; 
    System.Action<int> a;
    private void Awake()
    {
        this.gameObject.GetComponent<Collider>().enabled = false;
        mr = this.GetComponent<MeshRenderer>();
        orbType = eOrbType.OrbA;
        Debug.Log(orbType);
        mat = mr.material;
        mat.SetColor("_Outline_Color", color[(int)orbType]);
        a += transform.root.GetComponentInChildren<OrbFire>().SetType;
    }
    private void ChangeColor(eOrbType ot)
    {
        mat.SetColor("_Outline_Color", color[(int)ot]);
        uIOrb.OnLerpUI((int)ot);
        this.orbType = ot;
        a?.Invoke((int)ot);
    }
    private void OnCollisionEnter(Collision other) 
    {

        if(other.collider.CompareTag("RightHand"))
        {
            if(bGrabRightHand) return;
            AudioPool.instance.Play(onTouchSFX.name, 2, this.transform.position);
            Debug.Log("Test Hand");
            switch(orbType)
            {
                case eOrbType.OrbA:
                    ChangeColor(eOrbType.OrbB);
                    break;
                case eOrbType.OrbB:
                    ChangeColor(eOrbType.OrbC);
                break;
                case eOrbType.OrbC:
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
