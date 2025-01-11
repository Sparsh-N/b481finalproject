using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ChladniPattern : MonoBehaviour {
    // Variables
    // 1: my a-b variable slider
    [Range(-10, 10)]
    [SerializeField] private float a = 1f;
    [Range(-10, 10)]
    [SerializeField] private float b = 1f;
    [Range(-10, 10)]
    [SerializeField] private float n = 1f;
    [Range(-10, 10)]
    [SerializeField] private float m = 0.3f;
    [SerializeField] public bool rollingBallSelected = true;
    [SerializeField] private Button PlateRotation, PlatePattern;
    private Vector3 previousMousePosition;
    //2 rolling ball
    private Camera mainCamera;
    private Vector2 myMouseStartPosition;
    private Vector3 myObjectStartPosition;
    public float rollingBallRadius = 100.0f; // i made this much bigger since the underlying object/mesh is massive

    //main part
    void Start() {
        previousMousePosition = Input.mousePosition;
        PlateRotation.onClick.AddListener(OnRotationSelected);
        PlatePattern.onClick.AddListener(OnPatternSelected);
        mainCamera = Camera.main;
    }

    void Update() {
        if (rollingBallSelected) {
            RollingBall();
        } else {
            HandleMouseDrag();
            UpdateMaterial();
        }
    }

    private void OnRotationSelected() { rollingBallSelected = true; }
    private void OnPatternSelected() { rollingBallSelected = false; }

    // This is where i put my Chladni user interaction with plate
    // option 1 - user drags and makes the a, b vals of my script change
    void HandleMouseDrag() {
        if (Input.GetMouseButton(0)) {
            Vector3 mouseDelta = Input.mousePosition - previousMousePosition;
            a += mouseDelta.y * 0.01f;
            b += mouseDelta.x * 0.01f;
            a = Mathf.Clamp(a, -10f, 10f);
            b = Mathf.Clamp(b, -10f, 10f);
            previousMousePosition = Input.mousePosition;
        }
    }
    void UpdateMaterial() {
        GetComponent<Renderer>().material.SetFloat("_A", a);
        GetComponent<Renderer>().material.SetFloat("_B", b);
        GetComponent<Renderer>().material.SetFloat("_N", n);
        GetComponent<Renderer>().material.SetFloat("_M", m);
    }

    // opn 2 - rolling ball algorithm on the surface taken from lab9
    private void RollingBall() {
    if (Input.GetMouseButtonDown(0)) {
        myMouseStartPosition = Input.mousePosition;
        myObjectStartPosition = transform.position;
    }
    if (Input.GetMouseButton(0)) {
        Vector2 currentMousePosition = Input.mousePosition;
        Vector2 deltaMousePosition = currentMousePosition - myMouseStartPosition;
        float dr = deltaMousePosition.magnitude;
        if (dr == 0) return;
        Vector3 rotationAxis = new Vector3(-deltaMousePosition.y / dr, deltaMousePosition.x / dr, 0);
        rotationAxis.Normalize();
        float angle = dr / rollingBallRadius;
        transform.RotateAround(myObjectStartPosition, rotationAxis, angle * Mathf.Rad2Deg);
        myMouseStartPosition = currentMousePosition;
        }
    }
}
