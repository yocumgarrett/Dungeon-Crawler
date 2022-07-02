using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public bool follow_player;
    public string player_tag;



    private void OnEnable()
    {
        PlayerAttack.OnAttack += ShakeCamera;
    }

    private void OnDisable()
    {
        PlayerAttack.OnAttack -= ShakeCamera;
    }

    void Update()
    {
        if (follow_player)
            FollowPlayer();
    }

    public void FollowPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag(player_tag);
        if (player)
        {
            transform.position = player.transform.position + new Vector3(0, 0, -10f);
        }
    }

    public void ShakeCamera()
    {

        StartCoroutine(Shake(0.15f, 0.02f));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0f;

        while(elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.position = originalPosition + new Vector3(x, y, -10f);

            elapsed += Time.deltaTime;
            yield return 0;
        }
        transform.position = originalPosition;
    }
}
