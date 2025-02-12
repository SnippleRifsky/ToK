using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;
    private const string IsMoving = "IsMoving";
    private const string IsJumping = "IsJumping";
    private const string AttackTriggered = "Attack";
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator is null)
        {
            Debug.LogError("PlayerAnimationController: Animator not found!");
        }
        
        EventBus.Subscribe<PlayerAnimationEvents.MovementChanged>(OnMovementChanged);
        EventBus.Subscribe<PlayerAnimationEvents.JumpStarted>(OnJumpStarted);
        EventBus.Subscribe<PlayerAnimationEvents.AttackStarted>(OnAttackStarted);
    }

    private void OnMovementChanged(PlayerAnimationEvents.MovementChanged evt)
    {
        if (evt.Player != GameManager.Instance.Player) return;
        _animator.SetBool(IsMoving, evt.IsMoving);
    }
    
    private void OnJumpStarted(PlayerAnimationEvents.JumpStarted evt)
    {
        if (evt.Player != GameManager.Instance.Player) return;
        _animator.SetBool(IsJumping, evt.IsJumping);
    }
    
    private void OnAttackStarted(PlayerAnimationEvents.AttackStarted evt)
    {
        if (evt.Player != GameManager.Instance.Player) return;
        _animator.SetTrigger(AttackTriggered);
    }
    
    private void OnDestroy()
    {
        EventBus.Unsubscribe<PlayerAnimationEvents.MovementChanged>(OnMovementChanged);
        EventBus.Unsubscribe<PlayerAnimationEvents.JumpStarted>(OnJumpStarted);
        EventBus.Unsubscribe<PlayerAnimationEvents.AttackStarted>(OnAttackStarted);
    }
}