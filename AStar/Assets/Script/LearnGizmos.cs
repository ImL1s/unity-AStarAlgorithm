using UnityEngine;
using System.Collections;

public class LearnGizmos : MonoBehaviour 
{

	public Transform target;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnDrawGizmos()
	{
		if(target != null)
		{
//			DrawLine ();
			DrawRay ();
			DrawCube ();
		}
	}

	private void DrawLine()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine (transform.position, target.position);
	}

	private void DrawRay()
	{
		Gizmos.color = Color.yellow;
		Vector3 direction = transform.TransformDirection (Vector3.forward) * 5;
		Gizmos.DrawRay (transform.position,direction);
	}

	private void DrawCube()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawCube (transform.position, Vector3.one);
	}
}
