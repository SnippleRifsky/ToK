public class PlayerAnimationEvents
{
    public readonly struct MovementChanged
    {
        public readonly bool IsMoving;
        public readonly Player Player;
        
        public MovementChanged(bool isMoving, Player player)
        {
            IsMoving = isMoving;
            Player = player;
        }
    }
    
    public readonly struct JumpStarted
    {
        public readonly bool IsJumping;
        public readonly Player Player;
        
        public JumpStarted(bool isJumping, Player player)
        {
            IsJumping = isJumping;
            Player = player;
        }
    }
    
    public readonly struct AttackStarted
    {
        public readonly Player Player;
        
        public AttackStarted(Player player)
        {
            Player = player;
        }
    }
}