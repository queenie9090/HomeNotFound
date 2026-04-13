using UnityEngine;
using UnityEngine.InputSystem; 

public class ObjectiveManager : MonoBehaviour
{
    [Header("The Object to Display")]
    public GameObject targetObject;
    
    [Header("Money Display")]
    public MoneyVisualizer moneyVisualizer;

    [Header("Player Setup")]
    public Transform playerCamera; 

    [Header("Positioning Settings")]
    public float distanceFromCamera = 1.2f; 
    public float heightOffset = -0.2f;      

    [Header("VR Input")]
    public InputActionReference toggleButton;

    private void OnEnable()
    {
        if (toggleButton != null)
        {
            toggleButton.action.Enable();
            toggleButton.action.performed += ToggleObject;
        }
    }

    private void OnDisable()
    {
        if (toggleButton != null)
        {
            toggleButton.action.performed -= ToggleObject;
        }
    }

    private void ToggleObject(InputAction.CallbackContext context)
    {
        if (targetObject == null || playerCamera == null) return;

        bool isActive = !targetObject.activeSelf;
        targetObject.SetActive(isActive);

        if (isActive)
        {
            Vector3 forwardFlat = new Vector3(playerCamera.forward.x, 0, playerCamera.forward.z).normalized;
            Vector3 spawnPosition = playerCamera.position + (forwardFlat * distanceFromCamera);
            spawnPosition.y = playerCamera.position.y + heightOffset;

            targetObject.transform.position = spawnPosition;

            Vector3 targetLookPoint = playerCamera.position;
            targetLookPoint.y = targetObject.transform.position.y;
            targetObject.transform.LookAt(targetLookPoint);

     
            if (moneyVisualizer != null)
            {
                moneyVisualizer.RefreshDisplay();
            }
        }
    }
}