using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySegment
{
    public static float DistanceToSideRatio = 2f;

    public Vector2 CenterPosition { get; private set; }

    public Vector2 CenterOfMass { get; private set; }

    public float SideLenth { get; private set; }

    public float TotalMass { get; private set; }

    private readonly GravitySegment[,] subSegments = null;

    public bool IsTerminal => subSegments == null;

    private HashSet<AGravitator> gravitators = null;

    //total number of gravitators contained in this branch
    public int GravitatorCount { get; private set; } = 0; 

    public GravitySegment(int nestingDegree, float sideLength, Vector2 center)
    {
        var splittingDegree = 2;

        if (nestingDegree < 0 || sideLength <= 0)
        {
            throw new System.ArgumentOutOfRangeException($"cannot create gravity segment" +
                $" with nestingDegree = {nestingDegree} and sideLenght = {sideLength}");
        }

        CenterPosition = center;

        if (nestingDegree == 0)
        {
            gravitators = new HashSet<AGravitator>();
            return;
        }

        subSegments = new GravitySegment[splittingDegree, splittingDegree];


        var xDelta = sideLength / splittingDegree;
        var yDelta = sideLength / splittingDegree;

        var initVec = center - new Vector2(sideLength / 2, sideLength / 2) + new Vector2(xDelta, yDelta);

        for (int i = 0; i < splittingDegree; i++)
        {
            for (int j = 0; j < splittingDegree; j++)
            {
                subSegments[i, j] = new GravitySegment(nestingDegree - 1, sideLength / 2,
                    initVec + new Vector2(xDelta*i, yDelta*j));
            }
        }
    }

    public void RemoveAll()
    {
        if (GravitatorCount == 0)
        {
            return;
        }
    }

    public void CalcMassData()
    {
        if (GravitatorCount <= 0)
        {
            return;
        }

        var subSenters = new List<Vector2>();
        var subMasses = new List<float>();
        float nextTotalMass = 0f;
        if (IsTerminal)
        {
            //here if terminal
            foreach (var gravitator in gravitators)
            {
                subSenters.Add(gravitator.Position);
                var sMassEqv = gravitator.Mass * gravitator.AttractOthersMult;
                subMasses.Add(sMassEqv);
                nextTotalMass += sMassEqv;
            }
        }
        else
        {
            foreach (var segment in subSegments)
            {
                segment.CalcMassData();
                subSenters.Add(segment.CenterOfMass);
                subMasses.Add(segment.TotalMass);
                nextTotalMass += segment.TotalMass;
            }
        }

        CenterOfMass = ComputeCenterOfMass(subSenters, subMasses, nextTotalMass);
        TotalMass = nextTotalMass;
    }

    public bool ContainsPoint(Vector2 point)
    {
        var dx = Mathf.Abs(point.x - CenterPosition.x);
        var dy = Mathf.Abs(point.y - CenterPosition.y);
        return (dx <= SideLenth/2) && (dy <= SideLenth/2);
    }

    private Vector2 ComputeCenterOfMass(List<Vector2> points, List<float> masses, float massSum)
    {
        if (points.Count != masses.Count)
        {
            throw new System.ArgumentException($"can not compute center of mass: points.Count[{points.Count}] != masses.Count[{masses.Count}]");
        }
        var result = new Vector2(0, 0);
        for (int i = 0; i < points.Count; i++)
        {
            result += points[i] * (masses[i] / massSum);
        }
        return result;
    }

    public Vector2 CalcForce(Vector2 attractedPos, float gravityConst)
    {
        if (GravitatorCount <= 0)
        {
            return Vector2.zero;
        }

        //now using senterOfMass
        //possibly it will be better to use segment senter
        var distance = (CenterOfMass - attractedPos).magnitude;
        if (distance >= DistanceToSideRatio * SideLenth || IsTerminal)
        {
            return (CenterOfMass - attractedPos).normalized * (gravityConst * (TotalMass / (distance * distance)));
        }

        var result = Vector2.zero;
        foreach (var subsegment in subSegments)
        {
            result += subsegment.CalcForce(attractedPos, gravityConst);
        }
        return result;
    }
}
