using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickController : MonoBehaviour
{
    [SerializeField] private InputAction playerMove, playerAction1, playerAction2;
    [SerializeField] private float maxAngle;
    [SerializeField] private GameObject button1, button2;
    private Vector3 pushed1, unpushed1, pushed2, unpushed2;
    
    void Awake()
    {
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
        playerAction2 = new InputAction("Alternate", InputActionType.Button, "<Keyboard>/f");
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

    void Start()
    {
        maxAngle = 20f;
        pushed1 = new Vector3(button1.transform.localPosition.x,
                             button1.transform.localPosition.y - 0.0125f,
                             button1.transform.localPosition.z);
        unpushed1 = new Vector3(button1.transform.localPosition.x,
                               button1.transform.localPosition.y,
                               button1.transform.localPosition.z);
        pushed2 = new Vector3(button2.transform.localPosition.x,
                             button2.transform.localPosition.y - 0.0125f,
                             button2.transform.localPosition.z);
        unpushed2 = new Vector3(button2.transform.localPosition.x,
                               button2.transform.localPosition.y,
                               button2.transform.localPosition.z);
    }

    void Update()
    {
        ControlsJoystick();
        ControlsButtons();
    }

    void ControlsJoystick() // Mimic joystick controls
    {
        Vector2 input = playerMove.ReadValue<Vector2>();
        input.Normalize();
        float x = input.x * maxAngle;
        float y = 90;
        float z = input.y * maxAngle;
        transform.localEulerAngles = new Vector3(x, y, z);
    }

    void ControlsButtons() // Mimic button controls
    {
        if (playerAction1.IsPressed()) { button1.transform.localPosition = pushed1; }
        else { button1.transform.localPosition = unpushed1; }
        if (playerAction2.IsPressed()) { button2.transform.localPosition = pushed2; }
        else { button2.transform.localPosition = unpushed2; }
    }
}
// END