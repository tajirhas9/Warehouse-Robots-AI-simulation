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
   
            brain.Penalize(-0.075f);
        } else if (collision.gameObject.tag == "Robot") {
            Debug.Log("Collided with other robot...");

            brain.Penalize(-0.075f);
        } else if(collision.gameObject.tag == "Goal") {
            Debug.Log("Reached goal!!!");

            successCount++;

            successText.text = string.Format("{0}", successCount);

            StartCoroutine(UpdateUIForSuccess());
        }
    }

    private IEnumerator UpdateUIForSuccess() {

        Color originalColor = gameObject.GetComponent<Renderer>().material.color;
        bool blink = false;

        brain.waitToEndEpisode = true;

        for (int i = 0; i <= 10; ++i) {
            if(blink) {
                gameObject.GetComponent<Renderer>().material.color = new Color(0f, 255f, 0f);
            } else {
                gameObject.GetComponent<Renderer>().material.color = originalColor;
            }

            blink = !blink;
            yield return new WaitForSeconds(0.2f);

        }
        gameObject.GetComponent<Renderer>().material.color = originalColor;
        brain.GiveReward(1.0f);
    }
}
