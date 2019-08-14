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

    public string CharacterName;
    public Sprite DefaultSprite;

    [Header("List of possible expressions")]
    public List<EnumExpression> RawExpressionOrder = new List<EnumExpression> ();
    [Space]
    [Header("List of expression images (keep same order!)")]
    public List<Sprite> RawExpressions = new List<Sprite> ();
    [Space]
    [Header("List of talk expression images(keep same order!)")]
    public List<Sprite> RawTalkExpressions = new List<Sprite> ();
    [Space]
    //public List<SubTrait> RawSubTraits = new List<SubTrait> ();
    [Header("Character transforms for in the world")]
    public List<CharacterTransform> CharacterTransforms = new List<CharacterTransform>();
    [Space]
    [Header("Schedules")]
    public Schedule MorningSchedule;
    public Schedule MiddaySchedule;
    public Schedule NightSchedule;

    /////////////////////////////////////////////
    // This is the stuff we access in code

    //private HashSet<SubTrait> subTraits = new HashSet<SubTrait> ();
    //private HashSet<SuperTrait> superTraits;

    private Dictionary<EnumExpression, Sprite> expressions = new Dictionary<EnumExpression, Sprite> ();
    private Dictionary<EnumExpression, Sprite> talkExpressions = new Dictionary<EnumExpression, Sprite> ();

    //MoodTracker.AddMood
    //MoodTracker.RemoveMood
    //MoodTracker.PauseMood
    //MoodTracker.GetMoodNode
    ///MoodNode.Active -> DONT USE. Only for object pool
    //MoodNode.Paused
    //MoodeNod.Duration
    //MoodNode.Mood
    public MoodTracker MoodTracker = new MoodTracker ();

    public void InitCharacter()
    {
        //RegisterSubTraits ();
        //RegisterSuperTraits ();
        MoodController.Instance.RegisterMoodTracker (this, MoodTracker);
        InitSprites ();
    }

    //private void RegisterSubTraits()
    //{
    //    subTraits.Clear ();
    //    foreach (SubTrait trait in RawSubTraits)
    //    {
    //        if (!subTraits.Add (trait))
    //        {
    //            Debug.Log ("Sub Trait: " + trait.traitName + " is already registered.");
    //        }
    //    }
    //}

    //private void RegisterSuperTraits()
    //{
    //    superTraits = TraitController.Instance.GetSuperTraits (in subTraits);

    //    Debug.Log ("Super Traits for: " + CharacterName);
    //    foreach (SuperTrait superTrait in superTraits)
    //    {
    //        Debug.Log ("Trait: " + superTrait);
    //    }
    //}

    //In the future I want to make a node graph for this.
    private void InitSprites()
    {
        bool fail = false;
        if (RawExpressionOrder.Count != RawExpressions.Count)
        {
            Debug.Log ("Make sure there are the correct amount of expression sprites for the ordering.");
            fail = true;
        }

        if (RawExpressionOrder.Count != RawTalkExpressions.Count)
        {
            Debug.Log ("Make sure there are the correct amount of TALK expression sprites for the ordering.");
            fail = true;
        }

        expressions.Clear ();
        talkExpressions.Clear ();

        if (fail)
            return;

        for (int i = 0 ; i < RawExpressions.Count ; ++i)
        {
            expressions.Add (RawExpressionOrder [i], RawExpressions [i]);
        }

        for (int i = 0 ; i < RawTalkExpressions.Count ; ++i)
        {
            talkExpressions.Add (RawExpressionOrder [i], RawTalkExpressions [i]);
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

        Debug.Log ("COULDN'T FIND SPRITE FOR EXPRESSION: " + expression + ". Returning default sprite.");
        return DefaultSprite;
    }

    public Sprite GetDefaultSprite()
    {
        return DefaultSprite;
    }

    public Schedule GetSchedule(EnumTime timeOfDay)
    {
        switch (timeOfDay)
        {
            case EnumTime.MORNING:
                return MorningSchedule;
            case EnumTime.MIDDAY:
                return MiddaySchedule;
            case EnumTime.NIGHT:
            default:
                return NightSchedule;
        }
    }

    //These can be changed, just took from the art names.
    public enum EnumExpression
    {
        ANGRY,
        ANNOYED,
        BASHFUL,
        BIG_SMILE,
        BOTHERED,
        CURIOUS,
        EMBARRASED,
        EXITED,
        EYEBROWS,
        EYEROLL,
        FLUSTERED,
        HURT,
        LAUGH,
        NERVOUS,
        NEUTRAL,
        SAD,
        SERIOUS,
        SMALL_SMILE,
        SMIRK,
        SMUG,
        STARTLED,
        SURPRISED,
        THOUGHTFUL,
        UNSURE,
        SIZE
    }
}
