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
    private bool isChanging;
    private UIOrb uIOrb;
    private eOrbType orbType;
    private Coroutine coroutineholder;
    private MeshRenderer mr;
    private MaterialPropertyBlock matBlock;
    public float motionTime = 2f;
    private List<Color> colorList;
    //private Dictionary<int, Color> orbdic = new Dictionary<int, Color>();
    private Color[] color = new Color[] {new Color(0.01330545f, 0.1226415f, 0.01330545f), new Color(0.129365f,0.04142933f,0.4622642f) , new Color(0.957f, 0.4659617f, 0.1058823f) }; 
    System.Action<int> a;
    private void Awake()
    {
       // this.GetComponent<Collider>().enabled = true;
        uIOrb = this.GetComponentInParent<UIOrb>();
        matBlock = new MaterialPropertyBlock();
        mr = this.GetComponent<MeshRenderer>();
        orbType = eOrbType.OrbA;
        matBlock.SetColor("_Outline_Color", color[(int)orbType]);
        mr.SetPropertyBlock(matBlock);
        a += transform.root.GetComponentInChildren<OrbFire>().SetType;
        
    }
    private IEnumerator ChangeColor(eOrbType ot)
    {
        matBlock.SetColor("_Outline_Color", color[(int)ot]);
        mr.SetPropertyBlock(matBlock);
        StartCoroutine(uIOrb.OnLerpUI(isChanging));
        yield return new WaitForSeconds(motionTime);
        this.orbType = ot;
        a?.Invoke((int)ot);
        isChanging = false;
    }
    private void OnCollisionEnter(Collision other) 
    {
        if(other.collider.CompareTag("RightHand") && !bGrabRightHand && !isChanging)
        {
            isChanging = true;
            Debug.Log("Test Hand");
            switch(orbType)
            {
                case eOrbType.OrbA:
                    StartCoroutine(ChangeColor(eOrbType.OrbB));
                    break;
                case eOrbType.OrbB:
                    StartCoroutine(ChangeColor(eOrbType.OrbC));
                break;
                case eOrbType.OrbC:
                    StartCoroutine(ChangeColor(eOrbType.OrbA)); 
                break;
            }
        }
    }
    public void OnGrabRight() => bGrabRightHand = true;
    public void OffGrabRight() => bGrabRightHand = false;
}
