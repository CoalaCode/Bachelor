/*******************************************************************
* Author            : Max Schneider and Sebastian Lague
* Copyright         : MIT License
* File Name         : WorldMapManager.cs
* Description       : This file contains the logic for the generation of the earth mesh for the Simulation Mode.
*
/******************************************************************/

using UnityEngine;
public class SphereGenerator : MonoBehaviour
{
    public int resolution = 5; // Resolution for each face of the cube

    private void OnValidate()
    {
        Initialize();
    }

    void Initialize()
    {
        // Generate mesh data for the cube faces
        MeshData[] cubeFaces = GenerateFaces(resolution);

        // Combine cube face mesh data into a single mesh
        Mesh combinedMesh = CombineMeshes(cubeFaces);

        // Assign combined mesh to a GameObject
        //gameObject.AddComponent<MeshFilter>().sharedMesh = combinedMesh;
        //gameObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));

    }

    MeshData[] GenerateFaces(int resolution)
    {
        MeshData[] allMeshData = new MeshData[6];
        Vector3[] faceNormals =
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };

        for (int i = 0; i < faceNormals.Length; i++)
        {
            allMeshData[i] = CreateFace(faceNormals[i], resolution);
        }

        return allMeshData;
    }

    MeshData CreateFace(Vector3 normal, int resolution)
    {
        Vector3 axisA = new Vector3(normal.y, normal.z, normal.x);
        Vector3 axisB = Vector3.Cross(normal, axisA);
        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int vertexIndex = x + y * resolution;
                Vector2 t = new Vector2(x, y) / (resolution - 1f);
                Vector3 point = normal + axisA * (2 * t.x - 1) + axisB * (2 * t.y - 1);
                Vector3 pointOnSphere = PointOnCubeToPointOnSphere(point);
                vertices[vertexIndex] = pointOnSphere;

                if (x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triIndex + 0] = vertexIndex;
                    triangles[triIndex + 1] = vertexIndex + resolution + 1;
                    triangles[triIndex + 2] = vertexIndex + resolution;
                    triangles[triIndex + 3] = vertexIndex;
                    triangles[triIndex + 4] = vertexIndex + 1;
                    triangles[triIndex + 5] = vertexIndex + resolution + 1;
                    triIndex += 6;
                }
            }
        }
        return new MeshData(vertices, triangles);
    }

    //Turns the cube into a sphere 
    public static Vector3 PointOnCubeToPointOnSphere(Vector3 p)
    {
        float x2 = p.x * p.x;
        float y2 = p.y * p.y;
        float z2 = p.z * p.z;
        float x = p.x * Mathf.Sqrt(1 - (y2 + z2) / 2 + (y2 * z2) / 3);
        float y = p.y * Mathf.Sqrt(1 - (z2 + x2) / 2 + (z2 * x2) / 3);
        float z = p.z * Mathf.Sqrt(1 - (x2 + y2) / 2 + (x2 * y2) / 3);
        return new Vector3(x, y, z);
    }

    Mesh CombineMeshes(MeshData[] meshDataArray)
    {
        CombineInstance[] combineInstances = new CombineInstance[meshDataArray.Length];

        for (int i = 0; i < meshDataArray.Length; i++)
        {
            Mesh mesh = new Mesh();
            mesh.vertices = meshDataArray[i].vertices;
            mesh.triangles = meshDataArray[i].triangles;
            combineInstances[i].mesh = mesh;
            combineInstances[i].transform = Matrix4x4.identity;
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combineInstances, true, false);
        Debug.Log("Number of vertices: " + combinedMesh.vertexCount);
        return combinedMesh;
    }
}
