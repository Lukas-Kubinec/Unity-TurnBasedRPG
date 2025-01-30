using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;

public class AllyStateMachine : MonoBehaviour
{
    public BaseAlly ally; // creates variable of BaseAlly script
    private CombatStateMachine combatStateMachine; // variable of CombatStateMachine script
    private BaseAttack baseAttack; // variable of BaseAttack script

    public enum TurnState // list of turn states to be cycled through during gameplay
    {
        PROCESSING,
        ADDTOLIST,
        WAITING,
        SELECTING,
        ACTION,
        DEAD
    }

    public TurnState currentState; // created variable of the turn states
    
    // progress bar cooldown variables
    private float currentCooldown = 0.0f;
    private float maxCooldown = 5.0f;

    public Image progressBar;

    public GameObject selector;

    //IEnumerator variables
    public GameObject enemyToAttack;
    private bool actionStarted = false;
    private Vector3 startPosition;
    private float animSpeed = 5.0f;
    
    // turn state variables
    private bool alive = true;
    
    //ally panel variables
    private AllyStats stats;
    public GameObject allyPortraitPanel;
    public GameObject allyStatsPanel;
    private Transform allyPanelSpacer;
    public Transform allyGameObject;
    
    void Start()
    {
        // set current hp to base hp
        ally.currentHP = ally.baseHP;
        
        // create ally stats panel and fill in info
        CreateAllyStatsPanel();
        
        // find spacer
        allyPanelSpacer = GameObject.Find("AllyPanelSpacer").transform;
        
        // create ally portrait panel
        CreateAllyPortraitsPanel();
        
        // set start position as current position
        startPosition = transform.position;
        
        // set current cooldown between a range of 0s to 2.5s
        currentCooldown = Random.Range(0.0f, 2.5f);
        
        // turn off selector
        selector.SetActive(false);
        
        // begin turn in the processing state
        currentState = TurnState.PROCESSING;
        
        combatStateMachine = GameObject.Find("CombatManager").GetComponent<CombatStateMachine>();
    }

    void Update()
    {
        // create a switch of turn states
        switch (currentState)
        {
            case(TurnState.PROCESSING):
                // update progress bar during PROCESSING state
                UpdateProgressBar();
                break;
            case(TurnState.ADDTOLIST):
                // add this game object to turn list
                combatStateMachine.AlliesToManage.Add(this.gameObject);
                
                // update current state to WAITING
                currentState = TurnState.WAITING;
                break;
            case(TurnState.WAITING):
                break;
            case(TurnState.SELECTING):
                break;
            case(TurnState.ACTION):
                // start the TimeForAction method during ACTION state
                StartCoroutine(TimeForAction());
                break;
            case(TurnState.DEAD):
                if (!alive)
                {
                    // if not alive, return nothing
                    return;
                }
                else
                {
                    // change tag
                    this.gameObject.tag = "DeadAlly";
                    
                    // not attackable by enemies
                    combatStateMachine.AlliesInBattle.Remove(this.gameObject);
                    
                    // not manageable
                    combatStateMachine.AlliesToManage.Remove(this.gameObject);
                    
                    // deactivate the selector
                    selector.SetActive(false);
                    
                    // reset gui
                    combatStateMachine.elenaAbilityPanel.SetActive(false);
                    combatStateMachine.faelorAbilityPanel.SetActive(false);
                    combatStateMachine.taraAbilityPanel.SetActive(false);
                    combatStateMachine.gromAbilityPanel.SetActive(false);
                    combatStateMachine.enemySelectPanel.SetActive(false);
                    allyStatsPanel.SetActive(false);
                    
                    // remove item from perform list
                    for (int i = 0; i < combatStateMachine.PerformList.Count; i++)
                    {
                        if (combatStateMachine.PerformList[i].attackersGameObject == this.gameObject)
                        {
                            combatStateMachine.PerformList.Remove(combatStateMachine.PerformList[i]);
                        }
                    }
                    // !!REPLACE WITH DEATH ANIMATION!!
                    //this.gameObject.GetComponent<MeshRenderer>().material.color = new Color32(105, 105, 105, 255);
                    
                    // reset ally input
                    combatStateMachine.combatStates = CombatStateMachine.PerformAction.CHECKALIVE;
                    
                    // set ally to dead
                    alive = false;
                }
                break;
        }
    }

