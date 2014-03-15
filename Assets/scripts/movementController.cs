using UnityEngine;
using System.Collections;

public class movementController : MonoBehaviour {

	private Transform transformToMove = null;
	private float speed;
	// Use this for initialization
	void Start () {
	
	}

	public void setTransformtToMove(Transform t, float speedToApply) {
		if(t.tag == "cube0" || t.tag == "cube1" || t.tag == "cube2" || t.tag == "cube3") {
			transformToMove = t;
			speed = speedToApply;
		}
	}

	public void moveTo(Transform t, Vector3 newPosition) {
	
		t.position = newPosition;
	}

	public void stopMovement() {
		transformToMove = null;
	}

	// Update is called once per frame
	void Update () {

		if(transformToMove != null) {
			//Debug.Log("Update method "+this.transform.tag);
			transformToMove.Translate(Vector3.up * speed * Time.deltaTime);
		}

	}
}
