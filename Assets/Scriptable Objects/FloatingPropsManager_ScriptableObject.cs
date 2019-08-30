using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FloatingPropsManager_ScriptableObject", order = 1)]
public class FloatingPropsManager_ScriptableObject : ScriptableObject
{
    public List<Props> floatingObjects;
}

[System.Serializable]
public class Props
{
    public GameObject _prefab;
    public int maxCanBeInstatiate;
    public bool canInteractable;
}
