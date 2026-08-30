using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private InputAction actionMove, action1, action2;
    public Vector2 moveInput;
    public Vector3 moveInput3d;
    public bool button1isPressed, button2isPressed;
    void Awake()
    {   // In code defined player input bindings
        actionMove = new InputAction("Movement", InputActionType.Value);
        actionMove.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");
        
        action1 = new InputAction("Activate", InputActionType.Button, "<Keyboard>/space");
        action1.AddBinding("<Keyboard>/numpad0");
        action2 = new InputAction("Alternate", InputActionType.Button, "<Keyboard>/f");
        action2.AddBinding("<Keyboard>/numpadPeriod");
    }

    void OnEnable()
    {
        actionMove.Enable();
        action1.Enable();
        action2.Enable();
    }

    void OnDisable()
    {
        actionMove.Disable();
        action1.Disable();
        action2.Disable();
    }

    void Update()
    {   // Receive direct 2d input
        moveInput = actionMove.ReadValue<Vector2>();
        moveInput.Normalize();
        // 2d input converted to 3d
        moveInput3d = new Vector3(moveInput.x, 0, moveInput.y);
        button1isPressed = action1.IsPressed();
        button2isPressed = action2.IsPressed();
    }
} // END