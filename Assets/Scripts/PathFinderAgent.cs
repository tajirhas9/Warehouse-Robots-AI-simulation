using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using TMPro;

public class PathFinderAgent : Agent {

	[SerializeField]
	private GameObject episodeTextObject;
	[SerializeField]
	private GameObject rewardTextObject;

	private TMP_Text episodeText;
	private TMP_Text rewardText;

	[SerializeField]
	private float speed = 10.0f;
	[SerializeField]
	private float stepAmount;

	[SerializeField]
	private GameObject target;
	private Vector3 originalPosition;
	private Vector3 moveTo = Vector3.zero;
	private bool moveInProgress = false;

	private float overallReward = 0;
	private int direction = 0;
	private float totalDistance;
	private float previousDistance = 0f;

	public enum MoveToDirection {
		Idle,
		Left,
		Right,
		Forward,
		Backward
	}

	private MoveToDirection moveToDirection = MoveToDirection.Idle;

	public override void Initialize() {
		originalPosition = transform.localPosition;
		episodeText = episodeTextObject.GetComponent<TMP_Text>();
		rewardText = rewardTextObject.GetComponent<TMP_Text>();


		rewardText.text = "0";
		episodeText.text = "0";
	}

	public override void OnEpisodeBegin() {
	//	Debug.Log("Episode Begin...");
	//	Debug.Log("Initializing episode...");

		transform.localPosition = moveTo = new Vector3(Random.Range(-8f,8f), originalPosition.y, originalPosition.z);
		target.transform.localPosition = new Vector3(Random.Range(-8f,8f),target.transform.localPosition.y, target.transform.localPosition.z);
		transform.localRotation = Quaternion.identity;
	//	GameObject.Find("Robot spawner").GetComponent<RobotSpawner>().DestroyEpisodeRobots();
	//	GameObject.Find("Robot spawner").GetComponent<RobotSpawner>().Respawn();


		totalDistance = Vector3.Distance(target.transform.localPosition, transform.localPosition);
		previousDistance = totalDistance;

	//	Debug.Log("Initalization Complete...");

		moveInProgress = false;
	}

	public override void CollectObservations(VectorSensor sensor) {
	//	Debug.Log("Registering observations...");
		sensor.AddObservation(transform.localPosition);
		sensor.AddObservation(target.transform.localPosition);
	//	Debug.Log("Observations registered...");
	}

	public override void OnActionReceived(float[] vectorAction) {
		if(moveInProgress)
			return;
		direction = Mathf.FloorToInt(vectorAction[0]);

        switch (direction) {
            case 0: // idle
				moveTo = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - stepAmount);
				moveToDirection = MoveToDirection.Backward;
				moveInProgress = true;
				break;
			case 1: // left
                moveTo = new Vector3(transform.localPosition.x - stepAmount, transform.localPosition.y, transform.localPosition.z);
                moveToDirection = MoveToDirection.Left;
				transform.Rotate(0f, -90f, 0f);
				moveInProgress = true;
                break;
            case 2: // right
                moveTo = new Vector3(transform.localPosition.x + stepAmount, transform.localPosition.y, transform.localPosition.z);
                moveToDirection = MoveToDirection.Right;
				transform.Rotate(0f, 90f, 0f);
				moveInProgress = true;
                break;
            case 3: // forward
                moveTo = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + stepAmount);
                moveToDirection = MoveToDirection.Forward;
                moveInProgress = true;
                break;
        }
		
	}

	public override void Heuristic(float[] actionsOut) {

		//idle
        actionsOut[0] = 0;

        //move left
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            actionsOut[0] = 1;
        }

        //move right
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            actionsOut[0] = 2;
        }

        //move forward
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            actionsOut[0] = 3;
        }
    }

	public void Penalize(float penalty) {
	//	Debug.Log("Penalizing...");
		AddReward(penalty);
		UpdateUI();
		EndEpisode();
	}

	public void GiveReward(float reward) {
	//	Debug.Log("Rewarding...");
		AddReward(reward);
		UpdateUI();
		EndEpisode();
	}

	private void UpdateUI() {

		overallReward += this.GetCumulativeReward();

        rewardText.text = $"{overallReward.ToString("F2")}";
        episodeText.text = $"{ this.CompletedEpisodes }";
	}

    private void FixedUpdate() {
		if (!moveInProgress) {
		//	Debug.Log("Requesting Decision...");
			RequestDecision();
		}
    }

    void Update() {
		if(!moveInProgress)
			return;

		transform.localPosition = Vector3.MoveTowards(transform.localPosition, moveTo, Time.deltaTime * speed);

		if (Vector3.Distance(transform.localPosition, moveTo) <= 0.00001f) {
            moveInProgress = false;
			float distanceRemain = Vector3.Distance(target.transform.localPosition, transform.localPosition);
			float distanceCovered = totalDistance - distanceRemain;
			float rewardPoint = (distanceCovered / totalDistance) * 0.001f;

			if(distanceRemain > previousDistance) {
				AddReward(-(rewardPoint * 0.5f) );
				UpdateUI();
			} else {
				AddReward(rewardPoint);
				UpdateUI();
			}
			previousDistance = distanceRemain;
		}
    }
}

















