using System;
using UnityEngine;
using UnityEngine.Serialization;

public class BaseCharacter : MonoBehaviour
{
    protected StatSet StatSet;
    [SerializeField]
    protected string characterName = string.Empty;
    [SerializeField]
    protected int level = 1;

    private void Start()
    {
        StatSet = new StatSet();
    }
}
