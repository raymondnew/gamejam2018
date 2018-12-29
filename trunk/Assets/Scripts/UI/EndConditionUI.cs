using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(CanvasGroup))]
public class EndConditionUI : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text m_Text;

    [SerializeField]
    float m_FadeInTime = 3.0f;

    CanvasGroup cnvGrp;
    private void Awake()
    {
        cnvGrp = GetComponent<CanvasGroup>();
        cnvGrp.alpha = 0.0f;
        cnvGrp.interactable = false;
        cnvGrp.blocksRaycasts = false;

        GameRules.OnEndGame += EndGame;
    }

    void EndGame(GameRules.EndCondition endCondition)
    {
        if (endCondition == GameRules.EndCondition.Win)
        {
            m_Text.text = "YOU\nWIN";
            m_Text.color = Color.blue;
        }
        else if (endCondition == GameRules.EndCondition.Loss)
        {
            m_Text.text = "YOU\nLOSE";
            m_Text.color = Color.red;
        }

        StartCoroutine(FadeIn(m_FadeInTime));
    }

    IEnumerator FadeIn(float seconds)
    {
        cnvGrp.alpha = 0.0f;
        seconds = Mathf.Max(seconds, 1.0f);
        while(cnvGrp.alpha < 1.0f)
        {
            cnvGrp.alpha = Mathf.Clamp(cnvGrp.alpha + (Time.deltaTime / seconds), 0.0f, 1.0f);
            yield return null;
        }
    }
}
