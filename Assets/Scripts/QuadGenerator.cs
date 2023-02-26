using UnityEngine;

/// <summary>
/// This script generates a quad/plane by building a mesh out of:
/// - vertices
/// - triangles
/// - normals
/// - UVs
/// Size of the quad can be updated by manipulating width and height
/// </summary>
public class QuadGenerator : MonoBehaviour
{
    //[SerializeField] private int width;
    //[SerializeField] private int height;
    private Mesh _mesh;

    public void GenerateQuad(int width, int height)
    {
        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
        // Vertices
        // Due to the way that Unity retrieves Mesh data properties, it is much more efficient
        // to set up data in your own array and then assign the array to a property (for example:
        // to Mesh.vertices or Mesh.normals), rather than access the property array via individual elements.
        // https://docs.unity3d.com/Manual/Example-CreatingaBillboardPlane.html
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(width, 0, 0),
            new Vector3(0, height, 0),
            new Vector3(width, height, 0)
        };

        // Triangles (must be set clockwise)
        // Triangles make up 3D meshes. They are defined by specifying the indices of three vertices.
        int[] triangles = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        
        // Normals
        Vector3[] normals = new Vector3[4];
        for (int i = 0; i < normals.Length; ++i)
        {
            // the normals are set to the negative Z axis because the triangles are set clockwise
            // the camera will be in the negative Z axis but look toward the quad in the positive
            normals[i] = -Vector3.forward;
        }
        
        // UVs
        // UV coordinates are used to map a 2D texture to the surface of a 3D mesh, and should be a value of 0 or 1
        Vector2[] uvs = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        
        // Assign the vertices, triangles, normals, and uvs to the mesh component
        _mesh.Clear();
        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
        _mesh.normals = normals;
        _mesh.uv = uvs;
    }
}
