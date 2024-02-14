using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpeedManager : MonoBehaviour
{
    [SerializeField] Vector3 ScaleDown = Vector3.one * 0.75f;
    [SerializeField] Vector3 ScaleUp = Vector3.one;
    [SerializeField] Button[] Buttons;
    [SerializeField] Transform earth;

    public enum Speed { Stop = 0, Normal = 1, Fast = 2, UltraFast = 3 }
    private Speed _currentSpeed;
    private Coroutine rotationCoroutine;

    private void Start()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            Buttons[i].transform.localScale = ScaleDown;
        }

        SetSpeed(Speed.Normal);
        Buttons[0].onClick.AddListener(() => SetSpeed(Speed.Stop));
        Buttons[1].onClick.AddListener(() => SetSpeed(Speed.Normal));
        Buttons[2].onClick.AddListener(() => SetSpeed(Speed.Fast));
        Buttons[3].onClick.AddListener(() => SetSpeed(Speed.UltraFast));
    }

    private void OnDestroy()
    {
        Buttons[0].onClick.RemoveAllListeners();
        Buttons[1].onClick.RemoveAllListeners();
        Buttons[2].onClick.RemoveAllListeners();
        Buttons[3].onClick.RemoveAllListeners();
    }

    private void SetSpeed(Speed speed)
    {
        if (_currentSpeed != speed)
        {
            // Reset scale of all buttons
            foreach (Button button in Buttons)
            {
                button.transform.localScale = ScaleDown;
            }

            // Set scale of the button corresponding to the selected speed
            Buttons[(int)speed].transform.localScale = ScaleUp;

            _currentSpeed = speed;
            UpdateRotation();
        }
    }

    private void UpdateRotation()
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }

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
                rotationCoroutine = StartCoroutine(RotateEarth(100f));
                break;
        }
    }

    private IEnumerator RotateEarth(float rotationSpeed)
    {
        while (true)
        {
            earth.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
