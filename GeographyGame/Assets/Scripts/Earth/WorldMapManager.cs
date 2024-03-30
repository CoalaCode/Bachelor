using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class WorldMapManager : MonoBehaviour
{
    #region Variables
    [SerializeField] MeshRenderer EarthRenderer;
    [SerializeField] GameObject Clouds;
    [SerializeField] GameObject Glow;
    [SerializeField] public List<Country> countries;
    [SerializeField] public Material Earth;
    [SerializeField] public Material Population;
    [SerializeField] public Material Science;
    [SerializeField] public Material Transport;
    [SerializeField] public Material Disaster;
    [SerializeField] public Material Climat;
    [SerializeField] public Material EarthAugust;
    [SerializeField] public Material EarthJanuary;
    [SerializeField] public Material EarthDetails;
    [SerializeField] public Material EarthBorders;
    [SerializeField] public Material EarthNight;
    [SerializeField] public Material EarthDay;
    [SerializeField] public Material OceanFlow;
    [SerializeField] public Material SeaLevelRise;
    [SerializeField] public List<Texture2D> WorldLayersTextures;
    [Header("Use it for different zones on ClimateTexture")] [SerializeField] public List<Color> ClimatZonesColors;
    [SerializeField] public List<string> ClimatZonesNames;
    [Header("Use this file with void SetNames()")]
    [SerializeField] public TextAsset CountryNamesJSONFile;
    //[SerializeField] public TextAsset CountryPopulationJSonFile;
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

    public enum State { Earth = 0, Politic = 1, Population = 2, Science = 3, Transport = 4, Disaster = 5, Climat = 6, EarthDay = 7, EarthNight = 8, EarthJanuary = 9, EarthAugust = 10, EarthBorders = 11, EarthDetails = 12, OceanFlow = 13, SeaLevelRise = 14 }
    private State _currentState;
    public State CurrentState
    {
        get => _currentState;
        set
        {
            ChangeAllCountriesMaterials(EarthMaterialsByTypeOnCountries[(int)value]);
            Debug.Log("State Function");

            switch (value)
            {
                case State.Earth:
                    EarthRenderer.sharedMaterial = Earth;
                    Clouds.SetActive(true);
                    Glow.SetActive(true);
                    HideMap();
                    break;
                case State.Politic:
                    EarthRenderer.sharedMaterial = Earth;
                    ShowMap();
                    Clouds.SetActive(false);
                    Glow.SetActive(false);
                    break;
                case State.Population:
                    EarthRenderer.sharedMaterial = Population;
                    ShowMap();
                    Clouds.SetActive(false);
                    Glow.SetActive(false);
                    break;
                case State.Science:
                    EarthRenderer.sharedMaterial = Science;
                    Glow.SetActive(false);
                    ShowMap();
                    Clouds.SetActive(false);
                    break;
                case State.Transport:
                    EarthRenderer.sharedMaterial = Transport;
                    Glow.SetActive(false);
                    ShowMap();
                    Clouds.SetActive(false);
                    break;
                case State.Disaster:
                    EarthRenderer.sharedMaterial = Disaster;
                    Glow.SetActive(false);
                    Clouds.SetActive(false);
                    ShowMap();
                    break;
                case State.Climat:
                    EarthRenderer.sharedMaterial = Climat;
                    Glow.SetActive(false);
                    Clouds.SetActive(false);
                    ShowMap();
                    break;
                case State.EarthDay:
                    EarthRenderer.sharedMaterial = EarthDay;
                    Clouds.SetActive(true);
                    Glow.SetActive(true);
                    HideMap();
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
                case State.SeaLevelRise:
                    EarthRenderer.sharedMaterial = SeaLevelRise;
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
            item.ColorCountry = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
    }

    [ContextMenu("SetNames")]
    void SetNames()
    {
        string[] nms = CountryNamesJSONFile.text.Split('}');
        foreach (var str in nms)


            foreach (var item in countries)
            {
                if (str.Substring(11, 2) == item.name) item.Name = str.Substring(25, str.Length - 26);
                item.meshRenderer = item.GetComponent<MeshRenderer>();
            }

    }

    /*
    [ContextMenu("SetPopulation")]
    void SetPopulationAndWealth()
    {

        string[] nms = CountryPopulationJSonFile.text.Split('\n');
        foreach (var str in nms)
        {

            string[] cntr = str.Split('\t');

            foreach (var item in countries)
            {
                if (cntr[0].Trim().ToLower() == item.Name.ToLower())
                {
                    float.TryParse(cntr[3], out item.Population);
                    float.TryParse(cntr[2], out item.Wealth);
                }
            }

        }

    }
    */

    [ContextMenu("SetCapital")]
    void SetCapital()
    {
        string[] nms = CountryCapitalJSONFile.text.Split('}');
        foreach (var str in nms)


            foreach (var item in countries)
            {
                if (str.Substring(11, 2) == item.name) item.Capital = str.Substring(25, str.Length - 26);
                item.meshRenderer = item.GetComponent<MeshRenderer>();
            }

    }

    [ContextMenu("SetPopulation")]
    void SetPopulation()
    {
        string[] nms = CountryPopulationJSONFile.text.Split('}');
        foreach (var str in nms)


            foreach (var item in countries)
            {
                if (str.Substring(11, 2) == item.name) item.Population = str.Substring(25, str.Length - 26);
                item.meshRenderer = item.GetComponent<MeshRenderer>();
            }

    }

    [ContextMenu("SetSize")]
    void SetSize()
    {
        string[] nms = CountrySizeJSONFile.text.Split('}');
        foreach (var str in nms)


            foreach (var item in countries)
            {
                if (str.Substring(11, 2) == item.name) item.Size = str.Substring(25, str.Length - 26);
                item.meshRenderer = item.GetComponent<MeshRenderer>();
            }

    }

    [ContextMenu("SetLanguage")]
    void SetLanguage()
    {
        string[] nms = CountryLanguageJSONFile.text.Split('}');
        foreach (var str in nms)


            foreach (var item in countries)
            {
                if (str.Substring(11, 2) == item.name) item.Language = str.Substring(25, str.Length - 26);
                item.meshRenderer = item.GetComponent<MeshRenderer>();
            }

    }

    [ContextMenu("SetCurrency")]
    void SetCurrency()
    {
        string[] nms = CountryCurrencyJSONFile.text.Split('}');
        foreach (var str in nms)


            foreach (var item in countries)
            {
                if (str.Substring(11, 2) == item.name) item.Currency = str.Substring(25, str.Length - 26);
                item.meshRenderer = item.GetComponent<MeshRenderer>();
            }

    }

    [ContextMenu("SetGDP")]
    void SetGDP()
    {
        string[] nms = CountryGDPJSONFile.text.Split('}');
        foreach (var str in nms)


            foreach (var item in countries)
            {
                if (str.Substring(11, 2) == item.name) item.GDP = str.Substring(25, str.Length - 26);
                item.meshRenderer = item.GetComponent<MeshRenderer>();
            }

    }

    void SetMaskValues(Country currentCountry, Vector2 uvCoords)
    {
        currentCountry.science = instance.GetPercentByTexture(instance.WorldLayersTextures[3], uvCoords);
        currentCountry.logistics = instance.GetPercentByTexture(instance.WorldLayersTextures[4], uvCoords);
        currentCountry.disaster = instance.GetPercentByTexture(instance.WorldLayersTextures[5], uvCoords);
        currentCountry.climate = instance.ClimatZonesNames[instance.GetZone(instance.WorldLayersTextures[6], uvCoords)];
    }

    public int GetZone(Texture2D tex, Vector2 uv)
    {
        Color col = tex.GetPixel(Mathf.RoundToInt(uv.x * tex.width), Mathf.RoundToInt(uv.y * tex.height));
        float max = 1000000;

        int result = -1;
        for (int i = 0; i < ClimatZonesColors.Count; i++)
        {
            float temp = Vector3.Distance(new Vector3(col.r, col.g, col.b), new Vector3(ClimatZonesColors[i].r, ClimatZonesColors[i].g, ClimatZonesColors[i].b));
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
            //transform.RotateAround(transform.position, Vector3.up, rotationAmount);
        }
        /*
        if(InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primary2DAxis, out rotationThumbstickRight))
        {
            float rotationAmount = rotationThumbstickRight.y * rotationSpeed * Time.deltaTime;
            earthPlanet.transform.Rotate(Vector3.left, rotationAmount, Space.Self);
        }
        */
    }

    public void ResetRotation()
    {
        earthPlanet.transform.rotation = Quaternion.Euler(-90f, 117.59f, 0f);
    }

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