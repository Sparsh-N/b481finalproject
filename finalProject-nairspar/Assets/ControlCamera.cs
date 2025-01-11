using UnityEngine;
using UnityEngine.UI;

public class ControlCamera : MonoBehaviour
{
    [SerializeField] private Button resetButton;

    [Header("Camera Control")]
    [SerializeField] private float angleValue = 0f;
    [SerializeField] private bool isTopDownView = true;

    // cam posns
    private Vector3 targetPosition = new Vector3(140f, 890f, 25f);
    private Vector3 centerViewPosition = new Vector3(0f, 225f, 25f);
    private float topDownXRotation = 90f;
    private float centerViewXRotation = 30f;

    void Start() {
        SetInitialCameraPositionAndRotation();
        if (resetButton != null) {
            resetButton.onClick.AddListener(ResetCameraPosition);
        }
    }
    private void SetInitialCameraPositionAndRotation()
    {
        Camera.main.transform.position = targetPosition;
        Camera.main.transform.rotation = Quaternion.Euler(topDownXRotation, 0f, 0f);
    }
    void Update()  {
        UpdateCameraPositionAndRotation();
    }
    private void ResetCameraPosition(){
        isTopDownView = true;
        Camera.main.transform.position = targetPosition;
        Camera.main.transform.rotation = Quaternion.Euler(topDownXRotation, 0f, 0f);
    }
    private void UpdateCameraPositionAndRotation() {
        Vector3 targetPositionToUse = isTopDownView ? targetPosition : centerViewPosition;
        Camera.main.transform.position = targetPositionToUse;
        float xRotation = isTopDownView ? topDownXRotation : centerViewXRotation;
        Camera.main.transform.rotation = Quaternion.Euler(xRotation, angleValue * 360f, 0f);
    }
}
