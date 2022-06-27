using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    private Status status; 
    private BasicWeapon bw;
    private SkillShield sw;
    public static UIManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<UIManager>();

            return instance;
        }
    }


    
    /*------------------start Event Method-----------------*/
    [Header("HP_STATUS")]
        [SerializeField] private Slider HP;
        [SerializeField] private TextMeshProUGUI HP_text;
    [Header("CURRENT_BULLET")]
        [SerializeField] private TextMeshProUGUI bullet_text;
    [Header("SKILL_STATUS")]
        //[SerializeField] private Slider Sheild;
        //[SerializeField] private Slider TeslaCanon;
        [SerializeField] private Slider Ultimate_Skill;
        [SerializeField] private Slider Shield_Skill;
        [SerializeField] private Slider Tesla_Skill;

    public void HPUpdate(float cur_val, float max_val)
    {
        float per = cur_val / max_val;
        HP.value = per;
        if(HP.value == 0) 
        {
            HP_text.text = "0";
            return;
        }
        else HP_text.text = "" + cur_val;
            
    }

    public void ShieldUpdate(float cur_val, float max_val)
    {
        float per = cur_val / max_val;
        Shield_Skill.value = per;
    }

    public void TeslaHPUpdate(float cur_val, float max_val)
    {
        float per = cur_val / max_val;
        Tesla_Skill.value = per;
    }

    public void BulletUpdate(float cur_val, float max_val)
    {
        bullet_text.text = "" + cur_val;
    }
    /*------------------end Event Method-----------------*/

    /*------------------start Tweening Method-----------------*/
    [Header("EVENT_ALERT")]
    [SerializeField] private RectTransform UltimateAlert;
    // public void UlitimateGaugeFilled()
    // {   
    //     if(Ultimate_Skill.value != 1) return;
    //     else
    //     {
    //         UltimateAlert.DOScale(new Vector3(1,3,1),1).onComplete(() =>
    //         {
    //             Ultimate_Skill.
    //         });
    //     }
    // }
    /*------------------end Tweening Method-----------------*/

    // private void OnEnable()
    // {
    //     status.OnHpValueChange += HPUpdate;
    //     bw.OnValueChange += BulletUpdate;
    //     sw.OnValueChange += ShieldUpdate;
    // }

    // private void OnDisable() 
    // {
    //     status.OnHpValueChange -= HPUpdate;
    //     bw.OnValueChange -= BulletUpdate;
    //     sw.OnValueChange -= ShieldUpdate;
    // }
}
