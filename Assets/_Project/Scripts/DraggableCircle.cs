using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;  

public class DraggableCircle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Node currentNode;
    [SerializeField] private CircleColor myColor;
    private Vector3 startPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        GetComponent<SpriteRenderer>().sortingOrder = 10; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(eventData.position);
        mousePos.z = 0;
        transform.position = mousePos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<SpriteRenderer>().sortingOrder = 5;

        LevelManager manager = FindFirstObjectByType<LevelManager>();

        // Use OverlapCircleAll to detect the Node even if we are overlapping our own collider
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        Node targetNode = null;

        foreach (var hit in hits)
        {
            // Ignore our own collider
            if (hit.gameObject == this.gameObject) continue;

            if (hit.TryGetComponent<Node>(out Node n))
            {
                targetNode = n;
                break; // Found a node, stop looking
            }
        }
        
        if (targetNode != null)
        {
            Debug.Log("Target node found: " + targetNode.name);
            if (manager.IsConnected(currentNode, targetNode))
            {
                PerformSwap(targetNode);
                Debug.Log("Swap performed");
                manager.CheckWinCondition();
            }
            else
            {
                Debug.Log("Nodes are not connected");
                ReturnToStart();
            }
        }
        else
        {
            Debug.Log("No Node component found on overlapped objects. Did you add CircleCollider2D to your Nodes?");
            ReturnToStart();
        }
    }

    void PerformSwap(Node targetNode)
    {
        DraggableCircle otherCircle = targetNode.currentInnerCircle.GetComponent<DraggableCircle>();

        Node oldNode = currentNode;
        
        this.transform.DOMove(targetNode.transform.position, 0.25f).SetEase(Ease.OutQuad);
        this.currentNode = targetNode;
        targetNode.currentInnerCircle = this.gameObject;
        targetNode.currentColor = this.myColor;

        otherCircle.transform.DOMove(oldNode.transform.position, 0.25f).SetEase(Ease.OutQuad);
        otherCircle.currentNode = oldNode;
        oldNode.currentInnerCircle = otherCircle.gameObject;
        oldNode.currentColor = otherCircle.myColor;

    }

    void ReturnToStart()
    {
        transform.DOMove(startPosition, 0.25f).SetEase(Ease.InQuad);
    }
}