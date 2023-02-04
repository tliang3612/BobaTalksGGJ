using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;

    private void Update()
    {
        transform.position = new Vector3(FindObjectOfType<Player>().transform.position.x + _offset.x, transform.position.y);
    }
}
