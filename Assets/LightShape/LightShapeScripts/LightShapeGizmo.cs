#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class LightShapeGizmo : MonoBehaviour 
{	
	public LightShapeProbe LightShapeProbe;
	
	void OnEnable () 
	{
		LightShapeProbe = gameObject.GetComponent<LightShapeProbe>();
	}
	
	void OnDrawGizmosSelected () 
	{
		if(LightShapeProbe.myObjects.Length > 0)
		{
			foreach(GameObject i in LightShapeProbe.myObjects)
			{				
				if(i)
				{ 
					Gizmos.color = Color.grey * 1.5f;
					
					var midPosition = ((i.transform.position - LightShapeProbe.centerPos.transform.position) * 0.05f) + LightShapeProbe.centerPos.transform.position;
					
		    		Gizmos.DrawLine (i.transform.position, midPosition); 
		    	}
		    }
		}
		
		if(LightShapeProbe.LightShapeManager.showRanges)
		{		
			Gizmos.color = LightShapeProbe.gizColor;
			Gizmos.DrawWireSphere (transform.position, GetComponent<Collider>().bounds.extents.x);
		}
		
		if(!LightShapeProbe.LightShapeManager.showGizmo)
		{	
		    Gizmos.color = LightShapeProbe.gizColor;	
		
		    if(!LightShapeProbe.previewBall)
			{
		    	Gizmos.DrawCube (LightShapeProbe.centerPos.transform.position, new Vector3(1, 1, 1) * LightShapeProbe.gizScale);
			}
		}		
	}
	
	void OnDrawGizmos  () 
	{
		if(LightShapeProbe.LightShapeManager.showGizmo)
		{	
		    Gizmos.color = LightShapeProbe.gizColor;	
		
		    if(!LightShapeProbe.previewBall)
			{
		    	Gizmos.DrawCube (LightShapeProbe.centerPos.transform.position, new Vector3(1, 1, 1) * LightShapeProbe.gizScale);
			}
		}		
	}
}
#endif