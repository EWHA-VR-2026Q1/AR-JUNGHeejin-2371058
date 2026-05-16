using UnityEngine;

public class Trigger_Snap : MonoBehaviour
{
    [SerializeField] string snapPointTag = "Target";

    bool hasHitSnapPoint;
    Vector3 snapTargetPosition;

    void Update()
    {
        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            CheckAndSnap();
            GetComponent<IInteractable>()?.OnClick();
        }
    }

    void CheckAndSnap()
    {
        if (!hasHitSnapPoint)
        {
            return;
        }

        transform.position = snapTargetPosition;
        hasHitSnapPoint = false;
        Debug.Log($"[Snap] {gameObject.name} snapped to {snapTargetPosition}.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(snapPointTag))
        {
            hasHitSnapPoint = true;
            snapTargetPosition = other.transform.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(snapPointTag))
        {
            hasHitSnapPoint = false;
        }
    }
}
