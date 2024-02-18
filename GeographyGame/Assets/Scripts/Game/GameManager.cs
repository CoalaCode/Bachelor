using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    
    public List<TMP_Text> countryTexts;
    public List<Image> checkmarks;
    public TMP_Text gameOverText;
    //public Button restartGameBtn;
    [SerializeField] XRRayInteractor rayInteractor;

    private GameObject[] countriesGameObjects;
    private List<GameObject> countryList;
    private string countryName;
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
            //Debug.Log("Hello");
            countryName = countryList[i].GetComponent<Country>().Name;
            countryTexts[i].text = countryName;
        }
    }

    public void OnCountryClicked()
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
                //Debug.Log("Hallo");
                for (int i = 0; i < countryList.Count; i++)
                {
                    if (tempCountry.name.Equals(countryList[i].name))
                    {
                        
                        checkmarks[i].color = Color.green;
                        correctGuesses++;
                        Debug.Log(checkmarks[i].name);
                        Debug.Log(correctGuesses);

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

    public void RestartGame()
    {
        correctGuesses = 0;
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
            gameOverText.color = Color.green;
            //gameOverText.color.a = 1.0f;
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