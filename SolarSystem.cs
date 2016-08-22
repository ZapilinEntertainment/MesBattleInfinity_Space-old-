using UnityEngine;
using System.Collections;


public class SolarSystem : MonoBehaviour {
	SceneResLoader srl;
	public GameObject sun;
	public Material sun_material;
	public GameObject sun_pref;
	public Sprite star_sprite;
	public float system_radius=32768;
	public Vector3 system_center=Vector3.zero;
	public float star_R=0.05f;
	public float star_M=1;
	public float star_I=1;
	public float star_T=3000;
	public int index_x=0;
	public int index_y=0;
	public int index_z=0;
	public Vector3 time_left=Vector3.zero;

	public string system_id_code="system0";
	public string seed="abcdef";

	public bool loaded=false;
	public GameObject[] zone0; //коронарная
	public GameObject[] zone1; //прикоронарная
	public GameObject[] zone2; //внутренняя
	public GameObject[] zone3;//зеленая зона
	public GameObject[] zone4;//внешняя
	public GameObject[] zone5;//пояс Койпера
	public CorpusAster[] CorpiCelestii; //небесные тела
	public float corzone_r=0;
	public float precorzone_r=0;
	public float inzone_r=0;
	public float exozone_r=0;
	public float outzone_r=0;
	public byte player_zone=5;
	public float kf=512;

	// Use this for initialization
	void Start () {
		srl=GameObject.Find("menu").GetComponent<SceneResLoader>();
	}

