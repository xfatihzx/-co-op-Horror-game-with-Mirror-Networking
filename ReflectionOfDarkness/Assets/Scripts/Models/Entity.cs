using System;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviourEventable
{
    [OnInspector(ReadOnly = true)] public string id;
    [OnInspector(ReadOnly = true)] public bool isSet;
    [OnInspector(ReadOnly = true, Comment = "Start method'unda set edilecek")] public bool isUsableObject;

    [Header("Object Tags :")]
    [Space(20)] public UnityEvent @event;
    [Space(10)] public EntityType[] entities;

    public override void Start()
    {
        isUsableObject = gameObject.HasTagAny(EntityType.InteractionalObject);
        base.Start();
    }

    public void SetGuid()
    {
        isSet = true;

        id = Guid.NewGuid().ToString().ToLower();
    }
    public void Action()
    {
        @event.Invoke();
    }
}