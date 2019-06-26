using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTracker : ManagerBase<CharacterTracker>
{
    public List<Character> rawCharacters = new List<Character>();

    private HashSet<Character> characters = new HashSet<Character>();

    private void Awake()
    {
        characters.Clear();
        foreach (Character character in rawCharacters)
        {
            if (!characters.Add(character))
            {
                Debug.Log("Character: " + character.characterName + " is already registered.");
            }
            else
            {
                character.InitCharacter();
            }
        }

        Debug.Log("Registered Characters");
        foreach (Character character in characters)
        {
            Debug.Log("Character: " + character.characterName);
        }
    }
}
