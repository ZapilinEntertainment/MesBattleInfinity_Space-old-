using UnityEngine;
using System.Collections;

public class digger : MonoBehaviour {
	public resource_mine mine;
	public GameObject receiver;
	public GameObject supply_base;
	public float speed=1;
	public float rot_speed=4;
	public int maxdistance=10000;
	public float capacity;
	public float maxcapacity=3;
	public byte res_type=0; //0-mitol
	public float dig_speed=1;
	public float preparing_time=2;
	public float contact_distance=1;
	public byte step=0;
	public Renderer rr;
	Vector3 point;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
		switch (step) {
		case 0://start
			if (mine==null||receiver==null) {
				if (supply_base!=null) {
					point=supply_base.transform.position;
				}
				else point=transform.position;
			}
			else step=1;
			break;
		case 1://to mine
			if (mine==null) step=0;
			else point=mine.transform.root.position;
			break;
		case 2: //to base
			if (receiver==null) step=0;
			else point=receiver.transform.root.position;
			break;
		case 4: //await inside
			return;
			break;
		}
		transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.LookRotation(point-transform.position), rot_speed*Time.deltaTime);
		float d=Vector3.Distance(point,transform.position);
		if (d<=contact_distance) {
			if (step==1) {
				step=4;
				StartCoroutine(Loading(dig_speed*maxcapacity));
			}
			if (step==2) {
				step=4;
				StartCoroutine(Unloading(preparing_time+dig_speed*capacity));
			}
		}
		else {
			transform.Translate(new Vector3(0,0,speed*Time.deltaTime),Space.Self);
		}
	}

	IEnumerator Loading (float t) {
		rr.enabled=false;
		yield return new WaitForSeconds(t);
		transform.Rotate(0,180,0,Space.Self);
		rr.enabled=true;
		capacity=maxcapacity;
		step=2;
	}

	IEnumerator Unloading (float t) {
		rr.enabled=false;
		yield return new WaitForSeconds(t);
		transform.Rotate(0,180,0,Space.Self);
		rr.enabled=true;
		receiver.transform.root.SendMessage("AddResources",new Vector2(res_type,capacity),SendMessageOptions.DontRequireReceiver);
		capacity=0;
		step=1;
	}
}
