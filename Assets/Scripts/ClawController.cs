using UnityEngine;
using UnityEngine.InputSystem;

public class ClawController : MonoBehaviour
{
    [SerializeField] private InputAction playerMove, playerAction1;
    [SerializeField] private float speedH, speedV, speedC, rangeYmin, rangeYmax, elevation;
    [SerializeField] private float circum, length, offset, angle, openAngle, closeAngle;
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
        speedC = 180f; // Grab speed
        rangeYmax = transform.localPosition.y; // Highest pos
        rangeYmin = transform.localPosition.y - 0.95f; // Lowest pos
        elevation = rangeYmax; // Initial height
        length = clawArm.transform.localScale.y; // ARM initial length
        circum = clawArm.transform.localScale.x; // ARM initial circumference
        angle = 0; // PINCERS initial angle
        openAngle = -25; // How much to open claw
        closeAngle = 5; // How much to close claw
    }

    void Update()
    {
        float x = transform.localPosition.x;
        float y = transform.localPosition.y;
        float z = transform.localPosition.z;
        float x2 = clawArm.transform.localPosition.x;
        float y2 = clawArm.transform.localPosition.y;
        float z2 = clawArm.transform.localPosition.z;
        ControlsMoveH(x, y, z);
        ControlsMoveV(x, z, x2, y2, z2);
        ControlsGrab();
    }

    void ControlsMoveH(float x, float y, float z) // Claw horizontal movement
    {
        // Movement from 2d input converted to 3d local space
        Vector2 input = playerMove.ReadValue<Vector2>();
        Vector3 input2 = new Vector3(input.x, 0, input.y);
        input2.Normalize();
        clawRB.linearVelocity = input2 * Time.deltaTime * speedH;
    }

    void ControlsMoveV(float x, float z, float x2, float y2, float z2) // Claw vertical movement
    {
        if (playerAction1.IsPressed())
        {
            if (elevation > rangeYmin) // Lower claw and extend arm
            {
                elevation -= Time.deltaTime * speedV;
                offset = Time.deltaTime * speedV / 2;
                transform.localPosition = new Vector3(x, elevation, z);
                length += Time.deltaTime * (speedV / 2f);
                clawArm.transform.localScale = new Vector3(circum, length, circum);
                clawArm.transform.localPosition = new Vector3(x2, y2 + offset, z2);
            }
        }
        else
        {
            if (elevation < rangeYmax) // Raise claw and retract arm
            {
                elevation += Time.deltaTime * speedV;
                offset = Time.deltaTime * speedV / 2;
                transform.localPosition = new Vector3(x, elevation, z);
                length -= Time.deltaTime * (speedV / 2f);
                clawArm.transform.localScale = new Vector3(circum, length, circum);
                clawArm.transform.localPosition = new Vector3(x2, y2 - offset, z2);
            }
        }
    }

    void ControlsGrab() // Controls claw open and close
    {
        if (playerAction1.IsPressed())
        {
            if (angle > openAngle) { angle -= Time.deltaTime * speedC; }
            claw1.transform.eulerAngles = new Vector3(0, 45, angle);
            claw2.transform.eulerAngles = new Vector3(0, 315, angle);
            claw3.transform.eulerAngles = new Vector3(0, 135, angle);
            claw4.transform.eulerAngles = new Vector3(0, 225, angle);
        }
        else
        {
            if (angle < closeAngle) { angle += Time.deltaTime * speedC * 2; }
            claw1.transform.eulerAngles = new Vector3(0, 45, angle);
            claw2.transform.eulerAngles = new Vector3(0, 315, angle);
            claw3.transform.eulerAngles = new Vector3(0, 135, angle);
            claw4.transform.eulerAngles = new Vector3(0, 225, angle);
        }
    }
}
// END