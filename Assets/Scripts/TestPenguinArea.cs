using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestPenguinArea : MonoBehaviour
{
    [Tooltip("The agent inside the area")]
    public TestPenguinAgent testPenguinAgent;
    [Tooltip("The baby penguin inside the area")]
    public GameObject testPenguinBabyGo;
    [Tooltip("The TextMeshPro text that shows the cumulative reward of the agent")]
    public TextMeshPro cumulativeRewardText;
    [Tooltip("Prefab of a live fish")]
    public TestFish fishPrefab;

    private List<GameObject> fishList;

    public int FishRemaining
    {
        get { return fishList.Count; }
    }

    private void Start()
    {
        this.ResetArea();
    }

    private void Update()
    {
        this.cumulativeRewardText.text = this.testPenguinAgent.GetCumulativeReward().ToString("0.00");
    }

    public void ResetArea()
    {
        this.RemoveAllFish();
        this.PlacePenguin();
        this.PlaceBaby();
        this.SpawnFish(4, .5f);
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

    private void RemoveAllFish()
    {
        if (this.fishList != null)
        {
            for (int i = 0; i < fishList.Count; i++)
            {
                if (fishList[i] != null)
                {
                    Destroy(this.fishList[i]);
                }
            }
        }

        this.fishList = new List<GameObject>();
    }

    private void PlacePenguin()
    {
        Rigidbody agentRBody = this.testPenguinAgent.GetComponent<Rigidbody>();
        agentRBody.velocity = Vector3.zero;
        agentRBody.angularVelocity = Vector3.zero;
        this.testPenguinAgent.transform.position = ChooseRandomPosition(transform.position, 0f, 360f, 0f, 9f) + Vector3.up * .5f;
        this.testPenguinAgent.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
    }

    private void PlaceBaby()
    {
        Rigidbody babyRBody = this.testPenguinBabyGo.GetComponent<Rigidbody>();
        babyRBody.velocity = Vector3.zero;
        babyRBody.angularVelocity = Vector3.zero;
        this.testPenguinBabyGo.transform.position = ChooseRandomPosition(this.transform.position, -45f, 45f, 4f, 9f) + Vector3.up * .5f;
        this.testPenguinBabyGo.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    private void SpawnFish(int count, float fishSpeed)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject fishGo = Instantiate<GameObject>(this.fishPrefab.gameObject);
            fishGo.transform.position = ChooseRandomPosition(transform.position, 100f, 260f, 2f, 13f) + Vector3.up * .5f;
            fishGo.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

            fishGo.transform.SetParent(transform);
            fishList.Add(fishGo);
            fishGo.GetComponent<Fish>().fishSpeed = fishSpeed;
        }
    }
}
