using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class OptionsMenuHandler : MonoBehaviour
{
    public static OptionsMenuHandler instance;

    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private AnimationCurve toggleOnCurve;
    private RectTransform rectTransform;
    private bool animating = false;
    private InputAction optionsMenuToggle;
    private bool isPaused = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        rectTransform = optionsMenu.GetComponent<RectTransform>();

        Vector3 position = rectTransform.localPosition;
        position.y = toggleOnCurve[0].value;
        rectTransform.localPosition = position;
        optionsMenuToggle = InputSystem.actions.FindAction("ToggleMenu");
    }

    // Update is called once per frame
    void Update()
    {
        if (optionsMenuToggle.WasPressedThisFrame() && !animating)
        {
            ToggleOptionsMenu();
        }
    }

    public void ToggleOptionsMenu()
    {
        // Deactivate options menu
        if (isPaused)
        {

            Vector3 position = rectTransform.localPosition;
            position.y = toggleOnCurve[0].value;
            rectTransform.localPosition = position;

            optionsMenu.SetActive(false);
            UnpauseGame();
        }

        // Activate options menu
        else
        {
            optionsMenu.SetActive(true);
            StartCoroutine(AnimateOptionsMenu());


            PauseGame();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private IEnumerator AnimateOptionsMenu()
    {
        animating = true;

        float animationDuration = toggleOnCurve[toggleOnCurve.length - 1].time;
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + animationDuration)
        {
            Vector3 position = rectTransform.localPosition;
            position.y = toggleOnCurve.Evaluate(Time.realtimeSinceStartup - startTime);
            rectTransform.localPosition = position;
            yield return null;
        }

        animating = false;
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

}
