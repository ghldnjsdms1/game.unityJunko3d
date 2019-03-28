using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour {
    public GameObject mTarget;

    private void Start() { if (!mTarget) mTarget = Camera.main.gameObject;
        
    }

    void Update () {
        gameObject.transform.LookAt(mTarget.transform);
        //gameObject.transform.Rotate(0, 180, 0);                           // 상대 좌표 회전
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);       // 월드 좌표 회전
    }
}
