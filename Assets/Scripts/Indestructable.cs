using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indestructable : Brick // INHERITANCE
{
    // Start is called before the first frame update
    void Start()
    {
        var renderer = GetComponentInChildren<Renderer>();

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", Color.grey);
        renderer.SetPropertyBlock(block);
    }

    // POLYMORPHISM
    private void OnCollisionEnter(Collision collision)
    {
        // Override the collision method to ensure the brick is not destroyed
    }
}
