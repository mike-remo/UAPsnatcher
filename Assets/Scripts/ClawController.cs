using UnityEngine;

public class ClawController : MonoBehaviour
{
    [SerializeField] private float speedH, speedV, speedC, rangeYmin, rangeYmax, elevation;
    [SerializeField] private float circum, length, offset, angle, openAngle, closeAngle;
    [SerializeField] private GameObject clawArm, claw1, claw2, claw3, claw4;
    private Rigidbody clawRB;
    private PlayerInputHandler playerInput; // From PlayerInputHandler.cs

    void Awake()
    {
        playerInput = GameObject.Find("PlayerInputHandler").GetComponent<PlayerInputHandler>();
    }

    void Start()
    {
        clawRB = GetComponent<Rigidbody>();

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
        closeAngle = 10; // How much to close claw
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
        Vector3 input = playerInput.playerMoveInput3d;
        clawRB.linearVelocity = input * Time.deltaTime * speedH;
    }

    void ControlsMoveV(float x, float z, float x2, float y2, float z2) // Claw vertical movement
    {
        if (playerInput.playerButton1isPressed)
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
        if (playerInput.playerButton1isPressed || playerInput.playerButton2isPressed)
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