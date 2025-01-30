using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.VirtualTexturing;

public class EnemyStateMachine : MonoBehaviour
{
    public BaseEnemy enemy; // creates variable of BaseEnemy script
    private CombatStateMachine combatStateMachine; // variable of CombatStateMachine script
    
    public enum TurnState // list of turn states to be cycled through during gameplay
    {
        PROCESSING,
        CHOOSEACTION,
        WAITING,
        ACTION,
        DEAD
    }

    public TurnState currentState; // created variable of the turn states
    
    // progress bar cooldown variables
    private float currentCooldown = 0.0f;
    private float maxCooldown = 7.5f;
    
    private Vector3 startPosition;
    
    public GameObject selector;
    
    //enemy panel variables
    private EnemyStats stats;
    public GameObject enemyStatsPanel;
    public Transform enemyGameObject;
    
    //IEnumerator variables
    private bool actionStarted = false;
    public GameObject allyToAttack;
    private float animSpeed = 5.0f;
    
    // turn state variables
    private bool alive = true;
    
    void Start()
    {
        // set current hp to base hp
        enemy.currentHP = enemy.baseHP;
        
        // create enemy stats panel and fill in info
        CreateEnemyStatsPanel();
        
        // begin turn in the processing state
        currentState = TurnState.PROCESSING;
        
        // turn off selector
        selector.SetActive(false);
        
        combatStateMachine = GameObject.Find("CombatManager").GetComponent<CombatStateMachine>();
        
        // set start position as current position
        startPosition = transform.position;
    }

    void Update()
    {
        // create a switch of turn states
        switch (currentState)
        {
            case(TurnState.PROCESSING):
                // update cooldown during PROCESSING state
                Cooldown();
                break;
            case(TurnState.CHOOSEACTION):
                // call choose action method
                ChooseAction();
                
                // update current state to WAITING
                currentState = TurnState.WAITING;
                break;
            case(TurnState.WAITING):
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
                    this.gameObject.tag = "DeadEnemy";
                    
                    // not attackable by allies
                    combatStateMachine.EnemiesInBattle.Remove(this.gameObject);
                    
                    // deactivate the selector
                    selector.SetActive(false);
                    
                    // reset gui
                    enemyStatsPanel.SetActive(false);

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
                    
                    // set enemy to dead
                    alive = false;
                    
                    // reset enemy buttons
                    combatStateMachine.EnemyButtons();
                    
                    // check alive
                    combatStateMachine.combatStates = CombatStateMachine.PerformAction.CHECKALIVE;
                }
                break;
        }
    }
    
    private void Cooldown()
    {
        // decrease cooldown over time
        currentCooldown = currentCooldown + Time.deltaTime;
        
        // checks if current cooldown is greater than or equal to max cooldown
        if (currentCooldown >= maxCooldown)
        {
            // changes current state to CHOOSEACTION
            currentState = TurnState.CHOOSEACTION;
        }
    }

    // chooses a random action from a list of actions
    void ChooseAction()
    {
        TurnHandler myAttack = new TurnHandler();
        myAttack.attackerName = enemy.characterName;
        myAttack.type = "Enemy";
        myAttack.attackersGameObject = this.gameObject;
        myAttack.attackersTarget = combatStateMachine.AlliesInBattle[Random.Range(0, combatStateMachine.AlliesInBattle.Count)];
        
        int randomAttack = Random.Range(0, enemy.attacks.Count);
        myAttack.chosenAttack = enemy.attacks[randomAttack];
        combatStateMachine.CollectActions(myAttack);
    }

    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            // if action has started, then break the method
            yield break;
        }
        
        actionStarted = true;
        
        // animate the enemy near hero to attack
        Vector3 targetPosition = new Vector3(allyToAttack.transform.position.x + 1.5f, allyToAttack.transform.position.y, allyToAttack.transform.position.z);
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

    // do damage
    private void DoDamage()
    {
        // set damage amount to chosen attacks attack damage
        float damageAmount = combatStateMachine.PerformList[0].chosenAttack.attackDamage;
        allyToAttack.GetComponent<AllyStateMachine>().TakeDamage(damageAmount);
    }

    // take damage
    public void TakeDamage(float damageAmount)
    {
        // minus damage amount from enemy current hp
        enemy.currentHP -= damageAmount;
        if (enemy.currentHP <= 0)
        {
            enemy.currentHP = 0; // avoids health becoming negative number
            currentState = TurnState.DEAD;
        }
        // update stats panel values
        UpdateEnemyStatsPanel();
    }
    
    // creates an instance of enemy stats panel
    private void CreateEnemyStatsPanel()
    {
        enemyStatsPanel = Instantiate(enemyStatsPanel);
        stats = enemyStatsPanel.GetComponent<EnemyStats>();
        
        // make panel stats equal to enemy stats
        stats.enemyName.text = enemy.characterName;
        stats.enemyHP.maxValue = enemy.currentHP;
        stats.enemyHP.value = enemy.currentHP;

        // sets parent to enemy game object
        enemyStatsPanel.transform.SetParent(enemyGameObject, false);
    }
    
    // updates enemy stats panel
    private void UpdateEnemyStatsPanel()
    {
        // sets value of enemyHP slider to enemy current hp
        stats.enemyHP.value = enemy.currentHP;
    }
}


/*
References:
Unity Turn Based Battle / Combat - https://www.youtube.com/playlist?list=PLj0TSSTwoqAypUgag6HJoVZD-RmbpDtjF
*/