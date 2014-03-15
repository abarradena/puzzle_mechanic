using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;

public class cubeCreator : MonoBehaviour {
	[SerializeField]
	private GameObject 		cubeElement;

	[SerializeField]
	private GameObject 		whiteCubeElement;

	[SerializeField]
	private Color[] 		cubeColors;

	public float newLineTiming = 10f;
	public GameObject[,] 	matrixBoard;
	public int[]			indexLastRow;
	public int[]			columnWithWhiteCube;

	public float	cubeOffsetX			= 1.8f;
	public float	cubeOffsetY			= 1.2f;
	public int		columnNumber		= 4;
	public int		rowNumber			= 7;
	public int		initialRowNumber	= 2;
	public bool 	gameOn				= true;
	public int 		countdown			= 4;
	private int		whiteBlockNum		= 0;
	private matchLogic logicMgr;
	// Use this for initialization
	void Start () {
		cubeColors 			= new Color[3];
		cubeColors[0] 		= Color.red;
		cubeColors[1] 		= Color.yellow;
		cubeColors[2] 		= Color.blue;

		columnWithWhiteCube	= new int[columnNumber-1];
		indexLastRow 		= new int[columnNumber];
		matrixBoard 		= new GameObject[columnNumber,rowNumber];

		logicMgr = this.GetComponent<matchLogic>();

		initPuzzleBoard();
		StartCoroutine(addLine());
	}

	Color pickRandomColor(){
		int RandomIndex = UnityEngine.Random.Range(0, cubeColors.Length);
		return cubeColors[RandomIndex];
		//return Color.blue;
	}

	public void placeWhiteBlocks(bool addCube){
		int indexRow 	= 0;
		List<int> listEmptyLine = new List<int>();

		for(indexRow=0; indexRow < columnNumber ; indexRow++){
			listEmptyLine.Add(indexLastRow[indexRow]);
		}

		for(int i=0; i < 3; i++){
			int RandomIndex = UnityEngine.Random.Range(0, listEmptyLine.Count);
			if(listEmptyLine[RandomIndex] != 99){
				if(addCube) insertCube(listEmptyLine[RandomIndex], RandomIndex, true);
				listEmptyLine[RandomIndex] = 99;
				columnWithWhiteCube[i] = RandomIndex;
			}else{
				i--;
			}
		}
		listEmptyLine.Clear();
	}

	public void checkLastRow(){
		//check each column to find the last 
		int indexCol 	= 0;
		int indexRow 	= 0;
		for(indexCol = 0; indexCol < columnNumber; indexCol++){
			for(indexRow = 0; indexRow < rowNumber ; indexRow++){
				if(matrixBoard[indexCol, indexRow] == null || matrixBoard[indexCol, indexRow].renderer.material.color == Color.white){
					indexLastRow[indexCol] 	= indexRow;
					break;
				}
			}
		}
	}

	void initPuzzleBoard(){
		int indexCol 	= 0;
		int indexRow 	= 0;
		for(indexRow=0; indexRow < initialRowNumber; indexRow++){
			for(indexCol = 0; indexCol < columnNumber; indexCol++){
				insertCube(indexRow, indexCol, false);
			}
		}
		checkLastRow();
		placeWhiteBlocks(true);
		checkMatch();
	}


	public void checkMatch(){
		logicMgr.checkMatches();
	}

	public void insertCube(int rowIndex, int columnIndex, bool white){
		StartCoroutine(insertCubeByIndex(rowIndex, columnIndex, white));
	}

	IEnumerator insertCubeByIndex(int rowIndex, int columnIndex, bool white){
		GameObject cube;
		if(white){
			cube 				= Instantiate (whiteCubeElement) as GameObject;
			cube.name						= rowIndex+" WhiteCube "+columnIndex;
			cube.transform.position	 		= new Vector3 (columnIndex * cubeOffsetX, rowIndex * cubeOffsetY * -1, 0);
		}else{
			cube 				= Instantiate (cubeElement) as GameObject;
			cube.renderer.material.color 	= pickRandomColor();
			cube.name						= rowIndex+" cube "+columnIndex;
			cube.transform.position	 		= new Vector3 (columnIndex * cubeOffsetX, (rowIndex * cubeOffsetY) +10f, 0);

			TweenParms parms = new TweenParms();
			parms.Prop("position", new Vector3(columnIndex * cubeOffsetX,rowIndex * cubeOffsetY * -1,0));
			parms.Ease(EaseType.EaseOutCubic);
			parms.Delay(.3f);
			
			HOTween.To(cube.transform, .3f, parms );
		}
		cube.renderer.castShadows 		= false;
		cube.renderer.receiveShadows 	= false;
		updCubeData(cube, rowIndex, columnIndex);
		matrixBoard[columnIndex, rowIndex] = cube;

		yield return new WaitForSeconds(0f);
	}

