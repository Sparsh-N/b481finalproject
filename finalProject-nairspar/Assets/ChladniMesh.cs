using UnityEngine;

public class ChladniMesh : MonoBehaviour
{
    public int gridSize = 25;
    public float gridSpacing = 1.0f;
    [SerializeField]
    public float displacementFactor = 15.0f;
    private Mesh generatedMesh;
    private MeshCollider meshCollider;

    private void Awake() {
        meshCollider = GetComponent<MeshCollider>();
        GetComponent<Renderer>().material.SetFloat("_DisplacementFactor", displacementFactor);
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null) {
            generatedMesh = CreateGridMesh(gridSize, gridSpacing);
            meshFilter.mesh = generatedMesh;
        }
        if (meshCollider != null) {
            meshCollider.sharedMesh = generatedMesh;
        }
    }

    private void Update() {
        UpdateMeshVertices();
    }

    private void UpdateMeshVertices() {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null) {
            Vector3[] vertices = meshFilter.mesh.vertices;
            // for (int i = 0; i < vertices.Length; i++) {
            //     vertices[i].y += Mathf.Sin(vertices[i].x * 0.1f) * displacementFactor;
            // }
            meshFilter.mesh.vertices = vertices;
            meshFilter.mesh.RecalculateNormals();
        }
    }

    private Mesh CreateGridMesh(int size, float spacing)
    {
        Mesh mesh = new Mesh();

        int vertexCount = size * size;
        int quadCount = (size - 1) * (size - 1);

        Vector3[] vertices = new Vector3[vertexCount];
        Vector2[] uvs = new Vector2[vertexCount];

        int[] triangles = new int[quadCount * 6];

        int index = 0;
        for (int z = 0; z < size; z++) {
            for (int x = 0; x < size; x++) {
                float posX = (x - (size / 2)) * spacing;
                float posZ = (z - (size / 2)) * spacing;
                vertices[index] = new Vector3(posX, 0, posZ);
                uvs[index] = new Vector2((float)x / (size - 1), (float)z / (size - 1));
                index++;
            }
        }

        int triIndex = 0;
        for (int z = 0; z < size - 1; z++) {
            for (int x = 0; x < size - 1; x++) {
                int topLeft = z * size + x;
                int topRight = topLeft + 1;
                int bottomLeft = (z + 1) * size + x;
                int bottomRight = bottomLeft + 1;
                triangles[triIndex++] = topLeft;
                triangles[triIndex++] = bottomLeft;
                triangles[triIndex++] = topRight;
                triangles[triIndex++] = topRight;
                triangles[triIndex++] = bottomLeft;
                triangles[triIndex++] = bottomRight;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        return mesh;
    }
}
