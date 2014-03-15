using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;

public class matchLogic : MonoBehaviour {

	private cubeCreator creatorMgr; 
	private Color tempColorHoriz = Color.cyan;
	private Color tempColorVert = Color.clear;
	private Dictionary<string, GameObject> dictObjMatched;

	public List<GameObject> horizMatch;
	public List<GameObject> vertMatch;

	public int scorePoints = 0;

	// Use this for initialization
	void Start () {
		creatorMgr = this.GetComponent<cubeCreator>();
		dictObjMatched = new Dictionary<string, GameObject>();

		horizMatch = new List<GameObject>();
		vertMatch  = new List<GameObject>();

	}

	public void checkMatches(){
		StartCoroutine(checkHorizMatches());
	}

	IEnumerator checkHorizMatches(){
		yield return new WaitForSeconds(0f);
		int indexCol 		= 0;
		int indexRow 		= 0;

		for(indexRow=0; indexRow < creatorMgr.matrixBoard.GetLength(1); indexRow++){
			for(indexCol = 0; indexCol < creatorMgr.matrixBoard.GetLength(0); indexCol++){
				if(creatorMgr.matrixBoard[indexCol,indexRow] != null){
					Color colorCube = creatorMgr.matrixBoard[indexCol,indexRow].renderer.material.color;
					if(colorCube != Color.white){

						GameObject cubeHit = creatorMgr.matrixBoard[indexCol,indexRow];
						if(colorCube == tempColorHoriz){
							horizMatch.Add(cubeHit);
						}else{
							checkMatchesInHorList();
							horizMatch.Clear();
							horizMatch.Add (cubeHit);
							tempColorHoriz = colorCube;
						}
					}
				}
			}
			checkMatchesInHorList();
			horizMatch.Clear();
		}
		checkMatchesInHorList();
		horizMatch.Clear();
		StartCoroutine(checkVertMatches());
	}

	public void checkMatchesInHorList(){
		int i = 0;
		// if horizMatch == 2 check for L matches
		if(horizMatch.Count >= 3)
		{
			for(i=0; i < horizMatch.Count; i++)
			{
				if(horizMatch[i] != null){
					horizMatch[i].GetComponent<cubeProp>().ToRemove = true;
				}
			}
		}
	}

	public void checkHorizElleMatch(GameObject obj){
		cubeProp dataCube = obj.GetComponent<cubeProp>();
		Color cubeColor = dataCube.ColorCube;

		int columnIndex = dataCube.ColumnIndex;
		int rowIndex = dataCube.RowIndex;
		int rowIndexDown = rowIndex+1;
		int rowIndexUp = rowIndex-1;
		int lastRowIndex = creatorMgr.indexLastRow[columnIndex];
	
		if(rowIndexDown <= lastRowIndex && creatorMgr.matrixBoard[columnIndex,rowIndexDown] != null){
			if(creatorMgr.matrixBoard[columnIndex,rowIndexDown].renderer.material.color == cubeColor)
			{
				horizMatch[0].GetComponent<cubeProp>().ToRemove = true;
				horizMatch[1].GetComponent<cubeProp>().ToRemove = true;
				creatorMgr.matrixBoard[columnIndex,rowIndexDown].GetComponent<cubeProp>().ToRemove = true;
			}
		}

		if(rowIndexUp >= 0 && creatorMgr.matrixBoard[columnIndex,rowIndexUp] != null){
			if(creatorMgr.matrixBoard[columnIndex,rowIndexUp].renderer.material.color == cubeColor)
			{
				horizMatch[0].GetComponent<cubeProp>().ToRemove = true;
				horizMatch[1].GetComponent<cubeProp>().ToRemove = true;
				creatorMgr.matrixBoard[columnIndex,rowIndexUp].GetComponent<cubeProp>().ToRemove = true;
			}
		}
			
	}


	IEnumerator checkVertMatches(){
		yield return new WaitForSeconds(0f);
		int indexCol 		= 0;
		int indexRow 		= 0;

		for(indexCol = 0; indexCol < creatorMgr.matrixBoard.GetLength(0); indexCol++){
			for(indexRow=0; indexRow < creatorMgr.matrixBoard.GetLength(1); indexRow++){
				if(creatorMgr.matrixBoard[indexCol,indexRow] != null){
					Color colorCube = creatorMgr.matrixBoard[indexCol,indexRow].renderer.material.color;
					if(colorCube != Color.white){
						GameObject cubeHit = creatorMgr.matrixBoard[indexCol,indexRow];
						if(colorCube == tempColorVert){
							vertMatch.Add(cubeHit);
						}else{
							checkMatchesInVertList();
							vertMatch.Clear();
							vertMatch.Add (cubeHit);
							tempColorVert = colorCube;
						}
					}
				}
			}
			checkMatchesInVertList();
			vertMatch.Clear();
		}
		checkMatchesInVertList();
		vertMatch.Clear();
		removeElements();
	}

	public void checkMatchesInVertList(){
		int i = 0;
		if(vertMatch.Count >= 3)
		{
			for(i=0; i < vertMatch.Count; i++)
			{
				if(vertMatch[i] != null){
					vertMatch[i].GetComponent<cubeProp>().ToRemove = true;
				}
			}
		}
	}

