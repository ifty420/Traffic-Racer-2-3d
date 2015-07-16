#if UNITY_EDITOR
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[ExecuteInEditMode]
public class LightShapeManager : MonoBehaviour {
	
	public string savePath;
	public string folderName;
	public Transform cubeProbe;

	public Color globalTintColor = Color.black;
	
	public GameObject[] cubeProbes;
	public Transform currentProbe;
	
	public int curProbeInt;
	public int CubeMapSize = 128;
	public string[] cubeSizes = new string[11] {"2048", "1024", "512", "256", "128", "64", "32", "16", "8", "4", "2"};
	public int[] cubeSize = new int[11] {2048, 1024, 512, 256, 128, 64, 32, 16, 8, 4, 2};
	public bool hasMips = false;
	public Transform useLight;
	public Color gizColor = new Color(0.0f, 0.5f, 1.0f, 0.5f);
	public float gizScale = 1;
	public bool renderingCubeMaps = false;	
	public bool showRanges = true;
	public bool showGizmo = true;
	public bool useAltMethod;
	public AltMethodManger AltMethodManger;
	public bool altRendering = false;
	public bool keepConnections = true;
	string directoryPath;
	List<string> fileNames;
	private string outputMessage = "";  
	public string[] rewardStrings = new string[9] {"Sweet!", "Super Cool!", "U R Awesome!", "You Rule!", "Looks Great!", "You Now Have Lots Of Friends!", "Life Is Good!", "Fantastic!", "Hotness"};
	public int rewardInt;
	public Camera worldCamera;
	public bool pathLock = true;
	public GameObject[] reflectObjs;
	bool secondPass = false;
	public bool updatingCubemaps;
	
	public void ChangeGizColor()
	{
		foreach(GameObject i in cubeProbes)
		{
			var probeS = i.GetComponent("LightShapeProbe") as LightShapeProbe;
			probeS.gizColor = gizColor;
			probeS.gizScale = gizScale;
		}
	}
		
	void Initialize()
	{
		foreach(GameObject i in cubeProbes)
		{
			var probeIndex = i.GetComponent("LightShapeProbe") as LightShapeProbe;
			probeIndex.myIndex = System.Array.IndexOf (cubeProbes, i);
			i.gameObject.name = "LSProbe_" + (probeIndex.myIndex + 1);
		}
	}
	
	public void Default()
	{
	 	CubeMapSize = 128;
		globalTintColor = Color.black;
		hasMips = false;
		gizColor = new Color(0.0f, 0.5f, 1.0f, .5f);
		gizScale = 1;
		showGizmo = true;
		showRanges = true;
		keepConnections = true;
		ChangeGizColor();
	}
		 
	void Awake() 
	{
		gameObject.GetComponent<AltMethodManger>().hideFlags = HideFlags.HideInInspector;
		
		if(cubeProbes.Length == 0)
		{
			GameObject getProbe = Instantiate(AssetDatabase.LoadAssetAtPath("Assets/LightShape/LightShapePrefabs/LSProbe.prefab", typeof(GameObject))) as GameObject;
			getProbe.transform.parent = transform;
			List<GameObject> myCObjs = new List<GameObject>(cubeProbes);
			myCObjs.Add(getProbe);
			cubeProbes = myCObjs.ToArray();			
		}

		foreach(GameObject i in cubeProbes)
		{
			var probeIndex = i.GetComponent("LightShapeProbe") as LightShapeProbe;
			probeIndex.myIndex = System.Array.IndexOf (cubeProbes, i);
			i.gameObject.name = "LSProbe_" + (probeIndex.myIndex + 1);
		}

		ChangeGizColor();
	}
	
	void Start()
	{
		PrefabUtility.DisconnectPrefabInstance(gameObject);
		currentProbe = cubeProbes[0].transform;
	}
	
