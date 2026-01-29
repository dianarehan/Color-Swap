using UnityEngine;
using UnityEngine.EventSystems;

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
    }
}