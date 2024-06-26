using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooting : ObjShooting
{
    [Header("Boss Shooting")]
    [SerializeField] protected int countBullet = 8;

    protected override void Start()
    {
        this.bulletName = BulletSpawner.Instance.bossBullet_1;
    }

    protected override void IsShooting()
    {
        this.isShooting = true;
    }

    protected override void Shooting()
    {
        this.shootTimer += Time.fixedDeltaTime;

        if (!this.isShooting) return;
        if (this.shootTimer < this.shootDelay) return;
        this.shootTimer = 0f;

        Vector3 spawnPos = transform.parent.position;

        float angle = 360 / countBullet;
        
        for(int i=0; i< countBullet; i++)
        {
            Quaternion rotation = transform.parent.rotation * Quaternion.Euler(0, 0, angle * i);
            Transform newBullet = BulletSpawner.Instance.SpawnByName(bulletName, spawnPos, rotation);
            if (newBullet == null)
                return;

            BulletCtrl bulletCtrl = newBullet.GetComponent<BulletCtrl>();
            bulletCtrl.SetShotter(transform.parent);

            newBullet.gameObject.SetActive(true);
        }
        
    }
}