	public void removeElements(){
		int indexCol 	= 0;
		int indexRow 	= 0;

		for(indexCol = 0; indexCol < creatorMgr.columnNumber; indexCol++){
			for(indexRow = 0; indexRow < creatorMgr.matrixBoard.GetLength(1) ; indexRow++){
				if(creatorMgr.matrixBoard[indexCol,indexRow] != null)
				{
					GameObject currentObj = creatorMgr.matrixBoard[indexCol, indexRow];
					cubeProp dataCube = currentObj.GetComponent<cubeProp>();

					if(dataCube.ToRemove)
					{
						currentObj.SetActive(false);
						if(dataCube.RowIndex-1 == -1){
							creatorMgr.insertCube(dataCube.RowIndex, dataCube.ColumnIndex, false);
						}
						StartCoroutine(moveUpCubes(dataCube.ColumnIndex));
						creatorMgr.checkLastRow();
						Destroy(currentObj);
						GameObject objScoreText = GameObject.Find("scorepoint");
						tk2dTextMesh scoreText  = objScoreText.GetComponent<tk2dTextMesh>();
						scorePoints += 100;
						scoreText.text = scorePoints.ToString();
					}
				}
			}
		}
	}

	IEnumerator moveUpCubes(int columnIndex){
		creatorMgr.getHighestIndex();
		yield return new WaitForSeconds(0f);
		int j=0;
		for(int i=1; i < creatorMgr.rowNumber; i++){
			//Debug.LogError(creatorMgr.matrixBoard[columnIndex,i]);
			if(creatorMgr.matrixBoard[columnIndex,i] != null && creatorMgr.matrixBoard[columnIndex,i-1] == null && i > 0){
				GameObject cubeToMove = creatorMgr.matrixBoard[columnIndex,i];
				cubeProp dataCube = cubeToMove.GetComponent<cubeProp>();
				j = i;

				while(creatorMgr.matrixBoard[columnIndex,j-1] == null){
					TweenParms parms = new TweenParms();
					parms.Prop("position", new Vector3(columnIndex * creatorMgr.cubeOffsetX,(i-1) * creatorMgr.cubeOffsetY * -1,0));
					parms.Delay(0);
					
					HOTween.To(cubeToMove.transform, .3f, parms );
					creatorMgr.matrixBoard[columnIndex,i] 		= null;
					creatorMgr.matrixBoard[columnIndex,i-1]	= cubeToMove;
					dataCube.RowIndex = i-1;
					dataCube.CoordY = (i-1) * creatorMgr.cubeOffsetY * -1;
					
					cubeToMove.name = (i-1) +" cube "+ columnIndex;
					j--;
				}
			}
		}
		creatorMgr.checkMatch();
		creatorMgr.checkLastRow();
		creatorMgr.getHighestIndex();
	}
//-------------------------------------

	public void checkMatchesFromBottom(int columnIndex, Color colorCube){
		StartCoroutine(checkMatchesRecursive(columnIndex, colorCube));
	}

	IEnumerator checkMatchesRecursive(int columnIndex, Color colorCube){			
		yield return new WaitForSeconds(.5f);
		int rowIndex = creatorMgr.indexLastRow[columnIndex];
		GameObject cubeHit = creatorMgr.matrixBoard[columnIndex,rowIndex];
 		
		if(cubeHit.renderer.material.color == colorCube){
			Debug.LogError(cubeHit);
			if(!dictObjMatched.ContainsKey(cubeHit.name)){
				dictObjMatched.Add(cubeHit.name, cubeHit);
			}
			if(rowIndex-1 >= 0) checkNorth(columnIndex, rowIndex-1, colorCube);
			if(columnIndex-1 >= 0) checkWest(columnIndex-1, rowIndex, colorCube);
			if(columnIndex+1 < creatorMgr.matrixBoard.GetLength(0)) checkEast(columnIndex+1, rowIndex, colorCube);
		}
		
		if(dictObjMatched.Count >= 3){
			foreach(KeyValuePair<string, GameObject> obj in dictObjMatched){
				obj.Value.SetActive(false);
			}
		}
		dictObjMatched.Clear();
	}
	
	public void checkNorth(int columnIndex, int rowIndex, Color colorCube){
		GameObject cubeHitNorth = creatorMgr.matrixBoard[columnIndex,rowIndex];
		if(cubeHitNorth.renderer.material.color == colorCube){
			if(!dictObjMatched.ContainsKey(cubeHitNorth.name)){
				dictObjMatched.Add(cubeHitNorth.name, cubeHitNorth);
			}
			if(rowIndex-1 >= 0)	checkNorth(columnIndex, rowIndex-1, colorCube);
		}
	}

	public void checkWest(int columnIndex, int rowIndex, Color colorCube){
		GameObject cubeHitWest = creatorMgr.matrixBoard[columnIndex,rowIndex];
		if(cubeHitWest.renderer.material.color == colorCube){
			if(!dictObjMatched.ContainsKey(cubeHitWest.name)){
				dictObjMatched.Add(cubeHitWest.name, cubeHitWest);
			}
			if(columnIndex-1 >= 0) checkWest(columnIndex-1, rowIndex, colorCube);
		}
	}

	public void checkEast(int columnIndex, int rowIndex, Color colorCube){
		GameObject cubeHitEast = creatorMgr.matrixBoard[columnIndex,rowIndex];
		if(cubeHitEast.renderer.material.color == colorCube){
			if(!dictObjMatched.ContainsKey(cubeHitEast.name)){
				dictObjMatched.Add(cubeHitEast.name, cubeHitEast);
			}
			if(columnIndex+1 < creatorMgr.matrixBoard.GetLength(0))	checkEast(columnIndex+1, rowIndex, colorCube);
		}
	}
	
}
