using System;
using UnityEngine;

/// <summary>
/// Show Inspector AudioClip-AudioType Datas
/// </summary>
[Serializable]
public struct AudioClipData
{
    public AudioType type;
    public AudioClip clip;
}
[Serializable]
public struct KeyValueData<KeyType, ValueType>
{
    public KeyType key;
    public ValueType value;
}