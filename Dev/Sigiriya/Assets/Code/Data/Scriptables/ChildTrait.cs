using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Andrew Rimpici
//Can be used to create new Child Traits for characters.
[CreateAssetMenu(fileName = "New Child Trait", menuName = "Child Trait")]
public class ChildTrait : ScriptableObject
{
    public string traitName;
    [TextArea] public string traitDescription;
    public List<ParentTrait> rawParentTraits = new List<ParentTrait>();

    private HashSet<ParentTrait> parentTraits = new HashSet<ParentTrait>();

    private void OnEnable()
    {
        parentTraits.Clear();
        foreach (ParentTrait trait in rawParentTraits)
        {
            if (!parentTraits.Add(trait))
            {
                Debug.Log("Parent Trait: " + trait.traitName + " is already registered with child emotion: " + traitName);
            }
        }
    }

    //Option 1
    //Creating traits in code: id and parent traits
    //CharacterTrait happy = new CharacterTrait("happy", new HashSet<CharacterTrait>() { excitable, carefree });

    //Option 2
    //Creating traits as scriptable objects
}
