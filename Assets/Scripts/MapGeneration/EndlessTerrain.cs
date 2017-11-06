﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour {

	const float scale = 1;

	const float viewerMoveThresholdForChunkUpdate = 25f;
	const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

	public LODInfo[] detailLevels;
	public static float maxViewDistance;
	
	public Transform viewer;
	public Material mapMaterial;

	public static Vector2 viewerPosition;
	Vector2 viewerPositionOld;
	static MapGenerator mapGenerator;
	private int chunkSize;
	private int chunksVisibleInViewDistance;

	private Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
	static private List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

	private void Start(){
		mapGenerator = FindObjectOfType<MapGenerator>();

		maxViewDistance = detailLevels[detailLevels.Length - 1].visibleDistanceThreshold;
		chunkSize = MapGenerator.mapChunkSize - 1;
		chunksVisibleInViewDistance = Mathf.RoundToInt(maxViewDistance / chunkSize);

		UpdateVisibleChunks();
	}

	private void Update(){
		viewerPosition = new Vector2(viewer.position.x, viewer.position.z) / scale;

		if((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate) {
			viewerPositionOld = viewerPosition;
			UpdateVisibleChunks();
		}
	}

	private void UpdateVisibleChunks(){

		for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++){
			terrainChunksVisibleLastUpdate[i].SetVisible(false);
		}
		terrainChunksVisibleLastUpdate.Clear();

		int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
		int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

		for (int yOffset = -chunksVisibleInViewDistance; yOffset <= chunksVisibleInViewDistance; yOffset++){
			for (int xOffset = -chunksVisibleInViewDistance; xOffset <= chunksVisibleInViewDistance; xOffset++){
				Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

				if(terrainChunkDictionary.ContainsKey(viewedChunkCoord)){
					terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
				} else {
					terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, detailLevels, transform, mapMaterial));
				}
			}
		}
	}

	public class TerrainChunk {
		GameObject meshObject;
		Vector2 position;
		Bounds bounds;

		MeshRenderer meshRenderer;
		MeshFilter meshFilter;

		LODInfo[] detailLevels;
		LODMesh[] lodMeshes;

		MapData mapData;
		bool mapDataRecieved;
		int previousLODIndex = -1;

		public TerrainChunk(Vector2 coordinate, int size, LODInfo[] detailLevels, Transform parent, Material material){
			this.detailLevels = detailLevels;

			position = coordinate * size;
			bounds = new Bounds(position, Vector2.one * size);
			Vector3 positionV3 = new Vector3(position.x, 0, position.y);

			meshObject = new GameObject("Terrain Chunk");
			meshRenderer = meshObject.AddComponent<MeshRenderer>();
			meshFilter = meshObject.AddComponent<MeshFilter>();
			meshRenderer.material = material;

			meshObject.transform.position = positionV3 * scale;
			meshObject.transform.SetParent(parent);	
			meshObject.transform.localScale = Vector3.one * scale;	
			SetVisible(false);

			lodMeshes = new LODMesh[detailLevels.Length];
			for (int i = 0; i < detailLevels.Length; i++){
				lodMeshes[i] = new LODMesh(detailLevels[i].lod, UpdateTerrainChunk);
			}

			mapGenerator.RequestMapData(position, OnMapDataRecieved);
		}

		private void OnMapDataRecieved(MapData mapData){
			this.mapData = mapData;
			mapDataRecieved = true;

			Texture2D texture = TextureGenerator.TextureFromColorMap(mapData.colorMap, MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
			meshRenderer.material.mainTexture = texture;

			UpdateTerrainChunk();
		}

		public void UpdateTerrainChunk(){
			if(mapDataRecieved){
				float viewerDistanceFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
				bool visible = viewerDistanceFromNearestEdge <= maxViewDistance;

				if(visible) {
					int lodIndex = 0;

					for (int i = 0; i < detailLevels.Length - 1; i++)
					{
						if(viewerDistanceFromNearestEdge > detailLevels[i].visibleDistanceThreshold){
							lodIndex = i + 1;
						} else {
							break;
						}
					}

					if(lodIndex != previousLODIndex){
						LODMesh lodMesh = lodMeshes[lodIndex];
						if(lodMesh.hasMesh){
							previousLODIndex = lodIndex;
							meshFilter.mesh = lodMesh.mesh;
						}
						else if(!lodMesh.hasRequestedMesh){
							lodMesh.RequestMesh(mapData);
						}
					}

					terrainChunksVisibleLastUpdate.Add(this);
						
					
				}
				SetVisible(visible);
			}
		}

		public void SetVisible(bool visible){
			meshObject.SetActive(visible);
		}

		public bool IsVisible(){
			return meshObject.activeSelf;
		}
	}

	private class LODMesh {
		public Mesh mesh;
		public bool hasRequestedMesh;
		public bool hasMesh;
		private int lod;
		System.Action updateCallback;

		public LODMesh(int lod, System.Action updateCallback){
			this.lod = lod;
			this.updateCallback = updateCallback;
		}

		private void OnMeshDataRecieved(MeshData meshData){
			mesh = meshData.CreateMesh();
			hasMesh = true;

			updateCallback();
		}

		public void RequestMesh(MapData mapData){
			hasRequestedMesh = true;
			mapGenerator.RequestMeshData(mapData, lod, OnMeshDataRecieved);
		}
	}

	[System.Serializable]
	public struct LODInfo {
		public int lod;
		public float visibleDistanceThreshold;
	}
}
