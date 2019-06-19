using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Andrew Rimpici
//Holds a list of game objects as children.
//Each game object is responsible for tracking a specific status effect.
public class StatusEffectTracker : ManagerBase<StatusEffectTracker>
{
    public GameObject statusEffectPrefab;
    private GameObject parentObj;

    private void Start()
    {
        if (!parentObj)
        {
            parentObj = Instantiate(statusEffectPrefab);
            parentObj.name = "Status Effect Parent";
        }

        CreateStatusEffect(EnumStatusEffect.HAPPY, 100, new HashSet<Character>());
    }

    public void CreateStatusEffect(EnumStatusEffect type, float duration, in HashSet<Character> affectedCharacters)
    {
        GameObject newStatusEffect = Instantiate(statusEffectPrefab);
        StatusEffect effect = newStatusEffect.AddComponent<StatusEffect>();
        effect.Init(type, duration, affectedCharacters);
        newStatusEffect.transform.parent = parentObj.transform;
        newStatusEffect.name = "Type: " + type + " Affecting: " + affectedCharacters.Count;
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

    public void Init(EnumStatusEffect type, float duration, in HashSet<Character> affectedCharacters)
    {
        EffectType = type;
        Duration = duration;
        isInitialized = true;

        foreach (Character c in affectedCharacters)
        {
            AddAffectedCharacter(c);
        }
    }

    public void AddAffectedCharacter(Character character)
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
                isActive = false;
                Destroy(gameObject);
            }
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
