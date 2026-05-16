using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HW21CanvasModeButton : MonoBehaviour
{
    [SerializeField] private Text targetText;
    [SerializeField] private string modeName;
    [SerializeField] private string clickMessage;

    private int clickCount;

    public void Configure(Text text, string canvasModeName, string message)
    {
        targetText = text;
        modeName = canvasModeName;
        clickMessage = message;
    }

    public void UpdateModeText()
    {
        clickCount++;

        if (targetText == null)
        {
            return;
        }

        targetText.text = modeName + "\n" + clickMessage + "\nClick Count: " + clickCount;
    }
}

public class HW21DragRotateObjects : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 0.25f;

    private void Update()
    {
        var mouse = Mouse.current;
        if (mouse == null)
        {
            return;
        }

        var delta = mouse.delta.ReadValue();
        if (delta.sqrMagnitude < 0.01f)
        {
            return;
        }

        transform.Rotate(Vector3.up, -delta.x * rotateSpeed, Space.World);
        transform.Rotate(Vector3.right, delta.y * rotateSpeed, Space.World);
    }
}
