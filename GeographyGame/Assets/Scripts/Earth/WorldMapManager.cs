/*******************************************************************
* Author            : Max Schneider and u3d
* Copyright         : MIT License
* File Name         : WorldMapManager.cs
* Description       : This file contains the logic for the interaction of the earth.
*
******************************************************************/
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class WorldMapManager : MonoBehaviour
{
    #region Variables
    // References to  game objects and components
    [SerializeField] MeshRenderer EarthRenderer;
    [SerializeField] GameObject Clouds;
    [SerializeField] GameObject Glow;
    [SerializeField] public List<Country> countries;
    [SerializeField] public Material Earth;
    [SerializeField] public Material Climate;
    [SerializeField] public Material Disaster;

    // Materials added by Max Schneider
    [SerializeField] public Material EarthAugust;
    [SerializeField] public Material EarthJanuary;
    [SerializeField] public Material EarthDetails;
    [SerializeField] public Material EarthBorders;
    [SerializeField] public Material EarthNight;
    [SerializeField] public Material OceanFlow;

    [SerializeField] public List<Texture2D> WorldLayersTextures;

    [Header("Use it for different zones on ClimateTexture")] [SerializeField] public List<Color> ClimateZonesColors;
    [SerializeField] public List<string> ClimateZonesNames;

    //JSON Files created and added by Max Schneider
    [Header("JSON Files")]
    [SerializeField] public TextAsset CountryNamesJSONFile;
    [SerializeField] public TextAsset CountryCapitalJSONFile;
    [SerializeField] public TextAsset CountryPopulationJSONFile;
    [SerializeField] public TextAsset CountrySizeJSONFile;
    [SerializeField] public TextAsset CountryLanguageJSONFile;
    [SerializeField] public TextAsset CountryCurrencyJSONFile;
    [SerializeField] public TextAsset CountryGDPJSONFile;

    [Header("Use this file with void SetPopulationAndWealth()")]
    [SerializeField] public List<Material> EarthMaterialsByTypeOnCountries;

    [Header("Prefab for Select Point on Earth")]
    [SerializeField] GameObject UnitPoint;
    [SerializeField] GameObject earthPlanet;

    // RayInteractor of XR-Controller 
    [SerializeField] XRRayInteractor rayInteractor;

    private bool triggerPressed;
    private bool buttonXPressed;
    private bool buttonYPressed;
    private float rotationSpeed = 40;
    private Vector3 scaleFactor = new Vector3(0.01f, 0.01f, 0.01f);

    public Vector2 HoveredEarthUVCoord;
    public Vector2 SelectedEarthUVCoord;

    public Country CurrentHoveredCountry;
    public Country CurrentSelectedCountryInfo;
    private Country _currentSelectedCountry;

    // Get and set current selected country
    public Country CurrentSelectedCountry
    {
        get => _currentSelectedCountry;
        set
        {
            if (_currentSelectedCountry != null)
            {
                _currentSelectedCountry.meshRenderer.sharedMaterial = EarthMaterialsByTypeOnCountries[(int)CurrentState];
            }
            if (value != null)
            {
                value.meshRenderer.sharedMaterial = new Material(value.meshRenderer.sharedMaterial);
                value.meshRenderer.sharedMaterial.SetFloat("_StripesValue", 1);
            }
            _currentSelectedCountry = value;
        }
    }

    public float currentPointValue;
    LayerMask EarthMask;
    public GameObject CurrenUnitPoint;
    public static event Action EventChangeState;

    // Enum for different states of the world map
    public enum State { Earth = 0, Disaster = 1, Climate = 2, EarthNight = 3, EarthJanuary = 4, EarthAugust = 5, EarthBorders = 6, EarthDetails = 7, OceanFlow = 8}
    private State _currentState;
    public State CurrentState
    {
        get => _currentState;
        set
        {
            ChangeAllCountriesMaterials(EarthMaterialsByTypeOnCountries[(int)value]);
            Debug.Log("State Function");

            //Switch cases extended by Max Schneider
            switch (value)
            {
                case State.Earth:
                    EarthRenderer.sharedMaterial = Earth;
                    Clouds.SetActive(true);
                    Glow.SetActive(true);
                    HideMap();
                    break;
                case State.Disaster:
                    EarthRenderer.sharedMaterial = Disaster;
                    Glow.SetActive(false);
                    Clouds.SetActive(false);
                    ShowMap();
                    break;
                case State.Climate:
                    EarthRenderer.sharedMaterial = Climate;
                    Glow.SetActive(false);
                    Clouds.SetActive(false);
                    ShowMap();
                    break;
                case State.EarthNight:
                    EarthRenderer.sharedMaterial = EarthNight;
                    ShowMap();
                    Clouds.SetActive(false);
                    Glow.SetActive(false);
                    break;
                case State.EarthJanuary:
                    EarthRenderer.sharedMaterial = EarthJanuary;
                    ShowMap();
                    Clouds.SetActive(false);
                    Glow.SetActive(false);
                    break;
                case State.EarthAugust:
                    EarthRenderer.sharedMaterial = EarthAugust;
                    Glow.SetActive(false);
                    ShowMap();
                    Clouds.SetActive(false);
                    break;
                case State.EarthBorders:
                    EarthRenderer.sharedMaterial = EarthBorders;
                    Glow.SetActive(false);
                    ShowMap();
                    Clouds.SetActive(false);
                    break;
                case State.EarthDetails:
                    EarthRenderer.sharedMaterial = EarthDetails;
                    Glow.SetActive(false);
                    Clouds.SetActive(false);
                    ShowMap();
                    break;
                case State.OceanFlow:
                    EarthRenderer.sharedMaterial = OceanFlow;
                    Glow.SetActive(false);
                    Clouds.SetActive(false);
                    ShowMap();
                    break;
                default:
                    break;
            }
            _currentState = value;
            EventChangeState();
        }
    }

    #endregion
    public static WorldMapManager _instance;
    public static WorldMapManager instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<WorldMapManager>();
            }
            return _instance;
        }
        set { _instance = value; }
    }

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy(gameObject); return; };

        HideMap();
        SetNames();
        SetCapital();
        SetPopulation();
        SetSize();
        SetLanguage();
        SetCurrency();
        SetGDP();
    }

    void ShowMap()
    {
        Camera.main.cullingMask = ~0;
    }
    void HideMap()
    {
        Camera.main.cullingMask = ~LayerMask.GetMask("Water");
    }
    void Update()
    {
        SelectCountry();
        RotateEarth();
        ScaleEarth();
    }

    // Function to select country with XR-Controller by Max Schneider
    void SelectCountry()
    {
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed);

        RaycastHit hit;

        // Perform raycast
        if (rayInteractor.TryGetCurrent3DRaycastHit(out hit))
        {
            // Debug.Log("RayHit");
            if (hit.collider.gameObject == null)
            {
                // If raycast hits nothing, highlight the last selected country info (if any)
                if (CurrentSelectedCountryInfo != null)
                {
                    CurrentSelectedCountryInfo.Hovered = true;
                }
                return;
            }

            Country tempCountry = countries.Find(X => X.gameObject == hit.collider.gameObject);
            if (tempCountry != null)
            {
                if (triggerPressed)
                {
                    // Update the hovered country
                    if (tempCountry != CurrentHoveredCountry)
                    {
                        if (CurrentHoveredCountry != null) CurrentHoveredCountry.Hovered = false;
                        CurrentHoveredCountry = tempCountry;
                        HoveredEarthUVCoord = hit.textureCoord;
                        SetMaskValues(CurrentHoveredCountry, HoveredEarthUVCoord);

                    }

                    CurrentHoveredCountry.Hovered = true;
                }
                else
                {
                    // Highlight the last selected country info if trigger is released
                    if (tempCountry != CurrentSelectedCountryInfo)
                    {
                        if (CurrentSelectedCountryInfo != null) CurrentSelectedCountryInfo.Hovered = false;
                        CurrentSelectedCountryInfo = tempCountry;
                        HoveredEarthUVCoord = hit.textureCoord;
                        SetMaskValues(CurrentSelectedCountryInfo, HoveredEarthUVCoord);
                    }

                    CurrentSelectedCountryInfo.Hovered = true;
                }
            }
            else
            {
                if (triggerPressed)
                {
                    if (CurrentHoveredCountry != null) CurrentHoveredCountry.Hovered = false;
                    CurrentHoveredCountry = null;
                }
                else
                {
                    if (CurrentSelectedCountryInfo != null) CurrentSelectedCountryInfo.Hovered = false;
                    CurrentSelectedCountryInfo = null;
                }
            }
        }
        else
        {
            // If no raycast hit, highlight the last selected country info (if any)
            if (CurrentSelectedCountryInfo != null)
            {
                CurrentSelectedCountryInfo.Hovered = true;
            }
        }
    }

    void PlaceUnitPoint()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000))
            {
                CurrentSelectedCountry = CurrentHoveredCountry;
                SelectedEarthUVCoord = HoveredEarthUVCoord;


                UnitPoint.transform.position = hit.point;
                UnitPoint.transform.SetParent(FindObjectOfType<UnitEarth>().transform);
            }

        }
    }

    [ContextMenu("Select AllCountryes")]
    void SelectAllCountriesInEditor()
    {
        countries.Clear();
        countries.AddRange(FindObjectsOfType<Country>());
    }
    [ContextMenu("SetRandomColors")]
    void SetRandomColorsAllCountriesInEditor()
    {
        foreach (var item in countries)
        {
            item.colorCountry = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
    }

    // Set country name from JSON file, function by Max Schneider
    [ContextMenu("SetNames")]
    void SetNames()
    {
        string[] nms = CountryNamesJSONFile.text.Split('}');
        foreach (var str in nms)


            foreach (var item in countries)
            {
                if (str.Substring(11, 2) == item.name) item.countryName = str.Substring(25, str.Length - 26);
                item.meshRenderer = item.GetComponent<MeshRenderer>();
            }

    }

    // Set country capital from JSON file, function by Max Schneider
    [ContextMenu("SetCapital")]
    void SetCapital()
    {
        string[] nms = CountryCapitalJSONFile.text.Split('}');
        foreach (var str in nms)


            foreach (var item in countries)
            {
                if (str.Substring(11, 2) == item.name) item.capital = str.Substring(25, str.Length - 26);
                item.meshRenderer = item.GetComponent<MeshRenderer>();
            }

    }

    // Set country populations from JSON file, function by Max Schneider
    [ContextMenu("SetPopulation")]
    void SetPopulation()
    {
        string[] nms = CountryPopulationJSONFile.text.Split('}');
        foreach (var str in nms)


            foreach (var item in countries)
            {
                if (str.Substring(11, 2) == item.name) item.population = str.Substring(25, str.Length - 26);
                item.meshRenderer = item.GetComponent<MeshRenderer>();
            }

    }

    // Set country size from JSON file, function by Max Schneider
    [ContextMenu("SetSize")]
    void SetSize()
    {
        string[] nms = CountrySizeJSONFile.text.Split('}');
        foreach (var str in nms)


            foreach (var item in countries)
            {
                if (str.Substring(11, 2) == item.name) item.size = str.Substring(25, str.Length - 26);
                item.meshRenderer = item.GetComponent<MeshRenderer>();
            }

    }

    // Set country language from JSON file, function by Max Schneider
    [ContextMenu("SetLanguage")]
    void SetLanguage()
    {
        string[] nms = CountryLanguageJSONFile.text.Split('}');
        foreach (var str in nms)


            foreach (var item in countries)
            {
                if (str.Substring(11, 2) == item.name) item.language = str.Substring(25, str.Length - 26);
                item.meshRenderer = item.GetComponent<MeshRenderer>();
            }

    }

    // Set country curreny from JSON file, function by Max Schneider
    [ContextMenu("SetCurrency")]
    void SetCurrency()
    {
        string[] nms = CountryCurrencyJSONFile.text.Split('}');
        foreach (var str in nms)


            foreach (var item in countries)
            {
                if (str.Substring(11, 2) == item.name) item.currency = str.Substring(25, str.Length - 26);
                item.meshRenderer = item.GetComponent<MeshRenderer>();
            }

    }

    // Set country gdp from JSON file, function by Max Schneider
    [ContextMenu("SetGDP")]
    void SetGDP()
    {
        string[] nms = CountryGDPJSONFile.text.Split('}');
        foreach (var str in nms)


            foreach (var item in countries)
            {
                if (str.Substring(11, 2) == item.name) item.countryGdp = str.Substring(25, str.Length - 26);
                item.meshRenderer = item.GetComponent<MeshRenderer>();
            }

    }

    void SetMaskValues(Country currentCountry, Vector2 uvCoords)
    {
        currentCountry.science = instance.GetPercentByTexture(instance.WorldLayersTextures[3], uvCoords);
        currentCountry.logistics = instance.GetPercentByTexture(instance.WorldLayersTextures[4], uvCoords);
        currentCountry.disaster = instance.GetPercentByTexture(instance.WorldLayersTextures[5], uvCoords);
        currentCountry.climate = instance.ClimateZonesNames[instance.GetZone(instance.WorldLayersTextures[6], uvCoords)];
    }

    public int GetZone(Texture2D tex, Vector2 uv)
    {
        Color col = tex.GetPixel(Mathf.RoundToInt(uv.x * tex.width), Mathf.RoundToInt(uv.y * tex.height));
        float max = 1000000;

        int result = -1;
        for (int i = 0; i < ClimateZonesColors.Count; i++)
        {
            float temp = Vector3.Distance(new Vector3(col.r, col.g, col.b), new Vector3(ClimateZonesColors[i].r, ClimateZonesColors[i].g, ClimateZonesColors[i].b));
            if (max > temp)
            {
                max = temp;
                result = i;
            }
        }
        return result;
    }
    public int GetPercentByTexture(Texture2D tex, Vector2 uv)
    {
        Color col = tex.GetPixel(Mathf.RoundToInt(uv.x * tex.width), Mathf.RoundToInt(uv.y * tex.height));
        return Mathf.RoundToInt(col.r * 100);
    }

    // Rotate earth with XR-Controller, by Max Schneider
    void RotateEarth()
    {
        Vector2 rotationThumbstickLeft;
        Vector2 rotationThumbstickRight;

        // Get input from primary 2D axis of VR controller
        if (InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.primary2DAxis, out rotationThumbstickLeft))
        {
            // Calculate rotation angle based on input
            float rotationAmount = rotationThumbstickLeft.x * rotationSpeed * Time.deltaTime;

            // Rotate the GameObject around its Y-axis
            earthPlanet.transform.Rotate(Vector3.back, rotationAmount, Space.Self);
        }
    }

    // Reset earth rotation to origin, by Max Schneider
    public void ResetRotation()
    {
        earthPlanet.transform.rotation = Quaternion.Euler(-90f, 117.59f, 0f);
    }

    // Scale earth with XR-Controller, by Max Schneider
    void ScaleEarth()
    {
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out buttonXPressed);
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.secondaryButton, out buttonYPressed);

        if (buttonXPressed)
        {
            earthPlanet.transform.localScale += scaleFactor;
        }
        else if (buttonYPressed)
        {
            earthPlanet.transform.localScale -= scaleFactor;
        }
    }

    void ChangeAllCountriesMaterials(Material mat)
    {
        foreach (var item in countries)
        {
            item.meshRenderer.sharedMaterial = mat;
        }
    }
}