using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSystem : MonoBehaviour
{
    static public ColorSystem Instance { get; private set; }

    [System.Serializable]
    public class SquadColor
    {
        public string name;
        public Color32 []m_Colors;
    }

    public SquadColor[] SquadColors;

    public Color32[] GoCommandColors;

    private void Awake()
    {
        Instance = this;
    }
}