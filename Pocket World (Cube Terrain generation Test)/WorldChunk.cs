//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEngine;

using LibNoise;
using LibNoise.Generator;
using LibNoise.Operator;

using PWorlds.Gen.Biomes;

//A single chunk of the world map (32x32 blocks)
namespace PWorlds.Gen
{
	public class WorldChunk
	{
		public static int chunkSize = 32;
		private GameObject chunkObj;
		private GameObject chunkWater; //The water for this chunk.

		private float increment;

		private int totalSize;

		private float[,] heights;

		public WorldChunk (Noise2D heightmap, Vector2 pos, int numDivisions,
		                   float hMin, float increment, Biome biome,
		                   int totalSize)
		{
			if(chunkObj == null){
				chunkObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
				chunkObj.name = "Chunk: " + pos.ToString();
				chunkObj.transform.position = new Vector3(pos.x*chunkSize, 0, 
				                                          pos.y*chunkSize);
			}

			this.increment = increment;
			this.totalSize = totalSize;

			heights = new float[chunkSize,chunkSize];

			int localX = (int)pos.x*chunkSize;
			int localZ = (int)pos.y*chunkSize;

			//Modify the heights so that the minimum height is at 0.0
			for(int i = 0; i<chunkSize; i++){
				for(int j = 0; j<chunkSize; j++){
					heights[i,j] = (heightmap[localX+i,localZ+j]*
						biome.heightMultiplier)-hMin;
				}
			}

			//Determine which division each point belongs in, then
			//adjust the height to match that division's incremental height
			for(int i = 0; i<chunkSize; i++){
				for(int j = 0; j<chunkSize; j++){
					int div = Mathf.FloorToInt(heights[i,j]/increment);
					heights[i,j] = (div)*increment;
				}
			}

			//List of new verticies
			List<Vector3> verts = new List<Vector3>();
			//List of new triangles
			List<int> tris = new List<int>();
			//List of new UVs
			List<Vector2> uvs = new List<Vector2>();
			//The new mesh for the map.
			Mesh mesh;
			
			mesh = chunkObj.GetComponent<MeshFilter>().mesh;
			
			//Build the terrain as a single mesh.
			/*                 5         6
			 *                 /--------/
			 *				  / |      /|
			 *				 /	|     / |
			 *			  1 /--------/2 |
			 *              | 4/-----|--/ 7
			 *              | /      | /
			 *              |/       |/
			 *              /--------/3
			 *             0
			 * 
			 * 	Front:  1 -------- 2	Back:   6 -------- 5
			 * 			  |      |				  |      |
			 * 			  |      |				  |      |
			 *  		0 -------- 3            7 -------- 4
			 * 
			 *  Right:  2 -------- 6	Left:   5 -------- 1
			 * 			  |      |				  |      |
			 * 			  |      |				  |      |
			 *  		3 -------- 7            4 -------- 0
			 * 
			 *  Top:    5 -------- 6	Bottom: 0 -------- 3
			 * 			  |      |			   	  |      |
			 * 			  |      |				  |      |
			 *  		1 -------- 2            4 -------- 7
			 * 
			 *  Front-1: 0,1,3		Right-1: 3,2,7		Top-1:    1,5,2
			 *  Front-2: 2,3,1		Right-2: 6,7,2		Top-2:    6,2,5
			 *  Back-1:  7,6,4		Left-1:  4,5,0		Bottom-1: 4,0,7
			 *  Back-2:	 5,4,6		Left-2:  1,0,5		Bottom-2: 3,7,0
			 * 
			 * 	Get the height of the cube we're looking at.
			 * */
			
			for(int x = 0; x<chunkSize; x++){
				for(int z = 0; z<chunkSize; z++){
					
					//Look around the block to determine if a face needs
					//to be drawn, and then determine the height the face
					//needs to be at.

					//Check to see if we're at an edge.  If we are, draw
					//all the way to the base.  This will not affect
					//how the top is drawn.

					//Top
					BuildFace(new Vector3(x, heights[x,z], z), Vector3.forward, 
					          Vector3.right, true, verts, tris, uvs);

					//Left
					//Are we at the edge of the world map?
					if(isAtEdge(x-1,z,pos)){
						BuildFace(new Vector3(x, 0, z), 
						          new Vector3(0, heights[x,z], 0), 
						          Vector3.forward, false, verts, tris, uvs);
					} else {
						//if not, Get teh height of the adjacent block
						float ht = heights[x,z] -
							getAdjacentHeight(x-1,z,pos,heightmap, biome, hMin);
						//If this height is greater than 0, then we need to
						//draw that face
						if(ht > 0.0f){
							BuildFace(new Vector3(x, heights[x,z]-ht, z), 
							          new Vector3(0, ht, 0), 
							          Vector3.forward, false, verts, tris, uvs);
						}
					}


					//Right
					if(isAtEdge(x+1,z,pos)){
						BuildFace(new Vector3(x+1, 0, z), 
						          new Vector3(0, heights[x,z], 0), 
						          Vector3.forward, true, verts, tris, uvs);
					}else {
						float ht = heights[x,z] -
							getAdjacentHeight(x+1,z,pos,heightmap, biome, hMin);
						if(ht > 0.0f){
							BuildFace(new Vector3(x+1, heights[x,z]-ht, z), 
							          new Vector3(0, ht, 0), 
							          Vector3.forward, true, verts, tris, uvs);
						}
					}


					//Back
					if(isAtEdge(x, z-1, pos)){
						BuildFace(new Vector3(x, 0, z), 
						          new Vector3(0, heights[x,z], 0), 
						          Vector3.right, true, verts, tris, uvs);
					} else {
						float ht = heights[x,z] -
							getAdjacentHeight(x,z-1,pos,heightmap, biome, hMin);
						if(ht > 0.0f){
							BuildFace(new Vector3(x, heights[x,z]-ht, z), 
							          new Vector3(0, ht, 0), 
							          Vector3.right, true, verts, tris, uvs);
						}
					}


					//Front
					if(isAtEdge(x, z+1, pos)){
						BuildFace(new Vector3(x, 0, z+1), 
						          new Vector3(0, heights[x,z], 0), 
						          Vector3.right, false, verts, tris, uvs);
					}else {
						float ht = heights[x,z] -
							getAdjacentHeight(x,z+1,pos,heightmap, biome, hMin);
						if(ht > 0.0f){
							BuildFace(new Vector3(x, heights[x,z]-ht, z+1), 
							          new Vector3(0, ht, 0), 
							          Vector3.right, false, verts, tris, uvs);
						}
					}

				}
			}
			mesh.Clear();
			mesh.vertices = verts.ToArray();
			mesh.triangles = tris.ToArray();
			mesh.Optimize();
			mesh.RecalculateNormals();

			//Do we need to build water?
			bool hasWater = false;
			if(biome.hasWater){
				for(int i = 0; i<chunkSize; i++){
					for(int j = 0; j<chunkSize; j++){
						if(heights[i,j] <= biome.waterLevel){
							hasWater = true;
							break;
						}
					}
					if(hasWater){
						break;
					}
				}
			}

			if(hasWater){
				buildWater(biome, pos, totalSize);
			}


		}

