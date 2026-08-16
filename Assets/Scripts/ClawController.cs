using UnityEngine;
using UnityEngine.InputSystem;

public class ClawController : MonoBehaviour
{
    [SerializeField] private InputAction playerMove, playerAction1;
    [SerializeField] private float speedH, speedV, speedC, rangeYmin, rangeYmax;
    [SerializeField] private float elevation, angle, openAngle, closeAngle;
    [SerializeField] private GameObject clawArm, claw1, claw2, claw3, claw4;
    private Rigidbody clawRB;
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

        clawRB = GetComponent<Rigidbody>();
    }

    void OnDisable()
    {
        playerMove.Disable();
        playerAction1.Disable();
    }

    void Start()
    {
        speedH = 100f; // Horizontal movement speed
        speedV = 0.5f; // Vertical movement speed
        speedC = 180f; // Claw grab speed
        rangeYmax = 0.3f;
        rangeYmin = -0.6f;
        elevation = 0.3f; // Initial claw height
        angle = 0;
        openAngle = -20; // How much to open claw
        closeAngle = 10; // How much to close claw
    }

    void Update()
    {
        float x = transform.localPosition.x;
        float y = transform.localPosition.y;
        float z = transform.localPosition.z;
        ControlsMove(x,y,z);
        ControlsGrab(x,z);
    }

    void ControlsMove(float x, float y, float z) // Controls claw horizontal movement
    {
        // Movement from 2d input converted to 3d local space
        Vector2 input = playerMove.ReadValue<Vector2>();
        Vector3 input2 = new Vector3(input.x, 0, input.y);
        if (input2.magnitude > 1f) { input2.Normalize(); }
        clawRB.linearVelocity = input2 * Time.deltaTime * speedH;
    }

    void ControlsGrab(float x, float z) // Controls claw vertical movement and grab action
    {
        if (playerAction1.IsPressed())
        {
            if (elevation > rangeYmin) // Lower and open claw
            {
                elevation -= Time.deltaTime * speedV;
                transform.localPosition = new Vector3(x, elevation, z);
            }
            if (angle > openAngle) { angle -= Time.deltaTime * speedC; }
            claw1.transform.localEulerAngles = new Vector3(45, 0, angle);
            claw2.transform.localEulerAngles = new Vector3(315, 0, angle);
            claw3.transform.localEulerAngles = new Vector3(135, 0, angle);
            claw4.transform.localEulerAngles = new Vector3(225, 0, angle);
        }
        else
        {
            if (elevation < rangeYmax) // Raise and close claw
            {
                elevation += Time.deltaTime * speedV;
                transform.localPosition = new Vector3(x, elevation, z);
            }
            if (angle < closeAngle) { angle += Time.deltaTime * speedC * 2; }
            claw1.transform.localEulerAngles = new Vector3(45, 0, angle);
            claw2.transform.localEulerAngles = new Vector3(315, 0, angle);
            claw3.transform.localEulerAngles = new Vector3(135, 0, angle);
            claw4.transform.localEulerAngles = new Vector3(225, 0, angle);
        }
    }
}
