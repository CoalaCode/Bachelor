using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    
    public List<TMP_Text> countryTexts;
    [SerializeField] XRRayInteractor rayInteractor;

    private GameObject[] countriesGameObjects;
    private List<GameObject> countryList;
    private string currentCountry;
    private int correctGuesses = 0;
    private int wrongGuesses = 0;

    void Start()
    {
        countriesGameObjects = GameObject.FindGameObjectsWithTag("Country");
        if (countriesGameObjects.Length < 160)
        {
            Debug.Log("No GameObjects are tagged with 'Country'");
        }
        /*
        for(int i = 0; i < countries.Length; i++)
        {
            Debug.Log(countries[i].name);
        }
        */
        countryList = new List<GameObject>();
        SelectRandomCountries();
        DisplayCurrentCountry();
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
            Debug.Log("Hello");
            countryTexts[i].text = countryList[i].name;
        }
    }

    public void OnCountryClicked(string clickedCountry)
    {
        RaycastHit hit;
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (rayInteractor.TryGetCurrent3DRaycastHit(out hit))
        {
            Debug.Log("RayHit");
            if (hit.collider.gameObject == null) return;
            GameObject tempCountry = hit.collider.gameObject;
            if (tempCountry != null)
            {
                for (int i = 0; i < countryList.Count; i++)
                {
                    if (tempCountry == countryList[i])
                    {
                        correctGuesses++;
                    }
                    else
                    {
                        wrongGuesses++;
                        if (wrongGuesses > 3)
                        {
                            //GameOver Text
                        }
                    }
                }
            }
        }
    }

    /*
    void DisplayStatistics()
    {
        // Display statistics UI, showing correct guesses out of total
        Debug.Log("Game Over. Correct Guesses: " + correctGuesses + "/" + countries.Count);
    }
    */
}