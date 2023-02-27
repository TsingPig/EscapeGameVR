using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;

    [SerializeField] private SavedTime savedTime;
    private float time;

    [SerializeField] private GameObject textObject;
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(SetEndMenu());

        text = textObject.GetComponent<TextMeshProUGUI>();
        time = savedTime.time;

        

        text.text = $"Félicitation, vous avez fini le jeu en {Mathf.FloorToInt(time / 60)} minutes et {Mathf.FloorToInt(time % 60)} secondes !";
    }

    // Set the menu in front of the camera and rotate it to face the camera.
    private IEnumerator SetEndMenu()
    {
        yield return new WaitForSeconds(1);
        Vector3 position = new Vector3(mainCamera.transform.forward.x, -0.1f, mainCamera.transform.forward.z);
        transform.position = mainCamera.transform.position + 2 * position;

        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }

    public void MoveToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
