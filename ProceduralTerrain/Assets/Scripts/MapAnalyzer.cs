using UnityEngine;

public class MapAnalyzer : MonoBehaviour
{
    public void PrintHighestValue(Vector3[] vertices)
    {
        if (vertices == null || vertices.Length == 0)
        {
            Debug.LogWarning("The vertices array is null or empty.");
            return;
        }

        float maxHeight = float.MinValue;

        foreach (var vertex in vertices)
        {
            if (vertex.y > maxHeight)
            {
                maxHeight = vertex.y;
            }
        }
        
        Debug.Log($"The highest value in the map is: {maxHeight}");
    }
}