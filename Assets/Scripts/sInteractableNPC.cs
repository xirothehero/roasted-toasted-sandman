using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class sInteractableNPC : MonoBehaviour
{

    // The object showing the symbol of what button to press.
    
    public GameObject interactionHUD;
    public float textSpeedInterval = 0.05f;
    public cButton_NPC_Interactions npcInteractionHolder;
    //public bool textAnim = true;
    public Transform cameraSpot;
    public GameObject exclamationPoint;
    public NPCInteractions startingModule;
    public bool repeat = true;
    public Text spaceToAdvanceText;
    public bool disableInteraction = false;

    public Transform destination;

    [HideInInspector] public NPCInteractions curModule;
    private bool isEnabled = false;
    private bool animOn = false;
    private NavMeshAgent npcAgent;
    //private IEnumerator coroutine;
    [HideInInspector] public bool interacting = false;
    private bool skipped = false;

    private void Start()
    {

        npcAgent = gameObject.GetComponent<NavMeshAgent>();
        curModule = startingModule;
    }

    // Initiate the conversation. 
    void InteractionInit()
    {
        if (curModule)
        {
            if (textSpeedInterval > 0)
            {
                StartCoroutine(TextAnim(npcInteractionHolder.dialogueText, curModule.text));
            }
        }
        else
        {
            Debug.LogError("No module attached to this NPC");
        }
    }

    // Goes to the next module of npc interacitons.
    public void GoToNextModule(NPCInteractions _curModule)
    {
        curModule = _curModule;

        if (textSpeedInterval > 0)
        {
            StartCoroutine(TextAnim(npcInteractionHolder.dialogueText, curModule.text));
        }
        else
        {
            Debug.LogError("Attempted to interact with this NPC but no options are int this module: " + _curModule.name);
        }
    }

    private void Update()
    {
        if (isEnabled && !interacting)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                npcInteractionHolder.holder.gameObject.SetActive(true);
                interactionHUD.SetActive(false);
                Gamemanager.instance.thePlayer.allowInput = false;
                Gamemanager.instance.mainCamera.destination = cameraSpot;
                Gamemanager.instance.mainCamera.lookAtTarget = npcInteractionHolder.transform;
                interacting = true;
                //npcInteractionHolder.curNPC = this;
                Gamemanager.instance.mainCamera.followPlayer = false;

                InteractionInit();
            }
        }

        if (animOn && interacting)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                skipped = true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !disableInteraction)
        {
            isEnabled = true;
            interactionHUD.SetActive(true);
            exclamationPoint.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isEnabled = false;
            interactionHUD.SetActive(false);
            if (!disableInteraction)
                exclamationPoint.SetActive(true);
        }
    }

    IEnumerator TextAnim(Text _textObj, string _text)
    {
        Debug.Log(_text);
        animOn = true;
        skipped = false;
        string holder = "";
        int idx = 0;
        while (holder != _text && !skipped)
        {
            yield return new WaitForSeconds(0.05f);
            holder += _text[idx];
            idx++;
            _textObj.text = holder;
        }
        //yield return new WaitForSeconds(0.05f);
        animOn = false;
        if (skipped)
        {
            _textObj.text = _text;
        }
        if (curModule.options.Length > 0)
        {
            npcInteractionHolder.EnableButtonsAndTexts(curModule.options);
            //npcInteractionHolder.SetText(curModule.options.Length, curModule.options);
        }
        else if (curModule.replies.Length > 0 || curModule.finishMonologue)
        {
            yield return new WaitForSeconds(1.5f);
            bool pressed = false;
            while (!pressed)
            {
                if (Input.GetKey(KeyCode.Space))
                    pressed = true;
                yield return new WaitForEndOfFrame();
            }
            // If we're not finished with the interaction
            if (!curModule.finishMonologue)
            {
                if (curModule.itemWanted != "")
                {
                    bool hasIt = false;
                    for (int i = 0; i < Gamemanager.instance.thePlayer.itemsCollected.Count; i++)
                    {
                        if (Gamemanager.instance.thePlayer.itemsCollected[i] == curModule.itemWanted)
                        {
                            hasIt = true;
                        }
                    }

                    if (hasIt)
                        GoToNextModule(curModule.replies[0]);
                    else if (curModule.replies.Length > 1)
                        GoToNextModule(curModule.replies[1]);
                    else
                        FinishInteraction();
                }
                else
                    GoToNextModule(curModule.replies[0]);
            }
            else
                FinishInteraction();
        }
        else
        {
            Debug.LogError("You need to add replies or set this module to finishMonologue via enabling the boolean.");
        }
    }

    public void FinishInteraction()
    {
        npcInteractionHolder.holder.SetActive(false);
        if (!disableInteraction)
            interactionHUD.SetActive(true);
        Gamemanager.instance.mainCamera.followPlayer = true;
        Gamemanager.instance.thePlayer.allowInput = true;
        interacting = false;

        if (curModule.disableTheNPC)
        {
            disableInteraction = true;
        }
        if (curModule.repeat)
        {
            curModule = startingModule;
        }
        else if (curModule.secondPart)
        {
            startingModule = curModule.secondPart;
            curModule = curModule.secondPart;
        }
    }
}
