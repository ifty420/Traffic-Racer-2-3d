#if UNITY_EDITOR
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[ExecuteInEditMode]
public class LightShapeProbe : MonoBehaviour {
	
	public int myIndex;
	public LightShapeManager LightShapeManager;
	public LightShapeAssignGame LightShapeAssignGame;
	public Color gizColor = new Color(0.0f, 0.5f, 1.0f, 0.5f);
	public float gizScale = 1;

	public Camera[] myCameras; 
	
	public float primaryBrightness = 1f;
	public float secondaryBrightness = 1f;

	public float contrastR = 1f;
	public float contrastG = 1f;
	public float contrastB = 1f;
	public float mid = 0.5f;

	public float saturation = 1f;
	public float brightness = 0f;
	
	public Color BGColor = Color.black;
	public Color TintColor = Color.black;
	public bool renderLight = false;
	public GameObject lightShape;
	public float lightScaleX = 5f;
	public float lightScaleY = 5f;
	public float lightRot = 0f;
	public Light myLight;
	
	public bool renderAltLight = false;
	public GameObject altLightShape;
	public float altLightScaleX = 40f;
	public float altLightScaleY = 40f;
	public float altLightRot = 0f;
	public Light myBackLight;
	
	public Color lightShapeColor = new Color(0.5f, 0.5f, 0.5f);
	public Color altShapeColor = new Color(0.0f, 0.1f, 0.2f);
	public bool overRideLightColor = false;
	public bool overRideAltLightColor = false;
	public Cubemap myCubeMap;
	
	public GameObject groundShape;
		
	public SphereCollider myCollider;
	public Transform centerPos;
	public bool keepConnections;
	public GameObject[] myObjects;
	
	Camera cam;
	public GameObject previewBall;
	MeshFilter mF;
	MeshRenderer mR;
	RenderTexture rtex;
	public bool rePosition = false;
	public bool preview = false;
	GameObject lightShapeObjTemp;
	Material lsMat;
	GameObject lightShapeObjTempAlt;
	Material alsMat;
	MeshRenderer lightShapeMeshTemp;
	MeshRenderer lightShapeMeshTempAlt;	
	MeshRenderer groundShapeObjTempMesh;
	
	public bool hasGround = false;
	GameObject groundShapeObjTemp;
	Material gMat;

	public Color groundColor = new Color(0.0f, 0.0f, 0.0f);
	public float groundScale = 1f;
	Color[] CubeMapColors;
	Color[] CubeMapColors2;
	Color[] CubeMapColors3;
	Color[] CubeMapColorsR;
	
	public bool applyPerObjectInGame = true;
	public bool renderRelections = true;

	public Color reflectedColor = new Color(0.1f, 0.1f, 0.1f);
	public bool gatherAll = false;
	public GameObject[] excludeMe;
	public AltMethod AltMethod;
	bool stateChange;
	string directoryPath;
	private string outputMessage = ""; 

	private int rewardInt;
	public bool cubeMap;
	public bool useForDynamic = true;
	
	public int presetType = 0;
	public string[] presetTypes = new string[2] {"One Light", "Character"};
	public int[] presetInt = new int[2] {0, 1};	
	public bool overrideCameraColor;
	
	
	public void Default()
	{
		primaryBrightness = 1;		
		secondaryBrightness = 1;

		contrastR = 1f;
		contrastG = 1f;
		contrastB = 1f;
		mid = 0.5f;

		saturation = 1f;
		brightness = 0f;
		
		TintColor = Color.black;	
		BGColor = Color.black;
		
		renderLight = false;
		renderAltLight = false;
	        
		lightShape = AssetDatabase.LoadAssetAtPath("Assets/LightShape/Light_Shapes/lightShape_default.prefab", typeof(GameObject)) as GameObject;
		altLightShape = AssetDatabase.LoadAssetAtPath("Assets/LightShape/Light_Shapes/lightShape_Ground.prefab", typeof(GameObject)) as GameObject;
	    
		lightScaleX = 5f;
	    lightScaleY = 5f;
		lightRot = 0f;
		altLightScaleX = 20f;
	    altLightScaleY = 20f;
	    altLightRot = 0f;    
	    overRideLightColor = false;
		overRideAltLightColor = false;

		lightShapeColor = new Color(0.5f, 0.5f, 0.5f);
		hasGround = false;
		altShapeColor = new Color(0.0f, 0.1f, 0.2f);
		groundColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);;
		groundScale = 0.5f;
		centerPos.position = transform.position;
		myCollider.radius = 3;
		applyPerObjectInGame = true;
		useForDynamic = true;
		renderRelections = true;

