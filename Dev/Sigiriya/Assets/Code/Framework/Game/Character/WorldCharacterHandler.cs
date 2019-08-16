using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldCharacterHandler : MonoBehaviour
{
    [SerializeField] private RectTransform characterRect = null;
    [Space]
    [SerializeField] private Button button = null;
    [Space]
    [Header("Swap characters here to test them.")]
    [SerializeField] private Character characterInfo = null;
    [Space]
    [Header("Reset convo on character info change.")]
    [SerializeField] private bool resetConvo = true;
    public bool ResetConvo { get { return resetConvo; } private set { resetConvo = value; } }
    [Space]
    [Header("In the future, don't edit directly.")]
    [SerializeField] private List<Character> charactersInConvo = null;
    public List<Character> CharactersInConvo { get { return charactersInConvo; } private set { charactersInConvo = value; } }

    private ButtonInfo buttonInfoScript;

    //private void OnValidate()
    //{
    //    SetCharacter(resetConvo);
    //}

    private void Start()
    {
        //SetCharacter(resetConvo);
    }

    public void SetCharacter(Character character, bool resetCharactersInConvo)
    {
        characterInfo = character;
        SetCharacter(resetCharactersInConvo);
    }

    public void SetCharacter(bool resetCharactersInConvo)
    {
        if (button != null)
        {
            if (characterInfo)
            {
                gameObject.SetActive(true);

                //TODO: For now. In the future we can decide how we want to pick the transforms.
                if (characterInfo.CharacterTransforms.Count > 0)
                {
                    ApplyCharacterTransform(characterInfo.CharacterTransforms[0]);
                }
                else
                {
                    //Debug.LogWarning("Cannot update transforms. This object does not have any.");
                }

                button.GetComponent<Image>().sprite = characterInfo.DefaultSprite;
                buttonInfoScript = GetComponent<ButtonInfo>();

                if (resetCharactersInConvo)
                {
                    SetCharctersInConvo(new List<Character>() { characterInfo });
                }
            }
            else
            {
                SetCharctersInConvo(null);
                gameObject.SetActive(false);
                //button.GetComponent<Image>().sprite = null;
            }
        }
        else
        {
            //Debug.LogWarning("Button Image is null.");
        }
    }

    public void SetCharctersInConvo(List<Character> characters)
    {
        CharactersInConvo = characters;
    }

    public void AddCharactersToConvo(Character character)
    {
        foreach (Character c in CharactersInConvo)
        {
            if (c.GetInstanceID() == character.GetInstanceID())
            {
                //Debug.LogWarning("Character: " + character.CharacterName + " is already in this conversation.");
                return;
            }
        }

        CharactersInConvo.Add(character);
    }

    private void ApplyCharacterTransform(CharacterTransform charTransform)
    {
        charTransform.ApplyTransform(characterRect);

        RectTransform[] childrenRects = characterRect.GetComponentsInChildren<RectTransform>(true);
        foreach (RectTransform childRect in childrenRects)
        {
            foreach (ObjectTransform objTransform in charTransform.Children)
            {
                if (childRect.tag == objTransform.Tag)
                {
                    objTransform.ApplyTransform(childRect);
                    break;
                }
            }
        }
    }
}
