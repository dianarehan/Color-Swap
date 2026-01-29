using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelData currentLevelData; 
    [SerializeField] private List<Node> allNodes; 
    
    public bool isLevelComplete = false;
    
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

            // If any connected nodes share the same color, the player hasn't won yet
            if (nodeA.currentColor == nodeB.currentColor)
            {
                return; 
            }
        }
        
        OnWin();
    }

    private void OnWin()
    {
        isLevelComplete = true;
        Debug.Log("Excellence!"); // We will trigger the UI panel here next
    }
}