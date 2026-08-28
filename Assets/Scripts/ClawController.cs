using UnityEngine;

public class ClawController : MonoBehaviour
{
    private float speedH, speedV, speedC, rangeYmin, rangeYmax, elevation;
    private float circum, length, offset, angle, openAngle, closeAngle;
    [SerializeField] private GameObject arm;
    [SerializeField] private GameObject[] pincers;
    private Rigidbody Rb;
    private PlayerInputHandler playerInput; // From PlayerInputHandler.cs
    [SerializeField] private AudioSource audioMoveH, audioMoveV, audioMoveC;
    [SerializeField] private AudioClip soundMoveH, soundMoveV, soundMoveC;
    private OptionsManager optionsManager;

    void Start()
    {
        playerInput = GameObject.Find("PlayerInputHandler").GetComponent<PlayerInputHandler>();
        Rb = GetComponent<Rigidbody>();
        if (GameObject.Find("OptionsManager"))
            if (GameObject.Find("OptionsManager").TryGetComponent<OptionsManager>(out optionsManager))
            {
                audioMoveH.volume = optionsManager.options.volume;
                audioMoveV.volume = optionsManager.options.volume;
                audioMoveC.volume = optionsManager.options.volume;
            }
        InitValues();
    }

    void Update()
    {
        Move(playerInput.playerMoveInput3d);
        if (playerInput.playerButton1isPressed)
        {
            Move(true,
                transform.localPosition.x,
                transform.localPosition.z,
                arm.transform.localPosition.x,
                arm.transform.localPosition.y,
                arm.transform.localPosition.z);
        }
        else
        {
            Move(false,
                transform.localPosition.x,
                transform.localPosition.z,
                arm.transform.localPosition.x,
                arm.transform.localPosition.y,
                arm.transform.localPosition.z);
        }
        if (playerInput.playerButton1isPressed ||
            playerInput.playerButton2isPressed)
            Grab(true);
        else
            Grab(false);
    }

    void InitValues()
    {
        speedH = 100f; // Horizontal movement speed
        speedV = 0.5f; // Vertical movement speed
        speedC = 180f; // Grab speed
        rangeYmax = transform.localPosition.y; // Highest pos
        rangeYmin = transform.localPosition.y - 0.95f; // Lowest pos
        elevation = rangeYmax; // Initial height
        length = arm.transform.localScale.y; // ARM initial length
        circum = arm.transform.localScale.x; // ARM initial circumference
        angle = -5; // PINCERS initial angle
        openAngle = -25; // How much to open claw
        closeAngle = 7.5f; // How much to close claw
    }
    void Move(Vector3 input) // Claw horizontal movement
    {
        Rb.linearVelocity = input * Time.deltaTime * speedH;
        if (input != Vector3.zero)
            if (!audioMoveH.isPlaying) 
                audioMoveH.PlayOneShot(soundMoveH);
    }

    void Move(bool down, float x, float z, float xArm, float yArm, float zArm) // Claw vertical movement
    {
        if (down)
        {
            if (elevation < rangeYmin) { return; }
            // Lower claw and extend arm
            elevation -= Time.deltaTime * speedV;
            offset = Time.deltaTime * speedV / 2;
            transform.localPosition = new Vector3(x, elevation, z);
            length += Time.deltaTime * (speedV / 2f);
            arm.transform.localScale = new Vector3(circum, length, circum);
            arm.transform.localPosition = new Vector3(xArm, yArm + offset, zArm);
            if (!audioMoveV.isPlaying) 
                audioMoveV.PlayOneShot(soundMoveV);
        }
        else
        {
            if (elevation > rangeYmax) { return; }
            // Raise claw and retract arm
            elevation += Time.deltaTime * speedV;
            offset = Time.deltaTime * speedV / 2;
            transform.localPosition = new Vector3(x, elevation, z);
            length -= Time.deltaTime * (speedV / 2f);
            arm.transform.localScale = new Vector3(circum, length, circum);
            arm.transform.localPosition = new Vector3(xArm, yArm - offset, zArm);
            if (!audioMoveV.isPlaying) 
                audioMoveV.PlayOneShot(soundMoveV);
        }
    }

    void Grab(bool open) // Controls claw open and close
    {
        if (open)
        {
            if (angle < openAngle) { return; }
            angle -= Time.deltaTime * speedC;
            foreach (GameObject pincer in pincers)
                pincer.transform.eulerAngles = new Vector3(0, pincer.transform.eulerAngles.y, angle);
            if (!audioMoveC.isPlaying) 
                audioMoveC.PlayOneShot(soundMoveC);
        }
        else
        {
            if (angle > closeAngle) { return; }
            angle += Time.deltaTime * speedC * 2;
            foreach (GameObject pincer in pincers)
                pincer.transform.eulerAngles = new Vector3(0, pincer.transform.eulerAngles.y, angle);
            if (!audioMoveC.isPlaying) 
                audioMoveC.PlayOneShot(soundMoveC);
        }
    }
}
// END