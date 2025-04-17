using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float heightOffset = 25;
    private bool followTarget = true;

    [Header("Components")]
    [SerializeField] private Transform target;

    private void LateUpdate()
    {
        if (followTarget)
            FollowTarget();
    }

    private void FollowTarget()
    {
        Vector3 newPosition = new Vector3(target.position.x, target.position.y, -heightOffset);
        transform.position = newPosition;
    }

    private void SetTarget(Transform target)
    {
        this.target = target;
    }
}
