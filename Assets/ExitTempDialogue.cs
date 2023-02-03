using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTempDialogue : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && FindObjectOfType<GUIManager>() != null)
        {
            FindObjectOfType<GUIManager>().EndTempDialogue();
            GetComponent<Collider2D>().enabled = false;
        }
    }

}
