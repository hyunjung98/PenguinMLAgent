using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestPenguinArea : MonoBehaviour
{
    public TestPenguinAgent testPenguinAgent;
    public GameObject testPenguinBabyGo;
    public TextMeshPro cumulativeRewardText;
    public Fish fishPrefab;
    private List<GameObject> fishList;

    public int FishRemaining
    {
        get { return fishList.Count; }
    }

    public void ResetArea()
    {
        
    }

    public void RemoveSpecificFish(GameObject fishGo)
    {
        this.fishList.Remove(fishGo);
        Destroy(fishGo);
    }

    public static Vector3 ChooseRandomPosition(Vector3 center, float minAngle, float maxAngle, float minRadius, float maxRadius)
    {
        float radius = minRadius;
        float angle = minAngle;

        if (maxRadius > minRadius)
            radius = UnityEngine.Random.Range(minRadius, maxRadius);
        if (maxAngle > minAngle)
            angle = UnityEngine.Random.Range(minAngle, maxAngle);

        return center + Quaternion.Euler(0f, angle, 0f) * Vector3.forward * radius; 
    }

    //private void RemoveAllFish()
    //{
    //    if(this.fishList != null)
    //    {
    //        for (int i = 0; i < fishList.Count; i++)
    //        {
    //            if (fishList[i] != null)
    //            {
    //                Destroy(this.fishList[i]);
    //            }
    //        }
    //    }

    //    this.fishList = new List<GameObject>();
    //}

}
