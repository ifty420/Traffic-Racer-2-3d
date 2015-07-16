using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;  
using System.Collections.Generic; 

public class LightShape : EditorWindow 
{
	GameObject LightShapeManager;
	GameObject manager;
	int logoInt;
    Texture2D m_Logo;
	string directoryPath;
	
	List<string> fileNames;
	bool hasManager = false;
	
    void Awake()
    {
    	LightShapeManager = AssetDatabase.LoadAssetAtPath("Assets/LightShape/LightShapePrefabs/LSManager.prefab", typeof(GameObject)) as GameObject;	
		
		directoryPath = "Assets/LightShape/EditorAssets/EditorImages/";
				
		this.fileNames = new List<string>( Directory.GetFiles(this.directoryPath) );
		int howManyImages;
		howManyImages = fileNames.Count;
		
		logoInt = Random.Range (0, howManyImages);
    }
    
    
	[MenuItem ("Window/LightShape")]
    
    static void Init () 
    {
		LightShape window = (LightShape)EditorWindow.GetWindow (typeof (LightShape), false);
        
        window.maxSize = new Vector2(512, 265);
        window.minSize = window.maxSize;
        window.title  = ("Light Shape!");
        window.Show();
    }
    
	void OnEnable()
	{
		m_Logo = AssetDatabase.LoadAssetAtPath("Assets/LightShape/EditorAssets/EditorImages/lightShapeTitle" + logoInt + ".png", typeof(Texture2D)) as Texture2D;
	}
	
    void OnGUI () 
    {
        GUILayout.Label(m_Logo);
		GUI.color += new Color(-0.5f, 0.5f, -0.5f);
		GUILayout.Label("Welcome to LightShape! Enhanced Cubemap Generation for Artists and Designers.", EditorStyles.boldLabel);
		GUI.color = new Color(1f, 1f, 1f);		
      	
		if(GUILayout.Button("Create LightShape Setup"))
			Make();
		if(GUILayout.Button("Remove LightShape from Scene"))
			Take();
		GUI.color += new Color(-0.5f, 0.5f, -0.5f);
		GUILayout.Label("Information and Tutorials:", EditorStyles.boldLabel);
		GUI.color = new Color(1f, 1f, 1f);		
		
		if(GUILayout.Button("Learn LightShape"))
			Application.OpenURL ("http://www.cherubartist.com/lightshape");
		GUI.color += new Color(-0.5f, 0.5f, -0.5f);
		if(GUILayout.Button("Close Window"))
			this.Close();
    }
    
    void Make()
    {
		 GameObject[] cObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		 		
		 foreach (GameObject i in cObjects) 
	     {	
		 	if(i.tag == ("CubeProbeManager"))
		 	{
				hasManager = true;
		 		Debug.Log("There is already One Light Manager in Scene.");
		 	}
		 }
		 
		 if(!hasManager)
		 {
         	manager = Instantiate(LightShapeManager, new Vector3(0f,0f,0f), Quaternion.identity) as GameObject;
         	manager.name = manager.name.Replace("(Clone)", "");
			
			Debug.Log("Welcome to LightShape! Manager and Probe Created.");
		 }
    }

	void Take()
    {
		GameObject[] cObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		
		foreach (GameObject i in cObjects) 
	    {	
			if(i.tag == ("CubeProbe"))
			{
				DestroyImmediate( i );
			}
		}		

		if(!GameObject.FindWithTag("CubeProbeManager"))
		{
			Debug.Log("No LightShape Found.");			
		}
		
		if(GameObject.FindWithTag("CubeProbeManager"))
		{
			DestroyImmediate( GameObject.FindWithTag("CubeProbeManager"));
			
			EditorUtility.UnloadUnusedAssets();
			Debug.Log("LightShape Removed.");			
		}
						
		hasManager = false;
	}
	
}

