using UnityEngine;
using System;
using System.Collections;

public class SunAngle : MonoBehaviour {

	//the lengths of each month, in order
	private static int[] daysPerMonth = new int[]{31,28,31,30,31,30,31,31,30,31,30,31};

	public static Vector3 CalculateJulian(float degLat,float degLon, float decHour, int year, int month, int day){
		//DateTime startTime = System.DateTime.Now;

		//TODO: Remove sunrise, sunset and sun duration from this, don't need them
		int totDaysJ = calcTotDays(month, day);
		float julianDate = calcJulianDate(year, month, day, totDaysJ, decHour, degLon);	//get's the Julian date (# of days ins March, 1 4801 B.C.)
		float julianCentury = calcJulianCentury(julianDate);
		float julianGeometricMeanLong = calcJulianGeometricMeanLong(julianCentury);
		float julianGeometricMeanAnomoly = calcJulianGeometricMeanAnomoly(julianCentury);
		float julianEccentricEarthOrbit = calcJulianEccentEarthOrbit(julianCentury);
		float sunEquationOfCenter = calcSunEquationOfCenter(julianCentury, julianGeometricMeanAnomoly);
		float sunTrueLongitude = julianGeometricMeanLong + sunEquationOfCenter;
		float sunTrueAnomoly = julianGeometricMeanAnomoly + sunEquationOfCenter;
		float sunApparentLongitude = calcSunApparentLong(julianCentury, sunTrueLongitude);
		float meanObliqueEcliptic = calcMeanObliqueEcliptic(julianCentury);
		float obliqueCorrection = calcObliqueCorrection(julianCentury, meanObliqueEcliptic);
		float sunRightAscension = calcSunRightAscension(sunApparentLongitude, obliqueCorrection);
		float sunDeclination = calcSunDeclination(sunTrueLongitude, obliqueCorrection);
		float varY = calcVarY(obliqueCorrection);
		float equationOfTime = calcJulianEquationOfTime(julianGeometricMeanLong, 
		                                     julianGeometricMeanAnomoly, julianEccentricEarthOrbit, varY);
		float hourAngleSunrise = calcHourAngleSunrise(degLat, sunDeclination);
		float solarNoon = calcSolarNoon(degLon, equationOfTime);
		float sunriseTime = calcSunriseTime(hourAngleSunrise, solarNoon);
		float sunsetTime = calcSunsetTime(hourAngleSunrise, solarNoon);
		float sunlightDuration = hourAngleSunrise*8f;
		float trueSolarTime = calcTrueSolarTime(decHour, degLon, equationOfTime);
		float hourAngle = calcHourAngle(trueSolarTime);
		float solarZenithAngle = calcSolarZenithAngle(degLat, sunDeclination, hourAngle);
		float solarElevationAngle = 90f - solarZenithAngle;
		float atmosphericRefraction = calcAtmosphericRefraction(solarElevationAngle);
		float solarElevationAngleRefraction = solarElevationAngle + atmosphericRefraction;
		float solarAzimuthAngle = calcSolarAzimuthAngle(degLat, sunDeclination, hourAngle, solarZenithAngle, solarElevationAngleRefraction);

		/*DateTime endTime = System.DateTime.Now;


		Debug.Log ("Total Days: " + totDaysJ + " | " + decHour
				   +"\nJulian Date: " + julianDate + " + " + (julianDate%1)
		           +"\nJulian Century: " + julianCentury
		           +"\nGeometric Mean Longitude: " + julianGeometricMeanLong
		           +"\nGeometric Mean Anomoly: " + julianGeometricMeanAnomoly
		           +"\nEccent Earth Orbit: " + julianEccentricEarthOrbit
		           +"\nSun Equation of Center: "+ sunEquationOfCenter
		           +"\nSun True Longitude: " + sunTrueLongitude
		           +"\nSun True Anomoly: " + sunTrueAnomoly
		           +"\nSun Applied Longitude: " + sunApparentLongitude
		           +"\nMean Oblique Ecliptic: " + meanObliqueEcliptic
		           +"\nOblique Correction: " + obliqueCorrection
		           +"\nSun Right Ascension: " + sunRightAscension
		           +"\nSun Declination: " + sunDeclination
		           +"\nVar Y: " + varY
		           +"\nEquation of Time: " + equationOfTime
		           +"\nSunrise Hour Angle: " + hourAngleSunrise
		           +"\nSolar Noon: " + solarNoon
		           +"\nSunrise Time: " + sunriseTime
		           +"\nSunset Time: " + sunsetTime
		           +"\nSunlight Duration: " + sunlightDuration
		           +"\nTrue Solar Time: " + trueSolarTime
		           +"\nHour Angle: " + hourAngle
		           +"\nSolar Zenith Angle: " + solarZenithAngle
		           +"\nSolar Elevation Angle: " + solarElevationAngle
		           +"\nAtmospheric Refraction: " + atmosphericRefraction
		           +"\nSolar Elevation w/ Refraction Correction: " + solarElevationAngleRefraction
		           +"\nSolar Azimuth Angle: " + solarAzimuthAngle
		           +"\n"
		           +"\nCalculation Time: " + (endTime.Subtract(startTime) + " | " + startTime + " | " + endTime)
		           +"\n");*/
		           
		return new Vector3(solarElevationAngleRefraction, solarAzimuthAngle, solarElevationAngleRefraction);

	}

