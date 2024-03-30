/*******************************************************************
* Author            : Max Schneider and u3d
* Copyright         : MIT License
* File Name         : Country.cs
* Description       : This file contains the data for the countires for the interaction of the earth.
*
/******************************************************************/
using UnityEngine;

public class Country : MonoBehaviour
{
    // Fields related to country information, provided by Max Schneider
    [SerializeField] public Color ColorCountry;
    [SerializeField] public string Name;
    [SerializeField] public string Population;
    [SerializeField] public string Size;
    [SerializeField] public string Language;
    [SerializeField] public string Capital;
    [SerializeField] public string Government;
    [SerializeField] public string GDP;
    [SerializeField] public string Currency;
    [SerializeField] public float Wealth;
    [SerializeField] public MeshRenderer meshRenderer;

    // Fields related to country attributes, provided by u3d 
    [SerializeField] public float science;
    [SerializeField] public float logistics;
    [SerializeField] public float disaster;
    [SerializeField] public string climate;

    // Indicating if the country is being hovered over
    private bool _Hovered;
    public bool Hovered
    {
        get => _Hovered;
        set
        {
            if (value == true)
                if (WorldMapManager.instance.CurrentState != WorldMapManager.State.Earth) gameObject.layer = 0;
                else
             if (WorldMapManager.instance.CurrentState != WorldMapManager.State.Earth) gameObject.layer = 4;
            _Hovered = value;
        }
    }
    void Start()
    {
        ChangeColor();
    }


    void ChangeColor()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        // create new colors array where the colors will be created.
        Color[] colors = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
            colors[i] = ColorCountry;

        // assign the array of colors to the Mesh.
        mesh.colors = colors;
    }
    // Update is called once per frame



}