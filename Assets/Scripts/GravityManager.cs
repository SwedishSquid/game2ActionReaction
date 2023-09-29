using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GravityManager: MonoBehaviour 
{
    [SerializeField] float GravityConstant = 1;

    private List<IGravitator> gravitators = new List<IGravitator>();
    private void Awake()
    {
        foreach ( var grav in FindObjectsOfType(typeof(AGravitator))
            .Cast<IGravitator>())
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

    public void AddGravitator(IGravitator gravitator)
    {
        gravitators.Add(gravitator);
        Debug.Log("set gravitator");
    }
}
