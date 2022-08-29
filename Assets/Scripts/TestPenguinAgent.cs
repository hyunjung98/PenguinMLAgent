using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class TestPenguinAgent : Agent
{
    [Tooltip("How fast the agent moves forward")]
    public float moveSpeed = 5f;
    [Tooltip("How fast the agent turns")]
    public float turnSpeed = 180f;
    [Tooltip("Prefab of the heart that appears when the baby is fed")]
    public GameObject heartPrefab;
    [Tooltip("Prefab of the regurgitated fish that appears when the baby is fed")]
    public GameObject regurgitatedFishPrefab;

    private TestPenguinArea testPenguinArea;
    private Rigidbody rBody;
    private GameObject babyGo;
    private bool isFull;    // 배가 부른가?

    public override void Initialize()
    {
        this.testPenguinArea = this.GetComponentInParent<TestPenguinArea>();
        this.babyGo = this.testPenguinArea.testPenguinBabyGo;
        this.rBody = this.GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        this.isFull = false;
        this.testPenguinArea.ResetArea();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.isFull); // 1
        sensor.AddObservation(Vector3.Distance(this.babyGo.transform.position, this.transform.position));   //1
        sensor.AddObservation((this.babyGo.transform.position - this.transform.position).normalized);   //3
        sensor.AddObservation(this.transform.forward);  //3
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float forwardAmount = actions.DiscreteActions[0];

        float turnAmount = 0f;
        if (actions.DiscreteActions[1] == 1f)
            turnAmount = -1f;
        else if (actions.DiscreteActions[1] == 2f)
            turnAmount = 1f;

        this.rBody.MovePosition(this.transform.position + this.transform.forward * forwardAmount * this.moveSpeed * Time.fixedTime);
        this.transform.Rotate(this.transform.up * turnAmount * turnSpeed * Time.deltaTime);

        if (MaxStep > 0)
            AddReward(-1f / MaxStep);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("fish"))
            this.EatFish(collision.gameObject);
        else if (collision.transform.CompareTag("baby"))
            this.RegurgitateFish();
    }

    private void EatFish(GameObject fishGo)
    {
        if (this.isFull) return;    // 배부른 동안에는 먹지 않음
        isFull = true;
        this.testPenguinArea.RemoveSpecificFish(fishGo);
        AddReward(1f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forwardAction = 0;
        int turnAction = 0;
        if (Input.GetKey(KeyCode.W))        // 상
            forwardAction = 1;
        if (Input.GetKey(KeyCode.A))        // 좌
            turnAction = 1;
        else if (Input.GetKey(KeyCode.D))   // 우
            turnAction = 2;

        actionsOut.DiscreteActions.Array[0] = forwardAction;
        actionsOut.DiscreteActions.Array[1] = turnAction;
    }
}
