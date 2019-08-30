using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;
using UnityEngine.Windows.Speech;

public class STTScript : MonoBehaviour {

    // private KeywordRecognizer keywordRecognizer;
    public SpeechManager speechManager;
    public APIManager apiManager;
    [SerializeField]
    private Animator botAnimator;
  
    private DictationRecognizer dictationRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    // VideoPlayer videoPlayer;
    // public bool OpenDoor=false, CloseDoor=false;


    private void Start()
    {
        botAnimator = GameObject.FindGameObjectWithTag("Avatar").GetComponent<Animator>();

        // instance = this.gameObject.GetComponent<DoorInstructionManager>();
        // actions.Add("Please explain the door opening process",VideoOne);
        // actions.Add("Please explain the door closing process",VideoTwo);
        // keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        dictationRecognizer = new DictationRecognizer();
        //videoPlayer = videoObject.gameObject.GetComponent<VideoPlayer>();
        
        // keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
        dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;
        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
        // inputRootObject.contexts = "";
        // dictationRecognizer.Start();
        // inputRootObject = new InputRootObject();


}   
    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        Debug.Log("Complete Text : " + cause.ToString());
        //speechManager.Speak(cause.ToString());
        //  Speak("hello world");

    }

    // private void PostCause()
    // {
    //     Debug.Log("start");

    //     string jSon = JsonUtility.ToJson(inputRootObject);
    //    // string jSon = "{\r\n    \"contexts\": [\r\n      \"shop\"\r\n    ],\r\n    \"lang\": \"en\",\r\n    \"query\": \"open the door\",\r\n    \"sessionId\": \"12345\",\r\n    \"timezone\": \"America/New_York\"\r\n}";

    //     StartCoroutine(Post("https://api.dialogflow.com/v1/query", jSon));
    //     Debug.Log("start" +jSon);
    // }

    private void DictationRecognizer_DictationHypothesis(string text)
    {
      //  Debug.Log("dictQQQQQQQ" + text);
    }

    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
       // Debug.Log("Dict 1111111111111 : " + text);
        apiManager.CallApi(text);
    }

    // private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs speech)
    // {
    //     Debug.Log(speech.text);
    //     actions[speech.text].Invoke();
    // }

    // private void VideoOne()
    // {
    //     OpenDoor = true;
    //     CloseDoor = false;
    //     Debug.Log("Opening");
    //     //instance.audioInstruction("DoorOpen");
    // }
    // private void VideoTwo()
    // {
    //     OpenDoor = false;
    //     CloseDoor = true;
    //     Debug.Log("Closing");
    //    // instance.audioInstruction("CloseDoor0");
    // }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || WalkingAudioController.instance.canSpeak)
        {
            dictationRecognizer.Start();
            botAnimator.SetBool("isSpeaking", true);
          
        }
        if (Input.GetKeyDown(KeyCode.X) || !WalkingAudioController.instance.canSpeak)
        {
             dictationRecognizer.Stop();
            botAnimator.SetBool("isSpeaking", false);
        }

    }

    // IEnumerator Post(string url, string bodyJsonString)
    // {
    //     Debug.Log("url" + url);
    //     Debug.Log("bodyJsonString" + bodyJsonString);
    //     var request = new UnityWebRequest(url, "POST");
    //     byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
    //     request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
    //     request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    //     request.SetRequestHeader("Content-Type", "application/json");
    //     request.SetRequestHeader("Authorization", "Bearer 531af8ee3a8b4230a09bb4154cc2e435");

    //     yield return request.Send();
    //     resultClass = JsonUtility.FromJson<ResultRootObject>(request.downloadHandler.text);

    //     string action = resultClass.result.action;

    //     if(action.Trim() == "open-door")
    //     {
    //         Debug.Log("Started");
    //         VideoOne();
    //     }
    //     if (action.Trim() == "close-door")
    //     {
    //         Debug.Log("Stopped");
    //         VideoTwo();
    //     }
    // }
    // public void resetVoiceCommands()
    // {
    //     dictationRecognizer.Stop();
    //     dictationRecognizer.Start();
    // }
}

