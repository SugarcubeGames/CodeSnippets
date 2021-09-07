using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LibNoise;
using LibNoise.Generator;

using PWorlds.Gen.Biomes;
/*
 * The world anchor is the script which holds all the world data for the
 * curently-active world.  For now, it will also call the generation script.
 * */
namespace PWorlds.Gen
{
	public class WorldAnchor : MonoBehaviour {

		public MapSize mapSize;

		// Use this for initialization
		void Start () {

		}
		
		// Update is called once per frame
		void Update () {

			if(Input.GetKeyDown(KeyCode.G)){
				Biome plains = new Plains();
				plains.init();
				this.gameObject.GetComponent<MeshRenderer>().material.mainTexture 
					= Generate(plains, MapSize.LARGE);
			}
		}

		//Rceive a biome that will dictate map generation
		private static Texture2D Generate(Biome biome, int size){

			Texture2D texture;

			Perlin myPerlin = new Perlin();
			myPerlin.Seed = (int)(Random.Range(-2.0f, 2.0f)*10000f);
			myPerlin.OctaveCount = biome.numOctaves;
			myPerlin.Frequency = biome.frequency;
			myPerlin.Persistence = biome.persistence;

			ModuleBase myModule = myPerlin;
			
			Noise2D heightmap;
			heightmap = new Noise2D(size, size, myModule);
			
			heightmap.GeneratePlanar(
				biome.sampleOffsetX,
				biome.sampleOffsetX + biome.sampleSizeX,
				biome.sampleOffsetY,
				biome.sampleOffsetY + biome.sampleSizeY);

			texture = heightmap.GetTexture(GradientPresets.Terrain);

			WorldMap map = new WorldMap(heightmap, biome.numDivisions, size,
			                            biome);

			//cubeRenderer.material.mainTexture = texture;
			return texture;
		}
	}
}
