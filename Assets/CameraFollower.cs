using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform leader;
    public float followSharpness = 0.1f;

    public Vector3 _followOffset;

   

    void LateUpdate()
    {
        
        Vector3 targetPosition = leader.position + _followOffset;   
        transform.position += (targetPosition - transform.position) * followSharpness;
    }
    private void OnEnable()
    {
        GetComponent<AudioSource>().Play();
    }

    private void OnDisable()
    {
        GetComponent<AudioSource>().Stop();
    }
}
