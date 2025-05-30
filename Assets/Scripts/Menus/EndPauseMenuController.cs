using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class EndPauseMenuController : MonoBehaviour
{
    [SerializeField] private DefaultVolumeValues defaultVolumeValues;
    [SerializeField] private GameObject mainCamera = default;
    [SerializeField] private GameObject pauseWindow = default;
    [SerializeField] private GameObject settingsWindow = default;

    [Header("Debug Settings")]
    [Tooltip("Open/Close the pause menu.")]
    [SerializeField] private bool callPauseMenu = false;

    public static bool gameIsPaused = false;

    // This variable is used to check if the settings window is open.
    private static bool inSettingsWindow = false;
    // This variable is used to check if the primary button of the left hand controller is pressed.
    private static bool menuButtonIsPressed = false;

    // Update is called once per frame
    private void Update()
    {
        if (MenuButtonPressed() || callPauseMenu)
        {
            callPauseMenu = false;
            if (gameIsPaused)
            {
                if (inSettingsWindow)
                {
                    CloseSettingsWindow();
                }
                else
                {
                    Resume();
                }
            }
            else
            {
                Paused();
            }
        }
    }

    public void GetSettings()
    {
        settingsWindow.SetActive(true);
        inSettingsWindow = true;
    }

    // Exit the setting window.
    public void CloseSettingsWindow()
    {
        settingsWindow.SetActive(false);
        inSettingsWindow = false;
    }

    public void MoveToMainMenu()
    {
        Resume();
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        defaultVolumeValues.isSetVolume = false;
        Application.Quit();
    }

    public void Resume()
    {
        pauseWindow.SetActive(false);
        gameIsPaused = false;
    }

    public void Paused()
    {
        SetMenuPosition();
        pauseWindow.SetActive(true);
        gameIsPaused = true;
    }

    public void SetMenuPosition()
    {
        Vector3 position = new Vector3(mainCamera.transform.forward.x, -0.1f, mainCamera.transform.forward.z);
        transform.position = mainCamera.transform.position + 2 * position;

        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }

    private bool MenuButtonPressed()
    {
        // Get the left hand device.
        var leftHandedControllers = new List<InputDevice>();
        var desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandedControllers);

        // Check if the primary button is pressed of any of the left hand devices.
        foreach (var controller in leftHandedControllers)
        {
            // If the primary button is pressed, don’t check the other controllers and return a value.
            if (controller.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButton) && secondaryButton)
            {
                // As long as the button has not been released, return false.
                if (menuButtonIsPressed)
                {
                    return false;
                }
                else // If it’s the first time the button is pressed or it has been released, return true.
                {
                    menuButtonIsPressed = true;
                    return true;
                }
            }
        }

        // If the primary button is not pressed on any of the left hand devices, return false.
        menuButtonIsPressed = false;
        return false;
    }
}