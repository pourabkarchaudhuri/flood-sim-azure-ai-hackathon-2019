using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject target; //the enemy's target
    public float moveSpeed = 5; //move speed
    public float rotationSpeed = 5; //speed of turning
    private Rigidbody rb;
    private Vector3 mytransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mytransform = target.transform.position;
    }


    void Update()
    {
        //rotate to look at the player
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), rotationSpeed * Time.deltaTime);


        //move towards the player
        transform.position += transform.forward * Time.deltaTime * moveSpeed;

    }
}
