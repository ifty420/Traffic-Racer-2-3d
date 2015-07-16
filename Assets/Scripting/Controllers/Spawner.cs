using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour, IEventSubscriber
{
    public GameObject[] AICarPrefabs;

    public int MinCarsCount = 15;
    public int MaxCarCount = 25;
    public float SpawnTime = 0.25f;
    public float MaxSpawnTime = 1.5f;

    public float SpawnDistance = 1250;
    public float SpawnDistanceBack = 0;//100

    public AnimationCurve SpawnTimeSpeed;

    private List<Transform> SpawnedCars;
    private Transform _playerCar;

    void Awake()
    {
        SpawnedCars = new List<Transform>();
        EventController.Subscribe("car.ai.needdestroy", this);
    }

    void OnDestroy()
    {
        EventController.Unsubscribe("car.ai.needdestroy", this);
    }

    void Start()
    {
        _playerCar = GameObject.FindGameObjectWithTag("PlayerCar").transform;
        for (int i=0; i<MinCarsCount; i++)
            Invoke("SpawnNewCar", 0.1f * i);
        StartCoroutine(Spawn());
    }

    #region IEventSubscriber implementation

    public void OnEvent(string EventName, GameObject Sender)
    {
        switch (EventName)
        {
            case "car.ai.needdestroy":
                DestroyCar(Sender);
                if (SpawnedCars.Count<MinCarsCount)
                    SpawnNewCar();
                break;
        }
    }

    #endregion

    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(SpawnTime, MaxSpawnTime)*SpawnTimeSpeed.Evaluate(PlayerCarBehaviour.Instance.GetComponent<Rigidbody>().velocity.z));
            if (SpawnedCars.Count<MaxCarCount)
                SpawnNewCar();
        }
    }

    private void DestroyCar(GameObject Car)
    {
        if (SpawnedCars.Contains(Car.transform))
        {
            SpawnedCars.Remove(Car.transform);
            EventController.PostEvent("car.ai.destroyed",Car);
            Destroy(Car);
        }
    }

    private void SpawnNewCar()
    {
        bool forward = Random.value > 0.5f;
        
        GameObject car = GameObject.Instantiate(GetRandCarPrefab()) as GameObject;

        car.GetComponent<AIInputController>().Forward = forward;

        car.transform.rotation = Quaternion.Euler(new Vector3(0,forward?180:0,0));

        AIInputController aiic = car.gameObject.GetComponent<AIInputController>();
        aiic.TargetZ = forward?-1:1;
        float x = Random.value > 0.5f ? 3 : 7;
        if (forward) x*=-1;
        aiic.TargetX = x;
        bool ff = Random.value > 0.15f;
        Vector3 carPos = new Vector3(x, 7, _playerCar.position.z + (ff ? SpawnDistance : -SpawnDistanceBack));
        if (ff)
            car.GetComponent<CarDriver>().MaxSpeed*=0.6f;
        RaycastHit hit;
        car.transform.position = carPos;
        while (car.GetComponent<Rigidbody>().SweepTest(Vector3.down,out hit,6))
        {
            carPos.z += 25*(forward?1:-1);
            car.transform.position = carPos;
        }
        carPos.y = 0;
        car.transform.position = carPos;
        
        SpawnedCars.Add(car.transform);
        EventController.PostEvent("car.ai.spawned",car);
    }

    private GameObject GetRandCarPrefab()
    {
        return AICarPrefabs[Random.Range(0,AICarPrefabs.Length)];
    }
}
