using UnityEngine;

public class ClawController : MonoBehaviour
{
    [System.Serializable] private class Properties
    {
        public float speed;
        public AudioSource audioMove;
        public AudioClip soundMove;
    }

    [System.Serializable] private class PropertiesClaw : Properties
    {
        public float diameter, length, offset;
        public float angle, openAngle, closeAngle;
    }

    [SerializeField] private Properties pH, pV;
    [SerializeField] private PropertiesClaw pC;
    [SerializeField] private GameObject arm;
    [SerializeField] private GameObject[] pincers;
    private float rangeYmin, rangeYmax, elevation;
    private Rigidbody Rb;
    private PlayerInputHandler playerInput; // Gets input from PlayerInputHandler.cs
    private OptionsManager optionsManager; // Persistent settings from OptionsManager.cs

    void InitValues()
    {
        pH.speed = 100f; // Horizontal movement speed
        pV.speed = 0.5f; // Vertical movement speed
        pC.speed = 180f; // Grab speed
        pC.length = arm.transform.localScale.y; // ARM initial length
        pC.diameter = arm.transform.localScale.x; // ARM initial circumference
        pC.angle = -5; // PINCERS initial angle
        pC.openAngle = -25; // How much to open PINCERS
        pC.closeAngle = 7.5f; // How much to close PINCERS
        rangeYmax = transform.localPosition.y; // Highest pos
        rangeYmin = transform.localPosition.y - 0.95f; // Lowest pos
        elevation = rangeYmax; // Initial height
    }
    void Move(Vector3 input) // Claw horizontal movement
    {
        Rb.linearVelocity = input * Time.deltaTime * pH.speed;
        if (input != Vector3.zero)
            if (!pH.audioMove.isPlaying) 
                pH.audioMove.PlayOneShot(pH.soundMove);
    }

    void Move(bool down, float x, float z, float xArm, float yArm, float zArm) // Claw vertical movement
    {
        if (down)
        {
            if (elevation < rangeYmin) { return; }
            // Lower claw and extend arm
            elevation -= Time.deltaTime * pV.speed;
            pC.offset = Time.deltaTime * pV.speed / 2;
            transform.localPosition = new Vector3(x, elevation, z);
            pC.length += Time.deltaTime * (pV.speed / 2f);
            arm.transform.localScale = new Vector3(pC.diameter, pC.length, pC.diameter);
            arm.transform.localPosition = new Vector3(xArm, yArm + pC.offset, zArm);
            if (!pV.audioMove.isPlaying) 
                pV.audioMove.PlayOneShot(pV.soundMove);
        }
        else
        {
            if (elevation > rangeYmax) { return; }
            // Raise claw and retract arm
            elevation += Time.deltaTime * pV.speed;
            pC.offset = Time.deltaTime * pV.speed / 2;
            transform.localPosition = new Vector3(x, elevation, z);
            pC.length -= Time.deltaTime * (pV.speed / 2f);
            arm.transform.localScale = new Vector3(pC.diameter, pC.length, pC.diameter);
            arm.transform.localPosition = new Vector3(xArm, yArm - pC.offset, zArm);
            if (!pV.audioMove.isPlaying) 
                pV.audioMove.PlayOneShot(pV.soundMove);
        }
    }

    void Grab(bool open)
    {
        if (open)
        {
            if (pC.angle < pC.openAngle) { return; }
            // Open pincers
            pC.angle -= Time.deltaTime * pC.speed;
            foreach (GameObject pincer in pincers)
                pincer.transform.eulerAngles = new Vector3(0,
                    pincer.transform.eulerAngles.y, pC.angle);
            if (!pC.audioMove.isPlaying) 
                pC.audioMove.PlayOneShot(pC.soundMove);
        }
        else
        {
            if (pC.angle > pC.closeAngle) { return; }
            // Close pincers
            pC.angle += Time.deltaTime * pC.speed * 2;
            foreach (GameObject pincer in pincers)
                pincer.transform.eulerAngles = new Vector3(0,
                    pincer.transform.eulerAngles.y, pC.angle);
            if (!pC.audioMove.isPlaying) 
                pC.audioMove.PlayOneShot(pC.soundMove);
        }
    }

    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        playerInput = GameObject.Find("PlayerInputHandler").GetComponent<PlayerInputHandler>();
        if (GameObject.Find("OptionsManager"))
            if (GameObject.Find("OptionsManager").TryGetComponent<OptionsManager>(out optionsManager))
            {
                pH.audioMove.volume = optionsManager.options.volume;
                pH.audioMove.volume = optionsManager.options.volume;
                pH.audioMove.volume = optionsManager.options.volume;
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
}
// END