using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ScrollBoardItemUIBase : MonoBehaviour
{
    [SerializeField] protected Image itemBackgroundImage = null;
    [SerializeField] protected ScriptableObject parentData;

    public abstract bool InitBoardItemUI(ScriptableObject parent, Sprite background);
}