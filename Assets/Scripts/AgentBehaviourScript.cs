using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AgentBehaviourScript : MonoBehaviour
{
    private PathFinderAgent brain;
    [SerializeField]
    private GameObject successTextObject;
    private TMP_Text successText;
    private int successCount = 0;

    void Awake() {
        brain = GetComponent<PathFinderAgent>();
        successText = successTextObject.GetComponent<TMP_Text>();
        successText.text = "0";
        successCount = 0;
    }

    void Update() {
        if(transform.localPosition.y < 0) {
            Debug.Log("Agent fell off...");
            brain.Penalize(-0.075f);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            Debug.Log("Collided with other players...");
   
            brain.Penalize(-0.05f);
        } else if(collision.gameObject.tag == "Goal") {
            Debug.Log("Reached goal!!!");

            brain.GiveReward(1.0f);

            successCount++;

            successText.text = string.Format("{0}", successCount);
        }
    }
}
