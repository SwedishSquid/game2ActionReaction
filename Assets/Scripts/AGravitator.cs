using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGravitator : MonoBehaviour, IGravitator
{
    private GravityManager gManager;
    private Rigidbody2D rb;
    [SerializeField] float GravityMass = 1;
    [SerializeField] float OthersMult = 1;
    [SerializeField] float MeMult = 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (gManager == null)
        {
            throw new System.Exception("gManager not set on start");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //chill
    }

    public Vector2 Position => (Vector2)transform.position;

    public float Mass => GravityMass;

    public float AttractOthersMult => OthersMult;

    public float AttractMeMult => MeMult;

    public void ProcessForce(Vector2 force)
    {
        rb.AddForce(force);
    }
}
