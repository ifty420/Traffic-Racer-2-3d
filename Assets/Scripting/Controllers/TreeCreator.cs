using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GroundController))]
public class TreeCreator : MonoBehaviour 
{
    public GameObject[] TreePrefabs;

    public int TreeCount = 50;

    private Transform _car;
    private List<Transform> _trees;

    private GameObject _parent;

    void Start()
    {
        _trees = new List<Transform>();
        _car = GetComponent<GroundController>().CharCar;
        _parent = new GameObject();
        _parent.name = "TreeParent";
        for (int i=0; i<TreeCount; i++)
            CreateRandomTree();
    }

    private int _upds = 0;

    void Update()
    {
        _upds++;
        if (_upds > 10)
            _upds = 0;
        else
            return;

        List<int> d = new List<int>();
        List<Transform> n = new List<Transform>();

        for (int i =0;i<_trees.Count;i++)
        {
            if (Vector3.Distance(_car.position, _trees[i].position) > 400)
            {
                d.Add(i);
                n.Add(CreateForwardTree());
            }
        }

        for (int i = d.Count-1; i>=0; i--)
        {
            Destroy(_trees[d[i]].gameObject);
            _trees.RemoveAt(d [i]);
        }

        foreach (Transform t in n)
            _trees.Add(t);
    }

    private Vector2 GetMinMaxZPos()
    {
        if (_trees.Count == 0)
            return Vector2.zero;
        Vector2 res = new Vector2(_trees [0].position.z,_trees [0].position.z);
        for (int i = 1; i<_trees.Count; i++)
        {
            float z = _trees [i].position.z;
            res.x = Mathf.Min(res.x, z);
            res.y = Mathf.Max(res.y, z);
        }
        return res;
    }

    private void CreateRandomTree()
    {
        // x: 20-80
        // z: 0-300

        Vector3 pos = new Vector3(Random.Range(25f, 30f) * Mathf.Sign(Random.value - 0.5f), 0, Random.Range(0f, 300f) * Mathf.Sign(Random.value - 0.5f));
        Transform tree = (GameObject.Instantiate(GetRandomTreePrefab()) as GameObject).transform;
        tree.position = pos;
        tree.parent = _parent.transform;
        _trees.Add(tree);
    }

    private Transform CreateForwardTree()
    {
        Vector3 pos = new Vector3(0,0,0);
        pos.x = Random.Range(25f, 30f) * Mathf.Sign(Random.value - 0.5f);
        pos.z = _car.position.z + Mathf.Sin(_car.forward.z) * Random.Range(240f, 280f);
        Transform tree = (GameObject.Instantiate(GetRandomTreePrefab()) as GameObject).transform;
        tree.position = pos;
        tree.parent = _parent.transform;
        return tree;
    }


    private GameObject GetRandomTreePrefab()
    {
        return TreePrefabs[UnityEngine.Random.Range(0,TreePrefabs.Length)];
    }
}
