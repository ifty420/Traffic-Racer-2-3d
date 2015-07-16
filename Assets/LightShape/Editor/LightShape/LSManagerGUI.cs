using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(LightShapeManager))] 
public class LSManagerGUI : Editor 
{
	LightShapeManager myTarget;
	private SerializedObject m_Object;
	private SerializedProperty m_Property;
    private bool gizEnabled;
	private bool gatherEnabled = true;
	
	void OnEnable()
	{
		myTarget = (LightShapeManager) target;
		m_Object = new SerializedObject(target);		
	}
	
    public override void OnInspectorGUI () 
    {	
		if(!EditorApplication.isPlaying)
		{
			//Debug//myTarget.curProbeInt = EditorGUILayout.IntField("curProbeInt", myTarget.curProbeInt);
	        if(GUILayout.Button("Reset To Default Settings"))
	            Default();		
			GUI.color += new Color(-0.5f, 0.5f, -0.5f);
			GUILayout.Label("Output Directory:", EditorStyles.boldLabel);
			GUI.color = new Color(1f, 1f, 1f);
			myTarget.pathLock = EditorGUILayout.BeginToggleGroup ("Unlock Save Path?", myTarget.pathLock);
				
				GUI.color = new Color(1f, 1f, 1f);
				GUILayout.Label("Output Folder Name:");
				myTarget.folderName = EditorGUILayout.TextField("", myTarget.folderName);
				GUILayout.Label("Directory:");
				myTarget.savePath = EditorGUILayout.TextField("", myTarget.savePath);
	        	if(GUILayout.Button("Create Save Folder"))
	            	myTarget.CreateSavePath();
			EditorGUILayout.EndToggleGroup ();
			GUI.color += new Color(-0.5f, 0.5f, -0.5f);
			GUILayout.Label("Light Shape Global Options:", EditorStyles.boldLabel);
			GUI.color = new Color(1f, 1f, 1f);			
			
	        myTarget.CubeMapSize = EditorGUILayout.IntPopup("CubeMap Size: ", myTarget.CubeMapSize, myTarget.cubeSizes, myTarget.cubeSize);    	
	    	myTarget.globalTintColor = EditorGUILayout.ColorField("Global Tint", myTarget.globalTintColor);
	    	
	        myTarget.hasMips = EditorGUILayout.Toggle("Has Mip Maps?", myTarget.hasMips);
			myTarget.worldCamera = EditorGUILayout.ObjectField("World Camera:", myTarget.worldCamera, typeof(Camera), true) as Camera;
			GUI.color += new Color(-0.5f, 0.5f, -0.5f);
			GUILayout.Label("Gizmo Settings:", EditorStyles.boldLabel);
			GUI.color = new Color(1f, 1f, 1f);

			gizEnabled = EditorGUILayout.BeginToggleGroup ("Show Gizmo When Not Selected?", myTarget.showGizmo);
				myTarget.showGizmo = gizEnabled;
			myTarget.gizColor = EditorGUILayout.ColorField("Gizmo Color", myTarget.gizColor);
			myTarget.gizScale = EditorGUILayout.Slider("Gizmo Size", myTarget.gizScale, 0.2f, 5.0f);				
			GUILayout.BeginHorizontal();
					myTarget.showRanges = EditorGUILayout.Toggle("", myTarget.showRanges, GUILayout.Width(10.0f));
					GUILayout.Label("Show Probe Ranges?");
				GUILayout.EndHorizontal();			
			EditorGUILayout.EndToggleGroup ();

			GUI.color += new Color(-0.5f, 0.5f, -0.5f);
			GUILayout.Label("Rendering:", EditorStyles.boldLabel);
			GUI.color = new Color(1f, 1f, 1f);

			GUILayout.BeginHorizontal();
				myTarget.useAltMethod = EditorGUILayout.Toggle("", myTarget.useAltMethod, GUILayout.Width(10.0f));
				GUILayout.Label("Use ReadPixels? (Works With Free License)");
			GUILayout.EndHorizontal();

			GUI.color += new Color(-0.5f, 0.5f, -0.5f);
			if(GUILayout.Button("Render All CubeMaps!"))
			{
				if(!myTarget.useAltMethod)
				{
	           		myTarget.StartRender();
				}
				if(myTarget.useAltMethod)
				{
					myTarget.altRendering = true;
	           		myTarget.AltMethodManger.StartRender();
				}
			}
			GUI.color = new Color(1f, 1f, 1f);
			GUI.color += new Color(-0.5f, 0.5f, -0.5f);
			GUILayout.Label("Probe Management:", EditorStyles.boldLabel);
			GUI.color = new Color(1f, 1f, 1f);			
			gatherEnabled = EditorGUILayout.BeginToggleGroup ("UnFreeze Global Object Gathering?", myTarget.keepConnections);
			
			 	myTarget.keepConnections = gatherEnabled;
			 
				if(GUILayout.Button("Gather All Object Connections All Probes"))
	           	 CheckAllConnections();
	        	if(GUILayout.Button("Clear All Object Connections All Probes"))
	            	ClearAllConnections();
			
			EditorGUILayout.EndToggleGroup ();
			GUILayout.Label("");

	        if(GUILayout.Button("Gather All Probes"))
	            AddAllProbes();
	        if(GUILayout.Button("Remove All Probes"))
	            RemoveAllProbes();	
			GUI.color += new Color(-0.5f, 0.5f, -0.5f);
			GUILayout.Label("LightShape Probes:", EditorStyles.boldLabel);
			GUI.color = new Color(1f, 1f, 1f);			
			
	        var cubeProbes = myTarget.cubeProbes;
	
			for (int i = 0; i < cubeProbes.Length; ++i) 
			{
	            GUILayout.BeginHorizontal();
				#pragma warning disable 0168//Used because the variable `result` is in fact used here somehow, but gives a warning that it is not.			
	            var result = EditorGUILayout.ObjectField(cubeProbes[i], typeof(GameObject), true) as GameObject;
				#pragma warning restore 0168
	
	            if (GUILayout.Button("Remove", GUILayout.Width(60.0f))) 
					myTarget.RemoveProbe(i);
	 
	            GUILayout.EndHorizontal();
	        }
			
			DropAreaGUI();
			m_Object.ApplyModifiedProperties();
	        if(GUILayout.Button("Clean Up Scene"))
	            CleanScene();			
	        if (GUI.changed)       
	        	ChangeGizColor();
		}
    }	
	
