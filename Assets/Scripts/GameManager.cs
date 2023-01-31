using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GUIInputController InputController;

    public void Update()
    {
        if (InputController.RInputPressed)
        {
            InputController.UseRInput();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
            
    }
}
