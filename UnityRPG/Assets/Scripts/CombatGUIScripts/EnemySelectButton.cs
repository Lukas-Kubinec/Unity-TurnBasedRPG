using UnityEngine;

public class EnemySelectButton : MonoBehaviour
{
    public GameObject enemyPrefab; // which enemy to show selector for
    private bool selector;

    public void SelectEnemy()
    {
        // finds Input2 method from combat state machine script and passes enemyPrefab variable through
        GameObject.Find("CombatManager").GetComponent<CombatStateMachine>().Input2(enemyPrefab);
    }

    public void ShowSelector()
    {
        // sets the selector to true
        enemyPrefab.transform.Find("Selector").gameObject.SetActive(true);
    }
    
    public void HideSelector()
    {
        // sets the selector to false
        enemyPrefab.transform.Find("Selector").gameObject.SetActive(false);
    }
}


/*
References:
Unity Turn Based Battle / Combat - https://www.youtube.com/playlist?list=PLj0TSSTwoqAypUgag6HJoVZD-RmbpDtjF
*/