	public void CreateSavePath()
	{		
		if(savePath != "" && folderName != "")
		{		
			if(System.IO.Directory.Exists(savePath + folderName))
			{
				Debug.LogError(savePath + folderName + " Already Exsists! Please Delete The Directory Or Choose A Different Path.");
			}		
					
			if(!System.IO.Directory.Exists(savePath + folderName))
			{
				System.IO.Directory.CreateDirectory(savePath + folderName);
				AssetDatabase.Refresh();
				Debug.Log(savePath + folderName + " Output Folder Created.");
				pathLock = false;
			}
		}
		
		if(savePath == "" || folderName == "")
		{
			Debug.LogError("SavePath or FolderName are Empty!, Please Use At Least One Character In Each Field.");
		}		
	}
	
	public void StartRender()
	{
		directoryPath = savePath + folderName;
		curProbeInt = 0;
		try
		{
			string[] filePaths = Directory.GetFiles(this.directoryPath);
			foreach (string filePath in filePaths)
  			File.Delete(filePath);
			AssetDatabase.Refresh();
			
			GetReflectObjects();
				
			RenderCubeMaps();
			rewardInt = Random.Range(0, rewardStrings.Length);
		}
		
		catch (DirectoryNotFoundException directoryPathEx)
		{
			outputMessage = "Output Folder Not Found! Please Create An Output Folder." + directoryPathEx.Message; 
			Debug.LogError (outputMessage);
		}
	}
	
	void GetReflectObjects()
	{
		secondPass = false;
		List<GameObject> myCObjs = new List<GameObject>();	

		myCObjs.Clear();
		
		foreach(GameObject i in cubeProbes)
		{
			if(i)
			{
				var lsp = i.GetComponent("LightShapeProbe") as LightShapeProbe;
				if(!lsp.renderRelections)
				{
					myCObjs.Add(lsp.gameObject);
				}
			}
		}
		
		reflectObjs = myCObjs.ToArray();				
	}
	
	public void RenderCubeMaps()
	{	
		if(!secondPass)
		{
			CollectProbes();
			
			renderingCubeMaps = true;

			currentProbe = cubeProbes[curProbeInt].transform;
	
			var tempProbe = currentProbe.GetComponent("LightShapeProbe") as LightShapeProbe;
			
			tempProbe.RenderCubeMaps();
	
			curProbeInt += 1;
			
			if(curProbeInt < cubeProbes.Length)
			{	
				RepeatRenderCubeMaps();
			}
			
			if(curProbeInt >= cubeProbes.Length)
			{	
				curProbeInt = 0;
				
				if(reflectObjs.Length > 0)
				{
					secondPass = true;
					
					foreach(GameObject i in cubeProbes)
					{
						var ls = i.GetComponent("LightShapeProbe") as LightShapeProbe;
						ls.UpdateCubeMaps();
					}
					
					RepeatRenderCubeMaps();
				}
				
				if(!secondPass && reflectObjs.Length == 0)
				{				
					try
					{
						directoryPath = savePath + folderName;
						
						this.fileNames = new List<string>( Directory.GetFiles(this.directoryPath));
					}
					
					catch (DirectoryNotFoundException directoryPathEx)
					{
						outputMessage = "Scene Not Yet Saved. Please Save Scene To Use LightShape." + directoryPathEx.Message; 
						Debug.LogError (outputMessage);
					}
					
					int howManyImages;
					howManyImages = fileNames.Count;
					
					if(howManyImages > 1)
					{
						Debug.Log(rewardStrings[rewardInt] + " " + howManyImages + " CubeMaps Rendered!"); 
					}
					if(howManyImages == 1)
					{
						Debug.Log(rewardStrings[rewardInt] + " " + howManyImages + " CubeMap Rendered!"); 
					}
														
					renderingCubeMaps = false;
					EditorUtility.UnloadUnusedAssets();
				}
			}
		}
		
		if(secondPass)
		{
			renderingCubeMaps = true;
			
			currentProbe = reflectObjs[curProbeInt].transform;
	
			var tempProbe = currentProbe.GetComponent("LightShapeProbe") as LightShapeProbe;
			
			tempProbe.RenderCubeMaps();
	
			curProbeInt += 1;
			
			if(curProbeInt < reflectObjs.Length)
			{	
				RepeatRenderCubeMaps();				
			}
			
			if(curProbeInt >= reflectObjs.Length)
			{	
				curProbeInt = 0;
								
				try
				{
					directoryPath = savePath + folderName;
					
					this.fileNames = new List<string>( Directory.GetFiles(this.directoryPath));
				}
				
				catch (DirectoryNotFoundException directoryPathEx)
				{
					outputMessage = "Scene Not Yet Saved. Please Save Scene To Use LightShape." + directoryPathEx.Message; 
					Debug.LogError (outputMessage);
				}
				
				int howManyImages;
				howManyImages = fileNames.Count;
				
				if(howManyImages > 1)
				{
					Debug.Log(rewardStrings[rewardInt] + " " + howManyImages + " CubeMaps Rendered!"); 
				}
				if(howManyImages == 1)
				{
					Debug.Log(rewardStrings[rewardInt] + " " + howManyImages + " CubeMap Rendered!"); 
				}
													
				renderingCubeMaps = false;
				secondPass = false;
				EditorUtility.UnloadUnusedAssets();
			}		
		}
		
		UpdateAllCubemaps();
	}

