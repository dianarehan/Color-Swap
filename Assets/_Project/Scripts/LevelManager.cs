using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{   
    [SerializeField] private int currentLevelIndex = 0;
    [SerializeField] private LevelData[] levelDataArray; 
    [SerializeField] private GameObject[] Levels;
    
    [SerializeField] private GameObject winPanel;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;

    private bool isLevelComplete = false;
    private bool allLevelsCompleted = false;
    private Node[] allNodesList; 

    void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
        
        InitializeLevel(currentLevelIndex);
        UpdateUIState();
    }
    
    void InitializeLevel(int index)
    {
        for (int i = 0; i < Levels.Length; i++)
        {
            if (Levels[i] != null) Levels[i].SetActive(i == index);
        }
        
        if (Levels[index] != null)
        {
            Transform nodesContainer = Levels[index].transform.Find("Nodes");
            if (nodesContainer != null)
                allNodesList = nodesContainer.GetComponentsInChildren<Node>();
            else
                allNodesList = Levels[index].GetComponentsInChildren<Node>();
        }
        
        isLevelComplete = false;
        if (winPanel != null) winPanel.SetActive(false);
    }
    
    public void LoadNextLevel()
    {
        if (currentLevelIndex < Levels.Length - 1)
        {
            currentLevelIndex++;
            InitializeLevel(currentLevelIndex);
            UpdateUIState();
        }
        else
        {
            Debug.Log("Already at max level");
        }
    }

    public void LoadPreviousLevel()
    {
        if (currentLevelIndex > 0)
        {
            currentLevelIndex--;
            InitializeLevel(currentLevelIndex);
            UpdateUIState();
        }
    }

    void UpdateUIState()
    {
        if (previousButton != null) previousButton.interactable = (currentLevelIndex > 0);
        if (nextButton != null) nextButton.interactable = (currentLevelIndex < Levels.Length - 1);
    }

    public bool IsConnected(Node a, Node b)
    {
        if (currentLevelIndex >= levelDataArray.Length) return false;

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
        
        // Show Win Panel instead of auto-loading
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            UpdateUIState(); // Refresh buttons based on current level
        }
    }
}