    private void UpdateProgressBar()
    {
        // decreases cooldown over time and projects it to the progress bar
        currentCooldown = currentCooldown + Time.deltaTime;
        float calculateCooldown = currentCooldown / maxCooldown;
        progressBar.transform.localScale = new Vector3(Mathf.Clamp(calculateCooldown, 0.0f, 1.0f), progressBar.transform.localScale.y, progressBar.transform.localScale.z);
        // checks if current cooldown is greater than or equal to max cooldown
        if (currentCooldown >= maxCooldown)
        {
            // changes current state to ADDTOLIST
            currentState = TurnState.ADDTOLIST;
        }
    }
    
    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            // if action has started, then break the method
            yield break;
        }
        
        actionStarted = true;

        // checks to see if the current attack is of type Melee
        if (combatStateMachine.PerformList[0].chosenAttack.attackType == "Melee")
        {
            // animate the enemy near hero to attack
            Vector3 targetPosition = new Vector3(enemyToAttack.transform.position.x - 1.5f, enemyToAttack.transform.position.y, enemyToAttack.transform.position.z);
            while (MoveTowardsTarget(targetPosition))
            {
                yield return null;
            }
        
            // wait a bit
            yield return new WaitForSeconds(0.5f);
        
            // do damage
            DoDamage();
            
            // !!INSERT ATTACK ANIMATION HERE!!
            
            
            // animate back to start position
            Vector3 defaultPosition = startPosition;
            while (MoveTowardsStart(defaultPosition))
            {
                yield return null;
            }
        }
        // checks to see if current attack is of type Ranged
        else if (combatStateMachine.PerformList[0].chosenAttack.attackType == "Ranged")
        {
            // wait for a bit
            yield return new WaitForSeconds(1.0f);
            
            // do damage
            DoDamage();
            
            // !!INSERT ATTACK ANIMATION HERE!!
            
            // wait for a bit
            yield return new WaitForSeconds(1.0f);
        }
        
        // remove this performance from list in combat state machine
        combatStateMachine.PerformList.RemoveAt(0);
        
        // check to see if combat state is not WIN or LOSE
        if (combatStateMachine.combatStates != CombatStateMachine.PerformAction.WIN && combatStateMachine.combatStates != CombatStateMachine.PerformAction.LOSE)
        {
            // reset combat state machine to WAIT
            combatStateMachine.combatStates = CombatStateMachine.PerformAction.WAIT;
            
            // reset state
            currentCooldown = 0.0f;
            currentState = TurnState.PROCESSING;
        }
        else
        {
            // reset combat state machine to WAITING
            currentState = TurnState.WAITING;
        }
        
        // set action started to false and end coroutine
        actionStarted = false;
    }
    
    // move towards target
    private bool MoveTowardsTarget(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
    
    // move back to starting position
    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    // take damage
    public void TakeDamage(float damageAmount)
    {
        // minus damage amount from ally current hp
        ally.currentHP -= damageAmount;
        if (ally.currentHP <= 0)
        {
            ally.currentHP = 0; // avoids health becoming negative number
            currentState = TurnState.DEAD;
        }
        // update stats panel values
        UpdateAllyStatsPanel();
    }

    // do damage
    private void DoDamage()
    {
        // set damage amount to chosen attacks attack damage
        float damageAmount = combatStateMachine.PerformList[0].chosenAttack.attackDamage;
        enemyToAttack.GetComponent<EnemyStateMachine>().TakeDamage(damageAmount);
    }

    // creates an instance of ally stats panel
    private void CreateAllyStatsPanel()
    {
        allyStatsPanel = Instantiate(allyStatsPanel);
        stats = allyStatsPanel.GetComponent<AllyStats>();
        
        // make panel stats equal to ally stats
        stats.allyName.text = ally.characterName;
        stats.allyHP.maxValue = ally.currentHP;
        stats.allyHP.value = ally.currentHP;
        progressBar = stats.progressBar;

        // sets parent to ally game object
        allyStatsPanel.transform.SetParent(allyGameObject, false);
    }

    // creates instance of ally portrait panel
    private void CreateAllyPortraitsPanel()
    {
        allyPortraitPanel = Instantiate(allyPortraitPanel);
        stats = allyStatsPanel.GetComponent<AllyStats>();
        
        // make portrait info equal to ally info
        stats.allyName.text = ally.characterName;
        
        // sets parent to ally panel spacer
        allyPortraitPanel.transform.SetParent(allyPanelSpacer, false);
    }

    // updates ally stats panel
    private void UpdateAllyStatsPanel()
    {
        // sets value of allyHP slider to ally current hp
        stats.allyHP.value = ally.currentHP;
    }
}


/*
References:
Unity Turn Based Battle / Combat - https://www.youtube.com/playlist?list=PLj0TSSTwoqAypUgag6HJoVZD-RmbpDtjF
*/