	void OnSceneGUI()
	{
		if(Tools.current == Tool.Rotate)
		{
			Tools.handleRotation = myTarget.transform.rotation;
		}
		SceneView.RepaintAll();
	}
	
	private void DropAreaGUI () 
	{
        var evt = Event.current;
        var dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drag Here to Add LightShape Probes");		
		int id = GUIUtility.GetControlID(FocusType.Passive);
		
        switch (evt.type) 
		{
        	case EventType.DragUpdated:
        	case EventType.DragPerform:
			
            if (!dropArea.Contains(evt.mousePosition)) break;
 
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            DragAndDrop.activeControlID = id;
 
            if (evt.type == EventType.DragPerform) 
			{
                DragAndDrop.AcceptDrag();
                foreach (var draggedObject in DragAndDrop.objectReferences) 
				{
                    var probe = draggedObject as GameObject;
                    if (!probe) continue;

					myTarget.AddProbe(probe);
                }
				
                DragAndDrop.activeControlID = 0;
            }
		
            Event.current.Use();
            break;
        }
	}
	
	void CheckAllConnections() 
	{
		myTarget.CheckConnections();	
		Debug.Log(myTarget.cubeProbes.Length + " Cubes Checked!");
	}
	
	void ClearAllConnections() 
	{
		myTarget.ClearAllConnections();	
		Debug.Log("All Connections Cleared!");
	}

	void AddAllProbes() 
	{
		myTarget.CollectProbes();
		Debug.Log("All Probes Gathered!");
	}

	void RemoveAllProbes() 
	{
		myTarget.RemoveProbes();
		Debug.Log("All Probes Removed!");
	}
	
	void ChangeGizColor()
	{
		myTarget.ChangeGizColor();
		SceneView.RepaintAll(); 
	}
	
	void Default()
	{
		myTarget.Default();
		SceneView.RepaintAll();
	}
	
	void CleanScene()
	{
		GameObject[] wObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		    
		foreach (GameObject wi in wObjects) 
		{
			if(wi.tag == "LightShapeObject")
			{
				DestroyImmediate( wi );
			}
		}
	}
}
