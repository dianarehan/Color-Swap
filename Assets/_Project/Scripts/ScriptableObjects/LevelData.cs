using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "ColorSwap/LevelData")]
public class LevelData : ScriptableObject
{
    [System.Serializable]
    public struct Connection
    {
        public int nodeAIndex;
        public int nodeBIndex;
    }

    public Connection[] connections;
}