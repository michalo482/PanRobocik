using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public enum CoverPerk
{
    Unavaible,
    CanTakeCover,
    CantTakeAndChangeCover
}
public enum GrenadePerk
{
    Unavaible,
    CanThrowGrenade
}
public class EnemyRange : Enemy
{


    [Header("Enemy Perks")]
    public EnemyRangeWeaponType weaponType;
    public CoverPerk coverPerk;
    public GrenadePerk grenadePerk;

    [Header("Grenade Perk")]
    public int grenadeDamage;
    public float grenadeCooldown;
    public float lastTimeGrenadeThrown = -10;
    public GameObject grenadePrefab;
    public float timeToTarget = 1.2f;
    public float explosionTimer = 0.75f;
    [SerializeField] private Transform grenadeStartPoint;
    public float impactPower;

    [Header("Advance Perk")]
    public float advanceSpeed;
    public float advanceStoppingDistance;
    public float advanceTime = 2.5f;


    [Header("Cover System")]
    //public bool canUseCovers = true;
    public float minCoverTime;
    public float safeDistance;
    public CoverPoint lastCover {  get; private set; }
    public CoverPoint currentCover {  get; private set; }
    //public List<Cover> allCovers;

    [Header("Weapon Details")]
    public EnemyRangeWeaponData weaponData;
    public float attackDelay;
    [Space]
    public Transform gunPoint;
    public Transform weaponHolder;
    public GameObject bulletPrefab;

    [SerializeField] List<EnemyRangeWeaponData> avaliableWeaponData;

    [Header("Aim details")]
    public float SlowAim = 4;
    public float FastAim = 20;
    public Transform Aim;
    public Transform PlayersBody;
    public LayerMask WhatToIgnore;

    [Header("Weapon Audio Data")]
    [SerializeField] private WeaponAudioDataEnemy weaponAudioDataEnemy;
    
    
    
    public IdleStateRange IdleStateRange { get; private set; }
    public MoveStateRange MoveStateRange { get; private set; }
    public BattleStateRange BattleStateRange { get; private set; }
    public AdvancePlayerStateRange AdvancePlayerStateRange { get; private set; }
    public RunToCoverStateRange RunToCoverStateRange { get; private set; }
    public ThrowGrenadeStateRange ThrowGrenadeStateRange { get; private set; }
    public DeadStateRange DeadStateRange { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        IdleStateRange = new IdleStateRange(this, StateMachine, "Idle");
        MoveStateRange = new MoveStateRange(this, StateMachine, "Move");
        BattleStateRange = new BattleStateRange(this, StateMachine, "Battle");
        RunToCoverStateRange = new RunToCoverStateRange(this, StateMachine, "Run");
        AdvancePlayerStateRange = new AdvancePlayerStateRange(this, StateMachine, "Advance");
        ThrowGrenadeStateRange = new ThrowGrenadeStateRange(this, StateMachine, "ThrowGrenade");
        DeadStateRange = new DeadStateRange(this, StateMachine, "Idle");
    }

    protected override void Start()
    {
        base.Start();

        PlayersBody = Player.GetComponent<Player>().PlayerBody;
        Aim.parent = null;

        StateMachine.Initialize(IdleStateRange);
        EnemyVisuals.SetupRandomLook();
        SetupWeaponData();
        //allCovers.AddRange(CollectNearbyCovers());
    }

    protected override void Update()
    {
        base.Update();
        
        StateMachine.currentState.Update();
    }


    //audioenemies
    private void PlayShootSound()
{
    if (weaponAudioDataEnemy.shootSound != null)
    {
        AudioSource.PlayClipAtPoint(weaponAudioDataEnemy.shootSound, transform.position);
    }
}

    private void PlayGrenadeSound()
    {
        if (weaponAudioDataEnemy.granadeSound != null)
        {
            AudioSource.PlayClipAtPoint(weaponAudioDataEnemy.granadeSound, transform.position);
        }
    }



    public override void Die()
    {
        base.Die();

        if(StateMachine.currentState != DeadStateRange)
        {
            StateMachine.ChangeState(DeadStateRange);
        }
    }


    public override void EnterBattleMode()
    {
        
        if(inBattleMode)
            return;
        
        base.EnterBattleMode();
        if(CanGetCover())
        {
            StateMachine.ChangeState(RunToCoverStateRange);
        }
        else
        {
            StateMachine.ChangeState(BattleStateRange);
        }
    }

public bool CanThrowGrenade()
{
    if (grenadePerk == GrenadePerk.Unavaible)
    {
        return false;
    }

    if (Vector3.Distance(Player.transform.position, transform.position) < safeDistance)
    {
        return false;
    }

    if (Time.time > grenadeCooldown + lastTimeGrenadeThrown)
    {
        return true;
    }

    return false;
}

