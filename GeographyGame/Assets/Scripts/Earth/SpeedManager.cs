/*******************************************************************
* Author            : Max Schneider and u3d
* Copyright         : MIT License
* File Name         : SpeedManager.cs
* Description       : This file contains the logic for the automatic rotation of the earth.
*
/******************************************************************/

// Rewrote the whole logic from u3d. Only the idea to use enum is from u3d 

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpeedManager : MonoBehaviour
{
    [SerializeField] Vector3 scaleDown = Vector3.one * 0.75f;
    [SerializeField] Vector3 scaleUp = Vector3.one;
    [SerializeField] Button[] buttons;
    [SerializeField] Transform earth;

    public enum Speed { Stop = 0, Normal = 1, Fast = 2, UltraFast = 3 }
    private Speed _currentSpeed;
    private Coroutine rotationCoroutine;

    private void Start()
    {
        // Set initial scale for buttons
        for (int i = 1; i < transform.childCount; i++)
        {
            buttons[i].transform.localScale = scaleDown;
        }
        // Set listeners for button clicks
        buttons[0].onClick.AddListener(() => SetSpeed(Speed.Stop));
        buttons[1].onClick.AddListener(() => SetSpeed(Speed.Normal));
        buttons[2].onClick.AddListener(() => SetSpeed(Speed.Fast));
        buttons[3].onClick.AddListener(() => SetSpeed(Speed.UltraFast));

        SetSpeed(Speed.Normal);
    }

    private void OnDestroy()
    {
        // Remove all listeners to prevent memory leaks
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    // Function by Max Schneider to set the speed
    private void SetSpeed(Speed speed)
    {
        if (_currentSpeed != speed)
        {
            // Reset scale of all buttons
            foreach (Button button in buttons)
            {
                button.transform.localScale = scaleDown;
            }

            // Set scale of the button corresponding to the selected speed
            buttons[(int)speed].transform.localScale = scaleUp;

            _currentSpeed = speed;
            UpdateRotationSpeed();
        }
    }

    //Function by Max Schneider, update rotation speed
    private void UpdateRotationSpeed()
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }

        // Determine rotation speed based on the selected speed
        switch (_currentSpeed)
        {
            case Speed.Stop:
                break;
            case Speed.Normal:
                rotationCoroutine = StartCoroutine(RotateEarth(1f));
                break;
            case Speed.Fast:
                rotationCoroutine = StartCoroutine(RotateEarth(10f));
                break;
            case Speed.UltraFast:
                rotationCoroutine = StartCoroutine(RotateEarth(50f));
                break;
        }
    }

    //Function by Max Schneider, Coroutine to rotate the earth
    private IEnumerator RotateEarth(float rotationSpeed)
    {
        while (true)
        {
            earth.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
