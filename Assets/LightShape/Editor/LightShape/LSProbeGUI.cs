using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(LightShapeProbe))]
public class LSProbeGUI : Editor 
{		
	LightShapeProbe myTarget;
	private SerializedObject m_Object;
	private SerializedProperty m_Property;
    
 	bool renderEnabled;
	bool renderEnabledAlt;
	bool overrideEnabled;
	bool overrideEnabledB;
	bool useReflection;
	bool cubeMap;
	
	void OnEnable()
	{
		myTarget = (LightShapeProbe) target;
		m_Object = new SerializedObject(target);
	}
	
    public override void OnInspectorGUI () 
    {
		if(!EditorApplication.isPlaying)
		{
			GUI.color += new Color(-0.5f, 0.5f, -0.5f);
			GUILayout.Label("Presets:", EditorStyles.boldLabel);
			GUI.color = new Color(1f, 1f, 1f);			
			GUILayout.BeginHorizontal();
	        if(GUILayout.Button("Character"))
	            myTarget.PresetCharacter();
	        if(GUILayout.Button("Default"))
	            myTarget.Default();			
	        if(GUILayout.Button("Contrasty"))
	            myTarget.PresetContrast();
			GUILayout.EndHorizontal();
			//Debug//myTarget.myIndex = EditorGUILayout.IntField("myIndex", myTarget.myIndex);			
			GUI.color += new Color(-0.5f, 0.5f, -0.5f);
			GUILayout.Label("Value and Color Attributes:", EditorStyles.boldLabel);
			GUI.color = new Color(1f, 1f, 1f);	
			myTarget.brightness = EditorGUILayout.FloatField("Brightness:", myTarget.brightness);
			if(myTarget.brightness < 0)
				myTarget.brightness = 0;
			if(myTarget.brightness > 1.5f)
				myTarget.brightness = 1.5f;
			myTarget.contrastR = EditorGUILayout.FloatField("Contrast:", myTarget.contrastR);
			if(myTarget.contrastR < 0)
				myTarget.contrastR = 0;							
			myTarget.contrastG = myTarget.contrastR;
			myTarget.contrastB = myTarget.contrastR;
			
			myTarget.mid = EditorGUILayout.Slider("Contrast MidPoint:", myTarget.mid, 0, 1);

			myTarget.saturation = EditorGUILayout.FloatField("Saturation:", myTarget.saturation);
			if(myTarget.saturation < 0)
				myTarget.saturation = 0;
	
			myTarget.TintColor = EditorGUILayout.ColorField("Tint Color:", myTarget.TintColor);
				
	        myTarget.secondaryBrightness = EditorGUILayout.FloatField("World Brightness:", myTarget.secondaryBrightness);
			if(myTarget.secondaryBrightness < 0)
				myTarget.secondaryBrightness = 0;
			
			myTarget.overrideCameraColor = EditorGUILayout.BeginToggleGroup ("Override Camera Background Color?", myTarget.overrideCameraColor);
			
			if(myTarget.overrideCameraColor)
			{
	        	myTarget.BGColor = EditorGUILayout.ColorField("Background Color:", myTarget.BGColor);
			}
			
			if(!myTarget.overrideCameraColor)
			{
				if(myTarget.LightShapeManager && myTarget.LightShapeManager.worldCamera)
				{
	        		myTarget.BGColor = myTarget.LightShapeManager.worldCamera.backgroundColor;
				}
			}
			
			EditorGUILayout.EndToggleGroup ();
						
	        renderEnabled = EditorGUILayout.BeginToggleGroup ("Render LightShape?", myTarget.renderLight);
			
			myTarget.renderLight = renderEnabled;

			if(renderEnabled)
			{				
				GUI.color += new Color(-0.5f, 0.5f, -0.5f);
				GUILayout.Label(" Light Shape Options:", EditorStyles.boldLabel);
				GUI.color = new Color(1f, 1f, 1f);					

				myTarget.primaryBrightness = EditorGUILayout.FloatField(" Brightness:", myTarget.primaryBrightness);
				if(myTarget.primaryBrightness < 0)
					myTarget.primaryBrightness = 0;
			
		        myTarget.lightShape = EditorGUILayout.ObjectField(" Light Shape:", myTarget.lightShape, typeof(GameObject), false) as GameObject;
		        
		        myTarget.myLight = EditorGUILayout.ObjectField(" Use Light:", myTarget.myLight, typeof (Light), true) as Light;
		        myTarget.lightScaleX = EditorGUILayout.FloatField(" Light Shape SizeX:", myTarget.lightScaleX);
				if(myTarget.lightScaleX < 1)
					myTarget.lightScaleX = 1;		
				
				myTarget.lightScaleY = EditorGUILayout.FloatField(" Light Shape SizeY:", myTarget.lightScaleY);
				if(myTarget.lightScaleY < 1)
					myTarget.lightScaleY = 1;
				myTarget.lightRot = EditorGUILayout.FloatField(" Shape Rotation:", myTarget.lightRot);
					
				overrideEnabled = EditorGUILayout.BeginToggleGroup ("Override Use Light Color?", myTarget.overRideLightColor);
			
				myTarget.overRideLightColor = overrideEnabled;

		        myTarget.lightShapeColor = EditorGUILayout.ColorField(" Override Color:", myTarget.lightShapeColor);

		        EditorGUILayout.EndToggleGroup ();
				
				renderEnabledAlt = EditorGUILayout.BeginToggleGroup ("Render BackLight Shape?", myTarget.renderAltLight);
					
					myTarget.renderAltLight = renderEnabledAlt;
	
					if(renderEnabledAlt)
					{						
						GUI.color += new Color(-0.5f, 0.5f, -0.5f);
						GUILayout.Label(" BackLight Options:", EditorStyles.boldLabel);
						GUI.color = new Color(1f, 1f, 1f);	
						myTarget.altLightShape = EditorGUILayout.ObjectField(" Back Light Shape:", myTarget.altLightShape, typeof(GameObject), false) as GameObject;
						myTarget.myBackLight = EditorGUILayout.ObjectField(" Use Light:", myTarget.myBackLight, typeof (Light), true) as Light;
				        myTarget.altLightScaleX = EditorGUILayout.FloatField(" Back Shape SizeX:", myTarget.altLightScaleX);
						if(myTarget.altLightScaleX < 1)
							myTarget.altLightScaleX = 1;
				
						myTarget.altLightScaleY = EditorGUILayout.FloatField(" Back Shape SizeY:", myTarget.altLightScaleY);
						if(myTarget.altLightScaleY < 1)
							myTarget.altLightScaleY = 1;		
						myTarget.altLightRot = EditorGUILayout.FloatField(" Shape Rotation:", myTarget.altLightRot);
						
						
						overrideEnabledB = EditorGUILayout.BeginToggleGroup ("Override Use Light Color?", myTarget.overRideAltLightColor);
					
						myTarget.overRideAltLightColor = overrideEnabledB;
		
				        myTarget.altShapeColor = EditorGUILayout.ColorField(" Override Color:", myTarget.altShapeColor);
						
	
				        EditorGUILayout.EndToggleGroup ();					
					}
															
					EditorGUILayout.EndToggleGroup ();

				GUILayout.BeginHorizontal();
				myTarget.hasGround = EditorGUILayout.Toggle("", myTarget.hasGround, GUILayout.Width(10.0f));
				GUILayout.Label(" Use Ground?");
				GUILayout.EndHorizontal();					
				if(myTarget.hasGround)
				{
					myTarget.groundColor = EditorGUILayout.ColorField(" Ground Color:", myTarget.groundColor);
					myTarget.groundScale = EditorGUILayout.Slider(" Ground Scale:", myTarget.groundScale, 0.1f, 4f);
				}
			

			}
		    EditorGUILayout.EndToggleGroup ();
			GUILayout.BeginHorizontal();
			myTarget.renderRelections = EditorGUILayout.Toggle("", myTarget.renderRelections, GUILayout.Width(10.0f));
			GUILayout.Label(" Other Objects In Reflection Colored?");
			GUILayout.EndHorizontal();					
			if(myTarget.renderRelections)
			{
				myTarget.reflectedColor = EditorGUILayout.ColorField(" Reflected Color:", myTarget.reflectedColor);
			}			

			cubeMap = EditorGUILayout.BeginToggleGroup ("Probe Object", myTarget.cubeMap);
			myTarget.cubeMap = cubeMap;
			
			if(cubeMap)
			{			
				GUILayout.BeginHorizontal();
					myTarget.rePosition = EditorGUILayout.Toggle("", myTarget.rePosition, GUILayout.Width(10.0f));
					GUILayout.Label(" Re-Position?");
				GUILayout.EndHorizontal();
				
				if(GUILayout.Button( "Recenter"))
	            	myTarget.centerPos.position = myTarget.transform.position;

					GUILayout.BeginHorizontal();
						myTarget.preview = EditorGUILayout.Toggle("", myTarget.preview, GUILayout.Width(10.0f));
						GUILayout.Label(" Preview? (Pro Only)");
					GUILayout.EndHorizontal();

		      	myTarget.myCubeMap = EditorGUILayout.ObjectField("My Cubemap:", myTarget.myCubeMap, typeof(Cubemap), false) as Cubemap;	   
		
				GUILayout.BeginHorizontal();
	        		myTarget.applyPerObjectInGame = EditorGUILayout.Toggle("", myTarget.applyPerObjectInGame, GUILayout.Width(10.0f));
					myTarget.LightShapeAssignGame.applyPerObjectInGame = myTarget.applyPerObjectInGame;
					GUILayout.Label(" Set CubeMaps Per Object? (In Game)");					
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
	        		myTarget.useForDynamic = EditorGUILayout.Toggle("", myTarget.useForDynamic, GUILayout.Width(10.0f));
					myTarget.LightShapeAssignGame.useForDynamic = myTarget.useForDynamic;
					GUILayout.Label(" Use For Dynamic Objects? (In Game)");			
				GUILayout.EndHorizontal();				
				
			}
			
			if(!cubeMap)
			{
				myTarget.rePosition = false;
				myTarget.preview = false;
			}
			
			myTarget.LightShapeAssignGame.applyPerObjectInGame = myTarget.applyPerObjectInGame;
			EditorGUILayout.EndToggleGroup ();
			
			GUI.color += new Color(-0.5f, 0.5f, -0.5f);
			
	        if(GUILayout.Button( "Update My CubeMap"))
	            RenderMyCube();
			GUI.color = new Color(1f, 1f, 1f);
			GUI.color += new Color(-0.5f, 0.5f, 0.5f);
	        if(GUILayout.Button("Update All CubeMaps"))
	            RenderAllCube();
				GUI.color = new Color(1f, 1f, 1f);
				GUI.color += new Color(-0.5f, 0.5f, -0.5f);
				GUILayout.Label("Object Management:", EditorStyles.boldLabel);
				GUI.color = new Color(1f, 1f, 1f);
			
				myTarget.keepConnections = EditorGUILayout.Toggle("Freeze Connections?", myTarget.keepConnections);
				myTarget.gatherAll = EditorGUILayout.Toggle("Gather All Objects?", myTarget.gatherAll);

				myTarget.myCollider.radius = EditorGUILayout.FloatField("Gather Radius:", myTarget.myCollider.radius);

			
				if(myTarget.myCollider.radius < 0)
					myTarget.myCollider.radius = 0;
			
	        	if(GUILayout.Button("Gather My Connections"))
	           	 CheckConnections();
	        	if(GUILayout.Button("Clear My Connections"))
	            	ClearConnections();			 

			GUI.color += new Color(-0.5f, 0.5f, -0.5f);
			GUILayout.Label("My Objects:", EditorStyles.boldLabel);
			GUI.color = new Color(1f, 1f, 1f);	

	        var myObjects = myTarget.myObjects;
	        
			for (int i = 0; i < myObjects.Length; ++i) 
			{
	            GUILayout.BeginHorizontal();
				#pragma warning disable 0168//Used because the variable `result` is in fact used here somehow, but gives a warning that it is not.
	            var result = EditorGUILayout.ObjectField(myObjects[i], typeof(GameObject), true) as GameObject;
				#pragma warning restore 0168
	
	            if (GUILayout.Button("Remove", GUILayout.Width(60.0f))) 
					myTarget.RemoveObject(i);
	 
	            GUILayout.EndHorizontal();
	        }
			
			DropAreaGUI();
	            
			if (GUI.changed)
				myTarget.PreviewBall();
			
			
			GUI.color += new Color(-0.5f, 0.5f, -0.5f);
			GUILayout.Label("Objects To Leave Out Of Render:", EditorStyles.boldLabel);
			GUI.color = new Color(1f, 1f, 1f);				
			var excludeMe = myTarget.excludeMe;
			for (int e = 0; e < excludeMe.Length; ++e) 
			{
	            GUILayout.BeginHorizontal();
				#pragma warning disable 0168//Used because the variable `result` is in fact used here somehow, but gives a warning that it is not.
	            var exclude = EditorGUILayout.ObjectField("", excludeMe[e], typeof(GameObject), true) as GameObject;
				#pragma warning restore 0168

	            if (GUILayout.Button("Remove", GUILayout.Width(60.0f))) 
					myTarget.RemoveExclude(e);
	 
	            GUILayout.EndHorizontal();				
	        }
			
			m_Object.ApplyModifiedProperties();
			DropAreaGUI2();	
		}

    }	
	
