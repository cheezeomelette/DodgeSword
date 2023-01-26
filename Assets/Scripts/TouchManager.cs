using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    Transform mPivot;

#if UNITY_ANDROID && !UNITY_EDITOR
    public bool isTouchDown => Input.GetTouch(0).phase == TouchPhase.Began;

	public bool isTouchUp => Input.GetTouch(0).phase == TouchPhase.Ended;

    public bool isTouch => (Input.GetTouch(0).phase == TouchPhase.Moved) || (Input.GetTouch(0).phase ==TouchPhase.Stationary);
    
	public Vector2 touchPosition => Input.GetTouch(0).position;

    public Vector2 touch2BoardPosition => TouchToPosition(touchPosition);
#else
    public bool isTouchDown => Input.GetMouseButtonDown(0);
    public bool isTouchUp => Input.GetMouseButtonUp(0);
    public bool isTouch => Input.GetMouseButton(0);
    public Vector2 touchPosition => Input.mousePosition;
    public Vector2 touch2BoardPosition => TouchToPosition(touchPosition);
#endif

	public TouchManager(Transform pivot)
	{
        mPivot = pivot;
	}

    Vector2 TouchToPosition(Vector3 vtInput)
    {
        Vector3 vtMousePosW = Camera.main.ScreenToWorldPoint(vtInput);

        Vector3 vtContainerLocal = mPivot.transform.InverseTransformPoint(vtMousePosW);

        return vtContainerLocal;
    }
}
