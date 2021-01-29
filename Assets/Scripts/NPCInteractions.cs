using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Interaction", menuName = "Scriptable Objects/NPC Interactions")]
public class NPCInteractions : ScriptableObject
{
    public string text;
    public NPCInteractions[] replies;
    public string[] options;
}
