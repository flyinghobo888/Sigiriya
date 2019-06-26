using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Andrew Rimpici
//Can be used to create new Sub Traits for characters.
[CreateAssetMenu(fileName = "New Sub Trait", menuName = "Sub Trait")]
public class SubTrait : Trait
{
    //Option 1
    //Creating traits in code: id and parent traits
    //CharacterTrait happy = new CharacterTrait("happy", new HashSet<CharacterTrait>() { excitable, carefree });

    //Option 2
    //Creating traits as scriptable objects
}
