#if UNITY_EDITOR
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


public class AltMethodManger : MonoBehaviour 
{
    Color[] CubeMapColors;
	public LightShapeManager LightShapeManager;
	public int curProbeInt;
	private Transform currentProbe;
	string directoryPath;
	List<string> fileNames;
	private string outputMessage = ""; 
	public GameObject[] reflectObjs;
	public bool secondPass = false;
	
	public void StartRender()
	{		
		if(LightShapeManager.altRendering)
		{
			EditorApplication.isPlaying = true;
		}
	}
	
	public void Start()
	{
		if(LightShapeManager.altRendering && LightShapeManager.useAltMethod)
		{
			GetPath();
			
		}
	}
	
	void GetPath()
	{
		directoryPath = LightShapeManager.savePath + LightShapeManager.folderName;

		try
		{
			string[] filePaths = Directory.GetFiles(this.directoryPath);
			foreach (string filePath in filePaths)
  			File.Delete(filePath);
			AssetDatabase.Refresh();
			
			GetReflectObjects();
			
			StartCoroutine(RenderCubeMaps()); 
		}
		
		catch (DirectoryNotFoundException directoryPathEx)
		{
			EditorApplication.isPlaying = false;
			outputMessage = "Output Folder Not Found! Please Create An Output Folder." + directoryPathEx.Message; 
			Debug.LogError (outputMessage);
		}
	}
	
	void GetReflectObjects()
	{
		secondPass = false;
		List<GameObject> myCObjs = new List<GameObject>();	

		myCObjs.Clear();
		
		foreach(GameObject i in LightShapeManager.cubeProbes)
		{
			var lsp = i.GetComponent("LightShapeProbe") as LightShapeProbe;
			if(!lsp.renderRelections)
			{
				myCObjs.Add(lsp.gameObject);
			}
		}
		
		reflectObjs = myCObjs.ToArray();
	}
	
	public IEnumerator RenderCubeMaps()
	{	
		if(!secondPass)
		{
			yield return new WaitForEndOfFrame();
			LightShapeManager.renderingCubeMaps = true;
							
			yield return new WaitForEndOfFrame();
	
			currentProbe = LightShapeManager.cubeProbes[curProbeInt].transform;
			
			yield return new WaitForEndOfFrame();
			var tempProbe = currentProbe.GetComponent("LightShapeProbe") as LightShapeProbe;
			
			yield return new WaitForEndOfFrame();
	
			StartCoroutine(tempProbe.AltMethod.BatchStart());
	
			curProbeInt += 1;
		}
		
		if(secondPass)
		{
			yield return new WaitForEndOfFrame();
			LightShapeManager.renderingCubeMaps = true;
							
			yield return new WaitForEndOfFrame();
	
			currentProbe = reflectObjs[curProbeInt].transform;
			
			yield return new WaitForEndOfFrame();
			var tempProbe = currentProbe.GetComponent("LightShapeProbe") as LightShapeProbe;
			
			yield return new WaitForEndOfFrame();
	
			StartCoroutine(tempProbe.AltMethod.BatchStart());
	
			curProbeInt += 1;			
		}		
	}
	
	public void StopBatch()
	{				
		if(!secondPass)
		{	
			curProbeInt = 0;
			
			if(reflectObjs.Length > 0)
			{
				secondPass = true;			
			}
		}
		
		if(secondPass)
		{							
			if(curProbeInt == reflectObjs.Length)
			{
				secondPass = false;	
			}
		}		

		if(!secondPass)
		{	
			try
			{
				directoryPath = LightShapeManager.savePath + LightShapeManager.folderName;
				
				this.fileNames = new List<string>( Directory.GetFiles(this.directoryPath));				
			}
			
			catch (DirectoryNotFoundException directoryPathEx)
			{
				outputMessage = "Scene Not Yet Saved. Please Save Scene To Use LightShape." + directoryPathEx.Message; 
				Debug.LogError (outputMessage);
			}
	
			int howManyImages;
			howManyImages = fileNames.Count;
			var rewardInt = Random.Range(0, LightShapeManager.rewardStrings.Length);
	
			if(howManyImages > 1)
			{
				Debug.Log(LightShapeManager.rewardStrings[rewardInt] + " " + howManyImages + " CubeMaps Rendered!"); 
			}
			if(howManyImages == 1)
			{
				Debug.Log(LightShapeManager.rewardStrings[rewardInt] + " " + howManyImages + " CubeMap Rendered!"); 
			}
			
			EditorApplication.isPlaying = false;
		}
		
	}

	void RepeatRenderCubeMaps()
	{	
		StartCoroutine(RenderCubeMaps());
	}
}
#endif