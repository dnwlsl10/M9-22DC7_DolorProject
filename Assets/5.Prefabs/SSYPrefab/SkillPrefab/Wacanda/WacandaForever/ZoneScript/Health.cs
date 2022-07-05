using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Player Health")]
    public int startHealth;
    public int maxHealth;
    public int minHealth;
    public bool isAlive;

    [Header("Health UI")]
    public GameObject _healthSlider;
    private Slider healthSlider;

    [Header("Tutorial Only")]
    public GameObject _healthAmount;
    public Text healthText;

    void Start()
    {
        healthSlider = _healthSlider.GetComponent<Slider>();
        healthSlider.maxValue = maxHealth;
        healthSlider.minValue = minHealth;
        healthSlider.value = startHealth;

        healthText = _healthAmount.GetComponent<Text>();
    }

    public void ChangeHealth(int changeAmount)
    {
        healthSlider.value += changeAmount;

        healthText.text = healthSlider.value.ToString();
    }
}
