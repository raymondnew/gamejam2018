using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    List<Item> m_listOfItems = new List<Item>();

    public void AddItem(Item newItem)
    {
        Item.ItemType type = newItem.m_type;
        bool duplicate = false;

        foreach (Item element in m_listOfItems)
        {
            if (element.m_type == type)
                duplicate = true;
        }

        if (duplicate)
        { 
            return;
            //Debug.LogError("Duplicate Item!");
        }
        else
            m_listOfItems.Add(newItem);
    }

    public void RemoveItem(Item removeItem)
    {
        bool destroy = false;

        foreach (Item element in m_listOfItems)
        {
            if (element.m_type == removeItem.m_type && element.m_name == removeItem.m_name)
                destroy = true;
        }

        if (destroy)
            m_listOfItems.Remove(removeItem);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
