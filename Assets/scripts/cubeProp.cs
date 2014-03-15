using UnityEngine;
using System.Collections;


public class cubeProp : MonoBehaviour{
	public string 		Name;
	public int			ColumnIndex;
	public int			RowIndex;
	public float		CoordX;
	public float		CoordY;
	public float		CubeOffsetX	= 1.8f;
	public float		CubeOffsetY	= 1.2f;
	public Color		ColorCube;
	public bool			ToRemove = false;
}
