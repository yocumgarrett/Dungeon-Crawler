using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public bool follow_player;
    public string player_tag;
    public float smoothing;


    private void OnEnable()
    {
        PlayerAttack.OnAttack += ShakeCamera;
    }

    private void OnDisable()
    {
        PlayerAttack.OnAttack -= ShakeCamera;
    }

    private void Start()
    {
        
    }

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
            //transform.position = player.transform.position + new Vector3(0, 0, -10f);
            Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
    }

    public void ShakeCamera()
    {

        StartCoroutine(Shake(0.1f, 0.015f));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while(elapsed < duration)
        {
            float shakeX = Random.Range(-1f, 1f) * magnitude;
            float shakeY = Random.Range(-1f, 1f) * magnitude;
            transform.position = new Vector3(transform.position.x + shakeX, transform.position.y + shakeY, transform.position.z);

            elapsed += Time.deltaTime;
            yield return 0;
        }
    }
}
