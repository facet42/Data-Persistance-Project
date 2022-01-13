using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ToughBrick : Brick // INHERITANCE
{
    // Number of hits to destroy
    public int Strength = 2;
    private Renderer BrickRenderer;
    private MaterialPropertyBlock materialBlock;

    // Start is called before the first frame update
    void Start()
    {
        BrickRenderer = GetComponentInChildren<Renderer>();

        SetColour(Color.red);
    }

    private void SetColour(Color colour)
    {
        materialBlock = new MaterialPropertyBlock();
        materialBlock.SetColor("_BaseColor", colour);
        BrickRenderer.SetPropertyBlock(materialBlock);
    }

    // POLYMORPHISM
    private void OnCollisionEnter(Collision other)
    {
        // TODO: Handle more damage levels
        SetColour(Color.yellow);

        // Only destroy once it has been hit multiple times
        if (--Strength <= 0)
        {
            onDestroyed.Invoke(PointValue);

            //slight delay to be sure the ball have time to bounce
            Destroy(gameObject, 0.2f);
        }
    }
}
