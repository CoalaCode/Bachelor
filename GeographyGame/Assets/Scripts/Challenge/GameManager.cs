/*******************************************************************
* Author            : Max Schneider
* Copyright         : MIT License
* File Name         : GameManager.cs
* Description       : This file contains the logic for the challenges.
*
/******************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;


public class GameManager : MonoBehaviour
{
    // References to UI elements
    public List<TMP_Text> countryTexts;
    public List<Image> checkmarks;
    public GameObject gameOverText;

    // XR ray interactor for raycasting and selection of country
    [SerializeField] XRRayInteractor rayInteractor;

    private GameObject[] countriesGameObjects;
    private List<GameObject> countryList;
    private List<GameObject> guessedCountriesList;
    private string countryName;
    private int correctGuesses = 0;
    private int wrongGuesses = 0;

    bool triggerPressed;

    List<InputDevice> inputDevices = new List<UnityEngine.XR.InputDevice>();

    void Start()
    {
        // Find all game objects tagged with "Country"
        countriesGameObjects = GameObject.FindGameObjectsWithTag("Country");
        if (countriesGameObjects.Length < 160)
        {
            Debug.Log("No GameObjects are tagged with 'Country'");
        }

        // Initialize lists and select random countries
        countryList = new List<GameObject>();
        guessedCountriesList = new List<GameObject>();
        SelectRandomCountries();
        DisplayCurrentCountry();

        // Get input controller
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, inputDevices);
    }

    private void Update()
    {
        OnCountryClicked();
        GameOver();

    }

    // Select random countries from the list of game objects
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

    // Display the names of the selected countries
    void DisplayCurrentCountry()
    {
        for (int i = 0; i < countryList.Count; i++)
        {
            countryName = countryList[i].GetComponent<Country>().Name;
            countryTexts[i].text = countryName;
        }
    }

    // Check if clicked country is right or wrong
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
                        // Update UI and tracked data for correct guesses
                        checkmarks[i].color = Color.green;
                        correctGuesses++;
                        guessedCountriesList.Add(countryList[i]);
                    }
                    else
                    {
                        wrongGuesses++;
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
        guessedCountriesList.Clear();
        SelectRandomCountries();
        DisplayCurrentCountry();
        for (int i = 0; i < 10; i++)
        {
            checkmarks[i].color = Color.grey;
        }
    }

    void GameOver()
    {
        if (correctGuesses == 10)
        {
            // Display game over text when all countries are guessed correctly
            gameOverText.SetActive(true);
        }
        else if (wrongGuesses > 3)
        {
            //TODO
        }
    }

}