		//Build the water mesh
		private void buildWater(Biome biome, Vector2 pos, int totalSize){
			//Build the water mesh
			if(chunkWater == null){
				chunkWater = GameObject.CreatePrimitive(PrimitiveType.Cube);
				chunkWater.name = "Water";
				chunkWater.transform.position = new Vector3(pos.x*chunkSize, 
				                                            0, 
				                                            pos.y*chunkSize);
				chunkWater.transform.parent = chunkObj.transform;
			}

			//List of new verticies
			List<Vector3> verts = new List<Vector3>();
			//List of new triangles
			List<int> tris = new List<int>();
			//List of new UVs
			List<Vector2> uvs = new List<Vector2>();
			//The new mesh for the map.
			Mesh mesh;

			mesh = chunkWater.GetComponent<MeshFilter>().mesh;

			for(int x = 0; x<chunkSize; x++){
				for(int z = 0; z<chunkSize; z++){
					//Check to see if this location is below the water level 
					//cutoff if it is, create a cube of water.  The waterblock 
					//should be slightly lower than the incremental height 
					//of the division.

					//calculate the dive this is in.
					int div = Mathf.FloorToInt(heights[x,z]/increment);

					if(div < biome.waterLevel){
						//Debug.Log (div + " | " + heights[x,z]);
						//The only faces which need to be created are the top
						//face, and any face which is against the edge of the
						//visible map.

						//Increment modifier for water height;
						float iM = increment*0.21875f;
						//The height of the water face
						//float wH = biome.waterLevel*increment+iM;
						float wH = biome.waterLevel*increment-iM;

						//Top
						BuildFace(new Vector3(x, wH, z), 
						          Vector3.forward, Vector3.right, 
						          true, verts, tris, uvs);

						//Left
						if(isAtEdge(x-1, z, pos)){
							BuildFace(new Vector3(x, heights[x,z], z),
							          new Vector3(0, wH-heights[x,z], 0),
							          Vector3.forward,
							          false, verts, tris, uvs);
						}
						//Right
						if(isAtEdge(x+1, z, pos)){
							BuildFace(new Vector3(x+1, heights[x,z], z), 
							          new Vector3(0, wH-heights[x,z], 0), 
							          Vector3.forward, 
							          true, verts, tris, uvs);
						}
						//Back
						if(isAtEdge(x, z-1, pos)){
							BuildFace(new Vector3(x, heights[x,z], z), 
							          new Vector3(0, wH-heights[x,z], 0), 
							          Vector3.right, true, verts, tris, uvs);
						}
						//Front
						if(isAtEdge(x, z+1, pos)){
							BuildFace(new Vector3(x, heights[x,z], z+1), 
							          new Vector3(0, wH-heights[x,z], 0), 
							          Vector3.right, 
							          false, verts, tris, uvs);
						}
					}
				}
			}

			mesh.Clear();
			mesh.vertices = verts.ToArray();
			mesh.triangles = tris.ToArray();
			mesh.Optimize();
			mesh.RecalculateNormals();
		}

