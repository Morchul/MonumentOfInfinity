using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] Text difficultyInfoText;

    public void StartGame(int difficulty)
    {
        StaticValues.difficulty = difficulty;
        SceneManager.LoadScene("Game");
    }

    public void StartTutorial()
    {
        StaticValues.difficulty = 1;
        SceneManager.LoadScene("Tutorial");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetDifficultyInfo(int difficulty)
    {
        switch (difficulty)
        {
            case 1: difficultyInfoText.text = "Average Age: 60\nNo Catapult.\nAttacks are smaller."; break;
            case 2: difficultyInfoText.text = "Average Age: 50\n"; break;
            case 3: difficultyInfoText.text = "Average Age: 40\nRaiders can burn Buildings\nAttacks are bigger."; break;
        }
    }
}
