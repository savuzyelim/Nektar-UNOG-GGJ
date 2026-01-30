using System;
using UnityEngine;

public enum AbilityType
{
    Pull,
    Knockback
}

public static class AbilityEvents
{
    public static Action<
        AbilityType,
        Vector2,
        float,
        float
    > OnAbilityUsed;
}
