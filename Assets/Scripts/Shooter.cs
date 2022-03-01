using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attach this script to a GameObject to rotate around the target position.
public class Shooter : MonoBehaviour
{
    //Assign a GameObject in the Inspector to rotate around
    public Transform ballPivot;
    public GameObject ball;
    GameObject _nextTarget;
    float cooldown = 1;

    public void shoot(GameObject target) {
        _nextTarget = target;
        Vector3 position = ballPivot.transform.position;
        Quaternion rotation = ballPivot.transform.rotation;
        GameObject _nextBball = Instantiate(ball, position, rotation);
        _nextBball.GetComponent<Rigidbody>().AddForce(
            (target.transform.position - transform.position) * 4, 
            ForceMode.Impulse
        );
    }

    public void removeTarget()
    {
        _nextTarget = null;
    }

    void Update()
    {
        if (_nextTarget != null)
        {
            Vector3 targetDirection = _nextTarget.transform.position - transform.position;
            float singleStep = Time.deltaTime / cooldown;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            var rotation = Quaternion.LookRotation(newDirection);
            transform.rotation = rotation;
            var localRotation = transform.localRotation;
            localRotation.y = 0;
            transform.localRotation = localRotation;
        }
    }
}