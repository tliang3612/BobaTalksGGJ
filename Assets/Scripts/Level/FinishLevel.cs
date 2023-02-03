using UnityEngine;
using System.Collections;

public class FinishLevel : MonoBehaviour
{
	[SerializeField] private string _levelToGoTo;
	[SerializeField] private bool _hasCondition;

    private void Start()
    {
        if (_hasCondition)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.GetComponent<Player>() != null)
        {
			FindObjectOfType<LevelManager>().GoToLevel(_levelToGoTo);
		}
			
	}
}