using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static bool isMonumentBuild = false;
    public static bool lost = false;
    public static float AgeTimer { get; private set; }
    public static float AverageAge;

    private float ageTestTimer;

    //Game initializer
    private void Awake()
    {
        if (StaticValues.difficulty == 0) StaticValues.difficulty = 1;
        Button[] buttons = Resources.FindObjectsOfTypeAll<Button>();
        foreach(Button button in buttons)
        {
            button.AddButtonSound();
        }
        ResetGameValues();
        GetComponent<UIManager>().FinishLoading();
    }

    private void Start()
    {
        AverageAge = StaticValues.GetAverageAge();
        ResetGameValues();
    }

    private void Update()
    {
        AgeTimer += Time.deltaTime;

        ageTestTimer += Time.deltaTime;
        if(AgeTimer + 120 >= AverageAge && ageTestTimer > 60)
        {
            ageTestTimer = 0;
            if(PercentChance((AgeTimer + 120 - AverageAge) / 6))
            {
                Lost(LostReason.DiedOnAge);
            }
        }
    }

    public static LostReason lostReason { get; private set; }
    public static void Lost(LostReason reason)
    {
        lostReason = reason;
        lost = true;
    }

    public static string GetLostReason()
    {
        switch (lostReason)
        {
            case LostReason.DiedOnAge: return "You died on Age before you could finish your life work. Nobody will remember you :(";
            case LostReason.CapturedInAnAttack: return "You've been captured in an Attack and you can't finish your life work. Nobody wil rembember you :(";
            default: return "";
        }
    }

    private static void ResetGameValues()
    {
        isMonumentBuild = false;
        lost = false;
        AgeTimer = 0;
    }

    public static bool PercentChance(float percent)
    {
        return percent > Random.Range(0, 101);
    }

    public static GameObject FindGameManager()
    {
        return GameObject.Find("GameManager");
    }

    public static GameObject FindWorkManagers()
    {
        return GameObject.Find("WorkManagers");
    }

    public static GameObject FindTecnologyTree()
    {
        return GameObject.Find("TecnologyTree");
    }

    public static GameObject FindEnvironment()
    {
        return GameObject.Find("Environment");
    }

    public void BackToMainMenu()
    {
        ResetGameValues();
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
    }

    public enum LostReason
    {
        DiedOnAge,
        CapturedInAnAttack
    }
}
