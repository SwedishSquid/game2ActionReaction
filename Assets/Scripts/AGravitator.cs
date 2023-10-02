using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGravitator : MonoBehaviour, IGravitator
{
    private Rigidbody2D rb;
    [SerializeField] float mass = 1;
    [SerializeField] float othersMult = 1;
    [SerializeField] float meMult = 1;

    private static HashSet<int> testHashes = new HashSet<int>();

    static AGravitator()
    {
        Debug.LogWarning("using hashchecker for AGravitator");
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (testHashes.Contains(GetHashCode()))
        {
            throw new System.Exception("collision detected while testing hash for AGravitator");
        }
        else
        {
            testHashes.Add(GetHashCode());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.mass = mass;
    }

    // Update is called once per frame
    void Update()
    {
        //chill
    }

    public Vector2 Position => (Vector2)transform.position;

    public float Mass => mass;

    public float AttractOthersMult => othersMult;

    public float AttractMeMult => meMult;

    public void ProcessForce(Vector2 force)
    {
        rb.AddForce(force);
    }

    public void SetMass(float mass)
    {
        this.mass = mass;
        rb.mass = mass;
    }

    private int? id = null;

    public int Id { 
        get 
        {
            if (id == null)
            {
                throw new System.NullReferenceException($"id for object {gameObject} not set, but requested");
            }
            return (int)id;
        } 
        private set 
        {
            id = value;
        } 
    }

    public void SetId(int newId)
    {
        if (id != null)
        {
            Debug.LogWarning($"id for object {gameObject} set twice, which is strange");
        }
        id = newId;
    }
}
