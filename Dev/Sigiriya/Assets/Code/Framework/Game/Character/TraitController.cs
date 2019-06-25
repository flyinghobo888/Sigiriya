using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Andrew Rimpici
//Controls how the parent and child traits interact with each other.
public class TraitController : ManagerBase<TraitController>
{
    //Hashsets are not serializable, so we take in the traits as a list.
    public List<ParentTrait> rawParentTraits;
    public List<ChildTrait> rawChildTraits;

    //Then we convert the lists to hashsets so we make sure not to have duplicates.
    private HashSet<ParentTrait> parentTraits;
    private HashSet<ChildTrait> childTraits;

    private void Start()
    {
        RegisterTraits();
    }

    private void RegisterTraits()
    {
        foreach (ParentTrait trait in rawParentTraits)
        {
            if (!parentTraits.Add(trait))
            {
                Debug.Log("Parent Trait: " + trait.traitName + " is already registered.");
            }
        }

        foreach (ChildTrait trait in rawChildTraits)
        {
            if (!childTraits.Add(trait))
            {
                Debug.Log("Child Trait: " + trait.traitName + " is already registered.");
            }
        }
    }
}