	void RepeatRenderCubeMaps()
	{	
		RenderCubeMaps();
	}
			    	
	public void AddProbe(GameObject addP)
	{
		var otherCm = addP.GetComponent("LightShapeProbe") as LightShapeProbe;

		if(!otherCm)
		{
			Debug.Log ("Opologies, That Object Is Not a LightShape Probe.");
		}
		
		if(otherCm)
		{
			List<GameObject> probe = new List<GameObject>(cubeProbes);   		
			probe.Add(addP);	   		
	
			cubeProbes = probe.ToArray();
			Initialize();
		}
	}
	
	public void RemoveProbe(int deadProbe)
	{	
		List<GameObject> probe = new List<GameObject>(cubeProbes);
		probe.RemoveAt(deadProbe);
		
		cubeProbes = probe.ToArray();
		
		foreach(GameObject i in cubeProbes)
		{	
			if(i)
			{
				var tempProbe = i.GetComponent("LightShapeProbe") as LightShapeProbe;
				tempProbe.RecalcInt();
			}
		}
		Initialize();
	}
	
	
	public void CheckConnections()
	{
		Initialize();
		ClearAllConnections();
		
		foreach(GameObject i in cubeProbes)
		{		    	
			var otherCm = i.GetComponent("LightShapeProbe") as LightShapeProbe;
			if(otherCm.gatherAll)
			{
				otherCm.gatherAll = false;
				Debug.LogWarning ("Gather All Objects is Set on One or More of Your Probes. Now Deactivating so that Gather All Probe Connections can be used." );
			}
			otherCm.AssignCubeMaps();		
			otherCm.CheckConnections();		     
	    }
	}
	
	public void ClearAllConnections()
	{
		foreach(GameObject i in cubeProbes)
		{	
			var otherCm = i.GetComponent("LightShapeProbe") as LightShapeProbe;				
			otherCm.ClearConnections();			     
	    } 
	}
	
	public void CollectProbes()
	{
		GameObject[] wpObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];	
		List<GameObject> probes = new List<GameObject>();
		
	    foreach (GameObject p in wpObjects) 
	    {	
	        if(p.tag == ("CubeProbe"))
	        {				
				probes.Add(p);												
			}
			
		}
		
		cubeProbes = probes.ToArray();
		Initialize();
		ChangeGizColor();
	}
	
	public void RemoveProbes()
	{
		List<GameObject> probes = new List<GameObject>();
		probes.Clear();
		cubeProbes = probes.ToArray();
	}
	
	public void UpdateAllCubemaps()
	{
		updatingCubemaps = true;
		
		foreach(GameObject i in cubeProbes)
		{
			var p = i.GetComponent("LightShapeProbe") as LightShapeProbe;
			p.ReplaceCubeMap();
		}
		updatingCubemaps = false;
	}
}
#endif