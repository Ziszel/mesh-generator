using UnityEngine;

/// <summary>
/// This script generates a cube by building a mesh out of:
/// - vertices
/// - triangles
/// - normals
/// - UVs
/// The size of the cube on each axis (x, y, z), can be determined using the public variables: width, height, and depth
/// in the inspector (this will later be updated to be done 'in app')
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
        // Does not produce desired results
        //SubdivideMesh();
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

    // Use Catmull-Clark subdivision to subdivide a polygonal mesh
    private void SubdivideMesh()
    {
        // Add face point for each face -> set it to be the average of all original points
        // https://forum.unity.com/threads/determining-average-of-a-vector3-array.458195/
        Vector3[] facePoints = new Vector3[_mesh.vertices.Length];
        for (int i = 0; i < _mesh.vertices.Length; ++i)
        {
            Vector3 vert1 = new Vector3(_mesh.vertices[i].x + width, _mesh.vertices[i].y, _mesh.vertices[i].z);
            Vector3 vert2 = new Vector3(_mesh.vertices[i].x, _mesh.vertices[i].y + height, _mesh.vertices[i].z);
            Vector3 vert3 = new Vector3(_mesh.vertices[i].x + width, _mesh.vertices[i].y + height, _mesh.vertices[i].z);
            facePoints[i].x = (
                vert1.x
                + vert2.x
                + vert3.x) / 3;
            facePoints[i].y = (
                vert1.y
                + vert2.y
                + vert3.y) / 3;
            facePoints[i].z = (
                vert1.z
                + vert2.z
                + vert3.z) / 3;
        }
        
        // Add edge point for each edge -> set it to the average of the two neighbouring face points (AF)
        // and the midpoint of the edge (ME)
        // (AF + ME) / 2
        // https://www.calculatorsoup.com/calculators/geometry-plane/midpoint-calculator.php
        Vector3[] edgePoints = new Vector3[_mesh.vertices.Length];
        for (int i = 0; i < _mesh.vertices.Length - 1; ++i)
        {
            float averageEdgeX = (facePoints[i].x + facePoints[i + 1].x) / 2;
            float averageEdgeY = (facePoints[i].y + facePoints[i + 1].y) / 2;
            float averageEdgeZ = (facePoints[i].z + facePoints[i + 1].z) / 2;
            Vector3 pointOne = _mesh.vertices[i];
            Vector3 pointTwo = _mesh.vertices[i + 1];
            Vector3 edgeMidpoint = new Vector3(
                (pointOne.x + pointTwo.x) / 2,
                (pointOne.y + pointTwo.y) / 2,
                (pointOne.z + pointTwo.z) / 2
                );
            edgePoints[i] = new Vector3(
                (averageEdgeX + edgeMidpoint.x) / 2,
                (averageEdgeY + edgeMidpoint.y) / 2,
                (averageEdgeZ + edgeMidpoint.z) / 2);
        }

        // For each vertex (P) -> take average (F) of all n (face points), for faces touching P
        // take the average (R) of all n (edge midpoints) for edge touching P
        // F + 2R + (n - 3)P / n
        Vector3[] newVertices = new Vector3[_mesh.vertices.Length];
        for (int i = 0; i < facePoints.Length; ++i)
        {
            newVertices[i] = new Vector3(
                 facePoints[i].x + edgePoints[i].x + (facePoints.Length - 3) * _mesh.vertices[i].x * facePoints.Length,
                 facePoints[i].y + edgePoints[i].x + (facePoints.Length - 3) * _mesh.vertices[i].y * facePoints.Length,
                 facePoints[i].z + edgePoints[i].x + (facePoints.Length - 3) * _mesh.vertices[i].z * facePoints.Length);
            Debug.Log(newVertices[i]);
        }
        
        // reapply triangles
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
        
        _mesh.Clear();
        _mesh.vertices = newVertices;
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();
    }
    
    // Very useful for testing meshes
    /* private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        if (_mesh.vertices == null)
        {
            return;
        }

        for (int i = 0; i < _mesh.vertices.Length; ++i)
        {
            Gizmos.DrawSphere(_mesh.vertices[i], 0.1f);
        }
    } */
}