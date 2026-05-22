using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float smoothSpeed = 0.125f;
    [SerializeField] Vector3 offset = new Vector3(0f, 2f, -10f);
    [SerializeField] bool clampX;
    [SerializeField] float minX, maxX;

    Transform target;

    void LateUpdate()
    {
        if (target == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) target = p.transform;
            return;
        }

        Vector3 desired = target.position + offset;
        if (clampX) desired.x = Mathf.Clamp(desired.x, minX, maxX);

        transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed);
    }
}
