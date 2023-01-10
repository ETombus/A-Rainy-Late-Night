using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUnlockTrigger : MonoBehaviour
{
    [SerializeField] int abilityToUnlock;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<UnlockingAbilities>().UnlockAbility(abilityToUnlock);
        }
    }
}