	public static Vector2 calcSunDuration(float degLat,float degLon, float decHour, int year, int month, int day){

		int totDaysJ = calcTotDays(month, day);
		float julianDate = calcJulianDate(year, month, day, totDaysJ, decHour, degLon);	//get's the Julian date (# of days ins March, 1 4801 B.C.)
		float julianCentury = calcJulianCentury(julianDate);
		float julianGeometricMeanLong = calcJulianGeometricMeanLong(julianCentury);
		float julianGeometricMeanAnomoly = calcJulianGeometricMeanAnomoly(julianCentury);
		float julianEccentricEarthOrbit = calcJulianEccentEarthOrbit(julianCentury);
		float sunEquationOfCenter = calcSunEquationOfCenter(julianCentury, julianGeometricMeanAnomoly);
		float sunTrueLongitude = julianGeometricMeanLong + sunEquationOfCenter;
		float sunTrueAnomoly = julianGeometricMeanAnomoly + sunEquationOfCenter;
		float sunApparentLongitude = calcSunApparentLong(julianCentury, sunTrueLongitude);
		float meanObliqueEcliptic = calcMeanObliqueEcliptic(julianCentury);
		float obliqueCorrection = calcObliqueCorrection(julianCentury, meanObliqueEcliptic);
		//float sunRightAscension = calcSunRightAscension(sunApparentLongitude, obliqueCorrection);
		float sunDeclination = calcSunDeclination(sunTrueLongitude, obliqueCorrection);
		float varY = calcVarY(obliqueCorrection);
		float equationOfTime = calcJulianEquationOfTime(julianGeometricMeanLong, 
		                                                julianGeometricMeanAnomoly, julianEccentricEarthOrbit, varY);
		float hourAngleSunrise = calcHourAngleSunrise(degLat, sunDeclination);
		float solarNoon = calcSolarNoon(degLon, equationOfTime);
		//float sunriseTime = calcSunriseTime(hourAngleSunrise, solarNoon);
		//float sunsetTime = calcSunsetTime(hourAngleSunrise, solarNoon);
		float sunlightDuration = hourAngleSunrise*8f;

		return new Vector2(solarNoon, sunlightDuration);
	}

	private static int calcTotDays(int month, int day){
		//Works!
		int totDays = 0;
		for(int i = 0; i<month; i++){
			totDays += daysPerMonth[i]; //Add each month's total days until we hit our current month
		}
		//Then add the total number of days into the month we are.
		totDays += day;
		
		//Debug.Log("Day of the year: " + totDays);
		return totDays;
	}

