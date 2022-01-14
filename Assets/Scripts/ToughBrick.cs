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
    protected override void Start()
    {
        this.mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();

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
    protected override void OnCollisionEnter(Collision other)
    {
        //Debug.Log("Tough collision");

        this.Strength -= 1;
        GameManager.Instance.AddScore(1);

        // TODO: Handle more damage levels
        SetColour(Color.yellow);

        // Only destroy once it has been hit multiple times
        if (Strength <= 0)
        {
            RemoveBrick();

            //onDestroyed.Invoke(PointValue);

            ////slight delay to be sure the ball have time to bounce
            //Destroy(this.gameObject, 0.2f);
        }
    }
}
