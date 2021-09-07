using UnityEngine;
using System.Collections;

namespace PWorlds.Gen.Biomes{
	public class Plains:Biome{

		public override void init()
		{
			this.sampleSizeX = 4.0f;
			this.sampleSizeY = 4.0f;

			this.sampleOffsetX = 2.0f;
			this.sampleOffsetY = 1.0f;

			this.numModules = 1;

			this.numOctaves = 6;
			this.frequency = .6f;
			this.persistence = .1f;

			//this.numOctaves = 6;
			//this.frequency = 2f;
			//this.persistence = .5f;

			/*
			this.topColor = Color.green;
			this.bottomColor = new Color(0.0f, 0.0f, 0.0f);
			*/

			//Desert coloration is easier to look at, will have to adjust plains
			//this.topColor = new Color(1.0f,(195.0f/255.0f), 0.0f);
			//this.bottomColor = Color.red;
			this.topColor = Color.green;
			this.bottomColor = Color.black;
			this.numDivisions = 16;

			this.heightMultiplier = 6.0f;

			//Water variables
			this.hasWater = true;
			this.waterLevel = 4;
			//transparent cyan color
			this.waterColor = new Color(0.0f, 1.0f, 1.0f, .25f);

		}
	}
}