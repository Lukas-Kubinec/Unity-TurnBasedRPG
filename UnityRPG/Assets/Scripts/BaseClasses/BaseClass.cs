using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseClass
{
    public string characterName;

    public float baseHP;
    public float currentHP;
    public float baseDEF;
    public float currentDEF;

    public List<BaseAttack> attacks = new List<BaseAttack>(); // creates list of attacks that can be accessed by the game object
}


/*
References:
Unity Turn Based Battle / Combat - https://www.youtube.com/playlist?list=PLj0TSSTwoqAypUgag6HJoVZD-RmbpDtjF
*/