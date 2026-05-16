using UnityEngine;
using UnityEngine.EventSystems;

public class Trigger_PhysicalMousePointer : MonoBehaviour
{
    public GameObject InterfaceObject;
    public GameObject SenderObject;

    Camera mainCamera;
    IInteractable interactable;

    void Awake()
    {
        mainCamera = Camera.main;
        if (InterfaceObject == null) InterfaceObject = gameObject;
        if (SenderObject == null) SenderObject = gameObject;
        interactable = InterfaceObject.GetComponent<IInteractable>();
    }

    void Update()
    {
        if (!WasPressed(out var screenPosition))
        {
            return;
        }

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera == null) return;

        var ray = mainCamera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out var hit))
        {
            Debug.Log($"Object clicked: {hit.transform.name}");
            interactable?.OnClick(SenderObject);
        }
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
