using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapacityHouse : MonoBehaviour
{
    public int HouseLevel;
    public int BaseWoodUpgradeCost = 300;
    public int BaseEnergyUpgradeCost = 200;
    public int BaseGoldUpgradeCost = 200;
    public string HouseName;

    public float LevelUpMultiply = 1.5f;

    public ResourceType ResourceType; 
    public GlobalResourceManager GlobalResourceManager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
