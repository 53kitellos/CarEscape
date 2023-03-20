using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New level", menuName = "ScriptableObjects/Level")]
public class Level : ScriptableObject
{
    public int LevelIndex;
    public Sprite  LevelImage;
    public Object LoadScene;
}