		//Used during terrain generation.  First check to see if we're, at an
		//edge.  If we're not, then check to see if the currnet height is lower
		//than the adjacent height. If it is, we do not need to draw that face
		private bool isTransparent(int x, int z, Vector2 pos, float ht){
			//Rather than checking adjacent blocks, we are only checking to see
			//if the location would be outside of the visible map
			
			float locX = x+pos.x*chunkSize;
			float locZ = z+pos.y*chunkSize;
			if(locX <0 || locX >totalSize-1 || locZ<0 || locZ> totalSize-1){
				return true;
			}
			//We have to be able to retrieve data from a different
			//chunk.  for now, jsut return false if we leave the chunk bounds
			//if(x<0 || x>chunkSize-1 || z<0 || z>chunkSize-1){
				//return false;
			//}
			//if(heights[x,z] < ht){
				//return true;
			//}
			return false;
		}

		//Used during water generation, check to see if we're at an edge.
		//If we are not, return false.
		private bool isAtEdge(int x, int z, Vector2 pos){
			//Rather than checking adjacent blocks, we are only checking to see
			//if the location would be outside of the visible map

			float locX = x+pos.x*chunkSize;
			float locZ = z+pos.y*chunkSize;
			if(locX <0 || locX >totalSize-1 || locZ<0 || locZ> totalSize-1){
				return true;
			}
			return false;
		}

		private float getAdjacentHeight(int x, int z, Vector2 pos, 
		                                Noise2D heightmap, Biome biome,
		                                float hMin){
			int lX = (int)pos.x*chunkSize+x;
			int lZ = (int)pos.y*chunkSize+z;

			int div = Mathf.FloorToInt(
				((heightmap[lX, lZ]*biome.heightMultiplier)-hMin)/increment);

			return div*increment;
		}

		//Build a single face
		private void BuildFace(Vector3 corner, Vector3 up, Vector3 right,
		                       bool isReversed,
		                       List<Vector3> verts, List<int> tris,
		                       List<Vector2> uvs){
			
			int index = verts.Count;
			
			verts.Add(corner);
			verts.Add(corner + up);
			verts.Add(corner + up + right);
			verts.Add(corner+right);
			
			float uvWidth = 1/chunkSize;
			
			Vector2 uvCorner = new Vector2(0,index*uvWidth);
			uvs.Add(uvCorner);
			uvs.Add(new Vector2(uvCorner.x, uvCorner.y+uvWidth));
			uvs.Add(new Vector2(uvCorner.x+uvWidth, uvCorner.y+uvWidth));
			uvs.Add(new Vector2(uvCorner.x+uvWidth, uvCorner.y));
			
			if (isReversed)
			{
				tris.Add(index + 0);
				tris.Add(index + 1);
				tris.Add(index + 2);
				tris.Add(index + 2);
				tris.Add(index + 3);
				tris.Add(index + 0);
			}
			else
			{
				tris.Add(index + 1);
				tris.Add(index + 0);
				tris.Add(index + 2);
				tris.Add(index + 3);
				tris.Add(index + 2);
				tris.Add(index + 0);
			}
		}

		//Set the chunk's material
		public void setMaterialTerrain(Material mat){
			chunkObj.GetComponent<Renderer>().material = mat;
		}
		public void setMaterialWater(Material mat){
			if(chunkWater != null){
				chunkWater.GetComponent<Renderer>().material = mat;
			}
		}

		public void setParent(GameObject parent){
			chunkObj.transform.parent = parent.transform;
		}
	}
}

