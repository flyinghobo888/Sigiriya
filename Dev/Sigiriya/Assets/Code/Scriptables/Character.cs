using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Can be used to create new Characters by designers or artists.
[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    //  - traits
    //  - current emotional state
    //  - sprites based on emotions
    //  - current status effect

    //Status effects take precedence over current emotional state

    //This is the stuff implemented by the designers/artists
        
    public string characterName;
    public Sprite defaultSprite;

    public List<EnumExpression> rawExpressionOrder = new List<EnumExpression>();
    public List<Sprite> rawExpressions = new List<Sprite>();
    public List<Sprite> rawTalkExpressions = new List<Sprite>();

    public List<SubTrait> rawSubTraits = new List<SubTrait>();


    /////////////////////////////////////////////
    // This is the stuff we access in code

    private HashSet<SubTrait> subTraits = new HashSet<SubTrait>();
    private HashSet<SuperTrait> superTraits;

    private Dictionary<EnumExpression, Sprite> expressions = new Dictionary<EnumExpression, Sprite>();
    private Dictionary<EnumExpression, Sprite> talkExpressions = new Dictionary<EnumExpression, Sprite>();

    //MoodTracker.AddMood
    //MoodTracker.RemoveMood
    //MoodTracker.PauseMood
    //MoodTracker.GetMoodNode
    ///MoodNode.Active -> DONT USE. Only for object pool
    //MoodNode.Paused
    //MoodeNod.Duration
    //MoodNode.Mood
    public MoodTracker MoodTracker = new MoodTracker();

    public void InitCharacter()
    {
        RegisterSubTraits();
        RegisterSuperTraits();
        MoodController.Instance.RegisterMoodTracker(this, MoodTracker);
        InitSprites();
    }

    private void RegisterSubTraits()
    {
        subTraits.Clear();
        foreach (SubTrait trait in rawSubTraits)
        {
            if (!subTraits.Add(trait))
            {
                Debug.Log("Sub Trait: " + trait.traitName + " is already registered.");
            }
        }
    }

    private void RegisterSuperTraits()
    {
        superTraits = TraitController.Instance.GetSuperTraits(in subTraits);

        Debug.Log("Super Traits for: " + characterName);
        foreach (SuperTrait superTrait in superTraits)
        {
            Debug.Log("Trait: " + superTrait);
        }
    }

    //In the future I want to make a node graph for this.
    private void InitSprites()
    {
        bool fail = false;
        if (rawExpressionOrder.Count != rawExpressions.Count)
        {
            Debug.Log("Make sure there are the correct amount of expression sprites for the ordering.");
            fail = true;
        }

        if (rawExpressionOrder.Count != rawTalkExpressions.Count)
        {
            Debug.Log("Make sure there are the correct amount of TALK expression sprites for the ordering.");
            fail = true;
        }

        expressions.Clear();
        talkExpressions.Clear();

        if (fail) return;

        for (int i = 0; i < rawExpressions.Count; ++i)
        {
            expressions.Add(rawExpressionOrder[i], rawExpressions[i]);
        }

        for (int i = 0; i < rawTalkExpressions.Count; ++i)
        {
            talkExpressions.Add(rawExpressionOrder[i], rawTalkExpressions[i]);
        }
    }

    public Sprite GetSpriteFromExpression(EnumExpression expression, bool isTalking)
    {
        if (isTalking)
        {
            foreach (KeyValuePair<EnumExpression, Sprite> element in talkExpressions)
            {
                if (expression == element.Key)
                {
                    return element.Value;
                }
            }
        }
        else
        {
            foreach (KeyValuePair<EnumExpression, Sprite> element in expressions)
            {
                if (expression == element.Key)
                {
                    return element.Value;
                }
            }
        }

        Debug.Log("COULDN'T FIND SPRITE FOR EXPRESSION: " + expression + ". Returning default sprite.");
        return defaultSprite;
    }

    public Sprite GetDefaultSprite()
    {
        return defaultSprite;
    }

    //These can be changed, just took from the art names.
    public enum EnumExpression
    {
        ANGRY,
        SQUINTY_SMILE,
        SURPRISED,
        TIRED,
        HAPPY,
        NEUTRAL,
        SMUG,
        THINK,
        OH_REALLY,
        SMILE,
        SATISFIED,
        UNIMPRESSED,
        FIST,
        LOOK_AWAY,
        SCARED,
        EXPLAIN,
        SECRETS,
        CLOSED_OFF,
        OPENING_UP,
        CRY,
        AMUSED,
        ARMS_OPEN,
        GRUMPY,
        LAUGH,
        STERN,
        FRUSTRATED,
        INTERESTED,
        SAD
    }
}
