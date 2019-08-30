using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingAudioController : MonoBehaviour
{
    public static WalkingAudioController instance;
    public Animator botAnimator;
    public bool canSpeak;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip waterEffect;
    [SerializeField]
    AudioClip woodEffect;
    public GameObject teleportMesh;
    public GameObject TeleportStart, TeleportEnd;
    public GameObject botSpeechUI;
    public GameObject playerSpeechUI;
    public GameObject completionUI;
    // Start is called before the first frame update
    void Start()
    {
        botAnimator = GameObject.FindGameObjectWithTag("Avatar").GetComponent<Animator>();
        
        instance = this;
        MicroPhoneEnable();
    }

    void MicroPhoneEnable()
    {
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
            if (device.Contains("VIVE"))
            {
                Debug.Log("Microphone status " + Microphone.IsRecording(device));
                if (!Microphone.IsRecording(device))
                {
                    Microphone.Start(device, true, 10, 44100);
                    Debug.Log("Microphone status " + Microphone.IsRecording(device));
                }

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "TeleportStart")
        {
            EnableUICanvas();
            Debug.Log("CanSpeak");
            canSpeak = true;
            MicroPhoneEnable();
            teleportMesh.GetComponent<MeshRenderer>().enabled = false;
        }
        if (other.name == "TeleportEnd")
        {
            botAnimator.SetBool("SpeechComplete", true);
            TeleportEnd.SetActive(false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
       // Debug.Log("Name :" +other.name);
        if (other.name == "Walking Area Ocean")
        {
            audioSource.clip = waterEffect;
            audioSource.volume = 0.4f;
            audioSource.loop = true;

        }
        if (other.name == " Cluster_new Variant")
        {
            audioSource.clip = waterEffect;
            audioSource.volume = 0.4f;
            audioSource.loop = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Walking Area Ocean")
        {
            audioSource.clip = woodEffect;
            audioSource.volume = 1f;
            audioSource.Play();
        }
        if (other.name == "TeleportStart")
        {
            canSpeak = false;
            teleportMesh.GetComponent<MeshRenderer>().enabled = true;
            DisableUICanvas();
            botAnimator.gameObject.transform.localPosition = new Vector3(7f, 0.44f, -0.442f);
        }
    }

    public void PlayWalkAudio(bool start)
    {
        //Debug.Log("Bool " + start);
        if (start)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            audioSource.loop = false;
            audioSource.Stop();


        }
      
    }
    public void SpeechEnd()
    {
        Debug.Log("TeleportEnd");
       
        TeleportStart.SetActive(false);
        canSpeak = false;
        APIManager.instance.isBotResponseDone = false;
        Invoke("DisableUICanvas",5f);
        Invoke("EnableTeleportEnd",6f);

    }

    void EnableTeleportEnd()
    {
        TeleportEnd.SetActive(true);
    }

    void DisableUICanvas()
    {
        botSpeechUI.SetActive(false);
        playerSpeechUI.SetActive(false);
       
        
    }

    void EnableUICanvas()
    {
        botSpeechUI.SetActive(true);
        playerSpeechUI.SetActive(true);
    }
   
    public void ShowCompletionUI()
    {
        completionUI.SetActive(true);
    }
}
