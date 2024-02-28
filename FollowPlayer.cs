using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public float cameraSpeed = 20f;

    void Update()
    {
        if (playerTransform != null)
        {
            Vector3 targetPosition = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, cameraSpeed * Time.deltaTime);
        }
    }

    public void ChangeCameraSpeed(float newSpeed)
    {
        cameraSpeed = newSpeed;
    }
}

