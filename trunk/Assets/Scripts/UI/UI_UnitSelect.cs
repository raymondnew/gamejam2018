using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_UnitSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    Text m_selection;

    [SerializeField]
    Text m_commandLevel;

    private UI_Planning planning;
    public Transform currentTransform;
    private bool overUIElement = false;

    private int previousCommandLevel=0;

    // Start is called before the first frame update
    void Start()
    {
        planning = FindObjectOfType<UI_Planning>();

    }

    public int GetCurrentCommandLevel()
    {
        return int.Parse(m_commandLevel.text);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        overUIElement = true;
        Debug.Log("Cursor Entering " + name + " GameObject");
    }


    public void OnPointerExit(PointerEventData pointerEventData)
    {
        overUIElement = false;
        Debug.Log("Cursor Exiting " + name + " GameObject");
    }

    public void RemoveSelection(Transform pawnTransform)
    {
        if (pawnTransform != currentTransform || overUIElement)
            return;

        planning.m_selector = "None";
        m_selection.text = "NONE";
        if (currentTransform == null)
        {
            Debug.Log("No selection!");
            return;
        }

        currentTransform.Find("Selector").gameObject.SetActive(false);
        currentTransform = null;

        planning.m_selector = "";
    }

    public void RemoveSelection()
    {
        if (overUIElement)
            return;

        planning.m_selector = "None";
        m_selection.text = "NONE";
        if (currentTransform == null)
        {
            Debug.Log("No selection!");
            return;
        }

        currentTransform.Find("Selector").gameObject.SetActive(false);
        currentTransform = null;
        planning.m_selector = "";
    }

    public void AddSelection(Transform selection)
    {
        planning.m_selector = selection.GetComponent<Pawn>().m_name;
        currentTransform = selection;
        currentTransform.Find("Selector").gameObject.SetActive(true);
        m_selection.text = selection.GetComponent<Pawn>().m_name;
        GetPreviousCommandLevel();
        m_commandLevel.text = previousCommandLevel.ToString();
    }

    public void UpGoCommand()
    {
        if (currentTransform == null)
            return;
        int currentGoCommand = int.Parse(m_commandLevel.text);
        m_commandLevel.text = (currentGoCommand + 1).ToString();
    }

    public void DownGoCommand()
    {
        if (currentTransform == null)
            return;
        GetPreviousCommandLevel();
        int currentlevel = int.Parse(m_commandLevel.text);
        if (previousCommandLevel == currentlevel)
            return;

        m_commandLevel.text = (currentlevel - 1).ToString();

    }

    private void GetPreviousCommandLevel()
    {
        UI_WaypointList waypointlist = planning.GetUIWaypointMember(currentTransform.GetComponent<Pawn>().m_name);
        previousCommandLevel = waypointlist.m_waypoints[waypointlist.m_waypoints.Count - 1].m_goCommand;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
