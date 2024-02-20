using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;


public class GameManager : MonoBehaviour
{
    
    public List<TMP_Text> countryTexts;
    public List<Image> checkmarks;
    public GameObject gameOverText;

    [SerializeField] XRRayInteractor rayInteractor;
    InputManager inputManager;

    private GameObject[] countriesGameObjects;
    private List<GameObject> countryList;
    private List<GameObject> guessedCountriesList;
    private string countryName;
    private string currentCountry;
    private int correctGuesses = 0;
    private int wrongGuesses = 0;

    bool triggerPressed;
    //InputDevice rightController = new UnityEngine.XR.InputDevice();
    List<InputDevice> inputDevices = new List<UnityEngine.XR.InputDevice>();



    void Start()
    {
        countriesGameObjects = GameObject.FindGameObjectsWithTag("Country");
        if (countriesGameObjects.Length < 160)
        {
            Debug.Log("No GameObjects are tagged with 'Country'");
        }
     
        countryList = new List<GameObject>();
        guessedCountriesList = new List<GameObject>();
        SelectRandomCountries();
        DisplayCurrentCountry();

        //UnityEngine.XR.InputDevices.GetDevices(inputDevices);       
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, inputDevices);

        //inputManager = new InputManager();
    }

    private void Update()
    {
        OnCountryClicked();
        GameOver();
        
    }

    void SelectRandomCountries()
    {
        int index;

        for (int i = 0; i < 10; i++)
        {     
            index = Random.Range(0, this.countriesGameObjects.Length);
            countryList.Add(this.countriesGameObjects[index]);         
        }
        Debug.Log("CountryList Count: " + countryList.Count);
    }

    void DisplayCurrentCountry()
    {
        for (int i = 0; i < countryList.Count; i++)
        {
            countryName = countryList[i].GetComponent<Country>().Name;
            countryTexts[i].text = countryName;
        }
    }

    public void OnCountryClicked()
    {
        RaycastHit hit;

        if (rayInteractor.TryGetCurrent3DRaycastHit(out hit))
        {
    
            inputDevices[0].TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed);
   
            if (hit.collider.gameObject == null) return;
            GameObject tempCountry = hit.collider.gameObject;
            if (tempCountry != null)
            {              
                for (int i = 0; i < countryList.Count; i++)
                {
                    if (tempCountry.name.Equals(countryList[i].name) && !guessedCountriesList.Contains(tempCountry) && triggerPressed)
                    {
                        
                        checkmarks[i].color = Color.green;
                        correctGuesses++;
                        guessedCountriesList.Add(countryList[i]);
                        //Debug.Log(checkmarks[i].name);
                        //Debug.Log(correctGuesses);
                        //triggerPressed = false;
                        //Debug.Log("After Done" + triggerPressed);
                    }
                    else
                    {
                        wrongGuesses++;
                        //triggerPressed = false;
                        if (wrongGuesses > 3)
                        {
                            //GameOver Text
                        }
                    }
                }
            }
        }
    }

    public void RestartGame()
    {
        correctGuesses = 0;
        gameOverText.SetActive(false);
        countryList.Clear();
        SelectRandomCountries();
        DisplayCurrentCountry();
        for(int i = 0; i < 10; i++)
        {
            checkmarks[i].color = Color.grey;
        }
    }

    void GameOver()
    {
        if(correctGuesses == 10)
        {
            gameOverText.SetActive(true);
        }
        else if(wrongGuesses > 3)
        {
            //TODO
        }
    }

}