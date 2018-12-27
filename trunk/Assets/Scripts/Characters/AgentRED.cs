using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentRED : Agent
{
    protected override void Awake()
    {
        base.Awake();

        m_Pawn.faction = Agent.AgentFaction.RED;
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        m_Pawn.RegisterAsRED();
    }

    // Update is called once per frame
    protected override void Update()
    {
    }
}
