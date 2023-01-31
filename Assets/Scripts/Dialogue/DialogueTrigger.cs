using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private Dialogue _dialogueToTrigger;

    private DialogueManager _dialogueManager;
    private bool _dialogueFinished = false;

    private void Awake()
    {
        _dialogueManager = FindObjectOfType<DialogueManager>();       
    }

    private void Start()
    {
        _dialogueManager.DialogueExitedEvent += OnDialogueExited;
    }

    public void OnDialogueExited()
    {
        _dialogueFinished = true;
    }

    public void Interact()
    {
        _dialogueFinished = false;
        _dialogueManager.OnEnterDialogue(_dialogueToTrigger);
    }

    public bool CheckInteractFinished()
    {
        Debug.Log(_dialogueFinished);
        return _dialogueFinished;
    }
}
