using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GravityManager: MonoBehaviour 
{
    [SerializeField] float minSquareSide = 1;

    [SerializeField] float nestingDegree = 4;

    //[SerializeField] int 

    [SerializeField] bool showGrid = true;

    [SerializeField] float GravityConstant = 1;

    public static GravityManager instance;

    private List<AGravitator> gravitators = new List<AGravitator>();

    private int nextGravitatorId = 0;

    private void Awake()
    {
        if (instance != null)
        {
            throw new System.Exception("more then 1 GravityManager created");
        }
        instance = this;

        foreach ( var grav in FindObjectsOfType(typeof(AGravitator))
            .Cast<AGravitator>())
        {
            AddGravitator(grav);
        }
        
    }

    private void Update()
    {
        for (int i = 0; i < gravitators.Count; i++)
        {
            var attracted = gravitators[i];
            var gravityForce = Vector2.zero;
            for (int j = 0; j < gravitators.Count; j++)
            {
                if (i == j)
                {
                    continue;
                }
                var anotherGravitator = gravitators[j];
                gravityForce += (CalcForceScalar(attracted.Position, anotherGravitator.Position,
                    attracted.Mass, anotherGravitator.Mass) * anotherGravitator.AttractOthersMult)
                    * ((anotherGravitator.Position - attracted.Position).normalized);
            }
            gravityForce *= attracted.AttractMeMult;
            attracted.ProcessForce(gravityForce);
        }
    }

    public float CalcForceScalar(Vector2 pos1, Vector2 pos2, float mass1, float mass2)
    {
        return (GravityConstant * mass1 * mass2) / (pos2 - pos1).magnitude;
    }

    public void AddGravitator(AGravitator gravitator)
    {
        gravitators.Add(gravitator);
        gravitator.SetId(nextGravitatorId++);
    }

    void OnDrawGizmos()
    {
        if (!showGrid) return;

        Gizmos.color = Color.yellow;

        var bigCubeSide = minSquareSide * (int)System.Math.Round(System.Math.Pow(2, nestingDegree));

        Gizmos.DrawWireCube(transform.position, new Vector3(bigCubeSide, bigCubeSide, 0));
    }
}
