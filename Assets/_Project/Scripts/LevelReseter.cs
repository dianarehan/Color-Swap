using UnityEngine;

public class LevelReseter : MonoBehaviour
{
    [SerializeField] private Transform[] initialPositions;
    [SerializeField] private GameObject[] nodes;

    private void ResetLevel()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].transform.position = initialPositions[i].position;
        }
    }

    void OnEnable()
    {
        ResetLevel();
        Debug.Log("Level Reset");
    }
}
