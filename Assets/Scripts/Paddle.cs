using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Paddle : MonoBehaviour
{
    private const float PowerUpScale = 2f;
    [SerializeField] private int PowerUpSeconds = 10;
    public float Speed = 2.0f;
    public float MaxMovement = 2.0f;

    private float scale;
    private bool hasPowerUp;

    // Start is called before the first frame update
    void Start()
    {
        this.scale = this.gameObject.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        float input = Input.GetAxis("Horizontal");

        Vector3 pos = transform.position;
        pos.x += input * Speed * Time.deltaTime;

        if (pos.x > MaxMovement)
            pos.x = MaxMovement;
        else if (pos.x < -MaxMovement)
            pos.x = -MaxMovement;

        transform.position = pos;
    }

    public void ApplyPowerUp(int powerUp)
    {
        if (hasPowerUp == true)
        {
            return;
        }

        var colour = Color.yellow;
        SetColour(colour);

        SetPaddleScale(PowerUpScale);

        this.hasPowerUp = true;

        StopCoroutine(ResetPlayer());
        StartCoroutine(ResetPlayer());
    }

    private void SetPaddleScale(float x)
    {
        var scale = this.gameObject.transform.localScale;
        scale.x *= x;
        this.gameObject.transform.localScale = scale;
    }

    private void ResetPaddleScale()
    {
        var scale = this.gameObject.transform.localScale;
        scale.x = this.scale;
        this.gameObject.transform.localScale = scale;
    }

    private void SetColour(Color colour)
    {
        var renderer = GetComponentInChildren<Renderer>();
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", colour);
        renderer.SetPropertyBlock(block);
    }

    private IEnumerator ResetPlayer()
    {
        yield return new WaitForSeconds(PowerUpSeconds);

        ResetPaddleScale();
        SetColour(Color.grey);
        this.hasPowerUp = false;
    }
}
