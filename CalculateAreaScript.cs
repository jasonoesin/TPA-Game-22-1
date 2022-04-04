using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateAreaScript : MonoBehaviour
{
    private float totalFieldX = 1200f ;
    private float totalFieldZ = 1200f;
    private float startingPointX = 0f;
    private float startingPointZ = 0f;

    public static bool[,] validatedArea = new bool[9999,9999];

    // Start is called before the first frame update
    void Start()
    {
        for (float i = startingPointX; i <= totalFieldX; i++)
        {
            for (float j = startingPointZ; j <= totalFieldZ; j++)
            {
                if (validatePoint(i, j, i + 1, j + 1))
                {
                    validatedArea[Mathf.FloorToInt(i), Mathf.FloorToInt(j)] = true;
                }
            }
        }
    }

    public bool validatePoint(float x, float z, float xPlus, float zPlus)
    {
        float midX = (x + xPlus) / 2f;
        float midZ = (z + zPlus) / 2f;
        
        float y = Terrain.activeTerrain.SampleHeight(new Vector3(midX, Mathf.Infinity, midZ));
        Vector3 tempPos = new Vector3(midX, y, midZ);

        float personHeight = 1f;
        float radiusHeight = 3.5f;
        float radiusCheck = 0.5f;

        Vector3 bottom = new Vector3(midX, y +personHeight ,midZ);
        Vector3 upper = new Vector3(midX, y + radiusHeight, midZ);

        if (!Physics.CheckCapsule(bottom, upper, radiusCheck))
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
}
