using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDespawn : DespawnByDistance
{
    public override void DespawnObject()
    {
        BossSpawner.Instance.Despawn(transform.parent);
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        this.disLimit = 300;
    }
}
