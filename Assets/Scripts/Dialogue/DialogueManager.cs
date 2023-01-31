using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    private readonly List<char> DELAYED_PUNCTUATION = new List<char>
    {
        '.',
        ',',
        '!',
        '?'
    };

    [Header("TypeWriter")]
    [SerializeField] private float _punctuationDelay;
    [SerializeField] private float _typeWriterDelay;

    [Header("UI Elements")]
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private Image _portrait;
    [SerializeField] private GameObject _continueIcon;

    private bool _isCurrentlyTyping;
    private string _completeText;

    private Queue<Dialogue.DialogueLine> _dialogueQueue;
    private Dialogue _currentDialogue;
    private bool _inDialogue;

    public Action DialogueExitedEvent;
    public Action<PowerupType> PowerupReceivedEvent;

    private void Awake()
    {
        _inDialogue = false;
        _dialoguePanel.SetActive(false);
        _dialogueQueue = new Queue<Dialogue.DialogueLine>();
    }


    public void OnEnterDialogue(Dialogue dialogue)
    {
        Debug.Log("Dialogue Entered");
        if (_inDialogue)
        {
            OnEnterNextLine();
            return;
        }
        else
        {
            _inDialogue = true;
        }

        ShowDialogueBox();
        EnqueueDialogue(dialogue);
    }

    /// <summary>
    /// Exits the current dialogue. Sets the dialogue panel to inactive, and clears dialogue box
    /// </summary>
    public void OnExitDialogue()
    {
        DialogueExitedEvent?.Invoke();
        ClearDialogueBox();
        HideDialogueBox();

        _inDialogue = false;

    }
    public void EnqueueDialogue(Dialogue dialogue)
    {
        _dialogueQueue.Clear();
        _currentDialogue = dialogue;

        foreach (var line in dialogue.Lines)
        {
            _dialogueQueue.Enqueue(line);
        }
        OnEnterNextLine();
    }

    //completes the line if typewriter coroutine is still going, and goes to next if not
    public void OnEnterNextLine()
    {
        if (_isCurrentlyTyping)
        {
            CompleteLine();
            StopAllCoroutines();
            _isCurrentlyTyping = false;
            return;
        }
        DequeueDialogue();
    }

    public void DequeueDialogue()
    {
        if (_dialogueQueue.Count == 0)
        {
            HandlePowerupDialogue();
            OnExitDialogue();
            return;
        }
        Debug.Log("Dialogue Dequeued");

        //Gets dialogue info of the dequeued item
        Dialogue.DialogueLine lineContents = _dialogueQueue.Dequeue();

        HandleLineContents(lineContents);
    }
    
    private void HandlePowerupDialogue()
    {
        if(_currentDialogue is PowerupDialogue)
            PowerupReceivedEvent?.Invoke((_currentDialogue as PowerupDialogue).PowerupToReceive);
    }

    private void HandleLineContents(Dialogue.DialogueLine lineInfo)
    {
        _dialogueText.text = "";
        _completeText = lineInfo.LineText;

        SetUIContents(lineInfo);

        //TODO
        //AudioManager.instance.PlayClip(voice); 

        StartCoroutine(TypeWriterEffect(_completeText));
    }

    private void SetUIContents(Dialogue.DialogueLine lineInfo)
    {
        _portrait.color = Color.white;
        _portrait.sprite = lineInfo.CharacterSpeaking.CharacterSprite;
        _nameText.text = lineInfo.CharacterSpeaking.name;
    }


    /// <summary>
    /// Clears out all information in the dialogue box
    /// </summary>
    private void ClearDialogueBox()
    {
        _nameText.text = "???";
        _portrait.sprite = null;
        _portrait.color = Color.clear; //make the portrait invisible
        _dialogueText.text = "";
    }

    /// <summary>
    /// Shows the DialogueBox UI
    /// </summary>
    public void ShowDialogueBox()
    {
        ClearDialogueBox();
        _dialoguePanel.SetActive(true);
    }

    /// <summary>
    /// Hides the DialogueBox UI
    /// </summary>
    public void HideDialogueBox()
    {
        ClearDialogueBox();
        _dialoguePanel.SetActive(false);
    }

    private IEnumerator TypeWriterEffect(string text)
    {
        _continueIcon.SetActive(false);

        _isCurrentlyTyping = true;
        _dialogueText.text = text;
        _dialogueText.maxVisibleCharacters = 0;

        foreach (char c in text.ToCharArray())
        {
            yield return new WaitForSeconds(_typeWriterDelay);
            _dialogueText.maxVisibleCharacters++;

            if (DELAYED_PUNCTUATION.Contains(c))
            {
                yield return new WaitForSeconds(_punctuationDelay);
            }
        }
        CompleteLine();
    }

    /// <summary>
    /// Completes the current line of dialogue
    /// </summary>
    public void CompleteLine()
    {
        _dialogueText.maxVisibleCharacters = _completeText.Length;
        _isCurrentlyTyping = false;
        _continueIcon.SetActive(true);
    }
}
