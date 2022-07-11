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
    bool canChangeColor;
    public UIOrb uIOrb;
    private eOrbType orbType;
    private Coroutine coroutineholder;
    private MeshRenderer mr;
    private Material mat;
    public AudioClip onTouchSFX;
    private Color[] color = new Color[] {new Color(0.1136274f, 0.5188679f, 0.01330545f), new Color(0.129365f, 0.04142933f, 0.4622642f) };
    System.Action<int> changePrefabIndex;
    private Collider col;
    int numOrbType;
    IEnumerator Start()
    {
        numOrbType = System.Enum.GetValues(typeof(eOrbType)).Length;
        orbType = eOrbType.OrbA;

        col = GetComponent<Collider>();
        col.enabled = false;

        mr = this.GetComponent<MeshRenderer>();
        mat = mr.material;
        mat.SetColor("_Outline_Color", color[0]);

        changePrefabIndex = WeaponSystem.instance.GetComponentInChildren<OrbFire>().SetType;

        yield return new WaitForSeconds(3f);
        col.enabled = true;
    }
    private void ChangeColor(int orbIndex)
    {
        mat.SetColor("_Outline_Color", color[orbIndex]);
        
        StartCoroutine(uIOrb.OnLerpUI(orbIndex ,()=>{
            this.orbType = (eOrbType)orbIndex;
            changePrefabIndex?.Invoke(orbIndex);
            col.enabled = true;
        }));
    }
    private void OnCollisionEnter(Collision other) 
    {
        if (other.relativeVelocity.magnitude < 0.1f) return;

        if(other.collider.CompareTag("RightHand"))
        {
            col.enabled = false;
            AudioPool.instance.Play(onTouchSFX.name, 2, this.transform.position);
            ChangeColor(((int)orbType + 1) % numOrbType);
        }
    }
    public void OnGrabRight(){
        col.enabled = false;
    }
    public void OffGrabRight()
    {
        col.enabled = true;
    }
}
