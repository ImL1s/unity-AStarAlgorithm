using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour 
{
	// 場上的節點(格子)
	public Node[,] grid;

	// 網格的大小
	public Vector2 gridSize;

	// 節點的半徑
	public float nodeRadius;

	// 節點的直徑
	private float nodeDiameter;

	// 障礙物的Layer
	public LayerMask whatLayer;

	// 場上有幾個格子(X Y)
	public int gridCountX, gridCountY;

	// 玩家位置
	public Transform player;

	// 尋路路徑
	public List<Node> path = new List<Node>();


	public bool gizmoNode = false;


	void Start () 
	{
		nodeDiameter = nodeRadius * 2;
		gridCountX = (int) (gridSize.x / (nodeDiameter));
		gridCountY = (int) (gridSize.y / (nodeDiameter));
		CreateGrid ();
	}
	

	void Update () 
	{
	
	}

	private void CreateGrid()
	{
		grid = new Node[gridCountX, gridCountY];
		Vector3 startPoint = transform.position - (gridSize.x / 2 * Vector3.right) - (gridSize.y / 2 * Vector3.forward);

		for (int i = 0; i < gridCountX; i++) 
		{
			for (int j = 0; j < gridCountY; j++) 
			{
				// 加了半徑grid才會在中心點
				Vector3 nodePoint = startPoint + ((i * nodeDiameter + nodeRadius) * Vector3.right) + 
					((j * nodeDiameter + nodeRadius) * Vector3.forward);

				bool walkable = !Physics.CheckSphere (nodePoint, nodeRadius, whatLayer);

//				print (nodePoint);

				grid [i, j] = new Node (walkable, nodePoint, i, j);
			}	
		}
	}

	public void OnDrawGizmos()
	{
		if (grid == null)
			return;
		
		DrawNode ();
		DrawPath ();
		DrawWire ();
		DrawPlayer ();
	}


	// 畫Node
	private void DrawNode()
	{
		foreach (var node in grid) 
		{
			Gizmos.color = node.canWalk ? Color.white : Color.black;
			Gizmos.DrawCube (node.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
		}
	}

	// 畫Grid外框
	private void DrawWire()
	{
		Gizmos.DrawWireCube (transform.position, new Vector3 (gridSize.x, 1, gridSize.y));
	}

	// 畫路徑
	private void DrawPath()
	{
		if(path != null && path.Count > 0)
		{
			Gizmos.color = new Color(0.5f,1,0.5f,0.5f);

			foreach (var node in path) 
			{
				Gizmos.DrawCube (node.worldPosition, Vector3.one * (nodeDiameter - 0.1f));	
			}
		}
	}

	// 畫Player所在的位置
	private void DrawPlayer()
	{
		Node playerNode = GetFromPosition (player.position);

		if(playerNode != null && playerNode.canWalk)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawCube (playerNode.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
		}
	}

	// 根據世界座標,取得相對的格子
	public Node GetFromPosition(Vector3 position)
	{
		// gridSize.x /2 和 gridSize.y /2
		// 玩家的位置為世界座標系,加上一個grid.x/2  grid.y/2 就可以得到相對於grid的位置(距離Grid startPoint的長度)
		// 
		// 例如說玩家當前在(0,0)點,要轉換成相對網格的位置(gridSize假定為10) --> 
		//
		// percentX: (0 + 10 / 2) = 0.5 -> percentX: MathF.Clamp01(0.5) = 0.5 -> 
		// gridIndexX: (10 - 1)*0.5 = 4.5 (四捨六入 如果是五,選擇兩者中偶數之) -> 4
		//
		// percentY: (0 + 10 / 2) = 5
		// 

		// 取得當前點佔整個Grid的幾分之幾
		float percentX = (position.x + gridSize.x / 2) / gridSize.x;
		float percentY = (position.z + gridSize.y / 2) / gridSize.y;

		// 不能超過１(移動到Grid外面)
		percentX = Mathf.Clamp01 (percentX);
		percentY = Mathf.Clamp01 (percentY);

		// 取得是哪一個Node(%數 乘上 總共的格子數 再四捨五入)
		// 假如當前有10個格子,因為是從０開始,所以-1,然後將總共格子數 * %數,四捨五入後取得最接近的格子
		int x = Mathf.RoundToInt ((gridCountX - 1) * percentX);
		int y = Mathf.RoundToInt ((gridCountY - 1) * percentY);

		return grid [x, y];
	}

	/// <summary>
	/// 取得傳入Node的相鄰點
	/// </summary>
	/// <returns>The neibourhood.</returns>
	/// <param name="node">Node.</param>
	public List<Node>GetNeibourhood(Node node)
	{
		List<Node> neibourhood = new List<Node> ();

		// 上下左右
		for (int i = -1; i <= 1; i++) 
		{
			for (int j = -1; j <= 1; j++) 
			{
				if (i == 0 && j == 0)
					continue;

				int tempX = node.gridX + i;
				int tempY = node.gridY + j;

				if((tempX < gridCountX && tempX > 0) && (tempY < gridCountY && tempY > 0))
				{
					neibourhood.Add (grid [tempX, tempY]);
				}
			}	
		}

		return neibourhood;
	}
}
