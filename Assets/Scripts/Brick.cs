using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Brick : MonoBehaviour
{
    public UnityEvent<int> onDestroyed;
    
    public int PointValue;

    protected MainManager mainManager;

    protected virtual void Start()
    {
        this.mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();

        var renderer = GetComponentInChildren<Renderer>();

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        switch (PointValue)
        {
            case 1 :
                block.SetColor("_BaseColor", Color.green);
                break;
            case 2:
                block.SetColor("_BaseColor", Color.yellow);
                break;
            case 5:
                block.SetColor("_BaseColor", Color.blue);
                break;
            default:
                block.SetColor("_BaseColor", Color.red);
                break;
        }

        renderer.SetPropertyBlock(block);
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        //Debug.Log("Base collision");

        RemoveBrick();
    }

    public void RemoveBrick()
    {
        //Debug.Assert(this.mainManager != null);

        this.mainManager.ReduceBrickCount();

        // TODO: Add random chance
        this.mainManager.SpawnRandomPowerup(this.gameObject.transform.position);

        onDestroyed.Invoke(PointValue);

        //slight delay to be sure the ball have time to bounce
        Destroy(this.gameObject, 0.2f);
    }
}
