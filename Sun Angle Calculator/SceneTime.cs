using UnityEngine;
using System.Collections;

[System.Serializable]
public class SceneTime : MonoBehaviour {

	public Transform sun; //Holds the sun pos. data
	private Sun sunScr; //pointer to the sun's sun script

	public float dayCycleInMinutes; //How many real-life minutes an in-game day lasts
	public int dayCycleSelection; //Reference variable for the editor's day cycle length selector

	//Time constants
	private const float SECOND = 1;				//1 second
	private const float MINUTE = 60 * SECOND;	//Num of seconds in a minute
	private const float HOUR = 60 * MINUTE;		//Num of seconds in an hour
	private const float DAY = 24 * HOUR;		//Num of seconds in a day

	private const float DEGREES_PER_SECOND = 360 / DAY;	//360 per rotation divided by total sec in day

	private float timeConversionFactor; //The factor which converts real time to scene time.

	private float degreeRotation;

	private int[] daysPerMonth = new int[]{31,28,31,30,31,30,31,31,30,31,30,31};

	//The sunflare you want to use for the scene
	public Flare sunFlare;

	//Date data
	public int lightingYear;
	public int lightingMonth;
	public int lightingDay;

	//Latitude and Longitude variables
	public int latDeg;
	public int latMin;
	public int latSec;

	public int lonDeg;
	public int lonMin;
	public int lonSec;

	//degree values for position, for passing into SunAngle
	private float degLat;
	private float degLon;

	//Current day's sun duration and solar noon
	private float solarNoon;
	private float sunDuration;
	private float skyBlendDuration; //how long it takes for the skybox to blend from night to day or day to night

	//Time of Day
	public int curHour;
	public int curMinute;
	public float curSecond;

	void Start(){
		//check for a sun
		if (sun == null) {
			Debug.Log("Sun not assigned");
			return; //Prevent it from going any further to avoid errors
		}

		//We need to make sure the Sun script exists, and reference the Sun script on our sun
		sunScr = sun.gameObject.GetComponent<Sun>();
		if(sunScr == null){
			sun.gameObject.AddComponent(typeof(Sun));
			sunScr = sun.gameObject.GetComponent<Sun>();
			//Set max and min brightness
			sunScr.maxBrightness = 0.5f;
			sunScr.minBrightness = 0.0f;
		}

		CalculateTimeFactor();

		degLat = latDeg + (latMin/60f) + (latSec/3600f);
		degLon = lonDeg + (lonMin/60f) + (lonSec/3600f);

		//setup the sun duration for the initial day
		setSunDurations();

		//And then set up the initial sky blend
		updateSkyBox();

		//Debug.Log("Time Conversion Factor: " + timeConversionFactor); 
	}

	void Update(){

		curSecond += (Time.deltaTime * timeConversionFactor) ;
		//if curSecond is greater than 60, that means a minute has passed, and we need to add a minute to
		//curMinute.  We also need to account for the possiblity of more than oen minute having passed
		if(curSecond > 60){
			curMinute += Mathf.FloorToInt((curSecond/60f));
			curSecond = curSecond % 60; //set seconds back to below 60.
		}
		//Now do the same for hours
		if(curMinute > 60){
			curHour += Mathf.FloorToInt((curMinute/60f));
			curMinute = curMinute % 60; //set seconds back to below 60.
		}

		//now, account for transitions in the day (for now, just reset to the same day
		//TODO: Move the day forward one, and check for month transition.
		if(curHour > 23){
			curHour = curHour - 23;

			//Add one to the day
			lightingDay++;

			if(lightingDay > daysPerMonth[lightingMonth]){ //Check to see if we've reached the end of the month
				lightingDay = 1;
				lightingMonth ++;

				//Now to check and see if we need to move on to the next year
				if(lightingMonth > 11){
					lightingMonth = 0;
					lightingYear++;
				}
			}

			//since it's a new day, we need to recalculate sunup, sundown, and skyblox blend settings
			setSunDurations();
		}

		CalculateSunAngle();

		//Blend the skybox for the current time
		updateSkyBox();
	}

	public void CalculateSunAngle(){

		float decHour = curHour + (curMinute/60f) + (curSecond/3600f);

		Vector3 newRot = SunAngle.CalculateJulian(degLat, degLon, decHour, lightingYear, lightingMonth, lightingDay);
		sun.eulerAngles = newRot;
	}

	public void TestAngle(){
		float degLat = latDeg + (latMin/60f) + (latSec/3600f);
		float degLon = lonDeg + (lonMin/60f) + (lonSec/3600f);
		float decHour = curHour + (curMinute/60f) + (curSecond/3600f);

		SunAngle.CalculateJulian(degLat, degLon, decHour, lightingYear, lightingMonth, lightingDay);
		setSunDurations();
	}

	public void CalculateTimeFactor(){
		timeConversionFactor = DAY / (dayCycleInMinutes * (float) MINUTE);
	}

	private void setSunDurations(){
		//Debug.Log ("Set Sun Duration");
		float degLat = latDeg + (latMin/60f) + (latSec/3600f);
		float degLon = lonDeg + (lonMin/60f) + (lonSec/3600f);
		float decHour = curHour + (curMinute/60f) + (curSecond/3600f);
		
		Vector2 sDuration = SunAngle.calcSunDuration(degLat, degLon, decHour, lightingYear, lightingMonth, lightingDay);

		solarNoon = sDuration.x;
		sunDuration = sDuration.y;

		float skyBlendOffset = 18f/(180f/sunDuration); //amount of time before sunrise for dawn to start
		//skyBlendDuration = (sunDuration/2)+skyBlendOffset;
		skyBlendDuration = skyBlendOffset;

	}

	private void updateSkyBox(){
		float decHour = curHour + (curMinute/60f) + (curSecond/3600f);

		//calculate the percentage blend at the current time.
		float curBlend = 0.0f;
		//Teh calculation for hte blend percentage changes based on whether it is before or after solar noon.
		if((decHour/24f) < solarNoon){
			curBlend = Mathf.Clamp ((((solarNoon*24f*60f)-(sunDuration/2f)-(decHour*60f))/skyBlendDuration), 0, 1);
		} else {
			curBlend = Mathf.Clamp ( ( ( (decHour*60f) - ((solarNoon*24f*60f)+(sunDuration/2f)) )/skyBlendDuration), 0, 1);
		}
		//Debug.Log (curBlend);

		//apply that %blend to the skybox
		RenderSettings.skybox.SetFloat("_Blend", curBlend);

	}
}
 