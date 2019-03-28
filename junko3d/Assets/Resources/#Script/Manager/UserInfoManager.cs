using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { Alive, Die };

public class UserInfoManager : MonoBehaviour {
    public static UserInfoManager _instance;

    public Animator animator;                          
    public PlayerState unityState = PlayerState.Alive;    // 유니티 상태 (생존)

    public float unityHP = 1000;
    public float unityMaxHP = 1000;


    public GameObject fireBallPrefab;
    public GameObject erikBallPrefab;


    private void Awake()
    {
        if (!_instance) _instance = this;
        else if (_instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public float UnityHP {
        get { return unityHP; }
        set {
            switch (unityState)
            {
                case PlayerState.Alive:
                    if (value <= 0)
                    {
                        unityHP = 0;
                        unityState = PlayerState.Die;

                        // 자는 애니메이션, 연출 등
                        JunkoControl._instance.animator.Play("Sleep");
                        StartCoroutine("PlayerDie");
                        //animator.Play("Sleep");
                        //ThirdPersonCamera._instance.UnityDead();

                    }
                    else if (value >= unityMaxHP)
                    {
                        unityHP = unityMaxHP;
                    }
                    else
                    {
                        unityHP = value;
                    }

                    // UI 다시 그리기
                    UIManager._instance.DrawUI();
                    break;

                case PlayerState.Die:
                    break;
            }
        }
    }

    public float UnityMaxHP {
        get { return unityMaxHP; }
        set { unityMaxHP = value; }
    }

    public IEnumerator PlayerDie()
    {
        yield return new WaitForSeconds(1f);
        JunkoControl._instance._isMove = false;
        UIManager._instance.GameOverUI.SetActive(true);
    }

    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}
