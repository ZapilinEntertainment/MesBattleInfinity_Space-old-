using UnityEngine;
using System.Collections;

public class CorpusAster : MonoBehaviour {
	public float mass=1;
	public float radius=1;
	public Light astroLight;
	public MeshRenderer rr;
	public int temperature=-18;
	public SpriteRenderer halo;
	public SpriteRenderer background_sprite;
	public Vector3 realpos=Vector3.zero;
	public ParticleSystem.EmissionModule eps;
	public SolarSystem system;

	void Start() {
		realpos=transform.position*Global.envcam_kf;
	}

	void Update () {
		if (Global.envcam==null) return;
		if (background_sprite!=null) background_sprite.transform.position=(transform.position-Global.envcam.transform.position).normalized*(radius+0.01f)+transform.position;
		float d=Vector3.Distance(transform.position,Global.envcam.transform.position);
		Color clr=astroLight.color;
		if (d>100*radius) {
			if (rr.enabled) rr.enabled=false;
			if (halo.gameObject.activeSelf) halo.gameObject.SetActive(false);
			if (eps.enabled) eps.enabled=false;
		}
		else {
			if (!rr.enabled) rr.enabled=true;
			if (!halo.gameObject.activeSelf) halo.gameObject.SetActive(true);
			else {clr.a=1-d/100*radius;halo.color=clr;}
			if (!eps.enabled) eps.enabled=true;
		}
		clr.a=1;
		clr.a=d/(system.system_radius/system.kf);
		background_sprite.color=clr;
	}
}
