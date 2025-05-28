using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (collision.GetComponent<BaseCharacterStats>().isDead)
                    return;

            Debug.Log("Picked up Item");
            myItemObject.PickUpItem();
        }
    }
}
