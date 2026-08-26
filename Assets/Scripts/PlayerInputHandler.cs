using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private InputAction playerMove, playerAction1, playerAction2;
    public Vector2 playerMoveInput;
    public Vector3 playerMoveInput3d;
    public bool playerButton1isPressed, playerButton2isPressed;
    void Awake()
    {
        // In code defined player input bindings
        playerMove = new InputAction("Movement", InputActionType.Value);
        playerMove.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");
        
        playerAction1 = new InputAction("Activate", InputActionType.Button, "<Keyboard>/space");
        playerAction1.AddBinding("<Keyboard>/numpad0");
        playerAction2 = new InputAction("Alternate", InputActionType.Button, "<Keyboard>/f");
        playerAction2.AddBinding("<Keyboard>/numpadPeriod");
    }

    void OnEnable()
    {
        playerMove.Enable();
        playerAction1.Enable();
        playerAction2.Enable();
    }

    void OnDisable()
    {
        playerMove.Disable();
        playerAction1.Disable();
        playerAction2.Disable();
    }

    void Update()
    {
        // Receive direct 2d input
        playerMoveInput = playerMove.ReadValue<Vector2>();
        playerMoveInput.Normalize();
        // 2d input converted to 3d
        playerMoveInput3d = new Vector3(playerMoveInput.x, 0, playerMoveInput.y);

        playerButton1isPressed = playerAction1.IsPressed();
        playerButton2isPressed = playerAction2.IsPressed();
    }
}
// END