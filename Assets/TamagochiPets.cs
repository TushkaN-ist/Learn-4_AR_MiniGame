using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TamagochiPets : MonoBehaviour
{

    public Rigidbody rigidbody;
    public float speed = 0.4f;
    public float mlply = 10f;
    public float deltaROtation = 30;

    public float hungry = 1;
    public float deltaHungry = 0.05f;

    Vector3 targetPosition;
    List<Transform> targets = new List<Transform>();

    public TamagochiPets Clone(Vector3 position,Quaternion rotation){
        TamagochiPets pet = Instantiate(this);
        pet.transform.position = position;
        pet.transform.rotation = rotation;
        pet.targetPosition = position;
        return pet;
    }

    public void SetPointToMove(Vector3 position){
        targetPosition = position;
	}

    // Update is called once per frame
    void Update()
    {

        if (targets.Count > 0)
        {
            float dist = float.MaxValue;
            float distT;
            foreach (Transform item in targets)
            {
                if (dist >= (distT = (item.position - transform.position).magnitude))
                {
                    dist = distT;
                    SetPointToMove(item.position);
                }
            }
        }

        Vector3 targetDir = targetPosition - transform.position;
        targetDir.y = 0;
        if (targetDir.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDir), deltaROtation);
            rigidbody.velocity = transform.forward * Mathf.Min(targetDir.magnitude, speed)/Time.deltaTime;
        }
        hungry -= deltaHungry * Time.deltaTime;

        if (hungry < 0)
        {
            foreach (Transform item in targets)
            {
                Destroy(item.gameObject);
            }
            targets.Clear();
            Destroy(gameObject);
        }
    }

	internal void SetTarget(Transform transform)
	{
        targets.Add(transform);
    }

	private void OnCollisionEnter(Collision collision)
	{
        if (targets.Contains(collision.transform))
        {
            hungry = Mathf.Min(hungry+0.5f,1f);
            speed += 0.002f;
            targets.Remove(collision.transform);
            deltaHungry += 0.01f;
            Destroy(collision.gameObject);
        }
	}
}
