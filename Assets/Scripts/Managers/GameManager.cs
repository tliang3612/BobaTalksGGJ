using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GUIInputController InputController;
    private CinemachineVirtualCamera _camera;

    private void Awake()
    {
        _camera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    private void Start()
    {       
        _camera.m_Follow = FindObjectOfType<Player>().transform;      
    }

    public void Update()
    {
        if (InputController.RInputPressed)
        {
            InputController.UseRInput();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
