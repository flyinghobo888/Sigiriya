using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MemoryData", menuName = "Memory")]
public class Memory : ScriptableObject
{
	public Sprite memoryImage;
	public string memoryText;
	public FlagBank.Flags flag; //might not need

}
