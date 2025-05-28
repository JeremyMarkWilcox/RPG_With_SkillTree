using UnityEngine;
using System.Collections.Generic;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private float chanceToLoseItems;
    [SerializeField] private float chanceToLoseMaterials;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;
        
        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> MaterialsToLose = new List<InventoryItem>();

        foreach (InventoryItem item in inventory.GetEquipmentList())
        {
            if (Random.Range(0, 100) <= chanceToLoseItems)
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }
        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.UnEquipItem(itemsToUnequip[i].data as ItemData_Equipment);
        }

        foreach (InventoryItem item in inventory.GetStashList())
        {
            if (Random.Range(0, 100) <= chanceToLoseMaterials)
            {
                DropItem(item.data);  
                MaterialsToLose.Add(item);
            }
        }

        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.RemoveItem(MaterialsToLose[i].data);
        }
    }
}
