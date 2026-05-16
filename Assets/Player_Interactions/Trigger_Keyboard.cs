using UnityEngine;

public class Trigger_Keyboard : MonoBehaviour
{
    public KeyCode Key = KeyCode.Space;
    public GameObject InterfaceObject;
    public GameObject SenderObject;

    IInteractable interactable;

    void Awake()
    {
        if (InterfaceObject == null) InterfaceObject = gameObject;
        if (SenderObject == null) SenderObject = gameObject;
        interactable = InterfaceObject.GetComponent<IInteractable>();
    }

    void Update()
    {
        if (interactable == null) return;

        if (Input.GetKeyDown(Key))
        {
            interactable.OnEnter(SenderObject);
        }

        if (Input.GetKey(Key))
        {
            interactable.OnStay(SenderObject);
        }

        if (Input.GetKeyUp(Key))
        {
            interactable.OnClick(SenderObject);
            interactable.OnExit(SenderObject);
        }
    }
}
