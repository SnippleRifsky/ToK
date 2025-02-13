using UnityEngine;

public class JumpState : BaseState
{
    public JumpState(PlayerController player, Animator animator) : base(player, animator) { }
    
    public override void OnEnter()
    {
        animator.CrossFade(JumpHash, crossFadeDuration);
        Debug.Log("JumpState OnEnter Called");
    }
    
    public override void Update()
    {
       player.ProcessHorizontalMovement();
       player.ProcessVerticalMovement();
    }
}