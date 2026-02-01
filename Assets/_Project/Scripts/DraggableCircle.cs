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

    void Start()
    {
        if (currentNode == null)
            currentNode = GetComponentInParent<Node>();

        if (currentNode != null)
        {
            currentNode.currentInnerCircle = this.gameObject;
            currentNode.currentColor = myColor;
        }
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

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        Node targetNode = null;

        foreach (var hit in hits)
        {
            if (hit.gameObject == this.gameObject) continue;

            if (hit.TryGetComponent<Node>(out Node n))
            {
                if (n == currentNode) continue;

                targetNode = n;
                break;
            }
        }
        
        if (targetNode != null)
        {
            Debug.Log($"Attempting connect: {currentNode.name} (Index {currentNode.nodeIndex}) -> {targetNode.name} (Index {targetNode.nodeIndex})");
            
            if (manager.IsConnected(currentNode, targetNode))
            {
                PerformSwap(targetNode);
                Debug.Log("Swap performed");
                manager.CheckWinCondition();
            }
            else
            {
                Debug.Log($"Connection Failed: No link found between {currentNode.nodeIndex} and {targetNode.nodeIndex} in LevelData.");
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
        Debug.Log($"PerformSwap entered: Moving from {currentNode.name} to {targetNode.name}");
        
        GameObject otherCircleObj = targetNode.currentInnerCircle;
        DraggableCircle otherCircle = otherCircleObj != null ? otherCircleObj.GetComponent<DraggableCircle>() : null;

        Node oldNode = currentNode;
        
        this.transform.DOMove(targetNode.transform.position, 0.25f).SetEase(Ease.OutQuad);
        this.currentNode = targetNode;
        targetNode.currentInnerCircle = this.gameObject;
        targetNode.currentColor = this.myColor;

        if (otherCircle != null)
        {
            otherCircle.transform.DOMove(oldNode.transform.position, 0.25f).SetEase(Ease.OutQuad);
            otherCircle.currentNode = oldNode;
            oldNode.currentInnerCircle = otherCircle.gameObject;
            oldNode.currentColor = otherCircle.myColor;
        }
        else
        {
            oldNode.currentInnerCircle = null;
            oldNode.currentColor = CircleColor.None;
        }
    }

    void ReturnToStart()
    {
        transform.DOMove(startPosition, 0.25f).SetEase(Ease.InQuad);
    }
}