using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public LayerMask targetLayer;
    private void OnCollisionEnter(Collision collision)
    {
        if ((targetLayer & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer && !collision.gameObject.CompareTag("Ragdoll"))
        {
            Destroy(collision.gameObject);
        }
    }
}
