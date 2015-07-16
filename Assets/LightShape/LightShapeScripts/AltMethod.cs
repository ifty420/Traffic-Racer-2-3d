#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;  
using System.Collections.Generic;

public class AltMethod : MonoBehaviour 
{
    Color[] CubeMapColors;
	Color[] CubeMapColors2;
	Color[] CubeMapColors3;
	Color[] CubeMapColorsR;
	
	public LightShapeProbe LightShapeProbe;
	GameObject lightShapeObjTemp;
	GameObject lightShapeObjTempAlt;
	
	GameObject groundShapeObjTemp;
	Material gMat;
	MeshRenderer lightShapeMeshTemp;
	Material lsMat;
	MeshRenderer lightShapeMeshTempAlt;
	Material alsMat;
	MeshRenderer groundShapeObjTempMesh;
	int offSet = 2;
	int row = -1;
	private int rewardInt;
	
	public void StartRender()
	{
		if(!LightShapeProbe.LightShapeManager.renderingCubeMaps)
		{
			EditorApplication.isPlaying = true;			
		}		
	}
	
	public void Start()
	{
		if(LightShapeProbe.LightShapeManager.useAltMethod)
		{	
			if(!LightShapeProbe.LightShapeManager.altRendering)
			{
				if(LightShapeProbe.myIndex == LightShapeProbe.LightShapeManager.curProbeInt)
				{						
					StartCoroutine(BatchStart()); 
				}	
			}			
		}
	}
	
	public IEnumerator BatchStart()
	{	
		PlayerSettings.defaultScreenWidth = LightShapeProbe.LightShapeManager.CubeMapSize + offSet;
		PlayerSettings.defaultScreenHeight = LightShapeProbe.LightShapeManager.CubeMapSize + offSet;
		yield return new WaitForEndOfFrame();
		TurnOffCubes();

		RenderCubeMaps();
	}
	
	public void RenderCubeMaps()
	{
		var mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		mainCamera.enabled = false;		
		if(!LightShapeProbe.LightShapeManager)
		{
			LightShapeProbe.LightShapeManager = GameObject.FindWithTag("CubeProbeManager").GetComponent("LightShapeManager") as LightShapeManager;			
		}
		
		if(LightShapeProbe.hasGround && LightShapeProbe.renderLight)
		{
			if(!LightShapeProbe.groundShape)
			{
				LightShapeProbe.groundShape = AssetDatabase.LoadAssetAtPath("Assets/LightShape/Light_Shapes/lightShape_Ground.prefab", typeof(GameObject)) as GameObject;
			}
			
			if(LightShapeProbe.groundShape)
			{
				groundShapeObjTemp = Instantiate(LightShapeProbe.groundShape, transform.position, transform.rotation) as GameObject;
			}
							
			groundShapeObjTemp.transform.Translate(0, -0.1f, 0);
			groundShapeObjTemp.transform.eulerAngles = new Vector3(-90, 0, 0);
			groundShapeObjTempMesh = groundShapeObjTemp.GetComponent<MeshRenderer>();					
									
			var gTex = groundShapeObjTempMesh.sharedMaterial.GetTexture("_MainTex");
			gMat = new Material (Shader.Find("Particles/Alpha Blended"));
			gMat.SetColor("_TintColor", LightShapeProbe.groundColor);
			gMat.SetTexture("_MainTex", gTex);
			groundShapeObjTempMesh.sharedMaterial = gMat;
			
			groundShapeObjTemp.transform.localScale = new Vector3(LightShapeProbe.groundScale, LightShapeProbe.groundScale, LightShapeProbe.groundScale);
		}
		
		if(LightShapeProbe.renderLight)
		{			
			if(!LightShapeProbe.myLight)
			{
				Debug.LogError("No Light Assigned. Please Assign a Light to `Use Light` to use the `Render Light` option.");
				EditorApplication.isPlaying = false;
			}
			
			if(LightShapeProbe.myLight)
			{
				if(!LightShapeProbe.lightShape)
				{
					LightShapeProbe.lightShape = AssetDatabase.LoadAssetAtPath("Assets/LightShape/Light_Shapes/lightShape_default.prefab", typeof(GameObject)) as GameObject;
				}
				
				if(LightShapeProbe.lightShape)
				{
					lightShapeObjTemp = Instantiate(LightShapeProbe.lightShape, transform.position, transform.rotation) as GameObject;
				}
							
				lightShapeMeshTemp = lightShapeObjTemp.GetComponent<MeshRenderer>();					
												
				var lsTex = lightShapeMeshTemp.sharedMaterial.GetTexture("_MainTex");
				lsMat = new Material (Shader.Find("Particles/Alpha Blended"));

				lsMat.SetTexture("_MainTex", lsTex);
				var altZScale = 0;
				
				if(LightShapeProbe.renderAltLight)
				{
					if(!LightShapeProbe.altLightShape)
					{
						LightShapeProbe.altLightShape = AssetDatabase.LoadAssetAtPath("Assets/LightShape/Light_Shapes/lightShape_Ground.prefab", typeof(GameObject)) as GameObject;
					}
					
					if(LightShapeProbe.altLightShape)
					{
						lightShapeObjTempAlt = Instantiate(LightShapeProbe.altLightShape, transform.position, transform.rotation) as GameObject;
					}
											
					lightShapeMeshTempAlt = lightShapeObjTempAlt.GetComponent<MeshRenderer>();
					
					var alsTex = lightShapeMeshTempAlt.sharedMaterial.GetTexture("_MainTex");

					alsMat = new Material (Shader.Find("Particles/Alpha Blended"));
					alsMat.SetTexture("_MainTex", alsTex);
					
					if(LightShapeProbe.overRideAltLightColor)
					{
						alsMat.SetColor("_TintColor", LightShapeProbe.altShapeColor);	
						lightShapeMeshTempAlt.sharedMaterial = alsMat;
					}
			
					if(!LightShapeProbe.overRideAltLightColor)
					{
						alsMat.SetColor("_TintColor", LightShapeProbe.myBackLight.color);
						lightShapeMeshTempAlt.sharedMaterial = alsMat;
					}						
				}
				
				if(LightShapeProbe.overRideLightColor)
				{
					lsMat.SetColor("_TintColor", LightShapeProbe.lightShapeColor);	
					lightShapeMeshTemp.sharedMaterial = lsMat;
				}
		
				if(!LightShapeProbe.overRideLightColor)
				{
					lsMat.SetColor("_TintColor", LightShapeProbe.myLight.color);
					lightShapeMeshTemp.sharedMaterial = lsMat;
				}
								
				if(LightShapeProbe.myLight.type == LightType.Directional)
				{
					lightShapeObjTemp.transform.rotation = LightShapeProbe.myLight.gameObject.transform.rotation;
					lightShapeObjTemp.transform.position = transform.position;
					lightShapeObjTemp.transform.Translate(0, 0, -5);
					
					if(LightShapeProbe.renderAltLight && !LightShapeProbe.myBackLight)
					{
						lightShapeObjTempAlt.transform.rotation = lightShapeObjTemp.transform.rotation;
						lightShapeObjTempAlt.transform.Translate(0, 0, 10);	
						altZScale = -1;
					}
				}
		
				if(LightShapeProbe.myLight.type == LightType.Spot || LightShapeProbe.myLight.type == LightType.Point)
				{
					var lightDir = Quaternion.LookRotation(transform.position - LightShapeProbe.myLight.gameObject.transform.position);
					lightShapeObjTemp.transform.rotation = lightDir;						
					lightShapeObjTemp.transform.Translate(0, 0, -5);
					
					if(LightShapeProbe.renderAltLight && !LightShapeProbe.myBackLight)
					{
						lightShapeObjTempAlt.transform.rotation = lightShapeObjTemp.transform.rotation;
						lightShapeObjTempAlt.transform.Translate(0, 0, 10);
						altZScale = -1;
					}					
				}
				
				if(LightShapeProbe.renderAltLight  && LightShapeProbe.myBackLight)
				{
					if(LightShapeProbe.myBackLight.type == LightType.Directional)
					{
						lightShapeObjTempAlt.transform.rotation = LightShapeProbe.myBackLight.gameObject.transform.rotation;
						lightShapeObjTempAlt.transform.Translate(0, 0, -10);
						altZScale = 1;
					}
			
					if(LightShapeProbe.myBackLight.type == LightType.Spot || LightShapeProbe.myBackLight.type == LightType.Point)
					{
						var bLightDir = Quaternion.LookRotation(transform.position - LightShapeProbe.myBackLight.gameObject.transform.position);

						lightShapeObjTempAlt.transform.rotation = bLightDir;
						lightShapeObjTempAlt.transform.Translate(0, 0, -10);
						altZScale = 1;
				
					}
				}
				
				lightShapeObjTemp.transform.eulerAngles = new Vector3(lightShapeObjTemp.transform.eulerAngles.x, lightShapeObjTemp.transform.eulerAngles.y, LightShapeProbe.lightRot);					
				lightShapeObjTemp.transform.localScale = new Vector3(LightShapeProbe.lightScaleX, LightShapeProbe.lightScaleY, (LightShapeProbe.lightScaleX + LightShapeProbe.lightScaleY) / 2);
				
				if(LightShapeProbe.renderAltLight)
				{
					lightShapeObjTempAlt.transform.localScale = new Vector3(LightShapeProbe.altLightScaleX, LightShapeProbe.altLightScaleY, (LightShapeProbe.altLightScaleX + LightShapeProbe.altLightScaleY) * altZScale);	
					lightShapeObjTempAlt.transform.eulerAngles = new Vector3(lightShapeObjTempAlt.transform.eulerAngles.x, lightShapeObjTempAlt.transform.eulerAngles.y, LightShapeProbe.altLightRot);
				}			

				StartCoroutine(RenderCube(lightShapeObjTemp, lightShapeObjTempAlt, groundShapeObjTemp, gMat, lsMat, alsMat));
			}						
		}
		
		if(!LightShapeProbe.renderLight)
		{
			StartCoroutine(RenderCube(lightShapeObjTemp, lightShapeObjTempAlt, groundShapeObjTemp, gMat, lsMat, alsMat));
		}		
	}
	
