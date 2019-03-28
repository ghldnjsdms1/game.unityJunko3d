using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour {

    public GameObject floorObject = null;
    public GameObject prefabFloorWood = null;
    public GameObject prefabFloorStone = null;

	void Awake () {
        if (!prefabFloorWood)   prefabFloorWood = Resources.Load<GameObject>("#Prefab/floor_wood");
        if (!prefabFloorStone)  prefabFloorStone = Resources.Load<GameObject>("#Prefab/floor_stone");
        floorObject = new GameObject("Floor");
        

        for (int i = 0; i < 20; ++i) {
            for (int j = 0; j < 20; ++j) {
                GameObject instance = Instantiate(prefabFloorWood, new Vector3(2.5f * i - 24, 0.1f, 2.5f * j - 24), Quaternion.identity);
                instance.transform.SetParent(floorObject.transform);
            }
        }
    }
	
	void Update () {
		
	}
}
