using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDmgDealer : MonoBehaviour
{
    [SerializeField] int shieldDamage = 1;
 
    public int GetShieldDamage()
    {
        return shieldDamage;
    }
    public void Hit()
    {
        Destroy(gameObject);

    }

}
