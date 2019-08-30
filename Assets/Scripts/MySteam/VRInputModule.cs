using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class VRInputModule : BaseInputModule
{
    public Camera m_Camera;
    public SteamVR_Input_Sources m_TargetSource;

    public SteamVR_Action_Boolean m_ClickAction;

    private GameObject m_CurrentObject = null;
    private PointerEventData m_Data = null;

    public override void Process()
    {
        m_Data.Reset();
        m_Data.position = new Vector2(m_Camera.pixelWidth / 2, m_Camera.pixelHeight / 2);
        eventSystem.RaycastAll(m_Data, m_RaycastResultCache);
        m_Data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        m_CurrentObject = m_Data.pointerCurrentRaycast.gameObject;

        //clear
        m_RaycastResultCache.Clear();

        //Hover
        HandlePointerExitAndEnter(m_Data, m_CurrentObject);

        //Press
        if (m_ClickAction.GetStateDown(m_TargetSource))
            ProessPress(m_Data);

        //Rekarse
        if (m_ClickAction.GetStateUp(m_TargetSource))
            ProcessRelease(m_Data);
    }
     
    protected override void Awake()
    {
        m_Data = new PointerEventData(eventSystem);

    }
    //protected override void OnEnable()
    //{
    //    //m_ClickAction.AddOnStateDownListener(TriggerClick, m_TargetSource);
       
    //}



    //protected override void OnDisable()
    //{
    //    m_ClickAction.RemoveOnStateDownListener(TriggerClick, m_TargetSource);

    //}

    //private void TriggerClick(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    //{
    //    Debug.Log("MenuClick Handler Down");
    //    ProessPress(m_Data);
    //}

    public PointerEventData GetData()
    {
        return m_Data;
    }

    private void ProessPress(PointerEventData data)
    {
        //set raycast 
        data.pointerPressRaycast = data.pointerCurrentRaycast;

        //Check for Object Hit, get the downhandler call 
        GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(m_CurrentObject, data, ExecuteEvents.pointerDownHandler);

        //If No down handler try and get click handler
        if (newPointerPress = null)
            newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(m_CurrentObject);

        //set data
        data.pressPosition = data.position;
        data.pointerPress = newPointerPress;
        data.rawPointerPress = m_CurrentObject;


    }
   

    private void ProcessRelease(PointerEventData data)
    {
        //Execute piinter Up
        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

        //Check for Click Handler
        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(m_CurrentObject);

        //Check if actual
        if(data.pointerPress == pointerUpHandler)
        {
            ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerClickHandler);
        }

        //clear selected gameobject
        eventSystem.SetSelectedGameObject(null);

        //Reset Data
        data.pressPosition = Vector2.zero;
        data.pointerPress = null;
        data.rawPointerPress = null;

    }
}
