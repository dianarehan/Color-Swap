using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelData currentLevelData; 
    [SerializeField] private List<Node> allNodes; 
    
    public bool isLevelComplete = false;

    void Start()
    {
        if (allNodes != null)
        {
            for (int i = 0; i < allNodes.Count; i++)
            {
                if (allNodes[i] != null && allNodes[i].nodeIndex != i)
                {
                    Debug.LogError($"SETUP ERROR: Node at List Index {i} has `nodeIndex` {allNodes[i].nodeIndex}. They MUST match! Please re-number your Nodes or re-order the list.");
                }
            }
        }
    }
    
    public bool IsConnected(Node a, Node b)
    {
        foreach (var conn in currentLevelData.connections)
        {
            if ((conn.nodeAIndex == a.nodeIndex && conn.nodeBIndex == b.nodeIndex) ||
                (conn.nodeAIndex == b.nodeIndex && conn.nodeBIndex == a.nodeIndex))
            {
                return true;
            }
        }
        return false;
    }

    public void CheckWinCondition()
    {
        if (isLevelComplete) return;

        foreach (var connection in currentLevelData.connections)
        {
            if (connection.nodeAIndex >= allNodes.Count || connection.nodeBIndex >= allNodes.Count)
            {
                Debug.LogError($"Level Data Error: Connection references node index {Mathf.Max(connection.nodeAIndex, connection.nodeBIndex)} but there are only {allNodes.Count} nodes in the list.");
                return;
            }

            Node nodeA = allNodes[connection.nodeAIndex];
            Node nodeB = allNodes[connection.nodeBIndex];

            if (nodeA.currentColor == nodeB.currentColor)
            {
                Debug.Log($"Win Check Failed: Node {nodeA.nodeIndex} ({nodeA.currentColor}) and Node {nodeB.nodeIndex} ({nodeB.currentColor}) are connected and share the same color.");
                return; 
            }
        }
        
        OnWin();
    }

    private void OnWin()
    {
        isLevelComplete = true;
        Debug.Log("Excellence!");
    }
}