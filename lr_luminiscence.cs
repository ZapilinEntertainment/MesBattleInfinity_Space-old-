using UnityEngine;
using System.Collections;

public class lr_luminiscence : MonoBehaviour {
	public float tick=5;
	public float time=5;
	public float start=0.2f;
	public float end=1;
	public float length=5;
	public float radius=1;
	float t=0;
	public GameObject ray_pref;
	public Color clr;
	public SolarSystem system;
	public GameObject myHalo;
	// Use this for initialization

	void Start () {
		if (!ray_pref) Destroy(this);
		clr.a=0.5f;
	}

	void Update() {
		if (Global.cam==null) return;
		float d=Vector3.Distance(Global.cam.transform.position,transform.position);
		if (d>3000*radius) {return;}
		if (t<tick) {
			t+=Time.deltaTime;
		}
		else {			
			t=0;
			GameObject l=Instantiate(ray_pref);
			l.layer=8;
			LineRenderer lr=l.GetComponent<LineRenderer>();
			lr.SetWidth(start,end);
			Vector3 pos=Random.onUnitSphere*radius;
			if (pos.z<0) pos.z*=-1;
			pos=transform.TransformPoint(pos);
				lr.SetPosition(0, pos);
			lr.SetPosition(1,pos+(pos-transform.position)*length);
		    disappearance x=l.AddComponent<disappearance>();
			x.time=time;
			l.transform.parent=transform;
			Color clr2=clr;
			clr2.a=0;
			lr.SetColors(clr,clr2);
		}
	}

}
