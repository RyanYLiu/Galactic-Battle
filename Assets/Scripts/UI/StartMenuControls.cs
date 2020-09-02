using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuControls : MonoBehaviour
{
    [SerializeField] Canvas instructionsCanvas;
    [SerializeField] Canvas startMenuCanvas;
    [SerializeField] Canvas creditsCanvas;

    public void DisplayInstructions()
    {
        startMenuCanvas.gameObject.SetActive(false);
        instructionsCanvas.gameObject.SetActive(true);
    }

    public void HideInstructions()
    {
        instructionsCanvas.gameObject.SetActive(false); 
        startMenuCanvas.gameObject.SetActive(true);
    }

    public void DisplayCredits()
    {
        startMenuCanvas.gameObject.SetActive(false);
        creditsCanvas.gameObject.SetActive(true);
    }

    public void HideCredits()
    {
        creditsCanvas.gameObject.SetActive(false);
        startMenuCanvas.gameObject.SetActive(true);
    }
}
