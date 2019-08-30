using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using UnityEngine.XR;
using Valve.VR.InteractionSystem;


public class SpeechDisplay : Singleton<SpeechDisplay>
{
    public TMPro.TextMeshProUGUI botSpeech;
    public TMPro.TextMeshProUGUI speechPolarity;
    public TMPro.TextMeshProUGUI playerSpeech;
    public Text stressValue;
    public Text stress;
    public List<Color> _stressColorValue = new List<Color>();


    public GameObject homeScreen;
    public GameObject infoScren;
    public GameObject comingsoonScreen;

    public Transform point1_rescuePoint;
    public GameObject leftHandPointer;
    public GameObject rightHandPointer;

    public GameObject raft;
    public GameObject vrCamera;

    public CPC_CameraPath p;
    public float boatSpeed;
   
    //public Button med;

    private void Awake()
    {
        Camera.main.farClipPlane = 600f;

        Player.instance.gameObject.GetComponent<FloodAreaMove>().PlayerSpeed = 0;
        Player.instance.gameObject.GetComponent<FloodAreaMove>().enabled = false;
    }

   
    public void UpdateBot_speech(string s, string polarity)
    {
        Debug.Log(" BOT               " + s);
        //botSpeech.text = s;
        botSpeech.gameObject.GetComponent<TMPro.Examples.TeleType>().label01 = s;
        botSpeech.gameObject.GetComponent<TMPro.Examples.TeleType>().label02 = s;
        botSpeech.gameObject.GetComponent<TMPro.Examples.TeleType>().Start_SpeechToText();
        speechPolarity.text = "The victim took that as a " + polarity + " response";
    }

    public void UpdatePlayer_speech(string s)
    {

        // botSpeech.text = "";
        Debug.Log(" PLAYER               " + s);
        playerSpeech.text = s;
    }

    public void UpdateBot_stressLevel(int t)
    {
        stressValue.text = t + "%";

        if (t > 85)
        {
            Debug.Log(" GG 1" + t);
            // red color stress level
            stress.color = _stressColorValue[0];
            stressValue.color = _stressColorValue[0];
        }
        else if (t > 75)
        {
            // yellow color stress level
            Debug.Log(" GG 2" + t);
            stress.color = _stressColorValue[1];
            stressValue.color = _stressColorValue[1];
        }
        else if (t > 60)
        {
            // blue color stress level
            stress.color = _stressColorValue[2];
            stressValue.color = _stressColorValue[2];
        }
        else
        {
            // blue color stress level
            stress.color = _stressColorValue[2];
            stressValue.color = _stressColorValue[2];
        }
    }

    public void Reset_BothSpeech()
    {
        //playerSpeech.text = "";
        //botSpeech.text = "";
    }


    public void Medical_Student_Clicked()
    {
        Debug.Log("I am clicking Medical");

        //show instruction
        homeScreen.SetActive(false);
        infoScren.SetActive(true);
    }

    public void Architect_Clicked()
    {
        // show coming soon panel
        homeScreen.SetActive(false);
        comingsoonScreen.SetActive(true);
    }

    public void Back_ArchitectClicked()
    {
        // disable coming soon and show home screen
        comingsoonScreen.SetActive(false);
        homeScreen.SetActive(true);
    }

    public void Back_StudentClicked()
    {
        // disable info screen and show home screen
        infoScren.SetActive(false);
        homeScreen.SetActive(true);
    }

    public void Ok_ArchitectClicked()
    {
        // disable coming soon and show home screen
        comingsoonScreen.SetActive(false);
        homeScreen.SetActive(true);
    }

    public void Ok_StudentClicked()
    {
        // TEMPORARY CODE BEFORE RAFT IMPLEMETATION
        infoScren.SetActive(false);
        leftHandPointer.SetActive(false);
        rightHandPointer.SetActive(false);
        
        // disable info screen ans start camera animation
        raft.GetComponent<CameraFollower>().enabled = true;
        Invoke("Waitandplay",1.0f);

    }

    void Waitandplay()
    {

      //  raft.transform.parent = vrCamera.transform;
        p.PlayPath(boatSpeed);
        InputTracking.disablePositionalTracking = true;
      
        Invoke("Reset_FarClipPlane", boatSpeed);
    }

    void Reset_FarClipPlane()
    {
        //   Player.instance.gameObject.GetComponent<FloodAreaMove>().PlayerSpeed = 1.8f;
        Player.instance.gameObject.GetComponent<FloodAreaMove>().enabled = true;
        Player.instance.gameObject.GetComponent<FloodAreaMove>().PlayerSpeed = 1.8f;


        Camera.main.farClipPlane = 600f;
        InputTracking.disablePositionalTracking = false;
        Player.instance.transform.position = point1_rescuePoint.position;
        Camera.main.transform.localPosition = Vector3.zero;
        raft.GetComponent<CameraFollower>().enabled = false;
        //  raft.transform.parent = null;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            p.PlayPath(20.0f);
            InputTracking.disablePositionalTracking = true;
            Invoke("FogDisable", 15.0f);
        }
        if (Input.GetKey(KeyCode.B))
        {
            SceneManager.LoadScene(0);
        }
       
    }

   
}
