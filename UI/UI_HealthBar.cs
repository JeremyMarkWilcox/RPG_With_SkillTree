using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private BaseCharacter baseCharacter;
    private BaseCharacterStats myStats;
    private RectTransform myTransform;
    private Slider slider;

    void Start()
    {
        myTransform = GetComponent<RectTransform>();
        baseCharacter = GetComponentInParent<BaseCharacter>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<BaseCharacterStats>();

        baseCharacter.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    private void Update()
    {
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }


    private void FlipUI() => myTransform.Rotate(0, 180, 0);

    private void OnDisable()
    {
        baseCharacter.onFlipped -= FlipUI;
        myStats.onHealthChanged -= UpdateHealthUI;
    }    
}
