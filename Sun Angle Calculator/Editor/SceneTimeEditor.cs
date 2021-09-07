using UnityEditor;
using UnityEngine;

//Custom editor for the scene time class
 
[CustomEditor(typeof(SceneTime))]
public class SceneTimeEditor : Editor {

	private SceneTime st;

	//month selections
	private string[] months = new string[] {"January", "February", "March", "April", "May", "June", 
		"July", "August", "September", "October", "November", "December"};

	//Potential real-time day lengths for the scene
	private string[] cycleLengthOptions = new string[] {"30 Seconds", "1 Minute", "5 Minutes", "10 Minutes", "20 Minutes",
		"30 Minutes", "1 Hour", "2 hours", "Real Time"};

	private float[] cycleLengthMinutes = new float[] {.5f, 1f, 5f, 10f, 20f, 30f, 60f, 120f, 1440f};

	private int maxDays = 31;

	private GameObject curSun;

	public int latDeg, latMin, latSec;
	
	int prevCycleLength;

	void Awake() {
		st = (SceneTime)target;
		curSun = GameObject.Find ("Sun");
		if (curSun == null) {
			Debug.Log ("finding sun....");
			CreateSun(curSun);
			setSunSettings(curSun);
		}

		prevCycleLength = st.dayCycleSelection;
	}

	void Update(){

	}

	public override void OnInspectorGUI(){
		GUILayout.BeginVertical ();

		//We need a button to find or add a sun object
		if (GUILayout.Button ("Find / Add Sun and Moon")) {
			//The sun object will always be called Sun, the mmon object will always be called Moon
			//Search for the sun object.  If one doesn't exist, add a new one.
			GameObject sun = GameObject.Find("Sun");
			if(sun == null){
				CreateSun(sun);
			}
			setSunSettings(sun);
		}

		//Manual sun selection tool
		GUILayout.Space (5);
		curSun = (GameObject)EditorGUILayout.ObjectField ("Sun:", st.sun.gameObject, typeof(GameObject), true);
		st.sun = curSun.transform;

		//Set the sunflare you want to use
		GUILayout.Space (5);
		st.sunFlare = (Flare)EditorGUILayout.ObjectField ("Sun Flare:", st.sunFlare, typeof(Flare), false);

		//Set the amount of real time it takes to complete one in-scene day cycke
		GUILayout.Space(5);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Day Cycle Length:");
		GUILayout.Space(10);
		//int tempCycle = EditorGUILayout.IntSlider(st.dayCycleInMinutes, 1, 1440);
		//st.dayCycleInMinutes = 10*(tempCycle / 10); //We want the slider to move in ten minute intervals
		st.dayCycleSelection = EditorGUILayout.Popup(st.dayCycleSelection, cycleLengthOptions, GUILayout.Width(100));
		GUILayout.EndHorizontal();

		//Date Selector
		GUILayout.Space (5);
		GUILayout.BeginHorizontal ();
		GUILayout.Label("Select Date:", GUILayout.Width(75));
		st.lightingMonth = EditorGUILayout.Popup (st.lightingMonth, months, GUILayout.Width(80));
		st.lightingDay = EditorGUILayout.IntSlider (st.lightingDay, 1, maxDays);
		st.lightingYear = EditorGUILayout.IntField (st.lightingYear, GUILayout.Width(100));
		GUILayout.EndHorizontal ();

		//Default Time selector
		GUILayout.Space(5);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Time of Day:", GUILayout.Width(80));
		st.curHour = EditorGUILayout.IntSlider(st.curHour, 0, 23);
		st.curMinute = EditorGUILayout.IntSlider(st.curMinute, 0, 59);
		GUILayout.EndHorizontal();

		//Lat & Long input
		GUILayout.Space (5);
		GUILayout.Label ("Latitutde (-90 South, 90 North)");

		//deg min sec input
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Deg", GUILayout.Width (30));
		st.latDeg = EditorGUILayout.IntField (st.latDeg, GUILayout.Width(40));
		GUILayout.Label ("Min", GUILayout.Width (30));
		st.latMin = EditorGUILayout.IntField (st.latMin, GUILayout.Width(40));
		GUILayout.Label ("Sec", GUILayout.Width (30));
		st.latSec = EditorGUILayout.IntField (st.latSec, GUILayout.Width(40));
		GUILayout.EndHorizontal ();

		GUILayout.Label ("Longitude (180 West, -180 East)");
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Deg", GUILayout.Width (30));
		st.lonDeg = EditorGUILayout.IntField (st.lonDeg, GUILayout.Width(40));
		GUILayout.Label ("Min", GUILayout.Width (30));
		st.lonMin = EditorGUILayout.IntField (st.lonMin, GUILayout.Width(40));
		GUILayout.Label ("Sec", GUILayout.Width (30));
		st.lonSec = EditorGUILayout.IntField (st.lonSec, GUILayout.Width(40));
		GUILayout.EndHorizontal ();

		if(GUILayout.Button("Calculate Sun Angle")){
			st.TestAngle();
		}

		GUILayout.EndVertical();

		if (GUI.changed) {

			//Make sure that the max days in the day slider are correct
			if (st.lightingMonth == 0 || st.lightingMonth == 2 || st.lightingMonth == 4 || st.lightingMonth == 6
			    || st.lightingMonth == 7 || st.lightingMonth == 9 || st.lightingMonth == 11) {
				maxDays = 31;
			} else if (st.lightingMonth == 3 || st.lightingMonth == 5 || st.lightingMonth == 8 || st.lightingMonth == 10) {
				maxDays = 30;
			} else {
				maxDays = 28;
			}

			st.latDeg = Mathf.Clamp(st.latDeg, -90 , 90);
			st.latMin = Mathf.Clamp(st.latMin, 0, 59);
			st.latSec = Mathf.Clamp(st.latSec, 0, 59);

			st.lonDeg = Mathf.Clamp(st.lonDeg, -180 , 180);
			st.lonMin = Mathf.Clamp(st.lonMin, 0, 59);
			st.lonSec = Mathf.Clamp(st.lonSec, 0, 59);

			EditorUtility.SetDirty(st);

			//Update the cycle length if it changed
			if(st.dayCycleSelection != prevCycleLength){
				st.dayCycleInMinutes = cycleLengthMinutes[st.dayCycleSelection];
				Debug.Log (st.dayCycleInMinutes);
				//Update the time factor in scenetime, for testing.
				st.CalculateTimeFactor();
			}
		
		}
	}

