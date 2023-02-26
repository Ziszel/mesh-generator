using UnityEngine;

/// This script was created using the following resource: https://www.youtube.com/watch?v=QN39W020LqU
public class SphereGenerator : MonoBehaviour
{
    [SerializeField, HideInInspector]
    private MeshFilter[] meshFilters;
    private readonly int _meshCount = 6; // this will ALWAYS be 6
    private Mesh[] _meshes;
    // Each side (face) of the cube/sphere will need to face a different direction, these can be looped through
    // using an array of directions
    private readonly Vector3[] _directions = {
        Vector3.up, 
        Vector3.down, 
        Vector3.left, 
        Vector3.right, 
        Vector3.forward, 
        Vector3.back
    };

    public void Initialise(int size)
    {
        // don't create a new MeshFilter if one already exists
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[_meshCount];
        }
        _meshes = new Mesh[_meshCount];

        for (int i = 0; i < _meshCount; ++i)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform; // keep hierarchy clean
                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            // add a new face to the face array, passing down size and direction
            // localUp will be set to the direction and allow the code in TerrainFace to construct a mesh from the 
            // appropriate angle
            _meshes[i] = ConstructMesh(_directions[i], meshFilters[i].sharedMesh, size);
        }
    }

    private Mesh ConstructMesh(Vector3 direction, Mesh mesh, int size)
    {
        // axis A represents localUp, but with each value shifted to the left. Therefore it can be seen as a complimentary vector axis
        Vector3 axisA = new Vector3(direction.y, direction.z, direction.x);
        // vector which is perpendicular to local up and axisA (cross product)
        Vector3 axisB = Vector3.Cross(direction, axisA);
        
        // a single face is a grid of n vertices (size * size)
        Vector3[] vertices = new Vector3[size * size];
        // the number of triangles of a grid is ALWAYS:
        // the (number of vertices - 1) squared * 2 (each quad is two triangles) * 3 (each triangle is 3 vertices)
        int[] triangles = new int[(size - 1) * (size - 1) * 6]; // * 2 * 3 is equal to * 6
        int triangleIndex = 0;

        for (int y = 0, i = 0; y < size; ++y)
        {
            // entire row of vertices
            for (int x = 0; x < size; ++x, ++i)
            {
                // returns a normalised value between 0 and 1 indicating how near to completion the loop is
                Vector2 percent = new Vector2(x, y) / (size - 1);
                // percent can now be used to generate a point between the three axis' 
                Vector3 pointOnUnitCube = direction + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB; 
                // this line makes the cube into a sphere
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = pointOnUnitSphere; // assign the vertex to the normalised value
                
                // to calculate triangles in a grid, clockwise
                // i, i+size+1. i+size, i, i+1, i+size+1
                if (x != size - 1 && y != size - 1) // if x and y are not at the bounds of the grid
                {
                    // first triangle: i, i+size+1. i+size
                    triangles[triangleIndex] = i;
                    triangles[triangleIndex + 1] = i + size + 1;
                    triangles[triangleIndex + 2] = i + size;
                    
                    // second triangle: i, i+1, i+size+1
                    triangles[triangleIndex + 3] = i;
                    triangles[triangleIndex + 4] = i + 1;
                    triangles[triangleIndex + 5] = i + size + 1;
                    triangleIndex += 6; // indices added = 6, so increment by 6 so nothing is overwritten
                }
            }
        }
        mesh.Clear(); // when updating the mesh with a lower resolution version, previous data can cause problems
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }
}
