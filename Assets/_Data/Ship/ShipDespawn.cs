using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDespawn : DespawnByDistance
{
    public override void DespawnObject()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
