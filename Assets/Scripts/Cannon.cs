using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Cannon : MonoBehaviour
{

    public LayerMask layer;
    public float fieldOfView = 30;
    public GameObject shooter;
    private List<GameObject> targets;
    float time = 0;
    bool shoot = false;
    int indexToShoot = 0;
    float cooldown = 1;

    // Update is called once per frame
    void Start()
    {
        targets = FindGameObjectsWithLayer(layer);
        if (targets != null)
        {
            targets = targets.Where(target => IsSeen(target, fieldOfView)).ToList();
        }
    }

    private void Update()
    {
        if (indexToShoot < targets.ToArray().Length) {
            if (targets[indexToShoot] != null)
            {
                time += Time.deltaTime;

                Vector3 targetDirection = targets[indexToShoot].transform.position - transform.position;
                float singleStep = Time.deltaTime / cooldown;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
                newDirection.y = 0;
                transform.rotation = Quaternion.LookRotation(newDirection);

                if (time >= cooldown)
                {
                    time = 0;
                    shoot = true;
                }

                if (shoot)
                {
                    shoot = false;
                    shooter.GetComponent<Shooter>().shoot(targets[indexToShoot]);
                    indexToShoot++;
                }
            } else
            {
                indexToShoot++;
            }
        }

        if (indexToShoot == targets.ToArray().Length) 
        {
            shooter.GetComponent<Shooter>().removeTarget();
        }
    }

    List<GameObject> FindGameObjectsWithLayer(LayerMask layerMask) {
        GameObject[] goArray = FindObjectsOfType<GameObject>();
        var goList = new List<GameObject>();
        for (var i = 0; i< goArray.Length; i++) {
            if ((layerMask & 1 << goArray[i].layer) == 1 << goArray[i].layer)
            {
                goList.Add(goArray[i]);
            }
        }
        if (goList.Count == 0)
        {
            return null;
        }
        return goList;
    }

    protected bool IsSeen(GameObject target, float fieldOfView)
    {
        RaycastHit hit;
        Vector3 rayDirection = target.transform.position - transform.position;

        if ((Vector3.Angle(rayDirection, transform.forward)) <= fieldOfView * 0.5f)
        {
            if (Physics.Raycast(transform.position, rayDirection, out hit))
            {
                return true;
            }
        }

        return false;
    }

}
