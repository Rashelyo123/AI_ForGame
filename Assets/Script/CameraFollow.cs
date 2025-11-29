using UnityEngine;

public class CameraFollow_MouseInfluence : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public float mouseInfluence = 2f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 midpoint = (target.position + mousePos) / 2f;

        Vector3 desiredPos = Vector3.Lerp(target.position, midpoint, 0.3f * mouseInfluence);

        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);

        transform.position = new Vector3(smoothedPos.x, smoothedPos.y, -10f);
    }
}
