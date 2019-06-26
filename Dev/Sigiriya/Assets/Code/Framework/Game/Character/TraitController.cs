using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Andrew Rimpici
//Controls how the super and sub traits interact with each other.
public class TraitController : ManagerBase<TraitController>
{
    //Hashsets are not serializable, so we take in the traits as a list.
    public List<SuperTrait> superTraits;

    private Dictionary<SuperTrait, HashSet<SubTrait>> traits = new Dictionary<SuperTrait, HashSet<SubTrait>>();

    private void Awake()
    {
        RegisterTraits();
    }

    private void RegisterTraits()
    {
        foreach (SuperTrait superTrait in superTraits)
        {
            HashSet<SubTrait> subTraits = new HashSet<SubTrait>();

            foreach (SubTrait subTrait in superTrait.rawSubTraits)
            {
                if (!subTraits.Add(subTrait))
                {
                    Debug.Log("Sub Trait: " + subTrait + " is already added for super trait: " + superTrait);
                }
            }

            //Add the super trait and all it's sub traits to the reference dictionary.
            traits.Add(superTrait, subTraits);
        }

        //Logging all the trait names for debug.
        Debug.Log("REGISTERED TRAITS");
        foreach (KeyValuePair<SuperTrait, HashSet<SubTrait>> element in traits)
        {
            Debug.Log("Trait: " + element.Key.traitName);
        }
    }

    public HashSet<SuperTrait> GetSuperTraits(in HashSet<SubTrait> queriedSubTraits)
    {
        HashSet<SuperTrait> validSuperTraits = new HashSet<SuperTrait>();
        foreach (KeyValuePair<SuperTrait, HashSet<SubTrait>> element in traits)
        {
            SuperTrait superTrait = element.Key;
            if (IsSuperTraitValid(superTrait, queriedSubTraits))
            {
                validSuperTraits.Add(superTrait);
            }
        }

        return validSuperTraits;
    }

    //This is gross and probably slow if we have a lot of traits.
    public bool IsSuperTraitValid(SuperTrait superTrait, in HashSet<SubTrait> queriedSubTraits)
    {
        int subTraitThreshold = superTrait.subTraitThreshold;
        HashSet<SubTrait> elementSubTraits = superTrait.subTraits;

        int count = 0;

        foreach (SubTrait elementSubTrait in elementSubTraits)
        {
            foreach (SubTrait queriedSubTrait in queriedSubTraits)
            {
                if (elementSubTrait.GetInstanceID() == queriedSubTrait.GetInstanceID())
                {
                    ++count;

                    if (count >= subTraitThreshold)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