	public void insertSlideCube(int columnIndex , Color colorCube){
		GameObject cube;
		int rowIndex= indexLastRow[columnIndex];
		cube 				= Instantiate (cubeElement) as GameObject;
		updCubeData(cube, rowIndex, columnIndex);
		Destroy(matrixBoard[columnIndex, rowIndex]);
		cube.renderer.material.color 	= colorCube;
		cube.name						= rowIndex+" cube "+columnIndex;
		cube.transform.position	 		= new Vector3 (columnIndex * cubeOffsetX, rowIndex * cubeOffsetY * -1, 0);
		cube.renderer.castShadows 		= false;
		cube.renderer.receiveShadows 	= false;

		matrixBoard[columnIndex, rowIndex] = cube;
		checkLastRow();
		int newRowIndex = indexLastRow[columnIndex];
		addNewWhiteCube(columnIndex);
		checkMatch();
	}

	void addNewWhiteCube(int columnIndex){
		int indexCol 		= 0;
		int indexRow 		= 0;

		List<int> columnNoWhite = new List<int>();

		for(indexCol = 0; indexCol < matrixBoard.GetLength(0); indexCol++){
			for(indexRow = 0; indexRow < matrixBoard.GetLength(1); indexRow++){
				if(matrixBoard[indexCol, indexRow] == null){
					columnNoWhite.Add(indexCol);
					break;
				}else{
					if(matrixBoard[indexCol, indexRow].renderer.material.color == Color.white)
						break;
				}
			}
		}

		int RandomIndex = UnityEngine.Random.Range(0, 1);
		int newColumnWhite = columnNoWhite[RandomIndex];
		StartCoroutine(insertCubeByIndex(indexLastRow[newColumnWhite], newColumnWhite, true));
		placeWhiteBlocks(false);
		checkLastRow();
	}

	void updCubeData(GameObject cube, int rowIndex, int columnIndex){
		cubeProp cubeData = cube.GetComponent<cubeProp>();

		cubeData.Name = cube.gameObject.name;
		cubeData.ColumnIndex = columnIndex;
		cubeData.RowIndex = rowIndex;
		cubeData.CoordX = cube.transform.position.x;
		cubeData.CoordY = cube.transform.position.y;
		cubeData.ColorCube = cube.renderer.material.color;
	}

	IEnumerator addLine(){
		checkMatch();
		yield return new WaitForSeconds(newLineTiming);
		StartCoroutine(moveCubesDown());
		checkLastRow();
		int indexCol=0;
		for(indexCol = 0; indexCol < columnNumber; indexCol++){
			insertCube(0, indexCol, false);
		}
		if(gameOn){
			StartCoroutine(addLine());
		}
	}

	public void getHighestIndex(){
		int tempIndex = indexLastRow[0];
		for(int i=0; i < columnNumber-1; i++){
			if(tempIndex < indexLastRow[i+1])
				tempIndex = indexLastRow[i+1];
		}
		countdown = (rowNumber) - tempIndex;

		GameObject objCountdownText = GameObject.Find("countdown");
		tk2dTextMesh countdownText  = objCountdownText.GetComponent<tk2dTextMesh>();
		countdownText.text = countdown.ToString();
	}

	IEnumerator moveCubesDown(){
		int indexCol 		= 0;
		int indexRow 		= 0;
		getHighestIndex();

		for(indexCol = 0; indexCol < matrixBoard.GetLength(0); indexCol++){
			for(indexRow=(matrixBoard.GetLength(1)-1); indexRow >= 0; indexRow--){
				if(matrixBoard[indexCol,indexRow] != null){
					GameObject obj = matrixBoard[indexCol,indexRow];
					cubeProp dataCube = obj.GetComponent<cubeProp>();
					if(indexRow+1 < rowNumber){
						TweenParms parms = new TweenParms();
						parms.Prop("position", new Vector3(indexCol * cubeOffsetX,(indexRow+1) * cubeOffsetY * -1,0));
						parms.Delay(0);
						
						HOTween.To(obj.transform, .3f, parms );
						matrixBoard[indexCol,indexRow] 		= null;
						matrixBoard[indexCol,indexRow+1]	= obj;
						dataCube.RowIndex = indexRow+1;
						dataCube.CoordY = (indexRow+1) * cubeOffsetY * -1;

						obj.name = (indexRow+1) +" cube "+ indexCol;
					} else {
						Debug.LogError("GAME OVER BABY");
						gameOn = false;
						//Application.LoadLevel(0);
						break;
					}
				}
			}
		}
		yield return new WaitForSeconds(0f);
		checkMatch();
		checkLastRow();
		getHighestIndex();
	}
}
