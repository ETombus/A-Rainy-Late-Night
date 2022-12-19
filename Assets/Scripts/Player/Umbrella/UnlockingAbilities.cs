using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockingAbilities : MonoBehaviour
{
    [SerializeField] bool slice;
    [SerializeField] bool shoot;
    [SerializeField] bool grapple;

    public void UnlockAbility(int abilityToUnlock)
    {
        switch (abilityToUnlock)
        {
            case 1:
                slice = true;
                GetComponent<Slice>().enabled = true;
                break;
            case 2:
                shoot = true;
                break;
            case 3:
                grapple = true;
                GetComponent<GrappleInput>().enabled = true;
                break;
            default:
                break;
        }
    }
}