	private static float calcJulianDate(int year, int month, int day, int totDaysJ, float decHour, float degLon){
		//Convert the current Gregorian date to its equivalent Julian date
		//Calculate the Julian date:

		//NOAA Excel sheet method, start with a base date of 01/01/1900, extrapolate hte total number of days from then
		//and add 2415018.5
		int ty = year - 1900;
		float jd = ((float)ty*365f) + ((float)ty/4f) + totDaysJ;
		int lstm = (int)(degLon/ 15f);

		float jdn = jd + 2415018.5f + (decHour/24f) + (lstm / 24f);

		return jdn;
	}

	private static float calcJulianCentury(float jdn){
		//pulled from the NOAA spreadsheet, no explanation

		return (jdn - 2451545f)/36525f;
	}

	private static float calcJulianGeometricMeanLong(float jc){
		//Calculate the geonetric mean longitude of the sun

		float gml = (280.46646f + jc*(36000.76983f+(jc*.0003032f)))%360f;
		return gml;
	}

	private static float calcJulianGeometricMeanAnomoly(float jc){

		float gma = 357.52911f + jc*(35999.05029f - 0.0001537f*jc);
		return gma;
	}

	private static float calcJulianEccentEarthOrbit(float jc){

		float eeo = 0.016708634f-jc*(0.000042037f+0.0000001267f*jc);
		return eeo;
	}

	private static float calcSunEquationOfCenter(float jc, float gma){
		//Convert the gma to radians for ease
		float radGMA = gma*Mathf.Deg2Rad;

		float eoc = Mathf.Sin(radGMA)*(1.914602f-jc*(0.004817f+0.000014f*jc))
			+ Mathf.Sin(2f*radGMA)*(0.019993f-0.000101f*jc)
				+ Mathf.Sin(3f*radGMA)*0.000289f;
		return eoc;
	}

	private static float calcSunApparentLong(float jc, float stl){

		float sal = stl - 0.00569f-0.00478f*Mathf.Sin((125.04f-1934.136f*jc)*Mathf.Deg2Rad);
		return sal;
	}

	private static float calcMeanObliqueEcliptic(float jc){

		float moe = 23f+(26f+((21.448f-jc*(46.815f+jc*(0.00059f-jc*0.001813f))))/60f)/60f;
		return moe;
	}

	private static float calcObliqueCorrection(float jc, float moe){

		float oc = moe + 0.00256f*Mathf.Cos((125.04f-1934.136f*jc)*Mathf.Deg2Rad);
		return oc;
	}

	private static float calcSunRightAscension(float sal, float oc){

		float x = Mathf.Cos(sal*Mathf.Deg2Rad);
		float y = Mathf.Cos(oc*Mathf.Deg2Rad)*Mathf.Sin(sal*Mathf.Deg2Rad);
		float sra = Mathf.Rad2Deg*(Mathf.Atan2(y,x));

		return sra;
	}

	private static float calcSunDeclination(float stl, float oc){

		float sd = (Mathf.Asin(Mathf.Sin (oc*Mathf.Deg2Rad)*Mathf.Sin (stl*Mathf.Deg2Rad)))*Mathf.Rad2Deg;
		return sd;
	}

	private static float calcVarY(float oc){
	
		float vy = (Mathf.Tan ((oc/2)*Mathf.Deg2Rad));
		vy *= vy;

		return vy;
	}

	private static float calcJulianEquationOfTime(float gml, float gma, float eeo, float vy){

		//gml & gma need to be in radians for this, so let's go ahead and convert them for ease
		gml *= Mathf.Deg2Rad;
		gma *= Mathf.Deg2Rad;

		//Break this equation into parts for ease
		float p1 = vy * Mathf.Sin (gml*2f);
		float p2 = 2f*eeo*Mathf.Sin (gma);
		float p3 = 4f*eeo*vy*Mathf.Sin (gma)*Mathf.Cos(gml*2f);
		float p4 = 0.5f * vy*vy*Mathf.Sin (gml*4f);
		float p5 = 1.25f*eeo*eeo*Mathf.Sin (gma*2f);

		float eot = (p1 - p2 + p3 - p4 - p5)*Mathf.Rad2Deg*4;

		return eot;
	}

