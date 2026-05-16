using UnityEngine;

public class Trigger_Drag : MonoBehaviour
{
    public GameObject InterfaceObject;

    Camera mainCamera;
    IInteractable interactable;
    bool isDragging;
    float zDistance;

    void Awake()
    {
        mainCamera = Camera.main;
        if (InterfaceObject == null) InterfaceObject = gameObject;
        interactable = InterfaceObject.GetComponent<IInteractable>();
    }

    void Update()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera == null) return;

        if (WasPressed(out var startPosition))
        {
            StartDrag(startPosition);
        }

        if (isDragging && WasHeld(out var dragPosition))
        {
            ExecuteDrag(dragPosition);
            interactable?.OnStay(gameObject);
        }

        if (isDragging && WasReleased())
        {
            isDragging = false;
            interactable?.OnExit(gameObject);
        }
    }

    void StartDrag(Vector2 screenPosition)
    {
        var ray = mainCamera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out var hit) && hit.transform == transform)
        {
            isDragging = true;
            zDistance = mainCamera.WorldToScreenPoint(transform.position).z;
            interactable?.OnEnter(gameObject);
        }
    }

    void ExecuteDrag(Vector2 screenPosition)
    {
        var world = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, zDistance));
        transform.position = new Vector3(world.x, transform.position.y, world.z);
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

    static bool WasHeld(out Vector2 screenPosition)
    {
        screenPosition = default;
        if (Input.GetMouseButton(0))
        {
            screenPosition = Input.mousePosition;
            return true;
        }
        if (Input.touchCount > 0)
        {
            screenPosition = Input.GetTouch(0).position;
            return true;
        }
        return false;
    }

    static bool WasReleased()
    {
        return Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended);
    }
}
