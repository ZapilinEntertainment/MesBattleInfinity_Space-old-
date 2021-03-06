using UnityEngine;
using System.Collections;

public class disappearance : MonoBehaviour {
	public float time=5;
	float t=0;
	Renderer rr;
	Color clr;

	void Start () {
		t=time;
		rr=gameObject.GetComponent<Renderer>();
		clr=rr.material.GetColor("_TintColor");
	}

	void Update() {
		t-=Time.deltaTime;
		if (t>0) {
			if (t<time/2)	clr.a=t/time/2; else clr.a=t/time/2;
			rr.material.SetColor("_TintColor",clr);
		}
		else {
			Destroy(gameObject);
		}
	}
}
