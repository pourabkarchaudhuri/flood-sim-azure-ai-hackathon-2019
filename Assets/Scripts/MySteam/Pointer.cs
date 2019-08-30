using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour
{
    public float m_DefaultLength = 5.0f;
    public GameObject m_Dot;
    public VRInputModule m_InputModule;

    private LineRenderer m_LineRenderer = null;

    

    private void Awake()
    {
        m_InputModule= GameObject.FindObjectOfType(typeof(VRInputModule)) as VRInputModule;
        m_LineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        UpdateLine();   
    }

    void UpdateLine()
    {
        PointerEventData data = m_InputModule.GetData();

        // Use default or distance
        float targetLength = data.pointerCurrentRaycast.distance == 0 ? m_DefaultLength : data.pointerCurrentRaycast.distance;

        //Raycast
        RaycastHit hit = CreateRaycast(targetLength);

        //Default
        Vector3 endPosition = transform.position + (transform.forward * targetLength);

        //Or based on Hit
        if(hit.collider != null)
        {
            endPosition = hit.point;
        }

        //SetPosition of the dot
        m_Dot.transform.position = endPosition;

        //Set Linerenderer
        m_LineRenderer.SetPosition(0, transform.position);
        m_LineRenderer.SetPosition(1, endPosition);


    }

    private RaycastHit CreateRaycast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, m_DefaultLength);
        return hit;
    }
}
