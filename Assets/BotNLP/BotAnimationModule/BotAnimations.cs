using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAnimations : MonoBehaviour
{
    [SerializeField]
    private Animator botAnimator;
    public static BotAnimations instance;


    private void Awake()
    {
        botAnimator = GameObject.FindGameObjectWithTag("Avatar").GetComponent<Animator>();
        instance = this;
    }
    void Start()
    {
        
    }

   
   

    public void CheckEmotions(string mindState)
    {
        switch (mindState)
        {
            case "worried":
            {
                    botAnimator.Play("Worried", 1);
                    Debug.LogError("MindState :" + mindState);
                    break;
                }
            case "responsive":
                {
                    botAnimator.Play("EyeBlink", 1);

                    break;
                }
            case "relieved":
                {
                    botAnimator.Play("EyeBlink", 1);
                  
                    break;
                }
            case "shock":
                {
                    botAnimator.Play("Shock", 1);
                    break;
                }
            case "extreme":
                {
                    botAnimator.Play("ExtremeShock", 1);
                    break;
                }
            default:
                break;
        }
    }

    public void CompleteUICallBack()
    {
        WalkingAudioController.instance.ShowCompletionUI();
    }
    
}
