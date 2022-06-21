using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<UIManager>();

            return instance;
        }
    }

    [Header("HP_STATUS")]
        [SerializeField] private Slider HP;
        [SerializeField] private Text HP_text;
        [SerializeField] private Text bullet;
    [Header("SKILL_STATUS")]
        //[SerializeField] private Slider Sheild;
        //[SerializeField] private Slider TeslaCanon;
        [SerializeField] private Slider Ultimate_Skill;
        [SerializeField] private Slider Shield_Skill;
        [SerializeField] private Slider Tesla_Skill;

    float ultimateChargingTime = 0.0001f;
    float normalChargeingTime = 0.001f;

    // public void UpdateHPGauge(float damageAmount)
    // {
    //     HP.value -= damageAmount / 100f;
    //     //HP_text.text = "" + ;
    // }
    void HPUpdate(float cur_val, float max_val)
    {
        float per = cur_val / max_val;
        HP.value = per;
        HP_text.text = "" + (per * 100);
    }

    void BulletUpdate(float cur_val, float max_val)
    {
        bullet.text = "" + cur_val;
    }

    // void GaugeUpdate()
    // public void UpdateUltimateSkillGauge()
    // {
    //     if(Ultimate_Skill.value >= 1) return;
    //     else Ultimate_Skill.value += ultimateChargingTime; 
    // }

    // public void UpdateTeslaSkillGauge()
    // {
    //     if(Tesla_Skill.value >= 1) return;
    //     else Tesla_Skill.value += normalChargeingTime; 
    // }

    // public void UpdateSheildSkillGauge()
    // {
    //     if(Shield_Skill.value >= 1) return;
    //     else Shield_Skill.value += normalChargeingTime; 
    // }

    // private void FixedUpdate() 
    // {
    //        UpdateUltimateSkillGauge();
    //        UpdateTeslaSkillGauge();
    //        UpdateSheildSkillGauge();
    // }

    // private void OnEnable()
    // {
    //     BasicWeapon bw = null;
    //     ShieldWeapon sw = null;
    //     TeslaWeapon tw = null;
    //     bw.OnValueChange += HPUpdate;
    //     bw.OnValueChange += BulletUpdate;
    // }
}
