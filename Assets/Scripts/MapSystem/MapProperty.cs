using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapProperty : MonoBehaviour
{
    public static MapProperty ActiveMap { get; private set; }
    public HarvestAbleArena arena;
    public SpawnerStats spawnerStats;

    void OnEnable()
    {
        ActiveMap = this;
    }

    void OnDisable()
    {
        if (ActiveMap == this)
        {
            ActiveMap = null;
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
