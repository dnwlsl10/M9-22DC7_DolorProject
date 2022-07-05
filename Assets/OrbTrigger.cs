using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class OrbTrigger : MonoBehaviour
{
    public enum eOrbType{
        OrbA, OrbB , OrbC
    }
    public bool bGrabRightHand;
    private bool isChanging;
    private UIOrb uIOrb;
    private eOrbType orbType;
    private Coroutine coroutineholder;
    private MeshRenderer mr;
    private MaterialPropertyBlock matBlock;
    private Color oAc = new Color(0.1136274f, 0.04142933f , 0.4622642f);
    private Color oBc = new Color(0.957f, 0.4659617f, 0.1058823f);
    private Color oCc = new Color(0.01330545f,0.1226415f, 0.01330545f);
    public float motionTime = 2f;
    private List<Color> colorList;
    private Dictionary<int, Color> orbdic = new Dictionary<int, Color>();
    private void Awake()
    {
        Color[] colors = new Color[3] { oAc, oBc, oCc };
        this.colorList = colors.ToList<Color>();

        for(int i = 0; i < colorList.Count; i++){
            orbdic.Add(i, colors[i]);
        }

        uIOrb = this.GetComponentInParent<UIOrb>();
        matBlock = new MaterialPropertyBlock();
        mr = this.GetComponent<MeshRenderer>();
    }
    private IEnumerator ChangeColor(eOrbType ot)
    {
        matBlock.SetColor("_Outline_Color", orbdic[(int)ot]);
        mr.SetPropertyBlock(matBlock);
        yield return new WaitForSeconds(motionTime);

        this.orbType = ot;
        uIOrb.OnSelectedOrb((int)this.orbType);
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
                    StartCoroutine(uIOrb.OnLerpUI(isChanging));
                    break;
                case eOrbType.OrbB:
                    StartCoroutine(ChangeColor(eOrbType.OrbC));
                    StartCoroutine(uIOrb.OnLerpUI(isChanging));
                break;
                case eOrbType.OrbC:
                    StartCoroutine(ChangeColor(eOrbType.OrbA));
                    StartCoroutine(uIOrb.OnLerpUI(isChanging));
                break;
            }
        }
    }
    public void OnGrabLight() => bGrabRightHand = !bGrabRightHand ? true : false;
}
