using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectComponentHandler : MonoBehaviour
{
    public static float MeshHeight(Renderer renderer)
    {
        return renderer.bounds.size.y;
    }
}
