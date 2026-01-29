using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{   
    [SerializeField] private int currentLevelIndex = 0;
    [SerializeField] private LevelData[] levelDataArray; 
    [SerializeField] private GameObject[] Levels;
    bool isLevelComplete = false;
    bool allLevelsCompleted = false;
    private Node[] allNodesList; 

    void Start()
    {
        GetAllNodes();
    }
    
    void GetAllNodes()
    {
        allNodesList = Levels[currentLevelIndex].GetComponentsInChildren<Node>();
    }

    public bool IsConnected(Node a, Node b)
    {
        foreach (var conn in levelDataArray[currentLevelIndex].connections)
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

        foreach (var connection in levelDataArray[currentLevelIndex].connections)
        {
            if (connection.nodeAIndex >= allNodesList.Length || connection.nodeBIndex >= allNodesList.Length)
            {
                Debug.LogError($"Level Data Error: Connection references node index {Mathf.Max(connection.nodeAIndex, connection.nodeBIndex)} but there are only {allNodesList.Length} nodes in the list.");
                return;
            }

            Node nodeA = allNodesList[connection.nodeAIndex];
            Node nodeB = allNodesList[connection.nodeBIndex];

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
        LoadNextLevel();
    }

    void LoadNextLevel()
    {
        if (allLevelsCompleted) return;
        Levels[currentLevelIndex].SetActive(false);
        currentLevelIndex++;
        if (currentLevelIndex >= Levels.Length)
        {
            allLevelsCompleted = true;
            Debug.Log("All levels completed!");
            return;
        }

        Levels[currentLevelIndex].SetActive(true);
        GetAllNodes();
        isLevelComplete = false;
    }
}