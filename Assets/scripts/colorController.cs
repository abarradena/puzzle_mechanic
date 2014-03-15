using UnityEngine;
using System.Collections;

public class colorController : MonoBehaviour {

	private int colorSelected = 0;
	private Color color;

	// Use this for initialization
	void Start () {
	
	}
	
	public void changeColor() {
	
		colorSelected ++;
		if (colorSelected == 3) colorSelected = 0;
		
		switch(colorSelected){
		case 0:
			color = Color.yellow;
			break;
		case 1:
			color = Color.red;
			break;
		case 2:
			color = Color.blue;
			break;
			
		}
		
		GameObject.FindWithTag ("cube0").renderer.material.color = color;
		GameObject.FindWithTag ("cube1").renderer.material.color = color;
		GameObject.FindWithTag ("cube2").renderer.material.color = color;
		GameObject.FindWithTag ("cube3").renderer.material.color = color;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
