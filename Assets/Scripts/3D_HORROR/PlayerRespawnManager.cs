using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnManager : MonoBehaviour
{
    public Transform playerObject;
    public Transform respawnPosition;

    public void ReEnablePlayer()
    {
        playerObject.gameObject.SetActive(true);
    }

    public void RePositionPlayer()
    {
        playerObject.position = respawnPosition.position;
        playerObject.rotation = respawnPosition.rotation;
    }
}
