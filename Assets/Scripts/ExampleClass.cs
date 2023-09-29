using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour
{
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(3, 4, 0));
    }
}