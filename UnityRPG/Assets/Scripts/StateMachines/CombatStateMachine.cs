using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class CombatStateMachine : MonoBehaviour
{
    public enum PerformAction // creates a state list of actions to perform
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION,
        CHECKALIVE,
        WIN,
        LOSE
    }

    public PerformAction combatStates; // variable of action perform states

    public List<TurnHandler> PerformList = new List<TurnHandler>(); // creates list of actions to be performed
    
    public List<GameObject> AlliesInBattle = new List<GameObject>(); // creates list of allies in battle
    public List<GameObject> EnemiesInBattle = new List<GameObject>(); // creates list of enemies in battle

    public enum AllyGUI // creates a state list of ally gui
    {
        ACTIVATE,
        WAITING,
        DONE
    }

    public AllyGUI allyInput; // variable of ally gui
    
    public List<GameObject> AlliesToManage = new List<GameObject>(); // creates a list of allies to manage
    private TurnHandler allyChoice;

    public GameObject enemyButton;
    public Transform spacer;

    // gui panels
    public GameObject elenaAbilityPanel;
    public GameObject faelorAbilityPanel;
    public GameObject taraAbilityPanel;
    public GameObject gromAbilityPanel;
    public GameObject enemySelectPanel;
    
    private List<GameObject> enemyButtons = new List<GameObject>(); // creates a list of enemy buttons
    
    // Win/Lose canvas
    public GameObject winCanvas;
    public GameObject loseCanvas;
    
    public GameObject combatCanvas;
    
    void Start()
    {
        // begins combat in WAIT state
        combatStates = PerformAction.WAIT;
        
        // finds range of enemies in battle
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        
        // finds range of allies in battle
        AlliesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Ally"));
        
        // sets ally input to ACTIVATE state
        allyInput = AllyGUI.ACTIVATE;
        
        // begins game with all panels turned off
        elenaAbilityPanel.SetActive(false);
        faelorAbilityPanel.SetActive(false);
        taraAbilityPanel.SetActive(false);
        gromAbilityPanel.SetActive(false);
        enemySelectPanel.SetActive(false);
        
        // creates a button for each enemy in battle
        EnemyButtons();
        
        // begins game with condition canvases off
        winCanvas.SetActive(false);
        loseCanvas.SetActive(false);
    }

    void Update()
    {
        // combat state switch
        switch (combatStates)
        {
            case (PerformAction.WAIT):
                // checks if count of perform list is greater than 0
                if (PerformList.Count > 0)
                {
                    // if greater than 0, sets combat state to TAKEACTION
                    combatStates = PerformAction.TAKEACTION;
                }
                break;
            case (PerformAction.TAKEACTION):
                // sets a variable of performer to the first object in the perform list
                GameObject performer = GameObject.Find(PerformList[0].attackerName);
                
                // checks if the first object in the perform list is an enemy
                if (PerformList[0].type == "Enemy")
                {
                    // creates a variable of the enemy state machine and links it to performer
                    EnemyStateMachine enemyStateMachine = performer.GetComponent<EnemyStateMachine>();
                    
                    // cycles through all allies currently in battle
                    for (int i = 0; i < AlliesInBattle.Count; i++)
                    {
                        // checks if attackers target is an ally in battle
                        if (PerformList[0].attackersTarget == AlliesInBattle[i])
                        {
                            enemyStateMachine.allyToAttack = PerformList[0].attackersTarget;
                            enemyStateMachine.currentState = EnemyStateMachine.TurnState.ACTION;
                            break;
                        }
                        else
                        {
                            PerformList[0].attackersTarget = AlliesInBattle[Random.Range(0, AlliesInBattle.Count)];
                            enemyStateMachine.allyToAttack = PerformList[0].attackersTarget;
                            enemyStateMachine.currentState = EnemyStateMachine.TurnState.ACTION;
                        }
                    }
                    enemyStateMachine.allyToAttack = PerformList[0].attackersTarget;
                    
                    // makes current turn state ACTION
                    enemyStateMachine.currentState = EnemyStateMachine.TurnState.ACTION;
                }
                
                // checks if first object in perform list is an ally
                if (PerformList[0].type == "Ally")
                {
                    AllyStateMachine allyStateMachine = performer.GetComponent<AllyStateMachine>();
                    allyStateMachine.enemyToAttack = PerformList[0].attackersTarget;
                    
                    // makes current turn state ACTION
                    allyStateMachine.currentState = AllyStateMachine.TurnState.ACTION;
                }

                // makes current combat state PERFORMACTION
                combatStates = PerformAction.PERFORMACTION;
                break;
            case (PerformAction.PERFORMACTION):
                // idle state
                break;
            case (PerformAction.CHECKALIVE):
                // checks if allies in battle is less than 1
                if (AlliesInBattle.Count < 1)
                {
                    // changes combat state to LOSE
                    combatStates = PerformAction.LOSE;
                }
                // checks if enemies in battle is less than 1
                else if (EnemiesInBattle.Count < 1)
                {
                    // changes combat state to WIN
                    combatStates = PerformAction.WIN;
                }
                else
                {
                    // clears the attack panel
                    ClearAttackPanel();
                    
                    // changes ally input to ACTIVATE
                    allyInput = AllyGUI.ACTIVATE;
                }
                break;
            case (PerformAction.WIN):
                for (int i = 0; i < AlliesInBattle.Count; i++)
                {
                    // changes all allies in battle to WAITING turn state
                    AlliesInBattle[i].GetComponent<AllyStateMachine>().currentState = AllyStateMachine.TurnState.WAITING;
                    
                    // activates win canvas
                    winCanvas.SetActive(true);
                    combatCanvas.SetActive(false);
                }
                break;
            case (PerformAction.LOSE):
                for (int i = 0; i < EnemiesInBattle.Count; i++)
                {
                    // changes all enemies in battle to WAITING turn state
                    EnemiesInBattle[i].GetComponent<EnemyStateMachine>().currentState = EnemyStateMachine.TurnState.WAITING;
                    
                    // activates lose canvas
                    loseCanvas.SetActive(true);
                    combatCanvas.SetActive(false);
                }
                break;
        }

        switch (allyInput)
        {
            case (AllyGUI.ACTIVATE):
                // checks if allies to manage is greater than 0
                if (AlliesToManage.Count > 0)
                {
                    // sets the first ally in the manage count to have their selector active
                    AlliesToManage[0].transform.Find("Selector").gameObject.SetActive(true);
                    allyChoice = new TurnHandler();

                    // checks if the ally selected is elena
                    if (AlliesToManage[0].gameObject.GetComponent<AllyStateMachine>().ally.characterName == "Elena")
                    {
                        // activates panel corresponding to the current ally selected
                        elenaAbilityPanel.SetActive(true);
                        faelorAbilityPanel.SetActive(false);
                        taraAbilityPanel.SetActive(false);
                        gromAbilityPanel.SetActive(false);
                    }
                    // checks if the ally selected is faelor
                    else if (AlliesToManage[0].gameObject.GetComponent<AllyStateMachine>().ally.characterName == "Faelor")
                    {
                        // activates panel corresponding to the current ally selected
                        faelorAbilityPanel.SetActive(true);
                        taraAbilityPanel.SetActive(false);
                        gromAbilityPanel.SetActive(false);
                        elenaAbilityPanel.SetActive(false);
                    }
                    // checks if the ally selected is tara
                    else if (AlliesToManage[0].gameObject.GetComponent<AllyStateMachine>().ally.characterName == "Tara")
                    {
                        // activates panel corresponding to the current ally selected
                        taraAbilityPanel.SetActive(true);
                        elenaAbilityPanel.SetActive(false);
                        gromAbilityPanel.SetActive(false);
                        faelorAbilityPanel.SetActive(false);
                    }
                    // checks if the ally selected is grom
                    else if (AlliesToManage[0].gameObject.GetComponent<AllyStateMachine>().ally.characterName == "Grom")
                    {
                        // activates panel corresponding to the current ally selected
                        gromAbilityPanel.SetActive(true);
                        taraAbilityPanel.SetActive(false);
                        faelorAbilityPanel.SetActive(false);
                        elenaAbilityPanel.SetActive(false);
                    }
                    
                    // sets ally input to WAITING
                    allyInput = AllyGUI.WAITING;
                }
                break;
            case (AllyGUI.WAITING):
                // idle state
                break;
            case (AllyGUI.DONE):
                AllyInputDone();
                break;
        }
    }

    public void CollectActions(TurnHandler input)
    {
        PerformList.Add(input);
    }

    public void EnemyButtons()
    {
        // button clean up
        foreach (GameObject enemyBtn in enemyButtons)
        {
            // destroys enemy button
            Destroy(enemyBtn);
        }
        enemyButtons.Clear();
        
        // create enemy select buttons
        foreach (GameObject enemy  in EnemiesInBattle)
        {
            GameObject newButton = Instantiate(enemyButton) as GameObject;
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();

            EnemyStateMachine currentEnemy = enemy.GetComponent<EnemyStateMachine>();
            
            TMP_Text buttonText = newButton.transform.Find("EnemySelectButtonText").GetComponent<TMP_Text>();
            buttonText.text = currentEnemy.enemy.characterName;

            button.enemyPrefab = enemy;
            enemyButtons.Add(newButton);
            
            newButton.transform.SetParent(spacer,false);
        }
    }

    public void Input1(BaseAttack chosenAttack)// Ability selection button
    {
        allyChoice.attackerName = AlliesToManage[0].GetComponent<AllyStateMachine>().ally.characterName;
        allyChoice.attackersGameObject = AlliesToManage[0];
        allyChoice.type = "Ally";
        allyChoice.chosenAttack = chosenAttack;
        
        elenaAbilityPanel.SetActive(false);
        faelorAbilityPanel.SetActive(false);
        taraAbilityPanel.SetActive(false);
        gromAbilityPanel.SetActive(false);
        enemySelectPanel.SetActive(true);
    }

    public void Input2(GameObject chosenEnemy)// enemy selection
    {
        allyChoice.attackersTarget = chosenEnemy;
        allyInput = AllyGUI.DONE;
    }

    private void AllyInputDone()
    {
        PerformList.Add(allyChoice);
        ClearAttackPanel();
        AlliesToManage[0].transform.Find("Selector").gameObject.SetActive(false);
        AlliesToManage.RemoveAt(0);
        allyInput = AllyGUI.ACTIVATE;
    }

    private void ClearAttackPanel()
    {
        enemySelectPanel.SetActive(false);
        elenaAbilityPanel.SetActive(false);
        faelorAbilityPanel.SetActive(false);
        taraAbilityPanel.SetActive(false);
        gromAbilityPanel.SetActive(false);
    }
}


/*
References:
Unity Turn Based Battle / Combat - https://www.youtube.com/playlist?list=PLj0TSSTwoqAypUgag6HJoVZD-RmbpDtjF
*/