using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Dialogue/DialogueCharacter")]
public class DialogueCharacter : ScriptableObject
{
    //Also acts as a key to the characterDatabase dictionary
    public string CharacterName;
    public Sprite CharacterSprite;
    public AudioClip CharacterVoice;
}

