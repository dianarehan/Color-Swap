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

        LevelManager manager = FindObjectOfType<LevelManager>();

        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.5f);
        
        if (hit != null && hit.TryGetComponent<Node>(out Node targetNode))
        {
            if (manager.IsConnected(currentNode, targetNode))
            {
                PerformSwap(targetNode);
                manager.CheckWinCondition();
            }
            else
            {
                ReturnToStart();
            }
        }
        else
        {
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