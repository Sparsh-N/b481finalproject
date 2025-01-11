using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class RoadOnSphere : MonoBehaviour {
    public float roadLatitude = 0f;
    public float roadLongitudeStart = -180f;
    public float roadLongitudeEnd = 180f;
    public float orbitRadius = 60f;
    public float orbitSpeed = 100f;
    public Vector3 orbitCenter = Vector3.zero;
    private MeshFilter meshFilter;
    private float currentAngle = 0f;

    [SerializeField]
    private bool isLightOn = true;


    // the light plate values will need modifications, so i rtference the changes here
    [SerializeField] private GameObject plate;
    [SerializeField] private GameObject cow;

    void Start() {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null) {
            return;
        }
        Mesh originalMesh = meshFilter.sharedMesh;
        if (originalMesh == null)
        {
            return;
        }
        Mesh deformedMesh = CreateRoadOnSphere(originalMesh);
        meshFilter.mesh = deformedMesh;
    }

    void Update() {
        currentAngle += orbitSpeed * Time.deltaTime;
        currentAngle = currentAngle % 360f;
        float x = orbitCenter.x + orbitRadius * Mathf.Cos(currentAngle * Mathf.Deg2Rad) * 50;
        float z = orbitCenter.z + orbitRadius * Mathf.Sin(currentAngle * Mathf.Deg2Rad) * 50;

        transform.position = new Vector3(x, transform.position.y, z);
        // float4 lightPosition = _LightPosition;
        // first  get plate renderer then set the val that is moved to.
        plate.GetComponent<Renderer>().material.SetVector("_LightPosition", transform.position);
        cow.GetComponent<Renderer>().material.SetVector("_LightPosition", transform.position);

        if (isLightOn == true) {
            plate.GetComponent<Renderer>().material.SetFloat("_LightStrength", 1.0f);
            cow.GetComponent<Renderer>().material.SetFloat("_LightStrength", 1.0f);
        } else {
            plate.GetComponent<Renderer>().material.SetFloat("_LightStrength", 0.0f);
            cow.GetComponent<Renderer>().material.SetFloat("_LightStrength", 0.0f);
        }
    }

    Mesh CreateRoadOnSphere(Mesh originalMesh) {
        Vector3[] vertices = originalMesh.vertices;
        int[] triangles = originalMesh.triangles;
        float radius = transform.localScale.x;
        for (int i = 0; i < vertices.Length; i++) {
            Vector3 vertex = vertices[i];
            float latitude = Mathf.Asin(vertex.y / radius) * Mathf.Rad2Deg;
            float longitude = Mathf.Atan2(vertex.z, vertex.x) * Mathf.Rad2Deg;
            if (Mathf.Abs(latitude - roadLatitude) < 0.1f)
            {
                if (longitude >= roadLongitudeStart && longitude <= roadLongitudeEnd)
                {
                    vertex += vertex.normalized * 0.1f;
                }
            }
            vertices[i] = vertex;
        }
        Mesh deformed = new Mesh();
        deformed.vertices = vertices;
        deformed.triangles = triangles;
        deformed.RecalculateNormals();
        deformed.RecalculateBounds();
        return deformed;
    }
}
