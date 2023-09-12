using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDGem : MonoBehaviour
{
   void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerMovement>();
            player.setDDs();
            gameObject.SetActive(false);
        }
    }
}
