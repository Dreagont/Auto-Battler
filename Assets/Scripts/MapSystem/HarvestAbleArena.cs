using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map System/Harvest Arena")]
public class HarvestAbleArena : ScriptableObject
{
    public Characters character;

    public float arenaSpeedBonus = 0.0f;
    public int arenaQualityBonus = 0;

    public int EnergyCost = 0; 
}

public enum Characters
{
    None,
    Fighter,
    Lumberjack,
    Miner,
    Cooker,
    Trader,
    Buider,
    Farmer
}
