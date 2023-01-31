using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogues/Dialogue Basic")]
public class Dialogue : ScriptableObject
{
    [Header("Dialogue information")]
    public DialogueLine[] Lines;

    [System.Serializable]
    public class DialogueLine
    {
        public DialogueCharacter CharacterSpeaking;

        [TextArea(4, 8)]
        public string LineText;
    }
}