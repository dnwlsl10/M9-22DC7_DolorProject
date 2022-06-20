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
        [SerializeField] private Text   HP_text;
    //[Header("SKILL_STATUS")]
    //[SerializeField] private Slider Sheild;
    //[SerializeField] private Slider TeslaCanon;
    //[SerializeField] private Slider Ultimate_Skill;

    public void UpdateHPGauge(float damageAmount)
    {
        HP.value -= damageAmount / 100;
        //HP_text.text = ;
    }

    //public void UpdateSheidGauge(int shield_guage)
    //{

    //}
    //public void UpdateTeslaGauge(int tesla_gauge)
    //{

    //}
    //public void UpdateUltimateSkillGauge(int US_gauge)
    //{

    //}
}
