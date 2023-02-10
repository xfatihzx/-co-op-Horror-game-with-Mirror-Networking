using System;
using UnityEngine;

//#if UNITY_EDITOR || DEBUG
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class OnInspectorAttribute : PropertyAttribute
{
    public bool ReadOnly { get; set; }
    public string Comment { get; set; }
    public string Tooltip { get; set; }
    public OnInspectorAttribute()
    {

    }
}
//#endif