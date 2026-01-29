using UnityEngine;
using UnityEngine.EventSystems;

public class NodeFX : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color color;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    void Start()
    {
        spriteRenderer.enabled = false;
        spriteRenderer.color = color;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sortingLayerName = "UI";
        Debug.Log("OnBeginDrag");
        audioSource.PlayOneShot(audioClip);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        spriteRenderer.enabled = false;
    }
}
