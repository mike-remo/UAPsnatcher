using UnityEngine;

public class ClawController : MonoBehaviour
{
    [System.Serializable] private class Properties
    {
        public float speed;
        public AudioSource audioMove;
        public AudioClip soundMove;
    }
    // INHERITANCE EXAMPLE (Child class inherits from parent and extends it)
    [System.Serializable] private class PropertiesClaw : Properties
    {
        public float diameter, length, offset, angle, openAngle, closeAngle;
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
        pC.diameter = arm.transform.localScale.x; // ARM diameter
        pC.openAngle = -25; // How much to open PINCERS
        pC.closeAngle = 7.5f; // How much to close PINCERS
        pC.angle = pC.closeAngle; // PINCERS initial angle
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
    //POLYMORPHISM EXAMPLE (Method overloading)
    void Move(bool down, float x, float z, Vector3 armPos) // Claw vertical movement
    {
        if (down)
        {
            if (elevation < rangeYmin) { return; }
            // Lower claw and extend arm
            elevation -= Time.deltaTime * pV.speed;
            pC.offset = Time.deltaTime * pV.speed / 2;
            pC.length += Time.deltaTime * (pV.speed / 2);
            transform.localPosition = new Vector3(x, elevation, z);
            arm.transform.localScale = new Vector3(pC.diameter, pC.length, pC.diameter);
            arm.transform.localPosition = new Vector3(armPos.x, armPos.y + pC.offset, armPos.z);
            if (!pV.audioMove.isPlaying) 
                pV.audioMove.PlayOneShot(pV.soundMove);
        }
        else
        {
            if (elevation > rangeYmax) { return; }
            // Raise claw and retract arm
            elevation += Time.deltaTime * pV.speed;
            pC.offset = Time.deltaTime * pV.speed / 2;
            pC.length -= Time.deltaTime * (pV.speed / 2);
            transform.localPosition = new Vector3(x, elevation, z);
            arm.transform.localScale = new Vector3(pC.diameter, pC.length, pC.diameter);
            arm.transform.localPosition = new Vector3(armPos.x, armPos.y - pC.offset, armPos.z);
            if (!pV.audioMove.isPlaying) 
                pV.audioMove.PlayOneShot(pV.soundMove);
        }
    }
    // ABSTRACTION EXAMPLE (Separate move claw code from out of update)
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
    {   // ABSTRACTION EXAMPLE (Raw input handled in another class)
        Move(playerInput.playerMoveInput3d);
        if (playerInput.playerButton1isPressed)
        {
            Move(true,
                transform.localPosition.x,
                transform.localPosition.z,
                arm.transform.localPosition);
        }
        else
        {
            Move(false,
                transform.localPosition.x,
                transform.localPosition.z,
                arm.transform.localPosition);
        }
        if (playerInput.playerButton1isPressed ||
            playerInput.playerButton2isPressed)
            Grab(true);
        else
            Grab(false);
    }
} // END