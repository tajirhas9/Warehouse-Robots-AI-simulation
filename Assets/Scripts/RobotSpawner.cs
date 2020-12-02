using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject robotObject;
    [SerializeField, Range(10,20)]
    private int minimumRobot;

    [SerializeField, Range(20,30)]
    private int maximumRobot;

    private int numberOfRobots;

    private void Awake() {
        numberOfRobots = Random.Range(minimumRobot, maximumRobot);

        for(int i = 0; i < numberOfRobots; ++i) {
            Instantiate(robotObject);
        }
    }

    public void DestroyEpisodeRobots() {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Robot");

        for(int i = 0; i < gameObjects.Length; ++i) {
            Destroy(gameObjects[i]);
        }
    }

    public void Spawn() {
        numberOfRobots = Random.Range(10, 20);

    //    Debug.Log("Robots on this episode: " + numberOfRobots);


        for (int i = 0; i < numberOfRobots; ++i) {
            Instantiate(robotObject, transform.parent, false);
        }
    }

    public void Respawn() {
        this.Spawn();
    }
}
