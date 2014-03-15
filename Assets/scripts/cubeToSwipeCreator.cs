using UnityEngine;
using System.Collections;

public class cubeToSwipeCreator : MonoBehaviour {


	public Transform cubeToSlide;
	public float	cubeOffsetX			= 1.8f;
	// Use this for initialization
	void Start () {
		StartCoroutine(createSlideCubes());
	}
	
	IEnumerator createSlideCubes(){
		yield return new WaitForSeconds(.5f);
		Vector3 position = this.transform.position;
		position.x = position.x - ((this.transform.localScale.x));
		
		position.z = -1;
		position.y += -4.2f;
		for(int i = 0; i < 4; i++) {
			cubeToSlide.renderer.material.color = Color.yellow; //new Color((225.0f/255),(36.0f/255),(36.0f/255));
			position.x = i * cubeOffsetX; 
			Transform clone = (Transform) Instantiate(cubeToSlide,position,Quaternion.identity);
			clone.tag = "cube"+i;
		}
	}
}