	public void AddObject(GameObject x) {
		float d=Vector3.Distance(x.transform.position,system_center);
		byte z=5;
		if (d<outzone_r) {
			if (d<corzone_r) z=0;
			else {
				if (d<precorzone_r) z=1;
				else {
					if (d<inzone_r) z=2;
					else {
						if (d<exozone_r) z=3;
						else {
							if (d<outzone_r) z=4;
							else z=4;
						}
					}
				}
			}
		}
	}
	//----------------------------------
	void Update () {
		if (!srl.localShip) return;
		float d=Vector3.Distance(srl.localShip.transform.position,system_center);
		byte z=5;
		if (d<outzone_r) {
			if (d<corzone_r) z=0;
			else {
				if (d<precorzone_r) z=1;
				else {
					if (d<inzone_r) z=2;
					else {
						if (d<exozone_r) z=3;
						else {
							if (d<outzone_r) z=4;
							else z=4;
						}
					}
				}
			}
		}
		if (z!=player_zone) {
			player_zone=z;
		}
	}
	//----------------------------------
	public void Create() {
		seed="";
		for (byte i=0;i<6;i++) {
			seed+=(char)(Random.value*127)+"";
		}
		byte gn=(byte)(seed[0]%7);
		GameObject corpus=Instantiate(sun_pref) as GameObject;
		corpus.transform.position=system_center;
		//corpus.layer=8; //Planets layer
		CorpusAster info=corpus.GetComponent<CorpusAster>();
		Light l=corpus.GetComponent<Light>();
		//l.type=LightType.Point;
		Color clr_light=new Color();
		Color clr_material=new Color();
		float h=0;
		float km=seed[1]/255.0f;
			float kr=seed[2]/255.0f;
		float kt=seed[3]/255.0f;
		switch (gn) {
		case 0: //o-class
			info.temperature=(int)(kt*30000+30000);
			clr_material.r=kt*5/255.0f;
			clr_material.g=(kt*80+100)/255.0f;
			clr_material.b=0.95f+kt*0.05f;
			clr_light=Color.HSVToRGB(0.569f,kt,1);
			info.mass=60+km*40;
			info.radius=info.mass/4;
			l.range=system_radius*5;
			break;
		case 1: //b-class
			info.temperature=(int)(kt*20000+10000);
			h=kt*0.208f+0.486f;
			clr_material=Color.HSVToRGB(h,kt*0.3f,1);
			clr_light=Color.HSVToRGB(h,kt*0.2f+0.1f,1);
			info.mass=16+km*4;
			info.radius=info.mass/2.5f;
			break;
		case 2: //a-class
			info.temperature=(int)(kt*10000+7500);
			clr_material=Color.white;
			clr_light=Color.HSVToRGB(kt,kt*0.05f,1);
			info.mass=2.5f+km*1.2f;
			info.radius=info.mass/1.5f;
			break;
		case 3: //f-class
			info.temperature=(int)(kt*6000+1500);
			clr_material=Color.HSVToRGB(0.15f,kt*0.15f,1);
			clr_light=Color.HSVToRGB(0.15f,kt*0.2f+0.2f,1);
			info.mass=1.5f+km*0.4f;
			info.radius=info.mass/1.3f;
			l.range=system_radius*1.1f;
			break;
		case 4: //g-class, our sun
			info.temperature=(int)(kt*5000+1000);
			h=kt*0.4f+0.5f;
			clr_material=Color.HSVToRGB(0.15f,h+0.1f,1);
			clr_light=Color.HSVToRGB(0.15f,h,1);
			info.mass=1+km*0.4f;
			info.radius=info.mass;
			break;
		case 5: //k-class
			info.temperature=(int)(kt*3500+1500);
			h=0.65f+kt*0.2f;
			clr_material=Color.HSVToRGB(0.139f+kt*0.014f,h+0.15f,1);
			clr_light=Color.HSVToRGB(0.036f+kt*0.089f,h,1);
			info.mass=0.6f+km*0.4f;
			info.radius=info.mass*1.5f;
			break;
		case 6: //m-class
			info.temperature=(int)(kt*2000+1500);
			h=0.8f+Random.value*0.1f;
			clr_material=Color.HSVToRGB(0.139f*kt,h+0.1f,1);
			clr_light=Color.HSVToRGB(0.139f*kt,h,1);
			info.mass=0.1f+km*0.2f;
			info.radius=info.mass*1.1f;
			break;
		}
		system_radius=(0.1f*info.radius/20+0.7f*info.mass/100+0.2f)*65536;
		sun_material.SetColor("_Color",clr_material);
		info.rr.material.color=clr_material;
		l.color=clr_light;
		l.intensity=info.temperature/100000.0f*8;
		l.range=system_radius*l.intensity*2;
		float ir=info.radius;
		corpus.transform.localScale=Vector3.one*ir;
		info.halo.color=clr_light;
		ParticleSystem ps=corpus.GetComponent<ParticleSystem>();
		ParticleSystem.ShapeModule sh=ps.shape;
		ps.startColor=clr_light;
		info.eps=ps.emission;
		lr_luminiscence lls=corpus.GetComponent<lr_luminiscence>();
		lls.clr=clr_light;
		lls.system=this;
		if (info.background_sprite!=null) {
			info.background_sprite.color=clr_light;
		}
		CorpiCelestii=new CorpusAster[1];
		CorpiCelestii[0]=info;
		info.system=this;

		star_R=info.radius;
		star_M=info.radius;
		star_I=l.intensity;
		star_T=info.temperature;

		corzone_r=system_radius*0.1f;
		precorzone_r=system_radius*0.1f+corzone_r; //0.2
		inzone_r=0.3f+precorzone_r; //0.5
		exozone_r=system_radius*0.1f+inzone_r; //0.6
		outzone_r=system_radius*0.2f+exozone_r; //0.8f

		int ks=seed[0]/255*(seed.Length-1);
		Vector3 abp=Vector3.zero;
		abp.x=seed[ks]/255*(system_radius-outzone_r)+outzone_r;
		ks++;
		if (ks>=seed.Length) ks=0;
		abp.y=seed[ks]/255*(system_radius-outzone_r)+outzone_r;
		ks++;
		if (ks>=seed.Length) ks=0;
		abp.z=seed[ks]/255*(system_radius-outzone_r)+outzone_r;
		ks++;
		if (ks>=seed.Length) ks=0;
		ks=seed[ks]*0.98f;
		float dxl=0;
		for (byte i=0;i<=ks;i++) {
			dxl=GenerateAsteroid(abp);
			abp+=new Vector3();
		}

		loaded=true;
	}

	//seed: 0-star type, 1-mass,2-radius,3-temperature

