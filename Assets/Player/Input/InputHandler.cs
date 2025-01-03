using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("Input Action Asset")] [SerializeField]
    private InputActionAsset playerControls;
    
    [Header("Acton Map Name References")]
    [SerializeField] private string actionMapName = "Player";
    
    [Header("Acton Name References")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string look = "Look";
    
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _lookAction;
    
    public Vector2 MoveInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public Vector2 LookInput { get; private set; }
    
    public static InputHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        _moveAction = playerControls.FindActionMap(actionMapName).FindAction(move);
        _jumpAction = playerControls.FindActionMap(actionMapName).FindAction(jump);
        _lookAction = playerControls.FindActionMap(actionMapName).FindAction(look);
        RegisterInpuitActions();

    }

    void RegisterInpuitActions()
    {
        _moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        _moveAction.canceled += context => MoveInput = Vector2.zero;

        _jumpAction.performed += context => JumpTriggered = true;
        _jumpAction.canceled += context => JumpTriggered = false;
        
        _lookAction.performed += context => LookInput = context.ReadValue<Vector2>();
        _lookAction.canceled += context => LookInput = Vector2.zero;
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _jumpAction.Enable();
        _lookAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _jumpAction.Disable();
        _lookAction.Disable();
    }
    
}
