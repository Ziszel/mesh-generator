using UnityEngine;


/// <summary>
/// This script generates a cube by building a mesh out of:
/// - vertices
/// - triangles
/// - normals
/// - UVs
/// The size of the cube on each axis (x, y, z), can be determined using the public variables: width, height, and depth
/// in the inspector
/// </summary>
public class CubeGenerator : MonoBehaviour
{
    public float width = 1.0f;
    public float height = 1.0f;
    public float depth = 1.0f;
    private Mesh _mesh;
    void Start()
    {
        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
        GenerateCube();
    }

    private void GenerateCube()
    {
        // Vertices
        // There are 8 vertices represented by Vector3 objects.
        // https://docs.unity3d.com/Manual/Example-CreatingaBillboardPlane.html
        Vector3[] vertices = new Vector3[8]
        {
            // these locations correspond directly to the triangles below
            new Vector3(-width, -height, -depth), // left bottom front
            new Vector3(-width, -height, depth), // left bottom back
            new Vector3(-width, height, -depth), // left top front
            new Vector3(-width, height, depth), // left top back
            new Vector3(width, -height, -depth), // right bottom front
            new Vector3(width, -height, depth), // right bottom back
            new Vector3(width, height, -depth), // right top front
            new Vector3(width, height, depth) // right top back
        };
        

        // Triangles
        // Triangles make up 3D meshes. They are defined by specifying the indices of three vertices.
        // There are 36 triangles that make up this cube.
        // https://catlikecoding.com/unity/tutorials/procedural-grid/
        int[] triangles = new int[36]
        {
            // The numbers in the triangles array define the indices of the vertices that make up each triangle.
            // triangles therefore define the connections between the vertices that make up the surface of a mesh
            // the order of these vertices defines which side of the mesh will face to, or away from, you
            // Front face quad
            0, 1, 2,    // first triangle
            1, 3, 2,    // second triangle

            // Right face quad
            1, 5, 3,    // first triangle
            5, 7, 3,    // second triangle

            // Back face quad
            5, 4, 7,    // first triangle
            4, 6, 7,    // second triangle

            // Left face quad
            4, 0, 6,    // first triangle
            0, 2, 6,    // second triangle

            // Top face quad
            4, 5, 0,    // first triangle
            5, 1, 0,    // second triangle

            // Bottom face quad
            2, 3, 6,    // first triangle
            3, 7, 6     // second triangle
        };

        // Normals
        // Normals are vectors that specify the direction that a surface is facing.
        // In this script, all of the normals are set to face in the negative z direction.
        Vector3[] normals = new Vector3[8];
        for (int i = 0; i < normals.Length; i++)
        {
            // the normals are set to the negative Z direction because the triangles are set counterclockwise
            normals[i] = -Vector3.forward;
        }
        
        // UVs
        // UV coordinates are used to map a 2D texture to the surface of a 3D mesh, and should be a value of 0 or 1
        // for a cube, this should describe how a texture should be a mapped to each of the six faces of the cube.
        Vector2[] uvs = new Vector2[8]
        {
            // this ensures the entire texture will be mapped to each face of the cube
            // a UV map is typically used to apply a complex texture to a complex shape
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
        };

        // Assign the vertices, triangles, normals, and uvs to the mesh component
        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
        _mesh.normals = normals;
        _mesh.uv = uvs;
    }

    private void SubdivideMesh()
    {
        
    }
}