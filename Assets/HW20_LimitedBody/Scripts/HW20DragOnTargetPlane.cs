using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HW20DragOnTargetPlane : MonoBehaviour
{
    [Header("Drag Movement")]
    public bool requireHitObject = false;
    public float fixedLocalHeight = 0.75f;
    public float maxLocalDistance = 4f;
    public float followSpeed = 18f;

    Camera mainCamera;
    bool dragging;
    int activeFingerId = -1;
    Vector3 dragOffsetWorld;
    Vector3 targetLocalPosition;

    public bool IsDragging => dragging;

    void Awake()
    {
        mainCamera = Camera.main;
        targetLocalPosition = transform.localPosition;
        fixedLocalHeight = transform.localPosition.y;
    }

    void Update()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            return;
        }

        if (TryGetDragInput(out var phase, out var screenPosition, out var fingerId))
        {
            if (phase == DragPhase.Started)
            {
                BeginDrag(screenPosition, fingerId);
            }
            else if (phase == DragPhase.Moved && dragging && fingerId == activeFingerId)
            {
                MoveDrag(screenPosition);
            }
            else if (phase == DragPhase.Ended && fingerId == activeFingerId)
            {
                EndDrag();
            }
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPosition, Time.deltaTime * followSpeed);
    }

    void BeginDrag(Vector2 screenPosition, int fingerId)
    {
        if (requireHitObject && !ScreenPointHitsThisObject(screenPosition))
        {
            return;
        }

        if (!TryGetPointOnTargetPlane(screenPosition, out var pointOnPlane))
        {
            return;
        }

        dragging = true;
        activeFingerId = fingerId;
        dragOffsetWorld = transform.position - pointOnPlane;
        Debug.Log("HW20 Drag started");
    }

    void MoveDrag(Vector2 screenPosition)
    {
        if (!TryGetPointOnTargetPlane(screenPosition, out var pointOnPlane))
        {
            return;
        }

        var worldPosition = pointOnPlane + dragOffsetWorld;
        var parent = transform.parent;
        var localPosition = parent != null ? parent.InverseTransformPoint(worldPosition) : worldPosition;
        localPosition.y = fixedLocalHeight;
        localPosition.x = Mathf.Clamp(localPosition.x, -maxLocalDistance, maxLocalDistance);
        localPosition.z = Mathf.Clamp(localPosition.z, -maxLocalDistance, maxLocalDistance);
        targetLocalPosition = localPosition;
    }

    void EndDrag()
    {
        dragging = false;
        activeFingerId = -1;
        Debug.Log("HW20 Drag ended");
    }

    bool ScreenPointHitsThisObject(Vector2 screenPosition)
    {
        var ray = mainCamera.ScreenPointToRay(screenPosition);
        return Physics.Raycast(ray, out var hit) && (hit.transform == transform || hit.transform.IsChildOf(transform));
    }

    bool TryGetPointOnTargetPlane(Vector2 screenPosition, out Vector3 point)
    {
        var parent = transform.parent;
        var planeNormal = parent != null ? parent.up : Vector3.up;
        var plane = new Plane(planeNormal, transform.position);
        var ray = mainCamera.ScreenPointToRay(screenPosition);

        if (plane.Raycast(ray, out var distance))
        {
            point = ray.GetPoint(distance);
            return true;
        }

        point = default;
        return false;
    }

    bool TryGetDragInput(out DragPhase phase, out Vector2 screenPosition, out int fingerId)
    {
        phase = DragPhase.None;
        screenPosition = default;
        fingerId = -1;

        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            screenPosition = touch.position;
            fingerId = touch.fingerId;

            if (touch.phase == UnityEngine.TouchPhase.Began)
            {
                phase = DragPhase.Started;
                return true;
            }

            if (touch.phase == UnityEngine.TouchPhase.Moved || touch.phase == UnityEngine.TouchPhase.Stationary)
            {
                phase = DragPhase.Moved;
                return true;
            }

            if (touch.phase == UnityEngine.TouchPhase.Ended || touch.phase == UnityEngine.TouchPhase.Canceled)
            {
                phase = DragPhase.Ended;
                return true;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            phase = DragPhase.Started;
            screenPosition = Input.mousePosition;
            fingerId = -1;
            return true;
        }

        if (Input.GetMouseButton(0))
        {
            phase = DragPhase.Moved;
            screenPosition = Input.mousePosition;
            fingerId = -1;
            return true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            phase = DragPhase.Ended;
            screenPosition = Input.mousePosition;
            fingerId = -1;
            return true;
        }

        return false;
    }

    public void SetTargetLocalPosition(Vector3 localPosition, bool instant)
    {
        localPosition.y = fixedLocalHeight;
        localPosition.x = Mathf.Clamp(localPosition.x, -maxLocalDistance, maxLocalDistance);
        localPosition.z = Mathf.Clamp(localPosition.z, -maxLocalDistance, maxLocalDistance);
        targetLocalPosition = localPosition;

        if (instant)
        {
            transform.localPosition = targetLocalPosition;
        }
    }

    enum DragPhase
    {
        None,
        Started,
        Moved,
        Ended
    }
}
