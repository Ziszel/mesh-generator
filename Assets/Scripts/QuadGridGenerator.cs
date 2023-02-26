using UnityEngine;

/// <summary>
/// This script generates a grid mesh
/// - vertices
/// - triangles
/// - normals
/// </summary>
public class QuadGridGenerator : MonoBehaviour
{
    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;

    void Start()
    {
        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();   
    }

    public void GenerateQuadGrid(int xSize, int ySize)
    {
        
        // Vertices
        // Due to the way that Unity retrieves Mesh data properties, it is much more efficient
        // to set up data in your own array and then assign the array to a property (for example:
        // to Mesh.vertices or Mesh.normals), rather than access the property array via individual elements.
        // https://docs.unity3d.com/Manual/Example-CreatingaBillboardPlane.html
        // Set the array = to the size of the width and height of the terrain
        _vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        
        // Triangles
        // the number of triangles of a grid is ALWAYS:
        // the (number of vertices - 1) squared * 2 (each quad is two triangles) * 3 (each triangle is 3 vertices)
        _triangles = new int[(xSize - 1) * (ySize - 1) * 6]; // * 2 * 3 is equal to * 6
        int triangleIndex = 0;
        // loop over the array of vertices, setting the position of each vertex AND the triangles relating to it
        // https://catlikecoding.com/unity/tutorials/procedural-grid/
        for (int y = 0, i = 0; y < ySize; ++y)
        {
            for (int x = 0; x < xSize; ++x)
            {
                // Assign the vertex a position at the index of the array i
                _vertices[i] = new Vector3(x, y, 0.0f);

                // to calculate triangles in a grid, clockwise
                // i, i+xSize. i+1, i+1, i+xSize, i+size+1
                if (x != xSize - 1 && y != ySize - 1) // if x and y are not at the bounds of the grid
                {
                    // Triangles (must be set clockwise)
                    // Triangles make up 3D meshes. They are defined by specifying the indices of three vertices.
                    // first triangle: i, i+size+1. i+size
                    _triangles[triangleIndex] = i;
                    _triangles[triangleIndex + 1] = i + xSize; // grid means can't just add one, this goes up one level
                    _triangles[triangleIndex + 2] = i + 1;
                    
                    // second triangle: i, i+1, i+size+1
                    _triangles[triangleIndex + 3] = i + 1;
                    _triangles[triangleIndex + 4] = i + xSize;
                    _triangles[triangleIndex + 5] = i + xSize + 1;
                    triangleIndex += 6; // indices added = 6, so increment by 6 so nothing is overwritten
                }
                ++i;
            }
        }

        // Assign the vertices, triangles, normals, and uvs to the mesh component
        _mesh.Clear(); // ensure the mesh is actually empty
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.RecalculateNormals();
    }

    // Very useful for testing meshes
    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        if (_vertices == null)
        {
            return;
        }

        for (int i = 0; i < _vertices.Length; ++i)
        {
            Gizmos.DrawSphere(_vertices[i], 0.1f);
        }
    }*/
}
