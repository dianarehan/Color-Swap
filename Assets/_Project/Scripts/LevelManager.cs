using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public LevelData currentLevelData; 
    public List<Node> allNodes; 
    
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