using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncestorSelectGUI : MonoBehaviour
{
    [SerializeField] private string _leftLevel;
    [SerializeField] private string _rightLevel;

    [SerializeField] private Dialogue _dialogue;

    private bool _isDialogueFinished;

    private void Start()
    {
        _isDialogueFinished = false;
        FindObjectOfType<DialogueManager>().DialogueExitedEvent += () => _isDialogueFinished = true;
        FindObjectOfType<DialogueManager>().OnEnterDialogue(_dialogue);
    }

    public void OnAncestorSelected(bool isLeft)
    {
        if(_isDialogueFinished)
            FindObjectOfType<LevelManager>().GoToLevel(isLeft ? _leftLevel : _rightLevel);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!_isDialogueFinished & !FindObjectOfType<GUIManager>().IsPaused)
                FindObjectOfType<DialogueManager>().OnEnterDialogue(_dialogue);
        }
    }


}
