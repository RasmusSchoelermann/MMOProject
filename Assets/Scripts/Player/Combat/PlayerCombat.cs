using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField]
    private int healthPoints = 10;

    [SerializeField]
    private int armorPoints = 0;

    private GameObject target;

    public GameObject Target
    {
        get { return target; }
        set { target = value; }
    }
    
    public bool HasTarget()
    {
        if (target != null)
            return true;

        return false;
    }
}
