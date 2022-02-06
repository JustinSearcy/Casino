using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionTargets { SINGLE_TARGET_ENEMY, ALL_ENEMIES, SELF, SINGLE_TARGET_ALLY, ALL }
public enum ActionNames { BASIC_ATTACK, AXE_STRIKE, DUAL_STRIKE, TRIPLE_SHURIKEN, BASIC_DEFENSE, FORTIFY, CHESTPLATE, GAUNTLET, GREAVES, HELMET}

public interface IDiceSide
{
    ActionTargets ActionTarget { get; }

    ActionNames ActionName { get; }

    public string Description { get; }

    public string RolledDescription { get; }

    public void Action();
}