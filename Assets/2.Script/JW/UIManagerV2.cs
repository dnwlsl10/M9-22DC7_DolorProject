using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class UIManagerV2 : MonoBehaviour
{
    /*-----Event Inject-----*/
<<<<<<< HEAD
    [SerializeField] private Status status;
    [SerializeField] private BasicWeapon bw;
    [SerializeField] private SkillShield sw;
    [SerializeField] private GuidedMissile gm;
=======
    [SerializeField]
    private Status status;
    [SerializeField]
    private BasicWeapon bw;
    [SerializeField]
    private SkillShield sw;
    [SerializeField]
    private GuidedMissile gm;
>>>>>>> develop
    /*----------------------*/

    [Header("HP_STATUS")]
    [SerializeField] private Slider HP;
    [SerializeField] private TextMeshProUGUI HP_text;
    [Header("CURRENT_BULLET")]
    [SerializeField] private TextMeshProUGUI bullet_text;
    [Header("SKILL_STATUS")]
    [SerializeField] private Image Ultimate_Skill;
    [SerializeField] private Image Shield_Skill;
    [SerializeField] private Image Orb_Skill;




   

    /*-----------------------Bool Check------------------- */
    private bool b_Notification = true;


    /*------------------start Event Method-----------------*/
    [ContextMenu("Initialize")]
    public void Reset()
    {
        if (this.status == null) status = this.transform.root.root.GetComponent<Status>();
        if (this.bw == null) bw = this.transform.root.root.Find("WeaponScript").Find("BasicWeapon").GetComponent<BasicWeapon>();
        if (this.sw == null) sw = this.transform.root.root.Find("root").GetComponentInChildren<SkillShield>();
        if (this.gm == null) gm = this.transform.root.root.Find("WeaponScript").Find("GuidedMissile").GetComponent<GuidedMissile>();
    }

    public void HPUpdate(float cur_val, float max_val)
    {
        float per = cur_val / max_val;
        HP.value = per;
        if (HP.value == 0)
        {
            HP_text.text = "0";
            return;
        }
        else HP_text.text = "" + cur_val;

    }

    public void ShieldUpdate(float cur_val, float max_val)
    {
        float per = cur_val / max_val;
        Shield_Skill.fillAmount = per;
    }

    public void OrbUpdate(float cur_val, float max_val)
    {
        float per = cur_val / max_val;
        Orb_Skill.fillAmount = per;
    }

        public void UlitimateUpdate(float cur_val, float max_val)
    {
        float per = cur_val / max_val;
        Ultimate_Skill.fillAmount = per;
    }

    public void BulletUpdate(float cur_val, float max_val)
    {
        bullet_text.text = "" + cur_val;
    }
    /*------------------end Event Method-----------------*/

    /*------------------start Tweening Method-----------------*/
    // public void UlitimatFilledTween()
    // {
    //     if(Ultimate_Skill.fillAmount >= 1 && b_Notification == false)
    //     {
    //         NotificationEvent.PlayNotification();
    //         b_Notification = true;
    //     }
    //     else if(Ultimate_Skill.fillAmount != 1)
    //     {
    //         b_Notification = false;
    //         return;
    //     }
    // }

    // public void HPWarningTween()
    // {

    // }
    /*------------------end Tweening Method-----------------*/

    private void OnEnable()
    {
        status.OnValueChange += HPUpdate;
        bw.OnValueChange += BulletUpdate;
        sw.OnValueChange += ShieldUpdate;
        gm.OnValueChange += UlitimateUpdate;
    }

    private void OnDisable()
    {
        status.OnValueChange -= HPUpdate;
        bw.OnValueChange -= BulletUpdate;
        sw.OnValueChange -= ShieldUpdate;
        gm.OnValueChange -= UlitimateUpdate;

    }
}