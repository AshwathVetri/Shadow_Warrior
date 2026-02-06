using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float mouseSensitivity = 150f;
    public Vector3 offset = new Vector3(0, 2, -4);

    float yRotation;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;

        target.rotation = Quaternion.Euler(0f, yRotation, 0f);

        transform.position = target.position + target.rotation * offset;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
