using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowLayerInfo : MonoBehaviour
{
    public static WindowLayerInfo instance;

    [SerializeField] public List<TMPro.TextMeshProUGUI> layersText;
    Country country;
    Vector2 uvCoords;

    private float maxWealth = float.MaxValue;
    void Awake()
    {
        instance = this;

        List<Country> countries = WorldMapManager.instance.countries;
        float maxWealth = 0;
        for (int i = 0; i < countries.Count; i++)
        {
            Country country = countries[i];
            if (country.Wealth > maxWealth) maxWealth = country.Wealth;
        }
        this.maxWealth = maxWealth;
    }

    private void Update()
    {

        country = WorldMapManager.instance.CurrentHoveredCountry;
        uvCoords = WorldMapManager.instance.HoveredEarthUVCoord;

        layersText[0].text = country ? country.Name : "No Country";

        layersText[1].text = country ? country.Population : "No Population";

        layersText[2].text = country ? country.Size : "No Size";

        layersText[3].text = country ? country.Capital : "No Language";

        layersText[4].text = country ? country.Language : "No Capital";

        layersText[5].text = country ? country.GDP : "No Government";

        layersText[6].text = country ? country.Currency : "No Currency";

        layersText[7].text = country ? country.science.ToString() + "%" : "No Science";

        layersText[8].text = country ? country.logistics.ToString() + "%" : "No Transport";

        layersText[9].text = country ? country.disaster.ToString() + "%" : "No Disaster";

        layersText[10].text = country ? country.climate : "No Climate";

        //layersText[8].text = country ? country.Wealth + "$" : "N/A";

        /*
        layersText[7].text = WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.WorldLayersTextures[3], uvCoords).ToString() + "%";

        layersText[8].text = WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.WorldLayersTextures[4], uvCoords).ToString() + "%";

        layersText[9].text = WorldMapManager.instance.GetPercentByTexture(WorldMapManager.instance.WorldLayersTextures[5], uvCoords).ToString() + "%";

        layersText[10].text = WorldMapManager.instance.ClimatZonesNames[WorldMapManager.instance.GetZone(WorldMapManager.instance.WorldLayersTextures[6], uvCoords)];
        */

    }
}
