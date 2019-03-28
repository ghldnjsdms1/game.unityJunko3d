using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager _instance;

    public Image HP_UI;         // 상태 UI (이미지)
    public Text HP_Text_UI;     // 상태 UI (텍스트)

    public GameObject GameOverUI;           // 클리어 UI



    public void Awake() {
        if (!_instance) _instance = this;
        else if (_instance != this) Destroy(gameObject);

       
    }

    private void Start() { DrawUI(); }
    public void DrawUI()
    {
        HP_UI.fillAmount = (float)UserInfoManager._instance.UnityHP / UserInfoManager._instance.UnityMaxHP;

        HP_Text_UI.text = UserInfoManager._instance.UnityHP.ToString()
            + " / " + UserInfoManager._instance.UnityMaxHP.ToString();
    }
}