	private static float calcHourAngleSunrise(float degLat, float sd){

		//convert degrees latitude to radians for ease, same for declination
		float radLat = degLat*Mathf.Deg2Rad;
		float rsd = sd*Mathf.Deg2Rad;

		float has = (Mathf.Acos(Mathf.Cos (90.833f*Mathf.Deg2Rad)/(Mathf.Cos(radLat)*Mathf.Cos(rsd))
		                        -Mathf.Tan(radLat)*Mathf.Tan(rsd))) * Mathf.Rad2Deg;
		return has;
	}

	private static float calcSolarNoon(float degLong, float eot){

		//first, calculate the timezone offset: 1 hr per 15deg
		int gmt = (int)degLong/15;

		float sn = (720f-4f*degLong-eot+gmt*60f)/1440f;
		return sn;
	}

	private static float calcSunriseTime(float has, float sn){
	
		float srt = sn-(has*4f/1440f);
		return srt;
	}

	private static float calcSunsetTime(float has, float sn){
		
		float sst = sn +(has*4f/1440f);
		return sst;
	}

	private static float calcTrueSolarTime(float decHour, float degLong, float eot){

		//first, calculate the timezone offset: 1 hr per 15deg
		int gmt = (int)degLong/15;

		//convert dechour to a percentage of the total day: 
		decHour /= 24f;

		//Debug.Log (decHour + " | " + gmt);

		float tst = (decHour*1440 + eot + 4*degLong - 60*gmt) % 1440;
		return tst;
	}

	private static float calcHourAngle(float tst){
	
		float ha = 0.0f;

		if(tst<0f){
			ha = tst/4f+180f;
		} else {
			ha = tst/4f-180f;
		}

		return ha;
	}

	private static float calcSolarZenithAngle(float degLat, float sd, float ha){
	
		//convert degLat, sd & ha to radians
		float radLat = degLat*Mathf.Deg2Rad;
		float rsd = sd*Mathf.Deg2Rad;
		float rha = ha*Mathf.Deg2Rad;

		float sza = (Mathf.Acos(Mathf.Sin (radLat)* Mathf.Sin (rsd) +
		                        Mathf.Cos(radLat)*Mathf.Cos(rsd)*Mathf.Cos(rha)))*Mathf.Rad2Deg;

		return sza;

	}

	private static float calcAtmosphericRefraction(float sea){
	
		//convert sea to radians for ease
		float rsea = sea*Mathf.Deg2Rad;

		float ar = 0.0f;

		if(sea > 85f){
		} else if(sea > 5f) {
			ar = 58.1f/Mathf.Tan (rsea) - 0.07f/Mathf.Pow(Mathf.Tan (rsea),3) + 0.000086f/Mathf.Pow(Mathf.Tan(rsea), 5);
		} else if(sea > -0.575f) {
			ar = 1735f + sea*(-518.2f+ sea*(103.4f + sea*(-12.79f+sea*0.711f)));
		} else {
			ar = -20.772f/Mathf.Tan (rsea);
		}

		ar /= 3600f;

		return ar;
	}

	private static float calcSolarAzimuthAngle(float degLat, float sd, float ha, float sza, float sear){

		//convert degLat, sd & sza to radians for ease
		float radLat = degLat*Mathf.Deg2Rad;
		float rsd = sd*Mathf.Deg2Rad;
		float rsza = sza*Mathf.Deg2Rad;

		float saa = 0.0f;
		if(ha > 0){
			//Debug.Log ("HA is positive");

			float p1 = (Mathf.Sin(radLat)*Mathf.Cos(rsza)-Mathf.Sin (rsd)) /  (Mathf.Cos (radLat)*Mathf.Sin (rsza));
			float p2 = Mathf.Acos(p1);
			float p3 = (Mathf.Rad2Deg*p2) + 180f;
			saa = p3%360f;

		} else {
			//Debug.Log ("HA is Negative");
			float p1 = (Mathf.Sin(radLat)*Mathf.Cos(rsza)-Mathf.Sin (rsd)) /  (Mathf.Cos (radLat)*Mathf.Sin (rsza));
			float p2 = Mathf.Acos(p1);
			float p3 = 540f - (Mathf.Rad2Deg*p2);
			saa = p3%360f;

		}

		return saa;
	}
}
