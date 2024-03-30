/*******************************************************************
* Author            : Max Schneider
* Copyright         : MIT License
* File Name         : MeshData.cs
* Description       : This file contains the mesh for the simulation mode.
*
/******************************************************************/

using UnityEngine;

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;

    public MeshData(Vector3[] vertices, int[] triangles)
    {
        this.vertices = vertices;
        this.triangles = triangles;
    }
}

