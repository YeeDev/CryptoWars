using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CryptoWars.Movement;

public class Follower : MonoBehaviour
{
    //TODO Set this variable on awake, not in inspector
    [SerializeField] Transform player = null;

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.position;
    }
}
