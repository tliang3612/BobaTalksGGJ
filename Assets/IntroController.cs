using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    [SerializeField] private Dialogue _dialogue;
    [SerializeField] private Animator _anim;

    private bool _isDialogueFinished;
    

    private void Start()
    {
        _isDialogueFinished = false;
        FindObjectOfType<DialogueManager>().DialogueExitedEvent += OnDialogueFinished;
        FindObjectOfType<DialogueManager>().OnEnterDialogue(_dialogue);
    }

    public void OnDialogueFinished()
    {
        _isDialogueFinished = true;
        StartCoroutine(StartAncestorPopUp());
    }

    public IEnumerator StartAncestorPopUp()
    {
        _anim.SetTrigger("PopUp");
        yield return new WaitForSeconds(6f);
        FindObjectOfType<LevelManager>().GoToLevel("Level Tutorial");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_isDialogueFinished & !FindObjectOfType<GUIManager>().IsPaused)
                FindObjectOfType<DialogueManager>().OnEnterDialogue(_dialogue);
        }
    }


}
