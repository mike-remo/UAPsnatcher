using UnityEngine;

public class JoystickController : MonoBehaviour
{
    [SerializeField] private GameObject button1, button2;
    private float maxAngle, btnOffset, stickX, stickY, stickZ;
    private Vector3 pushed1, unpushed1, pushed2, unpushed2;
    private PlayerInputHandler playerInput; // From PlayerInputHandler.cs

    void Joystick() // Joystick mimics move input
    {
        Vector2 input = playerInput.moveInput;
        stickX = input.x * maxAngle;
        stickY = 90;
        stickZ = input.y * maxAngle;
        transform.localEulerAngles = new Vector3(stickX, stickY, stickZ);
    }

    void Buttons() // Buttons mimic keypresses
    {
        if (playerInput.button1Pressed)
            button1.transform.localPosition = pushed1;
        else button1.transform.localPosition = unpushed1;
        if (playerInput.button2Pressed)
            button2.transform.localPosition = pushed2;
        else button2.transform.localPosition = unpushed2;
    }

    void Awake()
    {
        playerInput = GameObject.Find("PlayerInputHandler")
                                .GetComponent<PlayerInputHandler>();
    }

    void Start()
    {
        maxAngle = 20f;
        btnOffset = 0.0125f;
        pushed1 = new Vector3(button1.transform.localPosition.x,
                              button1.transform.localPosition.y - btnOffset,
                              button1.transform.localPosition.z);
        unpushed1 = new Vector3(button1.transform.localPosition.x,
                                button1.transform.localPosition.y,
                                button1.transform.localPosition.z);
        pushed2 = new Vector3(button2.transform.localPosition.x,
                              button2.transform.localPosition.y - btnOffset,
                              button2.transform.localPosition.z);
        unpushed2 = new Vector3(button2.transform.localPosition.x,
                                button2.transform.localPosition.y,
                                button2.transform.localPosition.z);
    }

    void Update()
    {
        Joystick();
        Buttons();
    }
} // END