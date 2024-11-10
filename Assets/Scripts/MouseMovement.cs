using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100.0f;
    private Vector2 rotation = Vector2.zero;

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        UpdateMouseRotation();
        ApplyRotation();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UpdateMouseRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        rotation.x = Mathf.Clamp(rotation.x - mouseY, -90.0f, 90.0f);
        rotation.y += mouseX;
    }

    private void ApplyRotation()
    {
        transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, 0.0f);
    }
}
