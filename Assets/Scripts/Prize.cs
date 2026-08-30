using UnityEngine;

public class Prize : MonoBehaviour
{
    public int pointValue = 5;
    public bool won = false;
    private Rigidbody rB;
    private float forceX, forceY, forceZ, maxRange;
    private Vector3 randomTorque;
    void Start()
    {   // Prizes get a slight random torque to randomize landing position
        rB = GetComponent<Rigidbody>();
        maxRange = 0.005f;
        forceX = Random.Range(0f, maxRange);
        forceY = Random.Range(0f, maxRange);
        forceZ = Random.Range(0f, maxRange);
        randomTorque = new Vector3(forceX, forceY, forceZ);
        rB.AddTorque(randomTorque, ForceMode.Impulse);
    }
} // END