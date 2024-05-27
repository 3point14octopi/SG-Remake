using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]public class Reaction:ScriptableObject
{
    public bool isCoroutine { get; protected set; }
    public Coroutine routine;

    public virtual void OnStart(GameObject g) { }
    public virtual void ReactFunction() { }
    public virtual IEnumerator ReactCoroutine() { yield return null; }
}
