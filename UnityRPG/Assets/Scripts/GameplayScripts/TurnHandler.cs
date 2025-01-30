using UnityEngine;
using System.Collections;

[System.Serializable]
public class TurnHandler
{
    public string attackerName; // name of attacker
    public string type;
    public GameObject attackersGameObject; // who is attacking
    public GameObject attackersTarget; // who is the target of the attack
    
    // which attack is performed
    public BaseAttack chosenAttack;
}


/*
References:
Unity Turn Based Battle / Combat - https://www.youtube.com/playlist?list=PLj0TSSTwoqAypUgag6HJoVZD-RmbpDtjF
*/