	public IEnumerator RenderCube(GameObject lightShapeObjTemp, GameObject lightShapeObjTempAlt, GameObject groundShapeObjTemp, Material gMat, Material lsMat, Material alsMat)
	{		
		yield return new WaitForEndOfFrame();
		var cubeCamera = new GameObject( "CubemapCamera", typeof(Camera) ) as GameObject;
		cubeCamera.hideFlags = HideFlags.HideInHierarchy;
		var cubeCam = cubeCamera.GetComponent("Camera") as Camera;
		var tempFull = new Cubemap (LightShapeProbe.LightShapeManager.CubeMapSize, TextureFormat.RGB24, LightShapeProbe.LightShapeManager.hasMips) as Cubemap;
		yield return new WaitForEndOfFrame();
		cubeCam.nearClipPlane = 0.001f;
		cubeCam.aspect = 1.0f;
		
		if(LightShapeProbe.LightShapeManager.worldCamera)
		{
			if(!RenderSettings.skybox)
			{
				if(!LightShapeProbe.overrideCameraColor)
				{
					cubeCam.backgroundColor = LightShapeProbe.LightShapeManager.worldCamera.backgroundColor;
				}
				
				if(LightShapeProbe.overrideCameraColor)
				{
					cubeCam.backgroundColor = LightShapeProbe.BGColor;
				}
			}

				if(RenderSettings.skybox)
				{
					if(!LightShapeProbe.overrideCameraColor)
					{
						cubeCam.clearFlags = CameraClearFlags.Skybox;
					}
					if(LightShapeProbe.overrideCameraColor)
					{
						cubeCam.clearFlags = CameraClearFlags.SolidColor;
						cubeCam.backgroundColor = LightShapeProbe.BGColor;
					}
				}
		}
		
		cubeCam.fov = 90;
		cubeCam.cullingMask = 1 << 0;		
	    cubeCamera.transform.position = transform.position;
				
		cubeCamera.transform.eulerAngles = new Vector3(0, 90, 0);
		Texture2D tex = new Texture2D (LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize, TextureFormat.RGB24, false);
		
		yield return new WaitForEndOfFrame();
		tex.ReadPixels (new Rect(1, 1, LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize), 0, 0);
		yield return new WaitForEndOfFrame();		
		tex.Apply ();
		CubeMapColors = tex.GetPixels();
		
		Color [,] bigPicture = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
 		
		row = -1;
		
		for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
		{
       		  if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
      		  bigPicture[row, i % LightShapeProbe.LightShapeManager.CubeMapSize] = CubeMapColors[i];
		}
		
		Color [,] flippedX = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
		
		
		for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
		{
        	for( int j = 0; j < LightShapeProbe.LightShapeManager.CubeMapSize; ++j)
        	{
				flippedX[LightShapeProbe.LightShapeManager.CubeMapSize - 1 - i, j] = bigPicture[i, j];
       		}
		}
		
		row = -1;
		
		for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
		{
        	if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
				CubeMapColors[i] = flippedX[row, i % LightShapeProbe.LightShapeManager.CubeMapSize];
		}		
		
		tempFull.SetPixels(CubeMapColors, CubemapFace.PositiveX);        	
		tempFull.Apply();

		cubeCamera.transform.eulerAngles = new Vector3(0, 270, 0);
		yield return new WaitForEndOfFrame();
		tex.ReadPixels (new Rect(1, 1, LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize), 0, 0);
		yield return new WaitForEndOfFrame();
		tex.Apply ();
		CubeMapColors = tex.GetPixels();
		
		bigPicture = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
 		
		row = -1;
		
		for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
		{
       		  if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
      		  bigPicture[row, i % LightShapeProbe.LightShapeManager.CubeMapSize] = CubeMapColors[i];
		}
		
		flippedX = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
		
		
		for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
		{
        	for( int j = 0; j < LightShapeProbe.LightShapeManager.CubeMapSize; ++j)
        	{
				flippedX[LightShapeProbe.LightShapeManager.CubeMapSize - 1 - i, j] = bigPicture[i, j];
       		}
		}
		
		row = -1;
		
		for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
		{
        	if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
			CubeMapColors[i] = flippedX[row, i % LightShapeProbe.LightShapeManager.CubeMapSize];
		}		
		
		tempFull.SetPixels(CubeMapColors, CubemapFace.NegativeX);        	
		tempFull.Apply();

		cubeCamera.transform.eulerAngles = new Vector3(0, 0, 0);
		yield return new WaitForEndOfFrame();
		tex.ReadPixels (new Rect(1, 1, LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize), 0, 0);
		yield return new WaitForEndOfFrame();
		tex.Apply ();
		CubeMapColors = tex.GetPixels();
		
		
		bigPicture = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
 		
		row = -1;
		
		for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
		{
       		  if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
      		  bigPicture[row, i % LightShapeProbe.LightShapeManager.CubeMapSize] = CubeMapColors[i];
		}
		
		flippedX = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
		
		
		for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
		{
        	for( int j = 0; j < LightShapeProbe.LightShapeManager.CubeMapSize; ++j)
        	{
				flippedX[LightShapeProbe.LightShapeManager.CubeMapSize - 1 - i, j] = bigPicture[i, j];
       		}
		}
		
		row = -1;
		
		for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
		{
        	if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
			CubeMapColors[i] = flippedX[row, i % LightShapeProbe.LightShapeManager.CubeMapSize];
		}		

		
		tempFull.SetPixels(CubeMapColors, CubemapFace.PositiveZ);        	
		tempFull.Apply();
	
		cubeCamera.transform.eulerAngles = new Vector3(0, 180, 0);
		yield return new WaitForEndOfFrame();
		tex.ReadPixels (new Rect(1, 1, LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize), 0, 0);
		yield return new WaitForEndOfFrame();
		tex.Apply ();
		CubeMapColors = tex.GetPixels();
		
		bigPicture = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
 		
		row = -1;
		
		for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
		{
       		  if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
      		  bigPicture[row, i % LightShapeProbe.LightShapeManager.CubeMapSize] = CubeMapColors[i];
		}
		
		flippedX = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
		
		
		for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
		{
        	for( int j = 0; j < LightShapeProbe.LightShapeManager.CubeMapSize; ++j)
        	{
				flippedX[LightShapeProbe.LightShapeManager.CubeMapSize - 1 - i, j] = bigPicture[i, j];
       		}
		}
		
		row = -1;
		
		for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
		{
        	if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
			CubeMapColors[i] = flippedX[row, i % LightShapeProbe.LightShapeManager.CubeMapSize];
		}		

		
		tempFull.SetPixels(CubeMapColors, CubemapFace.NegativeZ);        	
		tempFull.Apply();		
	
		cubeCamera.transform.eulerAngles = new Vector3(270, 0, 0);
		yield return new WaitForEndOfFrame();
		tex.ReadPixels (new Rect(1, 1, LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize), 0, 0);
		yield return new WaitForEndOfFrame();
		tex.Apply ();
		CubeMapColors = tex.GetPixels();
		
		bigPicture = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
 		
		row = -1;
		
		for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
		{
       		  if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
      		  bigPicture[row, i % LightShapeProbe.LightShapeManager.CubeMapSize] = CubeMapColors[i];
		}
		
		flippedX = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
		
		
		for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
		{
        	for( int j = 0; j < LightShapeProbe.LightShapeManager.CubeMapSize; ++j)
        	{
				flippedX[LightShapeProbe.LightShapeManager.CubeMapSize - 1 - i, j] = bigPicture[i, j];
       		}
		}
		
		row = -1;
		
		for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
		{
        	if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
			CubeMapColors[i] = flippedX[row, i % LightShapeProbe.LightShapeManager.CubeMapSize];
		}		
		

		
		tempFull.SetPixels(CubeMapColors, CubemapFace.PositiveY);        	
		tempFull.Apply();		
	
		cubeCamera.transform.eulerAngles = new Vector3(90, 0, 0);
		yield return new WaitForEndOfFrame();
		tex.ReadPixels (new Rect(1, 1, LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize), 0, 0);
		yield return new WaitForEndOfFrame();
		tex.Apply ();
		CubeMapColors = tex.GetPixels();
		
		bigPicture = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
 		
		row = -1;
		
		for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
		{
       		  if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
      		  bigPicture[row, i % LightShapeProbe.LightShapeManager.CubeMapSize] = CubeMapColors[i];
		}
		
		flippedX = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
		
		
		for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
		{
        	for( int j = 0; j < LightShapeProbe.LightShapeManager.CubeMapSize; ++j)
        	{
				flippedX[LightShapeProbe.LightShapeManager.CubeMapSize - 1 - i, j] = bigPicture[i, j];
       		}
		}
		
		row = -1;
		
		for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
		{
        	if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
			CubeMapColors[i] = flippedX[row, i % LightShapeProbe.LightShapeManager.CubeMapSize];
		}		
		
		tempFull.SetPixels(CubeMapColors, CubemapFace.NegativeY);        	
		tempFull.Apply();

		yield return new WaitForEndOfFrame();
			
			var cubeCamera2 = new GameObject( "CubemapCamera", typeof(Camera) ) as GameObject;
			cubeCamera2.hideFlags = HideFlags.HideInHierarchy;
			var cubeCam2 = cubeCamera2.GetComponent("Camera") as Camera;
			var tempFull2 = new Cubemap (LightShapeProbe.LightShapeManager.CubeMapSize, TextureFormat.RGB24, LightShapeProbe.LightShapeManager.hasMips) as Cubemap;	
			yield return new WaitForEndOfFrame();
			cubeCam2.clearFlags = CameraClearFlags.SolidColor;
			cubeCam2.nearClipPlane = 0.001f;
			cubeCam2.aspect = 1.0f;	
			cubeCam2.cullingMask = 2 << 0;
			cubeCam2.fov = 90;
			cubeCam2.backgroundColor = Color.black;	
			yield return new WaitForEndOfFrame();
		    cubeCamera2.transform.position = transform.position;
		
			cubeCamera2.transform.eulerAngles = new Vector3(0, 90, 0);
			
			yield return new WaitForEndOfFrame();
			tex.ReadPixels (new Rect(1, 1, LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize), 0, 0);
			yield return new WaitForEndOfFrame();
			tex.Apply ();
			CubeMapColors2 = tex.GetPixels();
		
			bigPicture = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
	 		
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	       		  if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
	      		  bigPicture[row, i % LightShapeProbe.LightShapeManager.CubeMapSize] = CubeMapColors2[i];
			}
			
