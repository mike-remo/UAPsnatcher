using UnityEngine;

public class JoystickController : MonoBehaviour
{
    [SerializeField] private float maxAngle;
    [SerializeField] private GameObject button1, button2;
    private Vector3 pushed1, unpushed1, pushed2, unpushed2;
    private PlayerInputHandler playerInput; // From PlayerInputHandler.cs
    void Awake()
    {
        playerInput = GameObject.Find("PlayerInputHandler").GetComponent<PlayerInputHandler>();
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
        Vector2 input = playerInput.playerMoveInput;
        float x = input.x * maxAngle;
        float y = 90;
        float z = input.y * maxAngle;
        transform.localEulerAngles = new Vector3(x, y, z);
    }

    void ControlsButtons() // Mimic button controls
    {
        if (playerInput.playerButton1isPressed) { button1.transform.localPosition = pushed1; }
        else { button1.transform.localPosition = unpushed1; }
        if (playerInput.playerButton2isPressed) { button2.transform.localPosition = pushed2; }
        else { button2.transform.localPosition = unpushed2; }
    }
}
// END