   public void ThrowGrenade()
{
    lastTimeGrenadeThrown = Time.time;

    EnemyVisuals.EnableGrenadeModel(false);

    GameObject newGrenade = ObjectPool.Instance.GetObject(grenadePrefab, grenadeStartPoint);

    EnemyGrenade newGrenadeScript = newGrenade.GetComponent<EnemyGrenade>();

    // Przypisanie dŸwiêku granata 
    newGrenadeScript.explosionSound = weaponAudioDataEnemy.granadeSound;

    if (StateMachine.currentState == DeadStateRange)
    {
        newGrenadeScript.SetupGrenade(whatIsAlly, transform.position, 1, explosionTimer, impactPower, grenadeDamage);
        return;
    }

    newGrenadeScript.SetupGrenade(whatIsAlly, Player.transform.position, timeToTarget, explosionTimer, impactPower, grenadeDamage);

    Debug.Log("RZUCAAAAM");
}



    

    public void FireSingleBullet()
    {
        Anim.SetTrigger("Shoot");

        //shootsounds
        PlayShootSound(); 


        Vector3 bulletDirection = (Aim.position - gunPoint.position).normalized;

        GameObject newBullet = ObjectPool.Instance.GetObject(bulletPrefab, gunPoint);
        //newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        newBullet.GetComponent<Bullet>().BulletSetup(whatIsAlly, weaponData.bulletDamage);

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        Vector3 bulletDirectionWithSpread = weaponData.ApplyWeaponSpread(bulletDirection);

        rbNewBullet.mass = 20 / weaponData.bulletSpeed;
        rbNewBullet.velocity = bulletDirectionWithSpread * weaponData.bulletSpeed;

    }

    private void SetupWeaponData()
    {
        List<EnemyRangeWeaponData> filteredData = new List<EnemyRangeWeaponData>();

        foreach (var weaponData in avaliableWeaponData)
        {
            if(weaponData.weaponType == weaponType)
            {
                filteredData.Add(weaponData);
            }
        }

        if(filteredData.Count > 0)
        {
            int random = Random.Range(0, filteredData.Count);
            weaponData = filteredData[random];
        }
        else
        {
            Debug.Log("nie ma takiej broni");
        }

        gunPoint = EnemyVisuals.CurrentWeaponModel.GetComponent<EnemyRangeWeaponModel>().gunPoint;
    }

    private List<Cover> CollectNearbyCovers()
    {
        List<Cover> collectedCovers = new List<Cover>();
        //float coverCollectionRadius = 60;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 15);

        foreach (Collider collider in hitColliders)
        {
            Cover cover = collider.GetComponent<Cover>();

            if(cover != null && collectedCovers.Contains(cover) == false)
            {
                Debug.Log("cover = " + cover.name);
                collectedCovers.Add(cover);
            }
        }

        return collectedCovers;
    }

    public bool CanGetCover()
    {
        if(coverPerk == CoverPerk.Unavaible)
        {
            return false;
        }

        currentCover = AttemptToFindCover()?.GetComponent<CoverPoint>();

        if(lastCover != currentCover && currentCover != null)
        {
            return true;
        }

        return false;
    }

    private Transform AttemptToFindCover()
    {
        List<CoverPoint> collectedCoverPoints = new List<CoverPoint>();

        foreach(Cover cover in CollectNearbyCovers())
        {
            collectedCoverPoints.AddRange(cover.GetValidCoverPoints(transform));
        }

        CoverPoint closestCoverPoint = null;
        float shortestDistance = float.MaxValue;

        foreach(CoverPoint coverPoint in collectedCoverPoints)
        {
            float currentDistance = Vector3.Distance(transform.position, coverPoint.transform.position);
            if(currentDistance < shortestDistance)
            {
                closestCoverPoint = coverPoint;
                shortestDistance = currentDistance;
            }
        }

        if(closestCoverPoint != null)
        {
            lastCover?.SetOccupied(false);
            lastCover = currentCover;
            currentCover = closestCoverPoint;
            currentCover.SetOccupied(true);

            return currentCover.transform;
        }

        return null;
    }

    public bool IsAimOnPlayer()
    {
        float distanceAimToPlayer = Vector3.Distance(Aim.position, Player.position);

        return distanceAimToPlayer < 2;
    }

    public void UpdateAimPosition()
    {
        float aimSpeed = IsAimOnPlayer() ? FastAim : SlowAim;
        Aim.position = Vector3.MoveTowards(Aim.position, PlayersBody.position, aimSpeed * Time.deltaTime);
    }

    public bool IsSeeingPlayer()
    {
        Vector3 myPosition = transform.position + Vector3.up;
        Vector3 directionToPlayer = PlayersBody.position - myPosition;

        if(Physics.Raycast(myPosition, directionToPlayer, out RaycastHit hit, Mathf.Infinity, ~WhatToIgnore))
        {
            if(hit.transform.root == Player.root)
            {
                UpdateAimPosition();
                return true;
            }
        }

        return false;
    }

}