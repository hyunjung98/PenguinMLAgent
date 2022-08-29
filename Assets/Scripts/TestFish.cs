using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFish : MonoBehaviour
{
    [Tooltip("The swim speed")]
    public float fishSpeed;

    private float randomizedSpeed = 0f;
    private float nextActionTime = -1f;
    private Vector3 targetPosition;

    private void FixedUpdate()
    {
        if (this.fishSpeed > 0f)
            Swim();
    }

    private void Swim()
    {
        if (Time.fixedTime >= this.nextActionTime)
        {
            this.randomizedSpeed = this.fishSpeed * UnityEngine.Random.Range(.5f, 1.5f);
            this.targetPosition = TestPenguinArea.ChooseRandomPosition(this.transform.position, 100f, 260f, 2f, 13f);

            this.transform.rotation = Quaternion.LookRotation(this.targetPosition - this.transform.position, Vector3.up);

            float timeToGetThere = Vector3.Distance(this.transform.position, this.targetPosition) / this.randomizedSpeed;
            this.nextActionTime = Time.fixedTime + timeToGetThere;
        }
        else
        {
            Vector3 moveVector = this.randomizedSpeed * this.transform.forward * Time.fixedDeltaTime;
            if (moveVector.magnitude <= Vector3.Distance(this.transform.position, this.targetPosition))
            {
                this.transform.position += moveVector;
            }
            else
            {
                this.transform.position = this.targetPosition;
                this.nextActionTime = Time.fixedTime;
            }
        }
    }
}
