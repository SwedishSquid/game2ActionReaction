using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGravitator
{
    public Vector2 Position { get; }

    public float AttractOthersMult { get; }

    public float AttractMeMult { get; }

    public float Mass { get; }

    public void ProcessForce(Vector2 force);
}
