using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    [SerializeField] GameObject scoreAlarm;
    [SerializeField] Transform showPivot;
    [SerializeField] Transform hidePivot;
    IEnumerator ShowNotice()
    {
        float distance = Vector2.Distance(showPivot.position, hidePivot.position);
        Debug.Log($"distance{distance}");
        Vector3 showPos = showPivot.position;
        Vector3 hidePos = hidePivot.position;
        while (scoreAlarm.transform.position != showPos)
        {
            Debug.Log("in");
            scoreAlarm.transform.position = Vector2.MoveTowards(scoreAlarm.transform.position, showPos, distance * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        while (scoreAlarm.transform.position != hidePos)
        {
            scoreAlarm.transform.position = Vector2.MoveTowards(scoreAlarm.transform.position, hidePos, distance * Time.deltaTime);
            yield return null;
        }
    }

    [ContextMenu("test")]
    public void Test()
    {
        StartCoroutine(ShowNotice());
    }
}
