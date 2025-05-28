using UnityEngine;

public class EnemyStats : BaseCharacterStats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;

    [Header("Level Details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifer = .4f;

    protected override void Start()
    {
        ApplyLevelModifers();

        base.Start();

        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();

    }

    private void ApplyLevelModifers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);
    }

    private void Modify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifer = _stat.GetValue() * percentageModifer;

            _stat.AddModifier(Mathf.RoundToInt(modifer));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();

        myDropSystem.GenerateDrop();
    }
}
