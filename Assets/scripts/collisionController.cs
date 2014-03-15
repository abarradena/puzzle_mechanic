using UnityEngine;
using System.Collections;

public class collisionController : MonoBehaviour {

	private Vector3 initialCubePosition = new Vector3();

	// Use this for initialization
	void Start () {
	
	}

	void OnEnable() {
		initialCubePosition = transform.position;
	}
	
	void  OnTriggerEnter (Collider other) {
		// Do something

		Debug.Log(this.transform.tag +" Initial position at OnTriggerEnter (" + this.transform.tag.Substring(this.transform.tag.Length-1,1) + ")" + initialCubePosition.ToString() + " Other " + other.name);
		
		cubeCreator cubeMgr = GameObject.Find("cubeCreator").GetComponent<cubeCreator>();
		cubeMgr.insertSlideCube(int.Parse(this.transform.tag.Substring(this.transform.tag.Length-1,1)), this.renderer.material.color);

		this.renderer.enabled = false;

		movementController movementCrl = GameObject.Find("launcherControl").GetComponent<movementController>();
		movementCrl.stopMovement();
		movementCrl.moveTo(this.transform, initialCubePosition);
		
		StartCoroutine(Wait());
	}
	
	IEnumerator Wait(){

		yield return new WaitForSeconds (1.0f);
		this.renderer.enabled = true;
		
	}

	// Update is called once per frame
	void Update () {
	
	}
}
