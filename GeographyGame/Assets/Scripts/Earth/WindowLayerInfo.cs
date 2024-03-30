/*******************************************************************
* Author            : Max Schneider and u3d
* Copyright         : MIT License
* File Name         : WindowLayerInfo.cs
* Description       : This file contains the logic to display the information in the Explorer Mode information window.
*
/******************************************************************/

using System.Collections.Generic;
using UnityEngine;

public class WindowLayerInfo : MonoBehaviour
{
    public static WindowLayerInfo instance;

    // List to hold TextMeshProUGUI components for displaying country information information
    [SerializeField] public List<TMPro.TextMeshProUGUI> layersText;
    Country country;
    Vector2 uvCoords;

    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        // Get the currently hovered country and its UV coordinates
        country = WorldMapManager.instance.CurrentHoveredCountry;
        uvCoords = WorldMapManager.instance.HoveredEarthUVCoord;

        // Display information about the hovered country in the UI text elements
        layersText[0].text = country ? country.countryName : "No Country";

        layersText[1].text = country ? country.population : "No Population";

        layersText[2].text = country ? country.size : "No Size";

        layersText[3].text = country ? country.capital : "No Language";

        layersText[4].text = country ? country.language : "No Capital";

        layersText[5].text = country ? country.countryGdp : "No Government";

        layersText[6].text = country ? country.currency : "No Currency";

        layersText[7].text = country ? country.science.ToString() + "%" : "No Science";

        layersText[8].text = country ? country.logistics.ToString() + "%" : "No Transport";

        layersText[9].text = country ? country.disaster.ToString() + "%" : "No Disaster";

        layersText[10].text = country ? country.climate : "No Climate";

    }
}