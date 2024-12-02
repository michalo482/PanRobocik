using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControlls _controls;
    private Player _player;
    private float _speed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSpeed;
    private float _verticalVelocity;
    private CharacterController _characterController;
    private Vector3 _movementDirection;
    private Animator _animator;


    public Vector2 moveInput { get; private set; }
    private bool _isRunning;
    private static readonly int XVelocity = Animator.StringToHash("xVelocity");
    private static readonly int ZVelocity = Animator.StringToHash("zVelocity");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _player = GetComponent<Player>();
        _speed = walkSpeed;
        AssignInputEvents();
    }

    private void AssignInputEvents()
    {
        _controls = _player.Controls;

        _controls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        _controls.Character.Movement.canceled += context => moveInput = Vector2.zero;

        _controls.Character.Run.performed += context =>
        {
            _speed = runSpeed;
            _isRunning = true;
        };
        _controls.Character.Run.canceled += context =>
        {
            _speed = walkSpeed;
            _isRunning = false;
        };
    }

    private void Update()
    {
        if (_player.PlayerHealth.IsDead) return;

        ApplyMovement();
        ApplyRotation();
        AnimatorControllers();
    }

    private void ApplyRotation()
    {
        var position = transform.position;
        Vector3 lookingDirection = _player.Aim.GetMouseHitInfo().point - position;
        lookingDirection.y = 0f;
        lookingDirection.Normalize();

        Quaternion desiredRotation = Quaternion.LookRotation(lookingDirection);

        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, turnSpeed * Time.deltaTime);
    }

    private void ApplyMovement()
    {
        if (moveInput.magnitude > 0)
        {
            _movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        }
        else
        {
            _movementDirection = Vector3.zero;


        }

        }

        //_movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        ApplyGravity();

        if (_movementDirection.magnitude > 0)
        {
            _characterController.Move(_movementDirection * (Time.deltaTime * _speed));
        }
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded == false)
        {
            _verticalVelocity -= 9.81f * Time.deltaTime;
            _movementDirection.y = _verticalVelocity;
        }
        else
        {
            _verticalVelocity = -.5f;
        }
    }

    private void AnimatorControllers()
    {
        Vector3 horizontalMovement = new Vector3(_movementDirection.x, 0, _movementDirection.z).normalized;


        float xVelocity = Vector3.Dot(_movementDirection, transform.right);
        float zVelocity = Vector3.Dot(_movementDirection, transform.forward);

        _animator.SetFloat(XVelocity, xVelocity);
        _animator.SetFloat(ZVelocity, zVelocity);

        bool playRunAnimation = _isRunning && _movementDirection.magnitude > 0;

        _animator.SetBool(IsRunning, playRunAnimation);
    }







        float xVelocity = Vector3.Dot(horizontalMovement, transform.right);
        float zVelocity = Vector3.Dot(horizontalMovement, transform.forward);
        
        _animator.SetFloat(XVelocity, xVelocity);
        _animator.SetFloat(ZVelocity, zVelocity);

        bool playRunAnimation = _isRunning && moveInput.magnitude > 0;
        
        _animator.SetBool(IsRunning, playRunAnimation);
    }  
}
