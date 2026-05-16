using UnityEngine;

[RequireComponent(typeof(HW20DragOnTargetPlane))]
public class HW20SnapToZone : MonoBehaviour
{
    [Header("Snap Zone")]
    public Vector3 snapLocalPosition = new Vector3(2.4f, 0.75f, 0f);
    public float snapRadius = 1.9f;
    public bool createZoneVisual = true;

    [Header("Snap Feedback")]
    public Color snappedColor = new Color(0.1f, 0.9f, 0.45f, 1f);
    public Color zoneIdleColor = new Color(0.02f, 0.95f, 0.38f, 0.72f);
    public Color zoneActiveColor = new Color(1f, 0.72f, 0.04f, 0.9f);
    public float feedbackDuration = 0.35f;

    HW20DragOnTargetPlane drag;
    Renderer objectRenderer;
    Renderer zoneRenderer;
    Material objectMaterial;
    Material zoneMaterial;
    Transform zoneVisual;
    bool wasDragging;
    float feedbackTimer;
    Color restoreColor;

    void Awake()
    {
        drag = GetComponent<HW20DragOnTargetPlane>();
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            objectMaterial = objectRenderer.material;
            restoreColor = objectMaterial.color;
        }

        if (createZoneVisual)
        {
            CreateZoneVisual();
        }
    }

    void Update()
    {
        if (objectMaterial != null && feedbackTimer > 0f)
        {
            feedbackTimer -= Time.deltaTime;
            if (feedbackTimer <= 0f)
            {
                objectMaterial.color = restoreColor;
            }
        }

        var nearZone = IsNearSnapZone();
        UpdateZoneFeedback(nearZone);

        if (wasDragging && !drag.IsDragging)
        {
            TrySnapAfterRelease();
        }

        wasDragging = drag.IsDragging;
    }

    void TrySnapAfterRelease()
    {
        if (!IsNearSnapZone())
        {
            return;
        }

        drag.SetTargetLocalPosition(snapLocalPosition, false);
        PlaySnapFeedback();
        Debug.Log("HW20 Snap activated");
    }

    bool IsNearSnapZone()
    {
        var current = transform.localPosition;
        var snap = snapLocalPosition;
        current.y = 0f;
        snap.y = 0f;
        return Vector3.Distance(current, snap) <= snapRadius;
    }

    void PlaySnapFeedback()
    {
        if (objectMaterial != null)
        {
            restoreColor = objectMaterial.color;
            objectMaterial.color = snappedColor;
            feedbackTimer = feedbackDuration;
        }
    }

    void UpdateZoneFeedback(bool nearZone)
    {
        if (zoneMaterial == null)
        {
            return;
        }

        zoneMaterial.color = nearZone ? zoneActiveColor : zoneIdleColor;

        if (zoneVisual != null)
        {
            var pulse = nearZone ? 1.15f : 1f;
            zoneVisual.localScale = new Vector3(snapRadius * 2f * pulse, 0.06f, snapRadius * 2f * pulse);
        }
    }

    void CreateZoneVisual()
    {
        var zone = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        zone.name = "HW20_Snap_Zone";
        zone.transform.SetParent(transform.parent, false);
        zone.transform.localPosition = new Vector3(snapLocalPosition.x, 0.03f, snapLocalPosition.z);
        zone.transform.localRotation = Quaternion.identity;
        zone.transform.localScale = new Vector3(snapRadius * 2f, 0.06f, snapRadius * 2f);

        var collider = zone.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        zoneRenderer = zone.GetComponent<Renderer>();
        if (zoneRenderer != null)
        {
            zoneMaterial = new Material(Shader.Find("Standard"));
            zoneMaterial.color = zoneIdleColor;
            zoneMaterial.SetFloat("_Mode", 3f);
            zoneMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            zoneMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            zoneMaterial.SetInt("_ZWrite", 0);
            zoneMaterial.DisableKeyword("_ALPHATEST_ON");
            zoneMaterial.EnableKeyword("_ALPHABLEND_ON");
            zoneMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            zoneMaterial.renderQueue = 3000;
            zoneRenderer.material = zoneMaterial;
        }

        zoneVisual = zone.transform;
    }
}
