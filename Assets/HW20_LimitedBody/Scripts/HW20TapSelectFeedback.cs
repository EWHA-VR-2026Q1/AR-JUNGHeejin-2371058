using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
#endif

[RequireComponent(typeof(Collider))]
public class HW20TapSelectFeedback : MonoBehaviour
{
    [Header("Tap Feedback")]
    public Color idleColor = new Color(0.12f, 0.42f, 0.95f, 1f);
    public Color selectedColor = new Color(1f, 0.78f, 0.16f, 1f);
    public Vector3 idleScale = new Vector3(2.2f, 2.2f, 2.2f);
    public Vector3 selectedScale = new Vector3(2.8f, 2.8f, 2.8f);
    public float feedbackSpeed = 10f;
    public bool acceptAnyScreenTap = true;

    [Header("Selection Marker")]
    public Transform selectionRing;
    public Color ringColor = new Color(0.1f, 0.9f, 0.45f, 0.55f);

    Renderer targetRenderer;
    Material runtimeMaterial;
    Camera mainCamera;
    bool selected;
    int lastTapFrame = -1;

#if ENABLE_INPUT_SYSTEM
    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }
#endif

    void Awake()
    {
        mainCamera = Camera.main;
        targetRenderer = GetComponent<Renderer>();
        if (targetRenderer != null)
        {
            runtimeMaterial = targetRenderer.material;
            runtimeMaterial.color = idleColor;
        }

        if (selectionRing == null)
        {
            selectionRing = CreateSelectionRing();
        }

        SetSelected(false, true);
    }

    void Update()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (WasTapStarted(out var screenPosition))
        {
            TryHandleTap(screenPosition);
        }

        var targetScale = selected ? selectedScale : idleScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * feedbackSpeed);
        UpdateSelectionRingPosition();
    }

    bool WasTapStarted(out Vector2 screenPosition)
    {
        screenPosition = default;

        if (Input.GetMouseButtonDown(0))
        {
            return RegisterTap(Input.mousePosition, out screenPosition);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == UnityEngine.TouchPhase.Began)
        {
            return RegisterTap(Input.GetTouch(0).position, out screenPosition);
        }

#if ENABLE_INPUT_SYSTEM
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            return RegisterTap(Pointer.current.position.ReadValue(), out screenPosition);
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            return RegisterTap(Mouse.current.position.ReadValue(), out screenPosition);
        }

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            return RegisterTap(Touchscreen.current.primaryTouch.position.ReadValue(), out screenPosition);
        }

        foreach (var touch in Touch.activeTouches)
        {
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                return RegisterTap(touch.screenPosition, out screenPosition);
            }
        }
#endif

        return false;
    }

    bool RegisterTap(Vector2 position, out Vector2 screenPosition)
    {
        screenPosition = position;

        if (lastTapFrame == Time.frameCount)
        {
            return false;
        }

        lastTapFrame = Time.frameCount;
        return true;
    }

    bool IsPointerOverUi()
    {
        if (EventSystem.current == null)
        {
            return false;
        }

        if (Input.touchCount > 0)
        {
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        }

        return EventSystem.current.IsPointerOverGameObject();
    }

    void TryHandleTap(Vector2 screenPosition)
    {
        if (acceptAnyScreenTap)
        {
            SetSelected(!selected, false);
            return;
        }

        if (mainCamera == null)
        {
            return;
        }

        if (IsPointerOverUi())
        {
            return;
        }

        var ray = mainCamera.ScreenPointToRay(screenPosition);
        if (!Physics.Raycast(ray, out var hit))
        {
            return;
        }

        if (hit.transform == transform || hit.transform.IsChildOf(transform))
        {
            SetSelected(!selected, false);
        }
    }

    public void SetSelected(bool value, bool instant)
    {
        selected = value;

        if (runtimeMaterial != null)
        {
            runtimeMaterial.color = selected ? selectedColor : idleColor;
        }

        if (selectionRing != null)
        {
            selectionRing.gameObject.SetActive(selected);
        }

        if (instant)
        {
            transform.localScale = selected ? selectedScale : idleScale;
        }

        if (!instant)
        {
            Debug.Log($"HW20 Tap selected: {selected}");
        }
    }

    Transform CreateSelectionRing()
    {
        var ring = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        ring.name = "Tap_Selected_Ring";
        ring.transform.SetParent(transform.parent, false);
        var ringRadius = Mathf.Max(idleScale.x, idleScale.z) * 1.35f;
        ring.transform.localPosition = new Vector3(transform.localPosition.x, 0.04f, transform.localPosition.z);
        ring.transform.localRotation = Quaternion.identity;
        ring.transform.localScale = new Vector3(ringRadius, 0.02f, ringRadius);

        var collider = ring.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        var renderer = ring.GetComponent<Renderer>();
        if (renderer != null)
        {
            var material = new Material(Shader.Find("Standard"));
            material.color = ringColor;
            material.SetFloat("_Mode", 3f);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
            renderer.material = material;
        }

        ring.SetActive(false);
        return ring.transform;
    }

    void UpdateSelectionRingPosition()
    {
        if (selectionRing == null)
        {
            return;
        }

        selectionRing.localPosition = new Vector3(transform.localPosition.x, 0.04f, transform.localPosition.z);
    }
}
