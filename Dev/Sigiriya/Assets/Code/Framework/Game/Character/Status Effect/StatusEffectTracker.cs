using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Andrew Rimpici
//Holds a list of game objects as children.
//Each game object is responsible for tracking a specific status effect.
public class StatusEffectTracker : MonoBehaviour
{
    public void CreateStatusEffect()
    {

    }
}

//Author: Andrew Rimpici
public class StatusEffect : MonoBehaviour
{
    public EnumStatusEffect EffectType { private set; get; }
    public float Duration { private set; get; }

    private HashSet<Character> affectedCharacters;
    private bool isInitialized;
    private bool isActive;

    //For now.
    //In the future we will use the world time system.
    private float durationCount;

    private void Start()
    {
        EffectType = EnumStatusEffect.SIZE;
        Duration = 0.0f;
        durationCount = 0.0f;

        affectedCharacters = new HashSet<Character>();
        isInitialized = false;
        isActive = false;
    }

    private void Init(EnumStatusEffect type, float duration)
    {
        EffectType = type;
        Duration = duration;
        isInitialized = true;
    }

    private void AddAffectedCharacter(Character character)
    {
        if (!affectedCharacters.Add(character))
        {
            Debug.Log("This character has already been added.");
        }
    }

    private void StartStatusEffect()
    {
        isActive = true;
    }

    private void Update()
    {
        if (isActive)
        {
            if (durationCount < Duration)
            {
                durationCount += Time.deltaTime;
            }
            else
            {
                //TODO: Send an event saying the status effect finished
                //TODO: In the future use an object pool for status effects.
                //It's expensive to create and destroy objects a lot.
                Destroy(gameObject);
            }
        }
    }

    public enum EnumStatusEffect
    {
        HAPPY,
        ANGRY,
        SYMPATHETIC,

        SIZE
    }
}