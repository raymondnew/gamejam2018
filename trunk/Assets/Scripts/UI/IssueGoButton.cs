using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IssueGoButton : MonoBehaviour
{
    public void IssueGoCommand()
    {
        PlanningManager.Instance.IssueNextGoCommand();
    }
}