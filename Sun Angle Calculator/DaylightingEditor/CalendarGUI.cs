using UnityEngine;
using System.Collections;

public class CalendarGUI : MonoBehaviour {

	//tracks days in the month.  Ignores leap yearas
	public class DaysInMonth{
		public static int JANUARY = 31;
		public static int FEBRUARY = 28;
		public static int MARCH = 31;
		public static int APRIL = 30;
		public static int MAY = 31;
		public static int JUNE = 30;
		public static int JULY = 31;
		public static int AUGUST = 31;
		public static int SEPTEMBER = 30;
		public static int OCTOBER = 31;
		public static int NOVEMBER = 30;
		public static int DECEMBER = 31;
	}

	enum months {January, February, March, April, May, June, July, August, September, October, November, December};



	public static int DAYSINWEEK = 7;

}
