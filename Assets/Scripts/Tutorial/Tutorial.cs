using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    [SerializeField] Text tutorialText;
    [SerializeField] Text buttonText;

    private int currentStep;

    // Start is called before the first frame update
    void Start()
    {
        currentStep = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Next()
    {
        switch (++currentStep)
        {
            case 1: MoveInformation(); break;
            case 2: TopMenuBar(); break;
            case 3: BuildingMenu(); break;
            case 4: BuildingInformations(); break;
            case 5: BuildingBuild(); break;
            case 6: BuildingDestroy(); break;
            case 7: BuildingInformation(); break;
            case 8: BarrackInformation(); break;
            case 9: SoldierInformation(); break;
            case 10: HappinessInformation(); break;
            case 11: GoalOfTheGame(); break;
            case 12: Finish(); break;
            case 13: GameController.FindGameManager().GetComponent<GameController>().BackToMainMenu(); break;
        }
    }

    private void MoveInformation()
    {
        tutorialText.text = "You can move around with the W, A, S, D Keys and rotate with Q, E";
    }

    private void TopMenuBar()
    {
        tutorialText.text = "On the top you see all your current Resources. Your current Workers, unemployed Workers and Soldiers. On the right you have a small Gear to access the Menu and Settings.";
    }

    private void BuildingMenu()
    {
        tutorialText.text = "On the bottom you have your builder menu. Press one of the Buttons to see all available Buildings in this categorie. If you hover over one you will see the buildcost needs and products of this building.";
    }

    private void BuildingInformations()
    {
        tutorialText.text = "Buildings with a Product are workbuildings and you need to assign workers to them. All House buildings will offer you two workers to assign to your workbuildings.";
    }

    private void BuildingBuild()
    {
        tutorialText.text = "To Build a building simple click the button and leftklick somewhere in the world. To cancel make a right- instead of a leftklick.";
    }

    private void BuildingDestroy()
    {
        tutorialText.text = "To Destroy a building press the Destroy button and after that the builing you will destroy. As long as the Destroy button is red your in Destroy mode and every building you klick will be destroyed. To leaf the destroy mode simply rightclick.";
    }

    private void BuildingInformation()
    {
        tutorialText.text = "To assign Workers to the Workbuildings. Klick on the Workbuildlings and on the left side a buildingwindow will open. There in the Dropdown you can choose up to two workers which will start to work in this building.";
    }

    private void BarrackInformation()
    {
        tutorialText.text = "Soldiers are very important later in the game. You can create a Soldier by assigning a Worker to the Barrack. The Worker will automaticaly convert to a soldier.";
    }

    private void SoldierInformation()
    {
        tutorialText.text = "There will spawn some events. And some like a raid, a fire or an attack can be eliminated with soldiers. (It's very recommended). To assign a Soldier to an event simple click on an event an with the dropdown assign a soldier.";
    }

    private void HappinessInformation()
    {
        tutorialText.text = "In the top you see the happiness of your worker. It will decrease if you don't fulfill the needs they have. You see the needs by pressing on the house of an worker. If the happiness is low, the worker will pay less and will work less longer";
    }

    private void GoalOfTheGame()
    {
        tutorialText.text = "Your goal to perpetuate yourself is to build the \"Monument\" building in the category \"Special Buildings\"";
    }

    private void Finish()
    {
        tutorialText.text = "These are already all the basics so you can start play a Free round. Always look on the information panel on the right side, where important news will be shown";
        buttonText.text = "Close And Go back to menu";
    }
}
