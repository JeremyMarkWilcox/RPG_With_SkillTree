using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item Effect/ Heal Effect")]

public class Effect_Heal : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercentage;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        

        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercentage);

        playerStats.IncreaseHealthBy(healAmount);
    }
}
