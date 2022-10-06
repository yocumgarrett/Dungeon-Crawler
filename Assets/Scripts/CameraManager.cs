using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public bool follow_player;
    public string player_tag;
    public float smoothing;

    void LateUpdate()
    {
        if (follow_player)
            FollowPlayer();
    }

    public void FollowPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag(player_tag);
        if (player)
        {
            Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
    }
}
