using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    public static APIManager instance;
    private string api_URl = "http://52.172.32.166:3000/ml/api/v1.0/assistant";
    private string reset_URL = "http://52.172.32.166:3000/ml/api/v1.0/reset?stress=78";
    private RootObject root;
    [SerializeField]
    private string inputTextFromPlayer;
    public List<Result> result;
    public SpeechManager speechManager;
    public bool isBotResponseDone=true;

   
    private void Start()
    {
        instance = this;
        CallResetAPI();

    }
    public void CallApi(string message)
    {
        Debug.Log("API Hit  "  + message);

        StartCoroutine(PostMessage(message));
    }
    public void CallResetAPI()
    {
        StartCoroutine(ResetApiResponse(reset_URL));
    }

    IEnumerator ResetApiResponse(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":  \nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }
    IEnumerator PostMessage(string message)
    {
       if(isBotResponseDone)
        {
         isBotResponseDone = false;
        inputTextFromPlayer = message;
        SpeechDisplay.Instance.UpdatePlayer_speech(message);
        string json = "{\"sentence\":\"" + message + "\"}";
        var request = new UnityWebRequest(api_URl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        root = JsonUtility.FromJson<RootObject>(request.downloadHandler.text);
        result = root.result;

            BotAnimations.instance.CheckEmotions(result[0].reaction);
                StartCoroutine(SpeakResponse(result[0]));

            
        }
    }

    IEnumerator  SpeakResponse(Result s)
    {
        yield return new WaitForSeconds(2f);
        SpeechDisplay.Instance.UpdateBot_speech(s.response,s.sentiment.polarity);
        SpeechDisplay.Instance.UpdateBot_stressLevel(s.stress);
        speechManager.Speak(s.response);
        //Debug.Log("GameOver2" + s.completion);
        // Checking Speech Conversation gets Complete
        if (s.completion)
        {
            Debug.Log("GameOver");
            WalkingAudioController.instance.SpeechEnd();
        }
    }
}
[Serializable]
public class Sentiment
{
    public string polarity;
    public string subjectivity;
}
[Serializable]
public class Result
{
    public bool completion;
    public object context;
    public object entities;
    public string intent;
    public string probability;
    public string query;
    public string reaction;
    public string response;
    public bool responsive;
    public Sentiment sentiment;
    public int stress;
    public object trigger;
}

[Serializable]
public class RootObject
{
    public object error;
    public List<Result> result;
}


