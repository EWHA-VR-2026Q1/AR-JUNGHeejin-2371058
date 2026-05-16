using UnityEngine;

public class HW20RealityJudgementGuides : MonoBehaviour
{
    [Header("Ground Contact")]
    public bool createShadow = true;
    public Vector3 shadowScale = new Vector3(2.25f, 0.025f, 2.25f);
    public Color shadowColor = new Color(0f, 0f, 0f, 0.38f);

    [Header("Distance Guide")]
    public bool createDistanceRuler = true;
    public float rulerZ = -2.6f;
    public float rulerLength = 6f;
    public int tickCount = 5;
    public Color rulerColor = new Color(1f, 0.95f, 0.22f, 1f);

    [Header("Direction Guide")]
    public bool createDirectionArrow = true;
    public Vector3 arrowOrigin = new Vector3(-3.2f, 0.08f, 0f);
    public Color arrowColor = new Color(1f, 0.24f, 0.18f, 1f);

    Transform guideRoot;
    Transform shadow;

    void Awake()
    {
        guideRoot = new GameObject("HW20_Reality_Judgement_Guides").transform;
        guideRoot.SetParent(transform.parent, false);

        if (createShadow)
        {
            shadow = CreateCylinder("GroundContact_Shadow", Vector3.zero, shadowScale, shadowColor, false);
        }

        if (createDistanceRuler)
        {
            CreateDistanceRuler();
        }

        if (createDirectionArrow)
        {
            CreateDirectionArrow();
        }
    }

    void LateUpdate()
    {
        if (shadow == null)
        {
            return;
        }

        shadow.localPosition = new Vector3(transform.localPosition.x, 0.025f, transform.localPosition.z);
    }

    void CreateDistanceRuler()
    {
        CreateCube(
            "Distance_Ruler_Line",
            new Vector3(0f, 0.055f, rulerZ),
            new Vector3(rulerLength, 0.05f, 0.08f),
            rulerColor);

        var start = -rulerLength * 0.5f;
        var step = rulerLength / Mathf.Max(1, tickCount - 1);

        for (var i = 0; i < tickCount; i++)
        {
            var x = start + step * i;
            var height = i == 0 || i == tickCount - 1 ? 0.65f : 0.42f;
            CreateCube(
                "Distance_Tick_" + (i + 1),
                new Vector3(x, 0.08f, rulerZ),
                new Vector3(0.08f, 0.08f, height),
                rulerColor);
        }
    }

    void CreateDirectionArrow()
    {
        CreateCube(
            "Direction_Arrow_Shaft",
            arrowOrigin + new Vector3(0f, 0f, 1.05f),
            new Vector3(0.18f, 0.1f, 1.9f),
            arrowColor);

        var head = CreatePrimitive("Direction_Arrow_Head", PrimitiveType.Cylinder, arrowOrigin + new Vector3(0f, 0f, 2.15f), new Vector3(0.55f, 0.16f, 0.55f), arrowColor);
        head.localRotation = Quaternion.Euler(90f, 0f, 0f);
    }

    Transform CreateCube(string objectName, Vector3 localPosition, Vector3 localScale, Color color)
    {
        return CreatePrimitive(objectName, PrimitiveType.Cube, localPosition, localScale, color);
    }

    Transform CreateCylinder(string objectName, Vector3 localPosition, Vector3 localScale, Color color, bool opaque)
    {
        return CreatePrimitive(objectName, PrimitiveType.Cylinder, localPosition, localScale, color, !opaque);
    }

    Transform CreatePrimitive(string objectName, PrimitiveType primitiveType, Vector3 localPosition, Vector3 localScale, Color color, bool transparent = false)
    {
        var go = GameObject.CreatePrimitive(primitiveType);
        go.name = objectName;
        go.transform.SetParent(guideRoot, false);
        go.transform.localPosition = localPosition;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = localScale;

        var collider = go.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        var renderer = go.GetComponent<Renderer>();
        if (renderer != null)
        {
            var material = new Material(Shader.Find("Standard"));
            material.color = color;

            if (transparent || color.a < 1f)
            {
                material.SetFloat("_Mode", 3f);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
            }

            renderer.material = material;
        }

        return go.transform;
    }
}
