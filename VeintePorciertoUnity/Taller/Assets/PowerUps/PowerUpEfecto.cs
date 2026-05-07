using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class PowerUpEfecto : ScriptableObject
{
    public abstract void Apply(GameObject target);
}
