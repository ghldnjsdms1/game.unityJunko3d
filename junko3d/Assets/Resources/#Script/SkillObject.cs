using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject : MonoBehaviour {
    public static SkillObject _instance;

    public GameObject[] skillArray;
    public SkillDamage[] skillArray2;

    private void Awake()
    {
        if (!_instance) _instance = this;
        else if (_instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
