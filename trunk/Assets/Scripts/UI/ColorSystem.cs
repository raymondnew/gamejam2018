using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSystem : MonoBehaviour
{
    [System.Serializable]
    public class SquadColor
    {
        public string name;
        public Color32 []m_Colors;
    }

    public SquadColor[] SquadColors;

    public Color32[] GoCommandColors;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}