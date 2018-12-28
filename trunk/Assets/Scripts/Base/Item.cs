using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MainWeapon
{
    public string name;
    public float damage;
    public float rof;
    public float muzzleVelocity;
}

public class Item : MonoBehaviour
{
    public enum ItemType {Weapon, Grenade, Armor, None };

    public string m_name = "none";
    public ItemType m_type = ItemType.None;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
