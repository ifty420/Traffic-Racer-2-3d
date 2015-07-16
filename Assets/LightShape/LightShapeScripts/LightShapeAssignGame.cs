using UnityEngine;
using System.Collections;

public class LightShapeAssignGame : MonoBehaviour 
{
	public bool applyPerObjectInGame;
	public Cubemap myCubeMap;
	public GameObject[] myObjects;
	public bool useForDynamic;
	
	void Start () 
	{	
		if(applyPerObjectInGame)
		{
			foreach(GameObject i in myObjects)
			{	 	
	 			var cObjecs = i.GetComponent<MeshRenderer>();
	 			
	 			cObjecs.material.SetTexture("_Cube", myCubeMap);
	 		}
		}
	}
	
	void OnTriggerEnter(Collider Collided)
	{
		if(useForDynamic)
		{
			var tempMat = Collided.GetComponent<MeshRenderer>();
		
			if(tempMat && tempMat.sharedMaterial.HasProperty("_Cube"))       
       		{			
				tempMat.sharedMaterial.SetTexture("_Cube", myCubeMap);
			}       
		}
	}
}

