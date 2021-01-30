using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Interaction", menuName = "Scriptable Objects/NPC Interactions")]
public class NPCInteractions : ScriptableObject
{
    [Header("Make sure to read tool tips")]
    [Tooltip("The dialogue text that the NPC says.")]
    public string text;
    [Tooltip("For a normal conversation, put up to four. If they want an item, put two (make sure to put the has the item dialogue in the first one). If it's just a monologue, or no replies, do just one.")]
    public NPCInteractions[] replies;
    [Tooltip("A series of options")]
    public string[] options;
    [Tooltip("A bool for finishing the interaction if there's no options available.")]
    public bool finishMonologue = false;
    [Tooltip("Does this NPC want anything?")]
    public string itemWanted = "";
    [Tooltip("For if the player has already talked to this NPC.")]
    public NPCInteractions secondPart;
    [Tooltip("Add this to the last module of the interaction to loop the same interaction. Don't enable this if you plan to have a second part")]
    public bool repeat;
    [Tooltip("Set this to true if you don't want the player to talk to this NPC anymore.")]
    public bool disableTheNPC;
}