		reflectedColor = new Color(0.1f, 0.1f, 0.1f);
		gatherAll = false;
		overrideCameraColor = false;
		
		Debug.Log("Default Settings Restored.");
	}

	public void PresetCharacter()
	{
		primaryBrightness = 1;		
		secondaryBrightness = 1;

		contrastR = 1f;
		contrastG = 1f;
		contrastB = 1f;
		mid = 0.5f;

		saturation = 1f;
		brightness = 0f;
		
		TintColor = new Color(0.1f, 0f, 0f);	
		BGColor = Color.black;
		
		renderLight = true;
		renderAltLight = true;
	        
		lightShape = AssetDatabase.LoadAssetAtPath("Assets/LightShape/Light_Shapes/lightShape_default.prefab", typeof(GameObject)) as GameObject;
		altLightShape = AssetDatabase.LoadAssetAtPath("Assets/LightShape/Light_Shapes/lightShape_Ground.prefab", typeof(GameObject)) as GameObject;
	    
		lightScaleX = 8f;
	    lightScaleY = 8f;
		lightRot = 0f;
		altLightScaleX = 40f;
	    altLightScaleY = 40f;
	    altLightRot = 0f;    
	    overRideLightColor = true;
		overRideAltLightColor = true;

		lightShapeColor = new Color(1f, 1f, 1f);
		hasGround = true;
		altShapeColor = new Color(0.02f, 0.3f, 0.55f);
		groundColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);
		groundScale = 0.5f;
		centerPos.position = transform.position;
		myCollider.radius = 3;
		applyPerObjectInGame = true;
		renderRelections = true;

		reflectedColor = new Color(0.1f, 0.1f, 0.1f);
		gatherAll = false;
		
		Debug.Log("Default Settings Restored.");
	}
	
	public void PresetContrast()
	{
		primaryBrightness = 1;		
		secondaryBrightness = 1;

		contrastR = 4.5f;
		contrastG = 4.5f;
		contrastB = 4.5f;
		mid = 0.5f;

		saturation = .25f;
		brightness = 0f;
		
		TintColor = Color.black;	
		BGColor = Color.black;
		
		renderLight = false;
		renderAltLight = false;
	        
		lightShape = AssetDatabase.LoadAssetAtPath("Assets/LightShape/Light_Shapes/lightShape_default.prefab", typeof(GameObject)) as GameObject;
		altLightShape = AssetDatabase.LoadAssetAtPath("Assets/LightShape/Light_Shapes/lightShape_Ground.prefab", typeof(GameObject)) as GameObject;
	    
		lightScaleX = 5f;
	    lightScaleY = 5f;
		lightRot = 0f;
		altLightScaleX = 20f;
	    altLightScaleY = 20f;
	    altLightRot = 0f;    
	    overRideLightColor = false;
		overRideAltLightColor = false;

		lightShapeColor = new Color(0.5f, 0.5f, 0.5f);
		hasGround = false;
		altShapeColor = new Color(0.0f, 0.1f, 0.2f);
		groundColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);
		groundScale = 1f;
		centerPos.position = transform.position;
		myCollider.radius = 3;
		applyPerObjectInGame = true;
		renderRelections = true;

		reflectedColor = new Color(0.1f, 0.1f, 0.1f);
		gatherAll = false;
		
		Debug.Log("Default Settings Restored.");
	}
	
	void Awake ()
	{
		LightShapeManager = GameObject.FindWithTag("CubeProbeManager").GetComponent("LightShapeManager") as LightShapeManager; 
		if(!LightShapeAssignGame)
		{
			LightShapeAssignGame.GetComponent<LightShapeAssignGame>();
		}
		LightShapeAssignGame.hideFlags = HideFlags.HideInInspector;
		gameObject.GetComponent<LightShapeGizmo>().hideFlags = HideFlags.HideInInspector;
		gameObject.GetComponent<AltMethod>().hideFlags = HideFlags.HideInInspector;
		myCollider.hideFlags = HideFlags.HideInInspector;
		centerPos.hideFlags = HideFlags.HideInHierarchy;

		EditorApplication.playmodeStateChanged = OnEditorStateChanged;
	}
    
	void OnEditorStateChanged()
    {
		stateChange = !EditorApplication.isPlaying;
		if(stateChange && LightShapeManager.useAltMethod)
			UpdateCubeMaps();
			LightShapeManager.UpdateAllCubemaps();
    }
	
	void Start ()
	{			
		PrefabUtility.DisconnectPrefabInstance(gameObject);				
	}
	
	public void StartRender()
	{
		directoryPath = LightShapeManager.savePath + LightShapeManager.folderName;
		
		if(LightShapeManager.savePath == "" || LightShapeManager.folderName == "")
		{
			outputMessage = "Output Folder Not Found! Please Create An Output Folder."; 
			Debug.LogError (outputMessage);			
		}
		
		if(LightShapeManager.savePath != "" && LightShapeManager.folderName != "")
		{
			try
			{
				string[] filePaths = Directory.GetFiles(this.directoryPath);
				
				if(filePaths != null)				
					RenderCubeMaps();
			}
			
			catch (DirectoryNotFoundException directoryPathEx)
			{
				outputMessage = "Output Folder Not Found! Please Create An Output Folder." + directoryPathEx.Message; 
				Debug.LogError (outputMessage);
			}
		}
	}
	
	public void RenderCubeMaps()
	{
		if(!LightShapeManager)
		{
			LightShapeManager = GameObject.FindWithTag("CubeProbeManager").GetComponent("LightShapeManager") as LightShapeManager;			
		}

		rewardInt = Random.Range(0, LightShapeManager.rewardStrings.Length);

		if(!LightShapeManager.useAltMethod)
		{
			if(hasGround && renderLight)
			{
				if(!groundShape)
				{
					groundShape = AssetDatabase.LoadAssetAtPath("Assets/LightShape/Light_Shapes/lightShape_Ground.prefab", typeof(GameObject)) as GameObject;
				}
				
				if(groundShape)
				{
					groundShapeObjTemp = Instantiate(groundShape, transform.position, transform.rotation) as GameObject;
				}
								
				groundShapeObjTemp.transform.Translate(0, -0.1f, 0);
				groundShapeObjTemp.transform.eulerAngles = new Vector3(-90, 0, 0);
				groundShapeObjTempMesh = groundShapeObjTemp.GetComponent<MeshRenderer>();					
										
				var gTex = groundShapeObjTempMesh.sharedMaterial.GetTexture("_MainTex") as Texture2D;
				gMat = new Material (Shader.Find("Particles/Alpha Blended"));
				gMat.SetColor("_TintColor", groundColor);
				gMat.SetTexture("_MainTex", gTex);
				groundShapeObjTempMesh.sharedMaterial = gMat;
				
				groundShapeObjTemp.transform.localScale = new Vector3(groundScale, groundScale, groundScale);
			}
			
			if(renderLight)
			{			
				if(!myLight)
				{
					Debug.LogError("No Light Assigned. Please Assign a Light to `Use Light` to use the `Render Light` option.");					
				}
				
				if(myLight)
				{
					if(!lightShape)
					{
						lightShape = AssetDatabase.LoadAssetAtPath("Assets/LightShape/Light_Shapes/lightShape_default.prefab", typeof(GameObject)) as GameObject;
					}
									
					if(lightShape)
					{
				   	 	lightShapeObjTemp = Instantiate(lightShape, transform.position, transform.rotation) as GameObject;
					}
						
					lightShapeMeshTemp = lightShapeObjTemp.GetComponent<MeshRenderer>();					
										
					var lsTex = lightShapeMeshTemp.sharedMaterial.GetTexture("_MainTex");
					lsMat = new Material (Shader.Find("Particles/Alpha Blended"));

					lsMat.SetTexture("_MainTex", lsTex);
					var altZScale = 0;
					
					if(renderAltLight)
					{
						if(!altLightShape)
						{
							altLightShape = AssetDatabase.LoadAssetAtPath("Assets/LightShape/Light_Shapes/lightShape_Ground.prefab", typeof(GameObject)) as GameObject;
						}						
					
						if(altLightShape)
						{
							lightShapeObjTempAlt = Instantiate(altLightShape, transform.position, transform.rotation) as GameObject;
						}
						
						lightShapeMeshTempAlt = lightShapeObjTempAlt.GetComponent<MeshRenderer>();
						
						var alsTex = lightShapeMeshTempAlt.sharedMaterial.GetTexture("_MainTex");
						alsMat = new Material (Shader.Find("Particles/Alpha Blended"));
						
						alsMat.SetTexture("_MainTex", alsTex);
						
						if(overRideAltLightColor)
						{
							alsMat.SetColor("_TintColor", altShapeColor);	
							lightShapeMeshTempAlt.sharedMaterial = alsMat;
						}
				
						if(!overRideAltLightColor && myBackLight)
						{
							alsMat.SetColor("_TintColor", myBackLight.color);
							lightShapeMeshTempAlt.sharedMaterial = alsMat;
						}
					
						if(!overRideAltLightColor && !myBackLight)
						{
							alsMat.SetColor("_TintColor", myLight.color * 0.5f);
							lightShapeMeshTempAlt.sharedMaterial = alsMat;
						}						
					}
					
					if(overRideLightColor)
					{
						lsMat.SetColor("_TintColor", lightShapeColor);	
						lightShapeMeshTemp.sharedMaterial = lsMat;
					}
			
					if(!overRideLightColor)
					{
						lsMat.SetColor("_TintColor", myLight.color);
						lightShapeMeshTemp.sharedMaterial = lsMat;
					}
									
					if(myLight.type == LightType.Directional)
					{
						lightShapeObjTemp.transform.rotation = myLight.gameObject.transform.rotation;
						lightShapeObjTemp.transform.position = transform.position;
						lightShapeObjTemp.transform.Translate(0, 0, -5);
						
						if(renderAltLight && !myBackLight)
						{
							lightShapeObjTempAlt.transform.rotation = lightShapeObjTemp.transform.rotation;
							lightShapeObjTempAlt.transform.Translate(0, 0, 10);	
							altZScale = -1;
						}
					}
			
					if(myLight.type == LightType.Spot || myLight.type == LightType.Point)
					{
						var lightDir = Quaternion.LookRotation(transform.position - myLight.gameObject.transform.position);
						lightShapeObjTemp.transform.rotation = lightDir;						
						lightShapeObjTemp.transform.Translate(0, 0, -5);
						
						if(renderAltLight && !myBackLight)
						{
							lightShapeObjTempAlt.transform.rotation = lightShapeObjTemp.transform.rotation;
							lightShapeObjTempAlt.transform.Translate(0, 0, 10);
							altZScale = -1;
						}					
					}
					
					if(renderAltLight  && myBackLight)
					{
						if(myBackLight.type == LightType.Directional)
						{
							lightShapeObjTempAlt.transform.rotation = myBackLight.gameObject.transform.rotation;
							lightShapeObjTempAlt.transform.Translate(0, 0, -10);
							altZScale = 1;
						}
				
						if(myBackLight.type == LightType.Spot || myBackLight.type == LightType.Point)
						{
							var bLightDir = Quaternion.LookRotation(transform.position - myBackLight.gameObject.transform.position);
	
							lightShapeObjTempAlt.transform.rotation = bLightDir;
							lightShapeObjTempAlt.transform.Translate(0, 0, -10);
							altZScale = 1;							
						}						
					}
					
					lightShapeObjTemp.transform.eulerAngles = new Vector3(lightShapeObjTemp.transform.eulerAngles.x, lightShapeObjTemp.transform.eulerAngles.y, lightRot);					
					lightShapeObjTemp.transform.localScale = new Vector3(lightScaleX, lightScaleY, (lightScaleX + lightScaleY) / 2);
					
					if(renderAltLight)
					{
						lightShapeObjTempAlt.transform.localScale = new Vector3(altLightScaleX, altLightScaleY, (altLightScaleX + altLightScaleY) * altZScale);	
						lightShapeObjTempAlt.transform.eulerAngles = new Vector3(lightShapeObjTempAlt.transform.eulerAngles.x, lightShapeObjTempAlt.transform.eulerAngles.y, altLightRot);
					}
										
					Render(lightShapeObjTemp, lightShapeObjTempAlt, groundShapeObjTemp, gMat, lsMat, alsMat);
				}						
			}
				
			if(!renderLight)
			{
				Render(lightShapeObjTemp, lightShapeObjTempAlt, groundShapeObjTemp, gMat, lsMat, alsMat);
			}		
		}
		
		if(LightShapeManager.useAltMethod)
		{
			Render(lightShapeObjTemp, lightShapeObjTempAlt, groundShapeObjTemp, gMat, lsMat, alsMat);
		}		
	}
	
	void Render(GameObject lightShapeObjTemp, GameObject lightShapeObjTempAlt, GameObject groundShapeObjTemp, Material gMat, Material lsMat, Material alsMat )
	{
		if(!LightShapeManager.useAltMethod)
		{
			TurnOffCubes();
			var cubeCamera = new GameObject( "CubemapCamera", typeof(Camera) ) as GameObject;
			cubeCamera.tag = "LightShapeObject";
			var cubeCam = cubeCamera.GetComponent("Camera") as Camera;
			cubeCam.nearClipPlane = 0.001f;
			cubeCam.farClipPlane = 1000f;
			cubeCam.cullingMask = 1 << 0;
			
			if(!LightShapeManager.worldCamera)
			{
				cubeCam.backgroundColor = BGColor;
			}
			
			if(LightShapeManager.worldCamera)
			{
				if(!RenderSettings.skybox)
				{
					if(!overrideCameraColor)
					{
						cubeCam.backgroundColor = LightShapeManager.worldCamera.backgroundColor;
					}
					
					if(overrideCameraColor)
					{
						cubeCam.backgroundColor = BGColor;
					}
				}

				if(RenderSettings.skybox)
				{
					if(!overrideCameraColor)
					{
						cubeCam.clearFlags = CameraClearFlags.Skybox;
					}
					if(overrideCameraColor)
					{
						cubeCam.clearFlags = CameraClearFlags.SolidColor;
						cubeCam.backgroundColor = BGColor;
					}
				}
			}
			
		    cubeCamera.transform.position = transform.position;
		    cubeCamera.transform.rotation = Quaternion.identity;
		
			var cubeCamera2 = new GameObject( "CubemapCamera", typeof(Camera) ) as GameObject;
			cubeCamera2.tag = "LightShapeObject";
			var cubeCam2 = cubeCamera2.GetComponent("Camera") as Camera;
			cubeCam2.clearFlags = CameraClearFlags.SolidColor;
			cubeCam2.cullingMask = 2 << 0;
			
			cubeCam2.backgroundColor = Color.black;				
		    
		    cubeCamera2.transform.position = cubeCamera.transform.position;
		    cubeCamera2.transform.rotation = cubeCamera.transform.rotation;
						
			var tempFull = new Cubemap (LightShapeManager.CubeMapSize, TextureFormat.RGB24, LightShapeManager.hasMips) as Cubemap;
			cubeCamera.GetComponent<Camera>().RenderToCubemap( tempFull );
			cubeCam2.nearClipPlane = 0.001f;
			var tempFull2 = new Cubemap (LightShapeManager.CubeMapSize, TextureFormat.RGB24, LightShapeManager.hasMips) as Cubemap;
			cubeCamera2.GetComponent<Camera>().RenderToCubemap( tempFull2 );
																	    						
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
				DestroyImmediate(lightShapeObjTemp.gameObject);
				DestroyImmediate(lsMat);
			}
		
			if(lightShapeObjTempAlt)
			{
				DestroyImmediate(lightShapeObjTempAlt.gameObject);
				DestroyImmediate(alsMat);
			}	
			
			DestroyImmediate( cubeCamera );
			DestroyImmediate( tempFull2 );
			DestroyImmediate( cubeCamera2 );
						
			if(hasGround)
			{	
				var cubeCamera3 = new GameObject( "CubemapCamera", typeof(Camera) ) as GameObject;
				cubeCamera3.tag = "LightShapeObject";
				var cubeCam3 = cubeCamera3.GetComponent("Camera") as Camera;
				cubeCam3.clearFlags = CameraClearFlags.SolidColor;
				cubeCam3.aspect = 1.0f;
				cubeCam3.cullingMask = 2 << 0;
				cubeCam3.nearClipPlane = 0.001f;
				cubeCam3.farClipPlane = 5f;
				cubeCam3.backgroundColor = Color.white;	
		   
				cubeCamera3.transform.position = transform.position;
		    	cubeCamera3.transform.rotation = transform.rotation;
				var tempFull3 = new Cubemap (LightShapeManager.CubeMapSize, TextureFormat.RGB24, false) as Cubemap;
				cubeCamera3.GetComponent<Camera>().RenderToCubemap( tempFull3 );

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
				
				//
				DestroyImmediate( tempFull3 );
				DestroyImmediate( cubeCamera3 );				
			}			
			
			if(groundShapeObjTemp)
			{
				DestroyImmediate(groundShapeObjTemp.gameObject);
				DestroyImmediate(gMat);
			}

			AssetDatabase.CreateAsset(tempFull, LightShapeManager.savePath + LightShapeManager.folderName + "/Cube_" + LightShapeManager.CubeMapSize + "_" + (myIndex + 1) + ".cubemap");				

			myCubeMap = AssetDatabase.LoadAssetAtPath(LightShapeManager.savePath + LightShapeManager.folderName + "/Cube_" + LightShapeManager.CubeMapSize + "_" + (myIndex + 1) + ".cubemap", typeof(Cubemap)) as Cubemap;
			
			List<Color> oldColors = new List<Color>(CubeMapColors);
			oldColors.Clear();
			CubeMapColors = oldColors.ToArray();					
			CubeMapColors2 = oldColors.ToArray();
			CubeMapColors3 = oldColors.ToArray();
			CubeMapColorsR = oldColors.ToArray();

			UpdateCubeMaps();
		}
		
		if(LightShapeManager.useAltMethod)
		{
			TurnOffCubes();
			
			if(!LightShapeManager.renderingCubeMaps)
			{
				LightShapeManager.curProbeInt = myIndex;
			}
			
			AltMethod.StartRender();
		}		
								
		EditorUtility.UnloadUnusedAssets();
		
		if(!LightShapeManager.renderingCubeMaps)
		{
			Debug.Log(LightShapeManager.rewardStrings[rewardInt] + " CubeMap Rendered!");
		}				
	}
	
	void SetPixels(Color[] CubeMapColors, Color[] CubeMapColors2)
	{
		for(var i = 0; i < CubeMapColors.Length; i++)
		{
			CubeMapColors[i]  *= secondaryBrightness;
			CubeMapColors2[i]  *= primaryBrightness;
			
			CubeMapColors[i] += CubeMapColors2[i];
			//Next Update will have Per channel Control																		
			CubeMapColors[i].r = ((CubeMapColors[i].r - mid) * contrastR + mid);
			CubeMapColors[i].g = ((CubeMapColors[i].g - mid) * contrastG + mid);
			CubeMapColors[i].b = ((CubeMapColors[i].b - mid) * contrastB + mid);
			
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
			
			CubeMapColors[i] = saturation * ( CubeMapColors[i] ) + (1.0f - saturation) * (desaturated);
			
			CubeMapColors[i].r += brightness;
			CubeMapColors[i].g += brightness;
			CubeMapColors[i].b += brightness;
			CubeMapColors[i]  += TintColor;
			CubeMapColors[i]  += LightShapeManager.globalTintColor;			
		}
	}

	void SetGround(Color[] CubeMapColors, Color[] CubeMapColors3)
	{
		for(var i = 0; i < CubeMapColors.Length; i++)
		{
			CubeMapColors[i] *= CubeMapColors3[i];
		}
	}
	
	public void RecalcInt()
	{
 		myIndex = System.Array.IndexOf(LightShapeManager.cubeProbes, gameObject);
	}

	public void AssignCubeMaps()
	{ 
		if(!keepConnections)
		{
			List<GameObject> myCObjs = new List<GameObject>(myObjects);
			myCObjs.Clear();
			myObjects = myCObjs.ToArray();
					
			GameObject[] wObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		    
		    foreach (GameObject wi in wObjects) 
		    {			    
		        var dist = Vector3.Distance(wi.transform.position, transform.position);
				var tempMat = wi.GetComponent<MeshRenderer>();
				
				if(!gatherAll)
				{
					if(dist < GetComponent<Collider>().bounds.extents.x)
					{			
			    		if(tempMat && tempMat.sharedMaterial.HasProperty("_Cube"))       
			           	{			
							List<GameObject> myObjs = new List<GameObject>(myObjects);
							myObjs.Add(wi.gameObject);																
							myObjects = myObjs.ToArray();
							LightShapeAssignGame.myObjects = myObjects;
						}         
			        }        
				}
				
				if(gatherAll)
				{	
			    	if(tempMat && tempMat.sharedMaterial.HasProperty("_Cube"))       
			        {			
						List<GameObject> myObjs = new List<GameObject>(myObjects);
						myObjs.Add(wi.gameObject);																
						myObjects = myObjs.ToArray();
						LightShapeAssignGame.myObjects = myObjects;
					}               
				}
		    }
		}		
	}

	public void CheckConnections()
	{
		if(!keepConnections)
		{
			GameObject[] wObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		
		    foreach (GameObject wi in wObjects) 
		    {	
		        if(wi != gameObject)
		        { 
					var otherCm = wi.GetComponent("LightShapeProbe") as LightShapeProbe;		
		    		
		    		if(otherCm && otherCm.myObjects.Length > 0)
		    		{
					    foreach (GameObject j in otherCm.myObjects  )
					    {
					        foreach (GameObject i in myObjects)
					        {
				               if ( i == j )
				               {               
				               		var jIndex = System.Array.IndexOf (myObjects, j);             
	
									List<GameObject> myCObjs = new List<GameObject>(myObjects);
									myCObjs.RemoveAt(jIndex);
									myObjects = myCObjs.ToArray();
									LightShapeAssignGame.myObjects = myObjects;
				                }
					        }
					    }
				    }			
			    }        
		    }
		
			foreach(GameObject o in myObjects)
			{
				var tempMat = o.GetComponent<MeshRenderer>();
		
				if(tempMat && tempMat.sharedMaterial.HasProperty("_Cube"))       
				{							
					tempMat.sharedMaterial.SetTexture("_Cube", myCubeMap);
				}				
			}
		}
		
		if(keepConnections)
		{
			Debug.LogWarning("Turn off Freeze Connections to Gather Connections for " + gameObject.name);
		}
	}

	public void UpdateCubeMaps()
	{	
		foreach (GameObject e in excludeMe) 
	    {
			var eObjecs = e.GetComponent<MeshRenderer>();
			if(eObjecs)
			{			
				eObjecs.enabled = true;
			}
		}
		
		myCubeMap = AssetDatabase.LoadAssetAtPath(LightShapeManager.savePath + LightShapeManager.folderName + "/Cube_" + LightShapeManager.CubeMapSize + "_" + (myIndex + 1) + ".cubemap", typeof(Cubemap)) as Cubemap;

		LightShapeManager.UpdateAllCubemaps();
		
		LightShapeAssignGame.myCubeMap = myCubeMap;
		EditorUtility.UnloadUnusedAssets();
	}
	
	public void ReplaceCubeMap()
	{	
		foreach(GameObject ooo in myObjects)
		{
			if(ooo)
			{
				var tempMat = ooo.GetComponent<MeshRenderer>();
				tempMat.sharedMaterial.SetTexture("_Cube", myCubeMap);
			}
		}
	}
	
	public void ClearConnections()
	{
		if(!keepConnections)
		{
			List<GameObject> myCObjs = new List<GameObject>();
			myObjects = myCObjs.ToArray();
			LightShapeAssignGame.myObjects = myObjects;
		}
		
		if(keepConnections)
		{
			Debug.LogWarning("Turn off Freeze Connections to Gather Connections for " + gameObject.name);
		}
	}
	
	public void PreviewBall() 
	{
		if(previewBall)
		{
       	 	DestroyImmediate (previewBall);
			EditorUtility.UnloadUnusedAssets();
			UpdateCubeMaps();
   		}		
		
		if(preview)
		{			
			if(!previewBall)
			{
				GameObject[] wObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
			 	
				foreach (GameObject i in wObjects) 
		     	{	
			 		if(i.name == ("Preview"))
			 		{
						DestroyImmediate( i );
			 		}
			 	}	
				
				previewBall = new GameObject("Preview");
				previewBall.hideFlags = HideFlags.HideInHierarchy;
				mF = previewBall.AddComponent<MeshFilter>();
				mF.mesh = AssetDatabase.LoadAssetAtPath("Assets/LightShape/EditorAssets/PreviewSphere.fbx", typeof(Mesh)) as Mesh;
				mR = previewBall.AddComponent<MeshRenderer>();
				mR.castShadows = false;
				mR.material = AssetDatabase.LoadAssetAtPath("Assets/LightShape/EditorAssets/PreviewMat.mat", typeof(Material)) as Material;
				previewBall.transform.parent = transform;
				previewBall.transform.position = centerPos.transform.position;
				previewBall.transform.localScale *= gizScale;
			}		
		}
	}
	
	void LateUpdate () 
	{
		if(preview)
		{	
	        if (!cam) 
	        {
	            var go = new GameObject ("lsCubemapCamera", typeof(Camera)) as GameObject;
	            
	            go.hideFlags = HideFlags.HideAndDontSave;
	            go.transform.position = transform.position;
	            go.transform.rotation = Quaternion.identity;

	            cam = go.GetComponent<Camera>();
	            cam.enabled = false;				
	        }
	        
	        if (!rtex)
	        {
		 		rtex = new RenderTexture (32, 32, 16);
		 		rtex.isCubemap = true;
		 		rtex.hideFlags = HideFlags.HideAndDontSave;
		 	}
			
		 	cam.backgroundColor = BGColor;
		 	cam.RenderToCubemap (rtex, 63);
		 	
		 	cam.transform.position = centerPos.transform.position;
			previewBall.transform.position = centerPos.transform.position;
			
			foreach(GameObject i in myObjects)
			{	 	
		 		var pv = i.GetComponent<MeshRenderer>();
		 		pv.sharedMaterial.SetTexture("_Cube", rtex);
				
		 	}
			
			mR.sharedMaterial.SetTexture("_Cube", rtex);
		}
		
		if(!preview && cam)
		{
			GameObject[] cObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		    
		    foreach (GameObject c in cObjects) 
		    {	
				if(c.name == "lsCubemapCamera")
				{
		    		DestroyImmediate (c);
				}
			}
			
			DestroyImmediate (rtex);
		    cam = null;
	        DestroyImmediate (previewBall);
	        rtex = null;	        
	    }
		
		transform.rotation = new Quaternion(0,0,0,0);
		transform.localScale = new Vector3(1,1,1);

	}	
		
	public void AddObject(GameObject addO)
	{
		List<GameObject> objects = new List<GameObject>(myObjects);   		
		objects.Add(addO);	   		
	
		myObjects = objects.ToArray();
		LightShapeAssignGame.myObjects = myObjects;
	}
	
	public void AddExclude(GameObject addE)
	{		
		List<GameObject> objects = new List<GameObject>(excludeMe);   		
		objects.Add(addE);	   		
	
		excludeMe = objects.ToArray();	
	}
	
	public void RemoveObject(int deadObject)
	{	
		List<GameObject> objectsE = new List<GameObject>(myObjects);
		objectsE.RemoveAt(deadObject);
		
		myObjects = objectsE.ToArray();
		
		foreach(GameObject i in LightShapeManager.cubeProbes)
		{	
			if(i)
			{
				var tempObject = i.GetComponent("LightShapeProbe") as LightShapeProbe;
				tempObject.RecalcInt();
			}
		}
		LightShapeAssignGame.myObjects = myObjects;
	}

	public void RemoveExclude(int deadExclude)
	{	
		List<GameObject> objectsE = new List<GameObject>(excludeMe);
		objectsE.RemoveAt(deadExclude);
		
		excludeMe = objectsE.ToArray();			
	}
	
	public void TurnOffCubes()
	{					
		if(!renderRelections)
		{
			GameObject[] wObjects = LightShapeManager.cubeProbes;
			
			foreach(GameObject o in wObjects)
			{	 
				var oo = o.GetComponent("LightShapeProbe") as LightShapeProbe;	
				
				foreach(GameObject ooo in oo.myObjects)
				{
					var tempMat = ooo.GetComponent<MeshRenderer>();
					
					if(myCubeMap)
					{
	 					tempMat.sharedMaterial.SetTexture("_Cube", myCubeMap);
					}
					
					if(!myCubeMap)
					{
						UseColor();
					}
				}
			}
		}
		
		if(renderRelections)
		{
			UseColor();
		}
		
	    foreach (GameObject e in excludeMe) 
	    {
			var eObjecs = e.GetComponent<MeshRenderer>();
			if(eObjecs)
			{
				eObjecs.enabled = false;
			}
		}
	}
	
	void UseColor()
	{
		var othersCube = new Cubemap (LightShapeManager.CubeMapSize, TextureFormat.RGB24, false) as Cubemap;
		
		CubeMapColorsR = othersCube.GetPixels(CubemapFace.PositiveX);			

		for(var rXp = 0; rXp < CubeMapColorsR.Length; rXp++)
		{
			CubeMapColorsR[rXp] = reflectedColor;
		}
		
		othersCube.SetPixels(CubeMapColorsR, CubemapFace.PositiveX);
		othersCube.Apply();
					
		CubeMapColorsR = othersCube.GetPixels(CubemapFace.NegativeX);
		for(var rXn = 0; rXn < CubeMapColorsR.Length; rXn++)
		{
			CubeMapColorsR[rXn] = reflectedColor;
		}
		
		othersCube.SetPixels(CubeMapColorsR, CubemapFace.NegativeX);
		othersCube.Apply();			
					
		CubeMapColorsR = othersCube.GetPixels(CubemapFace.PositiveY);
		for(var rYp = 0; rYp < CubeMapColorsR.Length; rYp++)
		{
			CubeMapColorsR[rYp] = reflectedColor;
		}
		
		othersCube.SetPixels(CubeMapColorsR, CubemapFace.PositiveY);
		othersCube.Apply();			
					
		CubeMapColorsR = othersCube.GetPixels(CubemapFace.NegativeY);
		for(var rYn = 0; rYn < CubeMapColorsR.Length; rYn++)
		{
			CubeMapColorsR[rYn] = reflectedColor;
		}
		
		othersCube.SetPixels(CubeMapColorsR, CubemapFace.NegativeY);
		othersCube.Apply();			
					
		CubeMapColorsR = othersCube.GetPixels(CubemapFace.PositiveZ);
		for(var rZp = 0; rZp < CubeMapColorsR.Length; rZp++)
		{
			CubeMapColorsR[rZp] = reflectedColor;
		}
		
		othersCube.SetPixels(CubeMapColorsR, CubemapFace.PositiveZ);
		othersCube.Apply();			
					
		CubeMapColorsR = othersCube.GetPixels(CubemapFace.NegativeZ);
		for(var rZn = 0; rZn < CubeMapColorsR.Length; rZn++)
		{
			CubeMapColorsR[rZn] = reflectedColor;
		}
		
		othersCube.SetPixels(CubeMapColorsR, CubemapFace.NegativeZ);
		othersCube.Apply();			
		
		GameObject[] wObjects = LightShapeManager.cubeProbes;
		
		foreach(GameObject o in wObjects)
		{	 
			if(o)
			{
				var oo = o.GetComponent("LightShapeProbe") as LightShapeProbe;	
				
				foreach(GameObject ooo in oo.myObjects)
				{
					if(ooo)
					{
						var tempMat = ooo.GetComponent<MeshRenderer>();
	
	 					tempMat.sharedMaterial.SetTexture("_Cube", othersCube);
					}
				}
			}
		}
	}
}
#endif