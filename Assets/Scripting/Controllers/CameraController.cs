using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour, IEventSubscriber
{
    public Transform Target = null;

    public float SmoothingPosition = 0.7f;
    public float SmoothingRotation = 0.1f;

    public float SleepRotation = 0.1f;

    private Transform _cam;
    private bool _pause = false;

    private Camera _first;
    private Camera _out;

    public int CameraMode = 0;

    private void UpdateCameras()
    {
        switch (CameraMode)
        {
            case 0:
                _first.enabled = true;
                _out.enabled = false;
                EventController.PostEvent("update.camera.switch1",_first.gameObject);
                break;
            case 1:
                _first.enabled = false;
                _out.enabled = true;
                _cam.localPosition = _cam.localPosition.normalized*15;
                EventController.PostEvent("update.camera.switch2",_out.gameObject);
                break;
            case 2:
                _first.enabled = false;
                _out.enabled = true;
                _cam.localPosition = _cam.localPosition.normalized*9;
                EventController.PostEvent("update.camera.switch3",_out.gameObject);
                break;
        }
    }

    #region IEventSubscriber implementation

    public void OnEvent(string EventName, GameObject Sender)
    {
        if (EventName == "input.screen.camera.down")
        {
            NextCamera();
        }
    }

    private void NextCamera()
    {
        CameraMode++;
        if (CameraMode > 2) CameraMode = 0;
        UpdateCameras();
    }

    #endregion

    CarDriver _driver;

    void Start()
    {
        transform.position = Target.position;
        transform.rotation = Target.rotation;
        _out = transform.GetComponentInChildren<Camera>();
        _cam = _out.transform;
        _driver = Target.GetComponent<CarDriver>();
        _first = _driver.FirstPersonCameraPoint.GetComponent<Camera>();
        UpdateCameras();
        EventController.Subscribe("input.screen.camera.down", this);
        UpdateCameras();

        NextCamera();
    }

    void OnDestroy()
    {
        EventController.Unsubscribe(this);
    }


    void FixedUpdate()
    {
        if (_pause)
            return;
        var x = transform.position.x;
        Vector3 targetPos = Vector3.Lerp(transform.position, Target.position, SmoothingPosition);
        targetPos.x = x;
        if (_cam.position.y < 1)
            targetPos.y += 1 - _cam.position.y;
        transform.position = targetPos;
        if (_driver.CheckGrounded())
            StartCoroutine(FSleepRotation(Target.rotation));
    }

    IEnumerator FSleepRotation(Quaternion rot)
    {
        yield return new WaitForSeconds(SleepRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation,rot, SmoothingRotation);
    }
}
