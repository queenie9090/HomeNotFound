using UnityEngine;
using UnityEngine.InputSystem; // Required for reading VR buttons

public class ObjectiveManager : MonoBehaviour
{
    [Header("The Object to Display")]
    public GameObject targetObject;

    [Header("Player Setup")]
    public Transform playerCamera; // Drag your Main Camera here

    [Header("Positioning Settings")]
    public float distanceFromCamera = 1.2f; // How far in front to spawn it
    public float heightOffset = -0.2f;      // Spawns slightly below eye level so it doesn't block vision

    [Header("VR Input")]
    [Tooltip("Drag the Input Action Reference for the left controller button here")]
    public InputActionReference toggleButton;

    private void OnEnable()
    {
        // Subscribe to the button press event when the script is active
        if (toggleButton != null)
        {
            toggleButton.action.Enable();
            toggleButton.action.performed += ToggleObject;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe when destroyed to prevent memory leaks
        if (toggleButton != null)
        {
            toggleButton.action.performed -= ToggleObject;
        }
    }

    private void ToggleObject(InputAction.CallbackContext context)
    {
        if (targetObject == null || playerCamera == null) return;

        // 1. Flip the active state
        bool isActive = !targetObject.activeSelf;
        targetObject.SetActive(isActive);

        // 2. If we just turned it ON, teleport and rotate it
        if (isActive)
        {
            // Flatten the camera's forward direction so it spawns level
            Vector3 forwardFlat = new Vector3(playerCamera.forward.x, 0, playerCamera.forward.z).normalized;

            // Calculate the exact point in the air
            Vector3 spawnPosition = playerCamera.position + (forwardFlat * distanceFromCamera);
            spawnPosition.y = playerCamera.position.y + heightOffset;

            // Move the object
            targetObject.transform.position = spawnPosition;

            // --- THE FIX: MAKE IT FACE YOU ---
            // Tell the object to look directly at the player's head
            Vector3 targetLookPoint = playerCamera.position;

            // This ensures the object doesn't tilt up or down to look at your face
            targetLookPoint.y = targetObject.transform.position.y;

            // Force the object to face the target point
            targetObject.transform.LookAt(targetLookPoint);

            // [VR QUIRK WARNING]
            // If your object is a UI Canvas, LookAt() will make the text backwards/mirrored!
            // If you are using a Canvas, delete the LookAt line above and use this line instead:
            // targetObject.transform.forward = targetObject.transform.position - playerCamera.position;
        }
    }
}