using System.Collections.Generic;
using UnityEngine;

public class Trigger_Lerp : MonoBehaviour
{
    public List<Transform> TargetPoints;
    public Transform NextPoint;

    bool isLerping;
    int index;
    Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
        if (NextPoint == null && TargetPoints.Count > 0)
        {
            NextPoint = TargetPoints[index];
        }
    }

    void Update()
    {
        HandleInput();

        if (isLerping && NextPoint != null)
        {
            var distance = Vector3.Distance(transform.position, NextPoint.position);
            if (distance > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, NextPoint.position, 0.03f);
            }
            else
            {
                transform.position = NextPoint.position;
                isLerping = false;
            }
        }
    }

    void HandleInput()
    {
        if (!WasPressed(out var screenPosition))
        {
            return;
        }

        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera == null) return;

        var ray = mainCamera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out var hit) && hit.transform == transform)
        {
            StartLerping();
        }
    }

    void StartLerping()
    {
        if (TargetPoints.Count == 0) return;

        isLerping = true;
        index = (index + 1) % TargetPoints.Count;
        NextPoint = TargetPoints[index];
    }

    static bool WasPressed(out Vector2 screenPosition)
    {
        screenPosition = default;
        if (Input.GetMouseButtonDown(0))
        {
            screenPosition = Input.mousePosition;
            return true;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            screenPosition = Input.GetTouch(0).position;
            return true;
        }
        return false;
    }
}
