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
    bool canChangeColor;
    public UIOrb uIOrb;
    private eOrbType orbType;
    private Coroutine coroutineholder;
    private MeshRenderer mr;
    private Material mat;
    private Collider collider;
    //new Color(0.1136274f,0.5188679f, 0.01330545f)
    //new Color(0.129365f ,0.04142933f, 0.4622642f)
    //new Color(0.957f, 0.4659617f, 0.1058823f)
    private Color[] color = new Color[] {Color.green, Color.black , new Color(0.957f, 0.4659617f, 0.1058823f) }; 
    System.Action<int> changePrefabIndex;
    int numOrbType;
    private void Awake()
    {
        numOrbType = System.Enum.GetValues(typeof(eOrbType)).Length;
        orbType = eOrbType.OrbA;

        collider = GetComponent<Collider>();
        canChangeColor = true;

        mr = this.GetComponent<MeshRenderer>();
        mat = mr.material;
        mat.SetColor("_Outline_Color", color[0]);

        changePrefabIndex = WeaponSystem.instance.GetComponentInChildren<OrbFire>().SetType;
    }
    private void ChangeColor(int orbIndex)
    {
        mat.SetColor("_Outline_Color", color[orbIndex]);
        uIOrb.OnLerpUI(orbIndex);
        orbType = (eOrbType)orbIndex;
        changePrefabIndex?.Invoke(orbIndex);
    }
    private void OnCollisionEnter(Collision other) 
    {
        if (other.relativeVelocity.magnitude < 1) return;

        if(canChangeColor && other.collider.CompareTag("RightHand"))
        {
            ChangeColor(((int)orbType + 1) % numOrbType);
        }
    }
    IEnumerator DelayAfterColorChange()
    {
        canChangeColor = false;
        yield return new WaitForSeconds(2);
        canChangeColor = true;
    }
    public void OnGrabRight(){
        collider.enabled = false;
    }
    public void OffGrabRight()
    {
        collider.enabled = true;
    }
}
