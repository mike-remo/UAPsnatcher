using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private InputAction actionMove1, actionMove2, action1, action2, action3;
    public Vector2 moveInput;
    public Vector3 moveInput3d;
    public bool button1Pressed, button2Pressed, button3Pressed;
    void Awake()
    {   // In code defined player input bindings
        actionMove1 = new InputAction("Movement", InputActionType.Value);
        actionMove1.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");
        actionMove2 = new InputAction("Movement", InputActionType.Value);
        actionMove2.AddBinding("<Gamepad>/leftStick");
        
        action1 = new InputAction("Activate", InputActionType.Button);
        action1.AddBinding("<Keyboard>/space");
        action1.AddBinding("<Keyboard>/numpad0");
        action1.AddBinding("<Gamepad>/buttonWest");
        action1.AddBinding("<Gamepad>/buttonNorth");
        action2 = new InputAction("Alternate", InputActionType.Button);
        action2.AddBinding("<Keyboard>/f");
        action2.AddBinding("<Keyboard>/numpadPeriod");
        action2.AddBinding("<Gamepad>/buttonSouth");
        action2.AddBinding("<Gamepad>/buttonEast");
        action3 = new InputAction("Pause", InputActionType.Button);
        action3.AddBinding("<Keyboard>/p");
        action3.AddBinding("<Gamepad>/select");
        action3.AddBinding("<Gamepad>/start");
    }

    void OnEnable()
    {
        actionMove1.Enable();
        actionMove2.Enable();
        action1.Enable();
        action2.Enable();
        action3.Enable();
    }

    void OnDisable()
    {
        actionMove1.Disable();
        actionMove2.Disable();
        action1.Disable();
        action2.Disable();
        action3.Disable();
        button1Pressed = false; // When disabled, reset any triggered buttons
        button2Pressed = false;
        button3Pressed = false;
    }

    void Update()
    {   // Receive direct 2d input from keyboard
        moveInput = actionMove1.ReadValue<Vector2>();
        moveInput.Normalize();
        // If no keyboard input, check controller, if available
        if (moveInput == Vector2.zero)
            if (Gamepad.current != null)
                //moveInput = Gamepad.current.leftStick.ReadValue(); // Raw input
                moveInput = actionMove2.ReadValue<Vector2>();
        // 2d input converted to 3d
        moveInput3d = new Vector3(moveInput.x, 0, moveInput.y);
        button1Pressed = action1.IsPressed();
        button2Pressed = action2.IsPressed();
        button3Pressed = action3.IsPressed();
    }
} // END