	private void CreateSun(GameObject sun){
		sun = new GameObject("Sun");	//Create the new GameObject for the sun, name it "Sun"
		sun.AddComponent( typeof (Light));	//Add a light to the new game Object
		sun.transform.position = new Vector3(0,0,-500);
		curSun = sun;
	}

	private void setSunSettings(GameObject sun){
		//There are a few things that the sun must have in order to work.
		//Here, we either add them to the new sun, or make sure they're set porpely in the existing sun
		if(sun.GetComponent<Light>() == null){
			sun.AddComponent( typeof(Light));
		}
		if(sun.GetComponent<Light>().type != LightType.Directional){
			sun.GetComponent<Light>().type = LightType.Directional; //Make it a directional light
		}
		//Make sure the light's flare is set to the sunflare you want to use
		if(!Flare.Equals(sun.GetComponent<Light>().flare, st.sunFlare)){
			sun.GetComponent<Light>().flare = st.sunFlare;
		}
		//Now set the shadows
		if(sun.GetComponent<Light>().shadows != LightShadows.Hard){
			sun.GetComponent<Light>().shadows = LightShadows.Hard; //Make it a directional light
		}

		//The sun object needs a Sun class
		if(sun.GetComponent<Sun>() == null){
			sun.AddComponent(typeof (Sun));
			//Set the default min and max values
			sun.GetComponent<Sun>().maxBrightness = 0.5f;
			sun.GetComponent<Sun>().minBrightness = 0.0f;
		}
		
		//Set the scene's sun to either the found sun, or the newly created one
		st.sun = sun.transform;
	}

}
