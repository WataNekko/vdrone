using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DestroyWithDelay : MonoBehaviour
{
    public float Delay = 0f;

    private void Start()
    {
        Destroy(gameObject, Delay);
    }
}