	void OnSceneGUI()
	{
		if(myTarget.rePosition)
		{
			myTarget.centerPos.position = Handles.PositionHandle (myTarget.centerPos.position, myTarget.centerPos.rotation);			
        	Tools.current = Tool.None;			
		}
		
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
        GUI.Box(dropArea, "Drag Here to Add Objects");		
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
                    var objects = draggedObject as GameObject;
                    if (!objects) continue;
					
					if(objects.GetComponent<MeshRenderer>())
					{
						myTarget.AddObject(objects);
					}
                }
				
                DragAndDrop.activeControlID = 0;
            }
			
            Event.current.Use();
            break;
        }
	}

	private void DropAreaGUI2 () 
	{
        var evt = Event.current;
        var dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drag Here to Add Objects");		
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
                    var objectsE = draggedObject as GameObject;
                    if (!objectsE) continue;
					
					if(objectsE.GetComponent<MeshRenderer>())
					{
						myTarget.AddExclude(objectsE);
					}					
                }
				
                DragAndDrop.activeControlID = 0;
            }
			
            Event.current.Use();
            break;
        }
	}
	void RenderMyCube()
	{
		if(myTarget.rePosition)
		{
			myTarget.rePosition = false;
		}
		
		if(myTarget.preview)
		{
			myTarget.preview = false;
		}
		myTarget.LightShapeManager.altRendering = false;
		myTarget.LightShapeManager.renderingCubeMaps = false;
		
		myTarget.StartRender();
		
	}

	void RenderAllCube()
	{
		if(myTarget.rePosition)
		{
			myTarget.rePosition = false;
		}
		
		if(myTarget.preview)
		{
			myTarget.preview = false;
		}
		
		if(!myTarget.LightShapeManager.useAltMethod)
		{
       		myTarget.LightShapeManager.StartRender();
		}
		if(myTarget.LightShapeManager.useAltMethod)
		{
			myTarget.LightShapeManager.altRendering = true;
       		myTarget.LightShapeManager.AltMethodManger.StartRender();
		}
	}

    void CheckConnections() 
    {
		if(!myTarget.keepConnections)
		{
    		myTarget.AssignCubeMaps();
			myTarget.CheckConnections();
			m_Object = new SerializedObject(myTarget);
			SceneView.RepaintAll();
       	    	Debug.Log(myTarget.myObjects.Length + " Objects Found!");
		} 
		
		if(myTarget.keepConnections)
		{
           Debug.LogWarning("Turn off Freeze Connections to Gather Connections.");
		}		
    }  
   
    void ClearConnections() 
    {
		if(!myTarget.keepConnections)
		{
			myTarget.ClearConnections();
		   
			m_Object = new SerializedObject(myTarget);
			SceneView.RepaintAll();
            Debug.Log("Connections Cleared!");
		}

		if(myTarget.keepConnections)
		{
           Debug.Log("Turn off Keep Connections to Clear.");
		}
    }
}