	public float GenerateAsteroid(Vector3 pos) {
	
		float edge=Vector3.Distance(transform.position,system_center)/system_radius*200;
		GameObject a=GameObject.CreatePrimitive(PrimitiveType.Cube);
		a.transform.position=pos;
		a.transform.localScale=Vector3.one*edge*2;
		a.name="asteroid";
		GameObject x;
		Vector3 ppt=Vector3.zero;

		float edge2=0;
		for (byte i=0;i<14;i++) {
			switch (i) {
			case 0: ppt=new Vector3(-edge,edge,-edge);if (transform.position.y%2==0) edge2=(transform.position.x%8)/8*edge;break;
			case 1: ppt=new Vector3(-edge,edge,edge);if (transform.position.y%2==1) edge2=(transform.position.z%8)/8*edge;break;
			case 2 : ppt=new Vector3(edge,edge,edge);if (transform.position.y%3==0) edge2=(transform.position.x%8)/8*edge;break;
			case 3: ppt=new Vector3(edge,edge,-edge);if (transform.position.y%3!=0) edge2=(transform.position.z%8)/8*edge;break;
			case 4:ppt=new Vector3(-edge,-edge,-edge);if (transform.position.y%5==0) edge2=(transform.position.x%8)/8*edge;break;
			case 5: ppt=new Vector3(-edge,-edge,edge);if (transform.position.y%5!=0) edge2=(transform.position.z%8)/8*edge;break;
			case 6: ppt=new Vector3(edge,-edge,edge);if (transform.position.y%7==0) edge2=(transform.position.x%8)/8*edge;break;
			case 7: ppt=new Vector3(edge,-edge,-edge);if (transform.position.y%7!=0) edge2=(transform.position.z%8)/8*edge;break;
			case 8:ppt=new Vector3(0,0,2*edge);if (transform.position.z%5==0) edge2=(transform.position.x%8)/8*edge;break;
			case 9: ppt=new Vector3(-edge,0,0);if (transform.position.x%5!=0) edge2=(transform.position.z%8)/8*edge;break;
			case 10: ppt=new Vector3(edge,0,0);if (transform.position.x%5==0) edge2=(transform.position.z%8)/8*edge;break;
			case 11: ppt=new Vector3(0,0,-edge);if (transform.position.z%2==0) edge2=(transform.position.x%8)/8*edge;break;
			case 12: ppt=new Vector3(0,edge,0);if (transform.position.y%2!=0) edge2=(transform.position.x%8)/8*edge;break;
			case 13: ppt=new Vector3(0,-edge,0);if (transform.position.y%2==0) edge2=(transform.position.x%8)/8*edge;break;
			}
			if (edge2!=0) {
				if (edge2>edge) edge2=0.5f*edge2;
				x=GameObject.CreatePrimitive(PrimitiveType.Cube);
				x.transform.parent=a.transform;
				x.transform.localPosition=ppt;
				x.transform.localScale=Vector3.one*edge2*2;
				x.transform.LookAt(a.transform.position);
			}
		}
		return(a);
	}

	public GameObject GenerateKasteroid(Vector3 pos) {
		float d=Vector3.Distance(pos,system_center);
		int main_chain=Mathf.RoundToInt(seed[1]/255.0f*d/2000+d/2000);
		int maxsize=10;
		int second_chain=(int)(seed[2]/255.0f*10*(1-d/system_radius));
		GameObject a=new GameObject("kasteroid");
		a.transform.position=pos;
		GameObject x=GameObject.CreatePrimitive(PrimitiveType.Cube);
		GameObject x2;
		float edge=1; 
		byte sip=0;//main chain index;
		float ek=0; //index using for scale
		float lastedge=1;
		byte vk=0; //string index for vertex position
		int last_ngvp=0;
		Vector3 point=Vector3.zero;
		for (byte i=0;i<main_chain;i++) {
			x=GameObject.CreatePrimitive(PrimitiveType.Cube);
			x.transform.parent=a.transform;
			x.transform.localPosition=point;
			ek=i;
			if (sip>=seed.Length) sip=0;
			edge=seed[sip]/255.0f*maxsize;
			sip+=2;
			if (edge/lastedge>=2&&i!=0) edge=1.5f*lastedge;
			if (edge/lastedge<=0.5f&&i!=0) edge=0.75f*lastedge;
			x.transform.localScale=new Vector3(2*edge,2*edge,2*edge);
			lastedge=edge;
			if (i==0) x.transform.localPosition=Vector3.zero;
			else {
				if (vk>=seed.Length) vk=0;
				int ngvp=(int)(seed[vk]/36.4f);
				if (last_ngvp==ngvp) ngvp++;
				if (ngvp>7) ngvp=0;
				vk++;
				Vector3 ppt=Vector3.zero;
				switch (ngvp) {
				case 0: ppt=new Vector3(-edge,edge,-edge);last_ngvp=6;break;
				case 1: ppt=new Vector3(-edge,edge,edge);last_ngvp=7;break;
				case 2: ppt=new Vector3(edge,edge,edge);last_ngvp=4;break;
				case 3:ppt=new Vector3(edge,edge,-edge);last_ngvp=5;break;
				case 4: ppt=new Vector3(-edge,-edge,-edge);last_ngvp=2;break;
				case 5: ppt=new Vector3(-edge,-edge,edge);last_ngvp=3;break;
				case 6:ppt=new Vector3(edge,-edge,edge);last_ngvp=0;break;
				case 7: ppt=new Vector3(edge,-edge,-edge);last_ngvp=1;break;					
				}
				point+=ppt;
				transform.localPosition=point;
			}
		}
		return(edge);
	}
}