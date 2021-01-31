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
    //public GameObject objectBlockingPath;

    [Tooltip("Make sure to put this on an NPC that is meant to block the path.")]
    public Transform blockingNPCDestination;
    public Image characterFrame;

    [HideInInspector] public NPCInteractions curModule;
    private bool isEnabled = false;
    private bool animOn = false;
    private bool gotItemWanted = false;
    private NavMeshAgent npcAgent;
    //private IEnumerator coroutine;
    [HideInInspector] public bool interacting = false;
    private bool skipped = false;
    private string itemWanted = "";
    private NPCInteractions gotItDialogue;

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
                if (gotItDialogue)
                {
                    //Debug.Log("Going to search");
                    CheckIfGotItem(gotItDialogue);
                }
                if (gotItemWanted)
                {
                    curModule = gotItDialogue.replies[0];
                }
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
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

        if (characterFrame)
            characterFrame.sprite = curModule.characterTalkingSprite;
        else
        {
            Debug.LogWarning("There currently isn't a characterFrame for: " + curModule.name);
        }
        animOn = true;
        skipped = false;
        string currentText = "";
        //string holder = "";
        string[] splitMessage = _text.Split(' ');
        string previousWord = splitMessage[0]; // Save previous word with no style            
        _textObj.text = "<i><color=yellow>" + splitMessage[0] + "</color></i>";
        int indexWord = 1;
        //int idx = 0;
        while (indexWord < splitMessage.Length && !skipped)
        {
            yield return new WaitForSeconds(0.05f);
            currentText += previousWord + " ";
            _textObj.text = currentText + "<i><color=yellow>" + splitMessage[indexWord] + "</color></i>";

            // Save previous word with no style and add 1
            previousWord = splitMessage[indexWord];
            indexWord += 1;
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
                    //wantsSomething = true;
                    //bool hasIt = false;
                    CheckIfGotItem(curModule);

                    gotItDialogue = curModule.replies[0];
                    Debug.Log(gotItDialogue);
                    if (gotItemWanted)
                    {
                        GoToNextModule(gotItDialogue);
                    }
                    else if (curModule.replies.Length > 1)
                    {
                        //itemWanted = curModule.itemWanted;
                        GoToNextModule(curModule.replies[1]);
                    }
                    else
                    {
                        gotItDialogue = curModule;
                        startingModule = curModule.secondPart;
                        FinishInteraction();
                    }

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

    void CheckIfGotItem(NPCInteractions theModule)
    {
        Debug.Log(theModule.itemWanted);
        for (int i = 0; i < Gamemanager.instance.thePlayer.itemsCollected.Count; i++)
        {
            //Debug.Log(Gamemanager.instance.thePlayer.itemsCollected[i]);
            if (Gamemanager.instance.thePlayer.itemsCollected[i] == theModule.itemWanted)
            {
                gotItemWanted = true;
                //Debug.Log("Does have item");
            }
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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (gotItemWanted)
        {
            if (blockingNPCDestination)
                npcAgent.destination = blockingNPCDestination.position;
            //Destroy(objectBlockingPath);
        }

        if (curModule.disableTheNPC)
        {
            disableInteraction = true;
            npcAgent.destination = blockingNPCDestination.position;
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
