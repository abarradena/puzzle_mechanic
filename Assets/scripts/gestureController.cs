using UnityEngine;
using System.Collections;

public class gestureController : MonoBehaviour {

	protected TKSwipeRecognizer swipeRecognizer;
	protected TKSwipeDirection currentDirection;
	protected TKAnyTouchRecognizer anyRecognizer;

	private Transform transformToMove;
	protected bool recognized = false;


	private int colorSelected = 0;
	private Color color;


	// Use this for initialization
	void Start () {
	
		anyRecognizer = new TKAnyTouchRecognizer(new TKRect(0, 0, Screen.width, Screen.height));
		anyRecognizer.onEnteredEvent 	+= doStartTouch;
		anyRecognizer.onExitedEvent 	+= doEndTouch;
		
		
		var recognizer = new TKSwipeRecognizer(TKSwipeDirection.Up);
		recognizer.gestureRecognizedEvent += doRecognizeSwipe;
		
		TouchKit.addGestureRecognizer( recognizer );
		TouchKit.addGestureRecognizer(anyRecognizer);
	}


	#region Touch
	protected void doStartTouch(TKAnyTouchRecognizer recognizer){
		recognized = false;
		Debug.Log("doStartTouch method ");
		Ray ray = Camera.main.ScreenPointToRay(recognizer.touchLocation());
		RaycastHit hit;

		if(Physics.Raycast(ray,out hit)){
			transformToMove = hit.transform;
		}else{
			transformToMove = null;
		}


	}
	
	protected void doRecognizeSwipe(TKSwipeRecognizer recognizer){
		Debug.Log( "doRecognizeSwipe method");

		if(transformToMove != null) {
			float speed = recognizer.swipeVelocity /100;
			recognized = true;
			movementController m = (movementController)GetComponent("movementController");
			m.stopMovement();
			m.setTransformtToMove(transformToMove,speed);
		}
	}
	
	protected void doEndTouch(TKAnyTouchRecognizer recognizer){
		Debug.Log("doEndTouch method ");

		//If touch wasn't a swipe gesture and touch was on a cube the code has to change the color of the cubes
		if(recognized == false && 
		   transformToMove != null && 
		   (transformToMove.tag == "cube0" || transformToMove.tag == "cube1" || transformToMove.tag == "cube2" || transformToMove.tag == "cube3") ){

			Debug.Log("Change COlor!!!!");

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
		
	}
	#endregion

	// Update is called once per frame
	void Update () {
	
	}
}
