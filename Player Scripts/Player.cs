using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[SelectionBase]
public class Player : BaseCharacter
{
    #region Move
    [Header("Move Info")]
    public float moveSpeed = 12.0f;
    public float jumpForce;
    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;
    #endregion
    #region Attack
    [Header("Attack Info")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;
    #endregion
    public bool isBusy { get; private set; }
    #region Dash
    public float dashSpeed;
    public float dashDuration;
    public float defaultDashSpeed;
    public float dashDir { get; private set; }
    #endregion 

    public SkillManager skill {  get; private set; }
    public GameObject sword {  get; private set; }


    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerBlackholeState blackholeState { get; private set; }
    public PlayerDeathState deathState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState =  new PlayerAirState(this, stateMachine,  "Jump");
        dashState =  new PlayerDashState(this, stateMachine,  "Dash");
        wallSlideState =  new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState =  new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttackState =  new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState =  new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        catchSwordState =  new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        aimSwordState =  new PlayerAimSwordState(this, stateMachine, "AimSword");  
        blackholeState =  new PlayerBlackholeState(this, stateMachine, "Jump");
        deathState = new PlayerDeathState(this, stateMachine, "Die");
    }

    protected override void Start()
    {
        base.Start();

        skill = SkillManager.instance;

        stateMachine.Intitialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckForDashInput();

        if(Input.GetKeyDown(KeyCode.F) && skill.crystal.crystalUnlocked)
        {
            skill.crystal.CanUseSkill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.instance.UseFlask();
            Debug.Log("Used Flask");
        }
    }

    public override void SlowBaseCharacter(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }
    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    private void CheckForDashInput()
    {
        if(IsWallDetected())
            return;

        if (skill.dash.dashUnlocked == false)
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
            {
                dashDir = facingDir;
            }
            stateMachine.ChangeState(dashState);
        }
    }
    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deathState);
    }
}