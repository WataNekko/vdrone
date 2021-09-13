using UnityEngine;

[DisallowMultipleComponent]
public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private Transform _camera;
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private int _mouseSensitivity = 150;

    private bool _lockCursor;
    private float _rotationX;

    void Start()
    {
        _rotationX = _camera.localEulerAngles.x;
        LockCursor = true;
    }

    private void Update()
    {
        KeyboardMove();

        if (Input.GetKeyDown(KeyCode.R))
        {
            LockCursor = !_lockCursor;
        }
        if (_lockCursor)
        {
            MouseLook();
        }
    }

    // Supports locking and unlocking the cursor
    private bool LockCursor
    {
        set
        {
            Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !value;
            _lockCursor = value;
        }
    }

    // Controls player movement with the keyboard
    private void KeyboardMove()
    {
        var move = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Up Down"),
            Input.GetAxisRaw("Vertical")
            ) * (_speed * Time.deltaTime);

        transform.Translate(move);
    }

    // Controls player rotation with the mouse
    private void MouseLook()
    {
        var mouse = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
            ) * (_mouseSensitivity * Time.deltaTime);

        _rotationX = Mathf.Clamp(_rotationX - mouse.y, -90f, 90f);
        _camera.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);

        transform.Rotate(0f, mouse.x, 0f);
    }

#if UNITY_EDITOR
    private void Reset()
    {
        // Try to attach Camera reference on reset
        Camera camRef = null;

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<Camera>(out camRef))
            {
                break;
            }
        }

        if (camRef != null)
        {
            _camera = camRef.transform;
        }
        else
        {
            Debug.LogWarning($"{nameof(PlayerInput)} requires a {nameof(Camera)} child.");
        }
    }
#endif
}
