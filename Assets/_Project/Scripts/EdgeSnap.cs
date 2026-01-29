using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class EdgeSnap : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float lineWidth = 0.2f;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // void Update()
    // {
    //     LinkNodes();
    // }

    [ContextMenu("Link Nodes")]
    void LinkNodes()
    {
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        if (pointA == null || pointB == null) return;

        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        
        lineRenderer.SetPosition(0, pointA.position);
        lineRenderer.SetPosition(1, pointB.position);
    }
}