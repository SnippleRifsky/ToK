using UnityEngine;

namespace Player.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float gravity = -9.81f;
        private Vector3 _currentMovement;
    
        private CharacterController _characterController;
        private InputHandler _inputHandler;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _inputHandler = InputHandler.Instance;
        }

        private void Update()
        {
            HandleAllMovement();
        }

        private void HandleAllMovement()
        {
            HandleMovement();
            HandleJump();
        }

        private void HandleMovement()
        {
            //TODO Prevent movement while not IsGrounded
            
            Vector3 worldDirection = Vector3.zero;
            Vector3 inputDirection = new Vector3(_inputHandler.MoveInput.x, 0, _inputHandler.MoveInput.y);
            worldDirection = transform.TransformDirection(inputDirection).normalized;
        
            _currentMovement.x = worldDirection.x * walkSpeed;
            _currentMovement.z = worldDirection.z * walkSpeed;
            _characterController.Move(_currentMovement * Time.deltaTime);
            
        }

        private void HandleJump()
        {
            if (_characterController.isGrounded)
            {
                _currentMovement.y = -0.5f;
                if (_inputHandler.JumpTriggered)
                {
                    _currentMovement.y = jumpForce;
                }
            }
            else
            {
                _currentMovement.y += gravity * Time.deltaTime;
            }
        }
    
    }
}
