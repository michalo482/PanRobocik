using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateRange : EnemyState
{

    private EnemyRange _enemyRange;

    private float lastTimeShot = -10;
    private int bulletsShot = 0;

    private int bulletsPerAttack;
    private float weaponCooldown;

    private float coverCheckTimer;

    private bool firstTimeAttack = true;

    public BattleStateRange(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        _enemyRange = enemyBase as EnemyRange;
    }

    public override void Enter()
    {
        base.Enter();
        SetupValuesForFirstAttack();

        _enemyRange.Agent.isStopped = true;
        _enemyRange.Agent.velocity = Vector3.zero;

        _enemyRange.EnemyVisuals.EnableIK(true, true);

        stateTimer = _enemyRange.attackDelay;

    }

    private void SetupValuesForFirstAttack()
    {
        if (firstTimeAttack)
        {
            _enemyRange.aggressionRange = _enemyRange.advanceStoppingDistance + 3;

            firstTimeAttack = false;
            bulletsPerAttack = _enemyRange.weaponData.GetBulletsPerAttack();
            weaponCooldown = _enemyRange.weaponData.GetWeaponCooldown();

        }
    }

    public override void Update()
    {
        base.Update();

        if (_enemyRange.IsSeeingPlayer())
        {
            _enemyRange.FaceTarget(_enemyRange.Aim.position);
        }

        if(_enemyRange.CanThrowGrenade())
        {
            stateMachine.ChangeState(_enemyRange.ThrowGrenadeStateRange);
        }


        if (_enemyRange.IsPlayerInAgressionRange() == false && ReadyToLeaveCover())
        {
            stateMachine.ChangeState(_enemyRange.AdvancePlayerStateRange);
        }

        
        ChengeCoverIfShould();

        if(stateTimer > 0)
        {
            return;
        }

        if (WeaponOutOfBullets())
        {

            if (WeaponOnCooldown())
            {
                AtempToResetWeapon();
            }
            return;
        }

        if (CanShoot() && _enemyRange.IsAimOnPlayer())
        {
            Shoot();
        }
    }

    private void ChengeCoverIfShould()
    {
        if(_enemyRange.coverPerk != CoverPerk.CantTakeAndChangeCover)
        {
            return;
        }
        //Debug.Log("sprawdzam czy trza zmienic cover");
        coverCheckTimer -= Time.deltaTime;

        if (coverCheckTimer < 0)
        {
            coverCheckTimer = 0.5f;

            if (ReadyToChangeCover() && ReadyToLeaveCover())
            {
                if (_enemyRange.CanGetCover())
                {
                    //Debug.Log("zmieniam cover");
                    stateMachine.ChangeState(_enemyRange.RunToCoverStateRange);
                }
            }
        }
    }

    private void AtempToResetWeapon()
    {
        bulletsShot = 0;
        bulletsPerAttack = _enemyRange.weaponData.GetBulletsPerAttack();
        weaponCooldown = _enemyRange.weaponData.GetWeaponCooldown();
    }

    private bool CanShoot()
    {
        return Time.time > lastTimeShot + 1 / _enemyRange.weaponData.fireRate;
    }

    private void Shoot()
    {
        _enemyRange.FireSingleBullet();
        lastTimeShot = Time.time;
        bulletsShot++;
    }

    private bool WeaponOnCooldown()
    {
        return Time.time > lastTimeShot + weaponCooldown;
    }

    private bool WeaponOutOfBullets()
    {
        return bulletsShot >= bulletsPerAttack;
    }

    public override void Exit()
    {
        base.Exit();
        _enemyRange.EnemyVisuals.EnableIK(false, false);
    }

    private bool IsPlayerClose()
    {
        return Vector3.Distance(_enemyRange.transform.position, _enemyRange.Player.transform.position) < _enemyRange.safeDistance;
    }

    private bool IsPlayerInClearSight()
    {
        Vector3 directionToPlayer = _enemyRange.Player.transform.position - _enemyRange.transform.position;

        if(Physics.Raycast(_enemyRange.transform.position, directionToPlayer, out RaycastHit hit))
        {
            //Debug.Log(hit.collider.gameObject.GetComponentInParent<Player>());
            //return !hit.collider.gameObject.GetComponentInParent<Player>();
            if (hit.transform.root == _enemyRange.Player.root)
                return true;
        }
        //Debug.Log("nie ma gracza");
        return false;
    }

    private bool ReadyToChangeCover()
    {
        bool inDanger = IsPlayerInClearSight() || IsPlayerClose();
        bool advanceTimeIsOver = Time.time > _enemyRange.AdvancePlayerStateRange.LastTimeAdvanced + _enemyRange.advanceTime;

        return inDanger && advanceTimeIsOver;
    }
    private bool ReadyToLeaveCover()
    {
        return Time.time > _enemyRange.minCoverTime + _enemyRange.RunToCoverStateRange.LastTimeTookCover;
    }
}
