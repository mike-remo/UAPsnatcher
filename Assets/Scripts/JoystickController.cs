using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickController : MonoBehaviour
{
    [SerializeField] private InputAction playerMove, playerAction1;
    [SerializeField] private float maxAngle;
    [SerializeField] private GameObject button;
    private Vector3 pushed, unpushed;
    
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
    }

    void OnEnable()
    {
        playerMove.Enable();
        playerAction1.Enable();
    }

    void OnDisable()
    {
        playerMove.Disable();
        playerAction1.Disable();
    }

    void Start()
    {
        maxAngle = 20f;
        pushed = new Vector3(button.transform.localPosition.x,
                             button.transform.localPosition.y - 0.02f,
                             button.transform.localPosition.z);
        unpushed = new Vector3(button.transform.localPosition.x,
                               button.transform.localPosition.y,
                               button.transform.localPosition.z);
    }

    void Update()
    {
        Vector2 input = playerMove.ReadValue<Vector2>();
        float x = input.x * maxAngle;
        float y = 90;
        float z = input.y * maxAngle;
        transform.localEulerAngles = new Vector3(x, y, z);

        if (playerAction1.IsPressed()) { button.transform.localPosition = pushed; }
        else { button.transform.localPosition = unpushed; }
    }
}
