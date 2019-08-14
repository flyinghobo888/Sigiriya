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
    [Space]
    [Header("In the future, don't edit directly.")]
    [SerializeField] private List<Character> charactersInConvo = null;
    public List<Character> CharactersInConvo { get { return charactersInConvo; } private set { charactersInConvo = value; } }

    private ButtonInfo buttonInfoScript;

    private void OnValidate()
    {
        SetCharacter(resetConvo);
    }

    public void SetCharacter(bool resetCharactersInConvo)
    {
        if (button != null)
        {
            if (characterInfo)
            {
                gameObject.SetActive(true);

                //TODO: For now. In the future we can decide how we want to pick the transforms.
                if (characterInfo.characterTransforms.Count > 0)
                {
                    ApplyCharacterTransform(characterInfo.characterTransforms[0]);
                }
                else
                {
                    Debug.LogWarning("Cannot update transforms. This object does not have any.");
                }

                button.GetComponent<Image>().sprite = characterInfo.defaultSprite;
                buttonInfoScript = GetComponent<ButtonInfo>();

                if (resetCharactersInConvo)
                {
                    SetCharctersInConvo(new List<Character>() { characterInfo });
                }
            }
            else
            {
                gameObject.SetActive(false);
                SetCharctersInConvo(null);
            }
        }
        else
        {
            Debug.LogWarning("Button Image is null.");
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
                Debug.LogWarning("Character: " + character.characterName + " is already in this conversation.");
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
