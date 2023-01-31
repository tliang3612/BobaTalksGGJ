using UnityEngine;
using System.Collections;

public class FinishLevel : MonoBehaviour
{
	[SerializeField] private string _levelToGoTo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.GetComponent<Player>() != null)
			GoToNextLevel();
    }

    public virtual void GoToNextLevel()
	{
		FindObjectOfType<LevelManager>().GoToLevel(_levelToGoTo);
	}
}