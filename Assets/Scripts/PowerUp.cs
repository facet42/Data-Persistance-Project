using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] int powerUpType;   // TODO: Create enum

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < 0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var player = other.gameObject.GetComponent<Paddle>();
            player.ApplyPowerUp(this.powerUpType);
            Destroy(this.gameObject);
        }
    }
}
