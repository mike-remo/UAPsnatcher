using UnityEngine;
using UnityEngine.InputSystem;

public class ClawController : MonoBehaviour
{
    [SerializeField] private InputAction playerMove, playerAction1;
    [SerializeField] private float speed, rangeX, rangeY, rangeZ, elevation, angle, openAngle, closeAngle;
    [SerializeField] private GameObject claw1, claw2, claw3, claw4;
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
        speed = 1f;
        rangeX = 1.2f;
        rangeY = 0.8f;
        rangeZ = 0.8f;
        elevation = 0;
        angle = 0;
        openAngle = -20;
        closeAngle = 10;
    }

    void Update()
    {
        float x = transform.localPosition.x;
        float y = transform.localPosition.y;
        float z = transform.localPosition.z;
        if(x < -rangeX) { transform.localPosition = new Vector3(x + 0.01f, y, z); }
        if(x > rangeX) { transform.localPosition = new Vector3(x - 0.01f, y, z); }
        if(y < -rangeY) { transform.localPosition = new Vector3(x, y + 0.01f, z); }
        if(y > 0) { transform.localPosition = new Vector3(x, y - 0.01f, z); }
        if(z < -rangeZ) { transform.localPosition = new Vector3(x, y, z + 0.01f); }
        if (z > rangeZ) { transform.localPosition = new Vector3(x, y, z - 0.01f); }

        Vector2 input = playerMove.ReadValue<Vector2>();
        Vector3 input2 = new Vector3(input.x, 0, input.y);
        if (input2.magnitude > 1f) { input2.Normalize(); }
        transform.Translate(input2 * Time.deltaTime * speed);

        if (playerAction1.IsPressed())
        {
            if (elevation > -rangeY) { elevation -= 0.01f; }
            transform.localPosition = new Vector3(transform.localPosition.x, elevation, transform.localPosition.z);
            if (angle > openAngle) { angle -= 1; }
            claw1.transform.localEulerAngles = new Vector3(45, 0, angle);
            claw2.transform.localEulerAngles = new Vector3(315, 0, angle);
            claw3.transform.localEulerAngles = new Vector3(135, 0, angle);
            claw4.transform.localEulerAngles = new Vector3(225, 0, angle);
        }
        else
        {
            if (elevation < -0.01f) { elevation += 0.01f; }
            transform.localPosition = new Vector3(transform.localPosition.x, elevation, transform.localPosition.z);
            if (angle < closeAngle) { angle += 1; }
            claw1.transform.localEulerAngles = new Vector3(45, 0, angle);
            claw2.transform.localEulerAngles = new Vector3(315, 0, angle);
            claw3.transform.localEulerAngles = new Vector3(135, 0, angle);
            claw4.transform.localEulerAngles = new Vector3(225, 0, angle);
        }
    }
}
