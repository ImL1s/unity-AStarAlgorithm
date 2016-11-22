using UnityEngine;
using System.Collections;

public class Node 
{
	// 此格子是否可行走
	public bool canWalk;

	// 此格子在世界座標係的位置
	public Vector3 worldPosition;

	// 此格子在陣列的下標
	public int gridX,gridY;

	// 此格子到起始格子的距離
	public int gCost;

	// 此格子到終點的距離
	public int hCost;

	// 總Cost
	public int FCost
	{
		get
		{
			return gCost + hCost;
		}
	}

	public Node Parent 
	{
		get;
		set;
	}


	public Node(bool canWalk,Vector3 position,int x,int y)
	{
		this.canWalk = canWalk;
		this.worldPosition = position;
		this.gridX = x;
		this.gridY = y;
	}
		

}
