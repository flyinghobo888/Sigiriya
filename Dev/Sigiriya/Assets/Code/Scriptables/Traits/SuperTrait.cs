using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Can be used to create new Super Traits for characters.
[CreateAssetMenu(fileName = "New Parent Trait", menuName = "Parent Trait")]
public class SuperTrait : Trait
{
    //The amount of sub traits needed for the super trait to become active.
    public int subTraitThreshold = -1;
    public List<SubTrait> rawSubTraits = new List<SubTrait>();

    public HashSet<SubTrait> subTraits = new HashSet<SubTrait>();

    private void OnEnable()
    {
        subTraits.Clear();
        foreach (SubTrait trait in rawSubTraits)
        {
            if (!subTraits.Add(trait))
            {
                //Debug.Log("Sub Trait: " + trait.traitName + " is already registered with super trait: " + traitName);
            }
        }

        if (subTraitThreshold > subTraits.Count || subTraitThreshold < 0)
        {
            subTraitThreshold = subTraits.Count;
        }
    }
}