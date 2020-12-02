using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehaviour : MonoBehaviour
{
	[SerializeField, Range(1,20)]
	private float speed;

	private bool moveInProgress;
	private Vector3 moveTo;
	private Vector3 currentDirection;
	private Vector3 spawnPosition;

	private Vector3 ChooseRandomDirection() {
		int x = Random.Range(0,3);

		if (x == 0)
			return Vector3.forward;
		else if (x == 1)
			return Vector3.left;
		else
			return Vector3.right;
	}

    private void Awake() {
		Respawn();
    }

    void Update() {
		if (moveInProgress) {
			if(Vector3.Distance(transform.localPosition , moveTo) <= 0.1) {
				moveInProgress = false;
            } else {
				transform.localPosition += (currentDirection * speed * Time.deltaTime);

				if(transform.localPosition.y < 0) {
					Respawn();
                }

			}
		} else {
			currentDirection = ChooseRandomDirection();

            moveTo = transform.localPosition + (currentDirection * speed);

			moveInProgress = true;
		}
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Wall") {
			FlipDirection();
		}
    }

	private void FlipDirection() {
		currentDirection *= (-1);
	}

    private void Respawn() {

		Vector3 agentPosition = GameObject.Find("Agent").transform.localPosition;

		while(true) {

			spawnPosition = new Vector3(Random.Range(-9, 9), 0.15f, Random.Range(-9, 9));

			if(Vector3.Distance(spawnPosition, agentPosition) > 1) {
				break;
            }
		}

		transform.localPosition = spawnPosition;

		moveInProgress = false;
    }
}
