using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;
//Player Movement Script using Left or Right the controllers.
public class FloodAreaMove : MonoBehaviour
{
    public SteamVR_Action_Boolean floodArea_Move;
    public SteamVR_Action_Boolean menu_Click;
    private Hand hand;
    public SteamVR_Input_Sources VR_InputSources;
    public float PlayerSpeed;
    public CharacterController characterController;
    public Transform m_head;
    // Start is called before the first frame update
    void Awake()
    {
        if (characterController == null)
            Debug.LogError("Character Controller Error");
        Debug.Log("Head Height" + m_head.localPosition.y);
        AdjustHeight();
        Debug.Log("Controller Heights" + characterController.height);


    }

    private void OnEnable()
    {
        if (hand == null)
            hand = this.GetComponent<Hand>();
        if (floodArea_Move == null)
        {
            Debug.LogError("<b>[SteamVR Interaction]</b> No Move action assigned");
            return;
        }
        
        floodArea_Move.AddOnStateUpListener(FloodedAreaMoveStatehandler, VR_InputSources);
        floodArea_Move.AddOnStateDownListener(FloodAreaMoveStateHandlerDown, VR_InputSources);
        menu_Click.AddOnStateDownListener(MenuClick, VR_InputSources);
    }
    private void MenuClick(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("MenuClick Handler Down");
         //Destroy(FindObjectOfType<InputModule>());
        SceneManager.LoadScene(0);
    }

   

    private void FloodAreaMoveStateHandlerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Move State Handler Down");
        WalkingAudioController.instance.PlayWalkAudio(true);

    }

    private void FloodedAreaMoveStatehandler(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Move State Handler Up");
        WalkingAudioController.instance.PlayWalkAudio(false);
    }


    private void OnDisable()
    {
        if (floodArea_Move != null) floodArea_Move.RemoveOnStateUpListener(FloodedAreaMoveStatehandler, VR_InputSources);
        if (floodArea_Move != null) floodArea_Move.RemoveOnStateDownListener(FloodAreaMoveStateHandlerDown, VR_InputSources);
    }

    private void Update()
    {
        SteamVR_Action_Boolean moveAction = SteamVR_Input.GetBooleanAction("FloodedArea_Walk");
        bool Moving =  moveAction.GetState(SteamVR_Input_Sources.Any);
        if(Moving)
        {
            characterController.SimpleMove(Player.instance.bodyDirectionGuess  * PlayerSpeed);
            //Player.instance.transform.Translate ( Player.instance.bodyDirectionGuess * Time.deltaTime * PlayerSpeed );
        }
        AdjustHeight();
    }

    public void AdjustHeight()
    {
     //   Debug.Log("Adjust height calling");
        // Fixing the Height of the Character Controller always btwn 1 - 1.75 meters.
        float headHeight = Mathf.Clamp(m_head.localPosition.y,1.0f,2.0f);
        characterController.height = headHeight;

        Vector3 newCenter = Vector3.zero;
        newCenter.y = characterController.height * 0.5f;
        newCenter.y += characterController.skinWidth;

        // recenter the character Controller Spherical Colloider.
        newCenter.x = m_head.localPosition.x;
        newCenter.z = m_head.localPosition.z;
        //newCenter = Quaternion.Euler(0,,0);

        characterController.center = newCenter;
    }
}
