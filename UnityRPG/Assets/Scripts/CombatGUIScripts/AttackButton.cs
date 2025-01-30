using UnityEngine;

public class AttackButton : MonoBehaviour
{
    public BaseAttack attackToPerform; // which attack to perform

    public void Attack()
    {
        // finds the input 1 method from the combat state machine script
        GameObject.Find("CombatManager").GetComponent<CombatStateMachine>().Input1(attackToPerform); // variable gets used in Input1 method
    }
}


/*
References:
Unity Turn Based Battle / Combat - https://www.youtube.com/playlist?list=PLj0TSSTwoqAypUgag6HJoVZD-RmbpDtjF
*/