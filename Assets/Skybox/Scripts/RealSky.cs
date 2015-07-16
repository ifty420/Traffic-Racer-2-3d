////////////////////////////////////////////
///                                      ///
///         RealSky Version 1.4          ///
///  Created by: Black Rain Interactive  ///
///                                      ///
//////////////////////////////////////////// 

// Super stripped down version 			  //
// Edited to be just a sky which spins 	  //

// Updated for work with EventController in car racing project (VallVerk)

using UnityEngine;
using System.Collections;

public class RealSky : MonoBehaviour, IEventSubscriber {

	public Texture dayTime;
	public float skySpeed = 0.0f;
	
    public Camera mainCamera;
    public int skyBoxLayer = 8;

    float counter = 0.0f;
    bool isPaused = false;
    GameObject skyCamera;
	
	void Awake(){

        StartCoroutine("Counter");
		StartCoroutine("SkyRotation");

        if (mainCamera == null)
            return;

        gameObject.layer = skyBoxLayer;

        skyCamera = new GameObject("SkyboxCamera");
        skyCamera.AddComponent<Camera>();
        skyCamera.GetComponent<Camera>().depth = -10;
        skyCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Color;
        skyCamera.GetComponent<Camera>().cullingMask = 1 << skyBoxLayer;
        skyCamera.transform.position = gameObject.transform.position;

        mainCamera.cullingMask = 1;
        mainCamera.clearFlags = CameraClearFlags.Depth;

        EventController.Subscribe("update.camera.switch1", this);
        EventController.Subscribe("update.camera.switch2", this);
        EventController.Subscribe("update.camera.switch3", this);
    }
    
    void Start(){

        GetComponent<Renderer>().material.SetTexture("_Texture01", dayTime);
	}

    void OnDestroy()
    {
        EventController.Unsubscribe(this);
    }

    #region IEventSubscriber implementation

    public void OnEvent(string EventName, GameObject Sender)
    {
        mainCamera = Sender.GetComponent<Camera>();
    }

    #endregion

    void Update(){

        if (mainCamera != null){
            skyCamera.transform.rotation = mainCamera.transform.rotation;
        }

    }

    IEnumerator Counter(){

        while (true){

            if (isPaused == false){
                counter += Time.deltaTime;
            }

            yield return null;

        }

    }
	
	IEnumerator SkyRotation(){
		
		while (true){

            transform.Rotate(Vector3.up * Time.deltaTime * skySpeed, Space.World);
			yield return null;
			
		}
	}

}