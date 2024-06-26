using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipUpgrade : NguyenMonoBehaviour
{
    [SerializeField] protected ShipCtrl shipCtrl;

    private static ShipUpgrade instance;
    public static ShipUpgrade Instance => instance;

    [SerializeField] protected int currentLevel = 0;
    public int GetCurrentLevel => currentLevel;
    [SerializeField] protected int maxLevel = 9;
    [SerializeField] protected int upgradePoint;
    public int GetUpgradePoint => upgradePoint;
    [SerializeField] protected int nextUpgradePoint;
    public int GetNextUpgradePoint => nextUpgradePoint;
    [SerializeField] protected ShipLooter shipLooter;

    [SerializeField] protected bool unLockLaser = false;
    [SerializeField] protected bool unLockRocket = false;
    [SerializeField] protected int levelUnLockLaser = 3;
    [SerializeField] protected int levelUnLockRocket = 5;
    
    protected override void Awake()
    {
        base.Awake();
        ShipUpgrade.instance = this;
    }

    protected override void Start()
    {
        this.nextUpgradePoint = shipCtrl.GetShootableObject.listUpgradePoint[currentLevel];
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadShipCtrl();
        this.LoadShipLooter();
    }

    protected virtual void LoadShipCtrl()
    {
        if (this.shipCtrl != null) return;
        this.shipCtrl = transform.parent.GetComponent<ShipCtrl>();
        Debug.Log(transform.name + ": LoadShipCtrl", gameObject);
    }

    protected virtual void LoadShipLooter()
    {
        if (this.shipLooter != null) return;
        this.shipLooter = transform.parent.GetComponentInChildren<ShipLooter>();
        Debug.Log(transform.name + ": LoadShipLooter", gameObject);
    }

    public void AddUpgradePoint(int point)
    {
        this.upgradePoint += point;
        this.checkUpgrade();
    }

    protected virtual void checkUpgrade()
    {
        if(this.upgradePoint >= this.nextUpgradePoint && this.currentLevel < this.maxLevel)
        {
            this.upgradePoint -= this.nextUpgradePoint;
            this.UpgradeLevel();
        }
    }

    public void UpgradeLevel()
    {
        this.currentLevel++;
        AudioManager.Instance.PlaySfx(AudioManager.Instance.levelUpAudioClip);

        if (this.currentLevel >= this.maxLevel)
            this.currentLevel = this.maxLevel;

        if(currentLevel < shipCtrl.GetShootableObject.listUpgradePoint.Count)
            this.nextUpgradePoint = shipCtrl.GetShootableObject.listUpgradePoint[currentLevel];
        this.UpgradeShoot();
        this.UnLockedSkill();
    }

    protected virtual void UpgradeShoot()
    {
        if (this.currentLevel < 3)
            shipCtrl.GetObjectShooting.SetBulletName(BulletSpawner.Instance.shipBullet_1);
        else if (this.currentLevel < 6)
            shipCtrl.GetObjectShooting.SetBulletName(BulletSpawner.Instance.shipBullet_2);
        else
            shipCtrl.GetObjectShooting.SetBulletName(BulletSpawner.Instance.shipBullet_3);
    }

    protected virtual void UnLockedSkill()
    {
        if (this.currentLevel >= levelUnLockLaser && !this.unLockLaser)
        {
            this.unLockLaser = true;
            shipCtrl.GetSkillCtrl.GetSkillLaser.UnLock();
        }
        if(this.currentLevel >= levelUnLockRocket && !this.unLockRocket)
        {
            this.unLockRocket = true;
            shipCtrl.GetSkillCtrl.GetSkillRocket.UnLock();
        }  
    }
}
