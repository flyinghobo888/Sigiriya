using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Andrew Rimpici
//Can be used to create new Parent Traits for characters.
[CreateAssetMenu(fileName = "New Parent Trait", menuName = "Parent Trait")]
public class ParentTrait : ScriptableObject
{
    public string traitName;
    [TextArea] public string traitDescription;
}