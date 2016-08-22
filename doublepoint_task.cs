using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class doublepoint_task : MonoBehaviour {
	public GameObject pointA;
	public GameObject pointB;
	public byte task=0;
	public List<GameObject> ships;
	// Use this for initialization
	void Start () {
		ships=new List<GameObject>();
		switch (task) {
		case 1://drilling
			GameObject x=Instantiate(Resources.Load<GameObject>("man_digger_ship")) as GameObject;
			ships.Add(x);
			x.transform.position=transform.position;
			digger ds=x.GetComponent<digger>();
			ds.mine=pointB.GetComponent<resource_mine>();
			ds.receiver=pointA;
			ds.supply_base=pointA;

			break;
		}
	}

}
