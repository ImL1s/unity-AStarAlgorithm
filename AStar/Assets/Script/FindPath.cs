using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindPath : MonoBehaviour 
{
	public Transform player,endPoint;

//	public Vector3 endPosition;

	private Grid _grid;

	private Coroutine findPathCoroutine;

	private bool isCoroutineRunning = false;

	private Vector3 playerPreviousPos,endPointPreviousPos;

	void Awake()
	{
		_grid = GetComponent<Grid> ();
		playerPreviousPos = player.position;
		endPointPreviousPos = endPoint.position;
	}

	void Start()
	{
		this.findPathCoroutine = StartCoroutine (this.FindingPathAsync ());
	}

	void Update () 
	{
//		FindingPath (player.position, endPoint.position);
//		
		if (playerPreviousPos != player.position || endPointPreviousPos != endPoint.position) 
		{
			if (!isCoroutineRunning) 
			{
				Debug.Log ("StartCoroutine");
				this.isCoroutineRunning = true;
				this.findPathCoroutine = StartCoroutine (this.FindingPathAsync ());
			}
		}

		playerPreviousPos = player.position;
		endPointPreviousPos = endPoint.position;
	}

	/// <summary>
	/// 尋找玩家與目標點的最佳路徑(使用協程,不會在一個Frame中佔用很多時間)
	/// </summary>
	/// <returns>The path async.</returns>
	private IEnumerator FindingPathAsync()
	{
		Debug.Log ("FindingPathAsync");

		List<Node> openList = new List<Node> ();
		HashSet<Node> closeList = new HashSet<Node> ();
		Node startNode = _grid.GetFromPosition (player.position);
		Node endNode = _grid.GetFromPosition (endPoint.position);

		openList.Add (startNode);

		while (openList.Count > 0) 
		{
			Node currentNode = openList [0];

			for (int i = 1; i < openList.Count; i++) 
			{
				if(openList[i].FCost < currentNode.FCost ||
					openList[i].FCost == currentNode.FCost && openList[i].hCost < currentNode.hCost)
				{
					currentNode = openList [i];
				}
			}

			openList.Remove (currentNode);
			closeList.Add (currentNode);

			if(currentNode == endNode)
			{
				GeneratePath (startNode, endNode);
				Debug.Log ("isCoroutineRunning = false");
				this.isCoroutineRunning = false;
				yield break;
			}


			List<Node> nebinourhoodList = _grid.GetNeibourhood (currentNode);

			// 尋找附近的點(上下左右 左上 右上 左下 右下)
			foreach (var node in nebinourhoodList) 
			{
				if (!node.canWalk || closeList.Contains (node))
					continue;

				int newGCost = GetDistanceNodes (currentNode, node);

				if(newGCost < node.gCost || !openList.Contains(node))
				{
					node.gCost = newGCost;
					node.hCost = GetDistanceNodes (node, endNode);
					node.Parent = currentNode;

					if(!openList.Contains(node))
					{
						openList.Add (node);
					}
				}
			}

			Debug.Log ("yield return");
			yield return null;
		}

		Debug.Log ("isCoroutineRunning = false");
		this.isCoroutineRunning = false;
	}

	/// <summary>
	/// 尋找玩家與目標點的最佳路徑
	/// </summary>
	/// <param name="startPosition">Start position.</param>
	/// <param name="endPosition">End position.</param>
	private void FindingPath(Vector3 startPosition,Vector3 endPosition)
	{
		// 開啟集合
		List<Node> openList = new List<Node> ();
		// 關閉集合
		HashSet<Node> closeList = new HashSet<Node> ();

		// 起始點
		Node startNode = _grid.GetFromPosition (startPosition);

		// 終點
		Node endNode = _grid.GetFromPosition (endPosition);

		openList.Add (startNode);

		while (openList.Count > 0) 
		{
			Node currentNode = openList [0];

			// 尋找當前開啟列表是否有更優的Node
			for (int i = 1; i < openList.Count; i++) 
			{
				if(openList[i].FCost < currentNode.FCost ||
					openList[i].FCost == currentNode.FCost && openList[i].hCost < currentNode.hCost)
				{
					currentNode = openList [i];
				}
			}

			// 將當前點移出開啟列表中
			openList.Remove (currentNode);
			// 將當前點放入關閉列表(已經找過,不會回頭再找了)
			closeList.Add (currentNode);

			// 如果當前的Node等於終點,就執行畫線動作(endNode.parent -> .parent -> .parent)
			if(currentNode == endNode)
			{
				GeneratePath (startNode, endNode);	
			}

			// 附近點的集合
			List<Node> nebinourhoodList = _grid.GetNeibourhood (currentNode);

			// 尋找附近的點(上下左右 左上 右上 左下 右下)
			foreach (var node in nebinourhoodList) 
			{
				// 如果附近的點不能行走或是在關閉列表中,就不判斷是否是當前最優的點
				if (!node.canWalk || closeList.Contains (node))
					continue;

				// 取得新的GCost,算法為當前點的GCost + 當前點與附近點的距離
				int newGCost = GetDistanceNodes (currentNode, node);

				// 如果新的GCost比鄰近點原本的GCost小 或是 開啟列表中還沒有加入過該鄰近點
				if(newGCost < node.gCost || !openList.Contains(node))
				{
					// 將鄰近點的GCost更新
					node.gCost = newGCost;
					// 重新計算鄰近點的HCost
					node.hCost = GetDistanceNodes (node, endNode);
					node.Parent = currentNode;

					if(!openList.Contains(node))
					{
						openList.Add (node);
					}
				}
			}
		}

	}

	/// <summary>
	/// 取得兩節點的距離(直的10 斜的14)
	/// </summary>
	/// <returns>The distance nodes.</returns>
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	private int GetDistanceNodes(Node a,Node b)
	{
		int x = Mathf.Abs (a.gridX - b.gridX);
		int y = Mathf.Abs (a.gridY - b.gridY);

		if(x > y)
		{
			return 14 * y + 10 * (x - y);
		}

		return 14 * x + 10 * (y - x);
	}

	/// <summary>
	/// 產生尋路路徑
	/// </summary>
	/// <param name="startNode">Start node.</param>
	/// <param name="endNode">End node.</param>
	private void GeneratePath(Node startNode,Node endNode)
	{
		List<Node> path = new List<Node> ();
		Node temp = endNode;

		while (temp != startNode) 
		{
			path.Add (temp);
			temp = temp.Parent;
		}

		this._grid.path = path;
	}
}