			flippedX = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
			
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	for( int j = 0; j < LightShapeProbe.LightShapeManager.CubeMapSize; ++j)
	        	{
					flippedX[LightShapeProbe.LightShapeManager.CubeMapSize - 1 - i, j] = bigPicture[i, j];
	       		}
			}
			
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
				CubeMapColors2[i] = flippedX[row, i % LightShapeProbe.LightShapeManager.CubeMapSize];
			}		
		
		
			tempFull2.SetPixels(CubeMapColors2, CubemapFace.PositiveX);        	
			tempFull2.Apply();
	
			cubeCamera2.transform.eulerAngles = new Vector3(0, 270, 0);
			yield return new WaitForEndOfFrame();
			tex.ReadPixels (new Rect(1, 1, LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize), 0, 0);
			yield return new WaitForEndOfFrame();
			tex.Apply ();
			CubeMapColors2 = tex.GetPixels();
		
			bigPicture = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
	 		
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	       		  if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
	      		  bigPicture[row, i % LightShapeProbe.LightShapeManager.CubeMapSize] = CubeMapColors2[i];
			}
			
			flippedX = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
			
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	for( int j = 0; j < LightShapeProbe.LightShapeManager.CubeMapSize; ++j)
	        	{
					flippedX[LightShapeProbe.LightShapeManager.CubeMapSize - 1 - i, j] = bigPicture[i, j];
	       		}
			}
			
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
				CubeMapColors2[i] = flippedX[row, i % LightShapeProbe.LightShapeManager.CubeMapSize];
			}		
		
		
			tempFull2.SetPixels(CubeMapColors2, CubemapFace.NegativeX);        	
			tempFull2.Apply();
	
			cubeCamera2.transform.eulerAngles = new Vector3(0, 0, 0);
			yield return new WaitForEndOfFrame();
			tex.ReadPixels (new Rect(1, 1, LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize), 0, 0);
			yield return new WaitForEndOfFrame();
			tex.Apply ();
			CubeMapColors2 = tex.GetPixels();
		
			bigPicture = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
	 		
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	       		  if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
	      		  bigPicture[row, i % LightShapeProbe.LightShapeManager.CubeMapSize] = CubeMapColors2[i];
			}
			
			flippedX = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
			
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	for( int j = 0; j < LightShapeProbe.LightShapeManager.CubeMapSize; ++j)
	        	{
					flippedX[LightShapeProbe.LightShapeManager.CubeMapSize - 1 - i, j] = bigPicture[i, j];
	       		}
			}
			
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
				CubeMapColors2[i] = flippedX[row, i % LightShapeProbe.LightShapeManager.CubeMapSize];
			}		
		
			tempFull2.SetPixels(CubeMapColors2, CubemapFace.PositiveZ);        	
			tempFull2.Apply();
		
			cubeCamera2.transform.eulerAngles = new Vector3(0, 180, 0);
			yield return new WaitForEndOfFrame();
			tex.ReadPixels (new Rect(1, 1, LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize), 0, 0);
			yield return new WaitForEndOfFrame();
			tex.Apply ();
			CubeMapColors2 = tex.GetPixels();
		
			bigPicture = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
	 		
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	       		  if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
	      		  bigPicture[row, i % LightShapeProbe.LightShapeManager.CubeMapSize] = CubeMapColors2[i];
			}
			
			flippedX = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
			
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	for( int j = 0; j < LightShapeProbe.LightShapeManager.CubeMapSize; ++j)
	        	{
					flippedX[LightShapeProbe.LightShapeManager.CubeMapSize - 1 - i, j] = bigPicture[i, j];
	       		}
			}
			
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
				CubeMapColors2[i] = flippedX[row, i % LightShapeProbe.LightShapeManager.CubeMapSize];
			}		
		
			tempFull2.SetPixels(CubeMapColors2, CubemapFace.NegativeZ);        	
			tempFull2.Apply();
				
		
			cubeCamera2.transform.eulerAngles = new Vector3(270, 0, 0);
			yield return new WaitForEndOfFrame();
			tex.ReadPixels (new Rect(1, 1, LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize), 0, 0);
			yield return new WaitForEndOfFrame();
			tex.Apply ();
			CubeMapColors2 = tex.GetPixels();
		
			bigPicture = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
	 		
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	       		  if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
	      		  bigPicture[row, i % LightShapeProbe.LightShapeManager.CubeMapSize] = CubeMapColors2[i];
			}
			
			flippedX = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
			
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	for( int j = 0; j < LightShapeProbe.LightShapeManager.CubeMapSize; ++j)
	        	{
					flippedX[LightShapeProbe.LightShapeManager.CubeMapSize - 1 - i, j] = bigPicture[i, j];
	       		}
			}
			
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
				CubeMapColors2[i] = flippedX[row, i % LightShapeProbe.LightShapeManager.CubeMapSize];
			}		
		
			tempFull2.SetPixels(CubeMapColors2, CubemapFace.PositiveY);        	
			tempFull2.Apply();		
		
			cubeCamera2.transform.eulerAngles = new Vector3(90, 0, 0);
			yield return new WaitForEndOfFrame();
			tex.ReadPixels (new Rect(1, 1, LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize), 0, 0);
			yield return new WaitForEndOfFrame();
			tex.Apply ();
			CubeMapColors2 = tex.GetPixels();
		
			bigPicture = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
	 		
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	       		  if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
	      		  bigPicture[row, i % LightShapeProbe.LightShapeManager.CubeMapSize] = CubeMapColors2[i];
			}
			
			flippedX = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
			
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	for( int j = 0; j < LightShapeProbe.LightShapeManager.CubeMapSize; ++j)
	        	{
					flippedX[LightShapeProbe.LightShapeManager.CubeMapSize - 1 - i, j] = bigPicture[i, j];
	       		}
			}
			
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
				CubeMapColors2[i] = flippedX[row, i % LightShapeProbe.LightShapeManager.CubeMapSize];
			}		
		
			tempFull2.SetPixels(CubeMapColors2, CubemapFace.NegativeY);        	
			tempFull2.Apply();
		
			yield return new WaitForEndOfFrame();
		
			CubeMapColors = tempFull.GetPixels(CubemapFace.PositiveX);		
			CubeMapColors2 = tempFull2.GetPixels(CubemapFace.PositiveX);
			
			SetPixels(CubeMapColors, CubeMapColors2);
			
			tempFull.SetPixels(CubeMapColors, CubemapFace.PositiveX);
			tempFull.Apply();
		
			CubeMapColors = tempFull.GetPixels(CubemapFace.NegativeX);		
			CubeMapColors2 = tempFull2.GetPixels(CubemapFace.NegativeX);
			
			SetPixels(CubeMapColors, CubeMapColors2);
			
			tempFull.SetPixels(CubeMapColors, CubemapFace.NegativeX);
			tempFull.Apply();
		
			CubeMapColors = tempFull.GetPixels(CubemapFace.PositiveY);		
			CubeMapColors2 = tempFull2.GetPixels(CubemapFace.PositiveY);
			
			SetPixels(CubeMapColors, CubeMapColors2);
			
			tempFull.SetPixels(CubeMapColors, CubemapFace.PositiveY);
			tempFull.Apply();
		
			CubeMapColors = tempFull.GetPixels(CubemapFace.NegativeY);		
			CubeMapColors2 = tempFull2.GetPixels(CubemapFace.NegativeY);
			
			SetPixels(CubeMapColors, CubeMapColors2);
			
			tempFull.SetPixels(CubeMapColors, CubemapFace.NegativeY);
			tempFull.Apply();
			
			CubeMapColors = tempFull.GetPixels(CubemapFace.PositiveZ);		
			CubeMapColors2 = tempFull2.GetPixels(CubemapFace.PositiveZ);
			
			SetPixels(CubeMapColors, CubeMapColors2);
			
			tempFull.SetPixels(CubeMapColors, CubemapFace.PositiveZ);
			tempFull.Apply();
		
			CubeMapColors = tempFull.GetPixels(CubemapFace.NegativeZ);		
			CubeMapColors2 = tempFull2.GetPixels(CubemapFace.NegativeZ);
			
			SetPixels(CubeMapColors, CubeMapColors2);
			
			tempFull.SetPixels(CubeMapColors, CubemapFace.NegativeZ);
			tempFull.Apply();
		
			if(lightShapeObjTemp)
			{
				Destroy(lightShapeObjTemp.gameObject);
				Destroy(lsMat);
			
			}
		
			if(lightShapeObjTempAlt)
			{
				Destroy(lightShapeObjTempAlt.gameObject);
				Destroy(alsMat);
			}

			if(LightShapeProbe.hasGround)
			{	
				var cubeCamera3 = new GameObject( "CubemapCamera", typeof(Camera) ) as GameObject;
				var cubeCam3 = cubeCamera3.GetComponent("Camera") as Camera;
			
				cubeCam3.clearFlags = CameraClearFlags.SolidColor;
				cubeCam3.nearClipPlane = 0.001f;
				cubeCam3.farClipPlane = 5f;
				cubeCam3.aspect = 1.0f;
				cubeCam3.cullingMask = 2 << 0;
			
				cubeCam3.fov = 90;
				cubeCam3.backgroundColor = Color.white;	
				yield return new WaitForEndOfFrame();	
		   
				cubeCamera3.transform.position = transform.position;
		    	cubeCamera3.transform.rotation = transform.rotation;
				var tempFull3 = new Cubemap (LightShapeProbe.LightShapeManager.CubeMapSize, TextureFormat.RGB24, false) as Cubemap;

				cubeCamera3.transform.eulerAngles = new Vector3(0, 90, 0);
			//1
			yield return new WaitForEndOfFrame();
			tex.ReadPixels (new Rect(1, 1, LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize), 0, 0);
			yield return new WaitForEndOfFrame();
			tex.Apply ();
			CubeMapColors3 = tex.GetPixels();
		
			bigPicture = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
	 		
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	       		  if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
	      		  bigPicture[row, i % LightShapeProbe.LightShapeManager.CubeMapSize] = CubeMapColors3[i];
			}
			
			flippedX = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
			
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	for( int j = 0; j < LightShapeProbe.LightShapeManager.CubeMapSize; ++j)
	        	{
					flippedX[LightShapeProbe.LightShapeManager.CubeMapSize - 1 - i, j] = bigPicture[i, j];
	       		}
			}
			
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
				CubeMapColors3[i] = flippedX[row, i % LightShapeProbe.LightShapeManager.CubeMapSize];
			}		
		
		
			tempFull3.SetPixels(CubeMapColors3, CubemapFace.PositiveX);        	
			tempFull3.Apply();
			//2
			cubeCamera3.transform.eulerAngles = new Vector3(0, 270, 0);
			yield return new WaitForEndOfFrame();
			tex.ReadPixels (new Rect(1, 1, LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize), 0, 0);
			yield return new WaitForEndOfFrame();
			tex.Apply ();
			CubeMapColors3 = tex.GetPixels();
		
			bigPicture = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
	 		
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	       		  if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
	      		  bigPicture[row, i % LightShapeProbe.LightShapeManager.CubeMapSize] = CubeMapColors3[i];
			}
			
			flippedX = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
			
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	for( int j = 0; j < LightShapeProbe.LightShapeManager.CubeMapSize; ++j)
	        	{
					flippedX[LightShapeProbe.LightShapeManager.CubeMapSize - 1 - i, j] = bigPicture[i, j];
	       		}
			}
			
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
				CubeMapColors3[i] = flippedX[row, i % LightShapeProbe.LightShapeManager.CubeMapSize];
			}		
		
		
			tempFull3.SetPixels(CubeMapColors3, CubemapFace.NegativeX);        	
			tempFull3.Apply();
			//3
			cubeCamera3.transform.eulerAngles = new Vector3(0, 0, 0);
			yield return new WaitForEndOfFrame();
			tex.ReadPixels (new Rect(1, 1, LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize), 0, 0);
			yield return new WaitForEndOfFrame();
			tex.Apply ();
			CubeMapColors3 = tex.GetPixels();
		
			bigPicture = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
	 		
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	       		  if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
	      		  bigPicture[row, i % LightShapeProbe.LightShapeManager.CubeMapSize] = CubeMapColors3[i];
			}
			
			flippedX = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
			
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	for( int j = 0; j < LightShapeProbe.LightShapeManager.CubeMapSize; ++j)
	        	{
					flippedX[LightShapeProbe.LightShapeManager.CubeMapSize - 1 - i, j] = bigPicture[i, j];
	       		}
			}
			
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
				CubeMapColors3[i] = flippedX[row, i % LightShapeProbe.LightShapeManager.CubeMapSize];
			}		
		
			tempFull3.SetPixels(CubeMapColors3, CubemapFace.PositiveZ);        	
			tempFull3.Apply();
			//4
			cubeCamera3.transform.eulerAngles = new Vector3(0, 180, 0);
			yield return new WaitForEndOfFrame();
			tex.ReadPixels (new Rect(1, 1, LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize), 0, 0);
			yield return new WaitForEndOfFrame();
			tex.Apply ();
			CubeMapColors3 = tex.GetPixels();
		
			bigPicture = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
	 		
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	       		  if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
	      		  bigPicture[row, i % LightShapeProbe.LightShapeManager.CubeMapSize] = CubeMapColors3[i];
			}
			
			flippedX = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
			
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	for( int j = 0; j < LightShapeProbe.LightShapeManager.CubeMapSize; ++j)
	        	{
					flippedX[LightShapeProbe.LightShapeManager.CubeMapSize - 1 - i, j] = bigPicture[i, j];
	       		}
			}
			
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
				CubeMapColors3[i] = flippedX[row, i % LightShapeProbe.LightShapeManager.CubeMapSize];
			}		
		
			tempFull3.SetPixels(CubeMapColors3, CubemapFace.NegativeZ);        	
			tempFull3.Apply();
			//5	
		
			cubeCamera3.transform.eulerAngles = new Vector3(270, 0, 0);
			yield return new WaitForEndOfFrame();
			tex.ReadPixels (new Rect(1, 1, LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize), 0, 0);
			yield return new WaitForEndOfFrame();
			tex.Apply ();
			CubeMapColors3 = tex.GetPixels();
		
			bigPicture = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
	 		
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	       		  if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
	      		  bigPicture[row, i % LightShapeProbe.LightShapeManager.CubeMapSize] = CubeMapColors3[i];
			}
			
			flippedX = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
			
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	for( int j = 0; j < LightShapeProbe.LightShapeManager.CubeMapSize; ++j)
	        	{
					flippedX[LightShapeProbe.LightShapeManager.CubeMapSize - 1 - i, j] = bigPicture[i, j];
	       		}
			}
			
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
				CubeMapColors3[i] = flippedX[row, i % LightShapeProbe.LightShapeManager.CubeMapSize];
			}		
		
			tempFull3.SetPixels(CubeMapColors3, CubemapFace.PositiveY);        	
			tempFull3.Apply();		
			//6
			cubeCamera3.transform.eulerAngles = new Vector3(90, 0, 0);
			yield return new WaitForEndOfFrame();
			tex.ReadPixels (new Rect(1, 1, LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize), 0, 0);
			yield return new WaitForEndOfFrame();
			tex.Apply ();
			CubeMapColors3 = tex.GetPixels();
		
			bigPicture = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
	 		
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	       		  if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
	      		  bigPicture[row, i % LightShapeProbe.LightShapeManager.CubeMapSize] = CubeMapColors3[i];
			}
			
			flippedX = new Color[LightShapeProbe.LightShapeManager.CubeMapSize, LightShapeProbe.LightShapeManager.CubeMapSize];
			
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	for( int j = 0; j < LightShapeProbe.LightShapeManager.CubeMapSize; ++j)
	        	{
					flippedX[LightShapeProbe.LightShapeManager.CubeMapSize - 1 - i, j] = bigPicture[i, j];
	       		}
			}
			
			row = -1;
			
			for( int i = 0; i < LightShapeProbe.LightShapeManager.CubeMapSize * LightShapeProbe.LightShapeManager.CubeMapSize; ++i )
			{
	        	if ( i % LightShapeProbe.LightShapeManager.CubeMapSize == 0 ) ++row;
				CubeMapColors3[i] = flippedX[row, i % LightShapeProbe.LightShapeManager.CubeMapSize];
			}		
		
			tempFull3.SetPixels(CubeMapColors3, CubemapFace.NegativeY);        	
			tempFull3.Apply();
		
			yield return new WaitForEndOfFrame();

				//1
				CubeMapColors = tempFull.GetPixels(CubemapFace.PositiveY);
				CubeMapColors3 = tempFull3.GetPixels(CubemapFace.PositiveY);		
				
				SetGround(CubeMapColors, CubeMapColors3);
				
				tempFull.SetPixels(CubeMapColors, CubemapFace.PositiveY);
				tempFull.Apply();
				
				//2
				CubeMapColors = tempFull.GetPixels(CubemapFace.NegativeY);
				CubeMapColors3 = tempFull3.GetPixels(CubemapFace.NegativeY);		
				
				SetGround(CubeMapColors, CubeMapColors3);
				
				tempFull.SetPixels(CubeMapColors, CubemapFace.NegativeY);
				tempFull.Apply();
				
				//3
				CubeMapColors = tempFull.GetPixels(CubemapFace.PositiveX);	
				CubeMapColors3 = tempFull3.GetPixels(CubemapFace.PositiveX);
				
				SetGround(CubeMapColors, CubeMapColors3);
				
				tempFull.SetPixels(CubeMapColors, CubemapFace.PositiveX);
				tempFull.Apply();
				
				//4
				CubeMapColors = tempFull.GetPixels(CubemapFace.NegativeX);
				CubeMapColors3 = tempFull3.GetPixels(CubemapFace.NegativeX);		
				
				SetGround(CubeMapColors, CubeMapColors3);
				
				tempFull.SetPixels(CubeMapColors, CubemapFace.NegativeX);
				tempFull.Apply();		
				//5
				CubeMapColors = tempFull.GetPixels(CubemapFace.PositiveZ);
				CubeMapColors3 = tempFull3.GetPixels(CubemapFace.PositiveZ);
				
				SetGround(CubeMapColors, CubeMapColors3);
				
				tempFull.SetPixels(CubeMapColors, CubemapFace.PositiveZ);
				tempFull.Apply();
				//6
				CubeMapColors = tempFull.GetPixels(CubemapFace.NegativeZ);
				CubeMapColors3 = tempFull3.GetPixels(CubemapFace.NegativeZ);		
				
				SetGround(CubeMapColors, CubeMapColors3);
					
				tempFull.SetPixels(CubeMapColors, CubemapFace.NegativeZ);
				tempFull.Apply();				

				DestroyImmediate( cubeCamera3 );				
			}			
			
			if(groundShapeObjTemp)
			{
				Destroy(groundShapeObjTemp.gameObject);
				Destroy(gMat);
			}		

		AssetDatabase.CreateAsset(tempFull, LightShapeProbe.LightShapeManager.savePath + LightShapeProbe.LightShapeManager.folderName + "/Cube_" + LightShapeProbe.LightShapeManager.CubeMapSize + "_" + (LightShapeProbe.myIndex + 1) + ".cubemap");

		yield return new WaitForEndOfFrame();
		
		if(LightShapeProbe.LightShapeManager.AltMethodManger.curProbeInt == LightShapeProbe.LightShapeManager.cubeProbes.Length)
		{
			if(!LightShapeProbe.LightShapeManager.AltMethodManger.secondPass)
			{
				LightShapeProbe.LightShapeManager.AltMethodManger.StopBatch();
			}			
		}
		
		if(LightShapeProbe.LightShapeManager.AltMethodManger.secondPass)
		{
			if(LightShapeProbe.LightShapeManager.AltMethodManger.curProbeInt == LightShapeProbe.LightShapeManager.AltMethodManger.reflectObjs.Length)
			{			
				LightShapeProbe.LightShapeManager.AltMethodManger.StopBatch();
			}
			
			if(LightShapeProbe.LightShapeManager.AltMethodManger.curProbeInt < LightShapeProbe.LightShapeManager.AltMethodManger.reflectObjs.Length)
			{			
				StartCoroutine(LightShapeProbe.LightShapeManager.AltMethodManger.RenderCubeMaps());
			}
		}
		
		if(!LightShapeProbe.LightShapeManager.AltMethodManger.secondPass)
		{		
			if(LightShapeProbe.LightShapeManager.AltMethodManger.curProbeInt < LightShapeProbe.LightShapeManager.cubeProbes.Length)
			{
				StartCoroutine(LightShapeProbe.LightShapeManager.AltMethodManger.RenderCubeMaps());
			}
		}
		
		UpdateCubeMaps();

		rewardInt = Random.Range(0, LightShapeProbe.LightShapeManager.rewardStrings.Length);
		
		if(!LightShapeProbe.LightShapeManager.renderingCubeMaps)
		{
			Debug.Log(LightShapeProbe.LightShapeManager.rewardStrings[rewardInt] + " CubeMap Rendered!");
			EditorApplication.isPlaying = false;	
		}
	}

	public void UpdateCubeMaps()
	{
		LightShapeProbe.myCubeMap = AssetDatabase.LoadAssetAtPath(LightShapeProbe.LightShapeManager.savePath + LightShapeProbe.LightShapeManager.folderName + "/Cube_" + LightShapeProbe.LightShapeManager.CubeMapSize + "_" + (LightShapeProbe.myIndex + 1) + ".cubemap", typeof(Cubemap)) as Cubemap;
		
		GameObject[] wObjects = LightShapeProbe.LightShapeManager.cubeProbes;
		
		foreach(GameObject o in wObjects)
		{	 
			var oo = o.GetComponent("LightShapeProbe") as LightShapeProbe;	
			
			foreach(GameObject ooo in oo.myObjects)
			{
				var tempMat = ooo.GetComponent<MeshRenderer>();
 				tempMat.sharedMaterial.SetTexture("_Cube", LightShapeProbe.myCubeMap);
			}
		}
	}
	
	void SetPixels(Color[] CubeMapColors, Color[] CubeMapColors2)
	{
		for(var i = 0; i < CubeMapColors.Length; i++)
		{
			CubeMapColors[i]  *= LightShapeProbe.secondaryBrightness;
			CubeMapColors2[i]  *= LightShapeProbe.primaryBrightness;
			
			CubeMapColors[i] += CubeMapColors2[i];
			//Next Update will have Per channel Control						
			CubeMapColors[i].r = ((CubeMapColors[i].r - LightShapeProbe.mid) * LightShapeProbe.contrastR + LightShapeProbe.mid);
			CubeMapColors[i].g = ((CubeMapColors[i].g - LightShapeProbe.mid) * LightShapeProbe.contrastG + LightShapeProbe.mid);
			CubeMapColors[i].b = ((CubeMapColors[i].b - LightShapeProbe.mid) * LightShapeProbe.contrastB + LightShapeProbe.mid);
			
			if(CubeMapColors[i].r < 0f)
			{
				CubeMapColors[i].r = 0f;
			}
			
			if(CubeMapColors[i].g < 0f)
			{
				CubeMapColors[i].g = 0f;
			}
			
			if(CubeMapColors[i].b < 0f)
			{
				CubeMapColors[i].b = 0f;
			}
			
			var desaturated = CubeMapColors[i];
			desaturated.r = (CubeMapColors[i].r * 0.299f + CubeMapColors[i].b * 0.587f + CubeMapColors[i].b * 0.114f);
			desaturated.g = (CubeMapColors[i].r * 0.299f + CubeMapColors[i].b * 0.587f + CubeMapColors[i].b * 0.114f);
			desaturated.b = (CubeMapColors[i].r * 0.299f + CubeMapColors[i].b * 0.587f + CubeMapColors[i].b * 0.114f);
			CubeMapColors[i] = LightShapeProbe.saturation * ( CubeMapColors[i] ) + (1.0f - LightShapeProbe.saturation) * (desaturated);
			
			CubeMapColors[i].r += LightShapeProbe.brightness;
			CubeMapColors[i].g += LightShapeProbe.brightness;
			CubeMapColors[i].b += LightShapeProbe.brightness;
			CubeMapColors[i]  += LightShapeProbe.TintColor;
			CubeMapColors[i]  += LightShapeProbe.LightShapeManager.globalTintColor;
		}
		
	}
	
	void SetGround(Color[] CubeMapColors, Color[] CubeMapColors3)
	{
		for(var i = 0; i < CubeMapColors.Length; i++)
		{
			CubeMapColors[i] *= CubeMapColors3[i];
		}
	}
	
	public void TurnOffCubes()
	{
		if(!LightShapeProbe.renderRelections)
		{
			GameObject[] wObjects = LightShapeProbe.LightShapeManager.cubeProbes;
			
			foreach(GameObject o in wObjects)
			{	 
				var oo = o.GetComponent("LightShapeProbe") as LightShapeProbe;	
				
				foreach(GameObject ooo in oo.myObjects)
				{
					var tempMat = ooo.GetComponent<MeshRenderer>();
					
					if(LightShapeProbe.myCubeMap)
					{
	 					tempMat.sharedMaterial.SetTexture("_Cube", LightShapeProbe.myCubeMap);
					}
					
					if(!LightShapeProbe.myCubeMap)
					{
						UseColor();
					}
				}
			}
		}
		
		if(LightShapeProbe.renderRelections)
		{
			UseColor();
		}
		
	    foreach (GameObject e in LightShapeProbe.excludeMe) 
	    {
			var eObjecs = e.GetComponent<MeshRenderer>();
			eObjecs.enabled = false;
		}
	}
	
	void UseColor()
	{
		var othersCube = new Cubemap (LightShapeProbe.LightShapeManager.CubeMapSize, TextureFormat.RGB24, false) as Cubemap;
		
		CubeMapColorsR = othersCube.GetPixels(CubemapFace.PositiveX);			

		for(var rXp = 0; rXp < CubeMapColorsR.Length; rXp++)
		{
			CubeMapColorsR[rXp] = LightShapeProbe.reflectedColor;
		}
		
		othersCube.SetPixels(CubeMapColorsR, CubemapFace.PositiveX);
		othersCube.Apply();
					
		CubeMapColorsR = othersCube.GetPixels(CubemapFace.NegativeX);
		for(var rXn = 0; rXn < CubeMapColorsR.Length; rXn++)
		{
			CubeMapColorsR[rXn] = LightShapeProbe.reflectedColor;
		}
		
		othersCube.SetPixels(CubeMapColorsR, CubemapFace.NegativeX);
		othersCube.Apply();			
					
		CubeMapColorsR = othersCube.GetPixels(CubemapFace.PositiveY);
		for(var rYp = 0; rYp < CubeMapColorsR.Length; rYp++)
		{
			CubeMapColorsR[rYp] = LightShapeProbe.reflectedColor;
		}
		
		othersCube.SetPixels(CubeMapColorsR, CubemapFace.PositiveY);
		othersCube.Apply();			
					
		CubeMapColorsR = othersCube.GetPixels(CubemapFace.NegativeY);
		for(var rYn = 0; rYn < CubeMapColorsR.Length; rYn++)
		{
			CubeMapColorsR[rYn] = LightShapeProbe.reflectedColor;
		}
		
		othersCube.SetPixels(CubeMapColorsR, CubemapFace.NegativeY);
		othersCube.Apply();			
					
		CubeMapColorsR = othersCube.GetPixels(CubemapFace.PositiveZ);
		for(var rZp = 0; rZp < CubeMapColorsR.Length; rZp++)
		{
			CubeMapColorsR[rZp] = LightShapeProbe.reflectedColor;
		}
		
		othersCube.SetPixels(CubeMapColorsR, CubemapFace.PositiveZ);
		othersCube.Apply();			
					
		CubeMapColorsR = othersCube.GetPixels(CubemapFace.NegativeZ);
		for(var rZn = 0; rZn < CubeMapColorsR.Length; rZn++)
		{
			CubeMapColorsR[rZn] = LightShapeProbe.reflectedColor;
		}
		
		othersCube.SetPixels(CubeMapColorsR, CubemapFace.NegativeZ);
		othersCube.Apply();			
		
		GameObject[] wObjects = LightShapeProbe.LightShapeManager.cubeProbes;
		
		foreach(GameObject o in wObjects)
		{	 
			var oo = o.GetComponent("LightShapeProbe") as LightShapeProbe;	
			
			foreach(GameObject ooo in oo.myObjects)
			{
				var tempMat = ooo.GetComponent<MeshRenderer>();

 				tempMat.sharedMaterial.SetTexture("_Cube", othersCube);
			}
		}
	}
}
#endif