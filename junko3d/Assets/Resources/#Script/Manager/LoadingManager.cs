using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour {
    public static LoadingManager _instance = null;

    public static string loadSceneName = "";    // 로드할 씬 이름

    public Image loadImage;                     // 로딩 이미지 (채우기)
    public Text loadText;                       // 로딩 텍스트 (경과 표시)

    private AsyncOperation asyncOperation;      // 비동기식 로딩 클래스
    private float nowTime;                      // 현재시간 체크
    private bool isLoad = false;                // 로딩 체크

    void Awake () {
        if (!_instance) _instance = this;

        nowTime = Time.time;
	}
	
	void Update () {
        // 최소 0.5초는 로딩바가 채워지도록 설정 (자연스러운 로딩을 위해서)
        if (Time.time - nowTime < 1f) {
            loadImage.fillAmount += Time.deltaTime;
            loadText.text = "로딩.." + ((loadImage.fillAmount * 100)).ToString() + "%";
        }

        // 0.5초가 지났다면 비동기식로딩 실행
        if (!isLoad && Time.time - nowTime > 0.5f) {
            isLoad = true;
            // 로딩 코루틴 호출 (비동기식 실행)
            StartCoroutine(LoadScene());
        }

        // 1초가 지났다면 씬전환 활성화 (비동기식 로딩이 완료되었다면 씬이 넘어가도록 설정)
        if (Time.time - nowTime > 3f) {
            asyncOperation.allowSceneActivation = true;
        }
    }

    public IEnumerator LoadScene() {
        bool IsDone = false;

        if (!IsDone) {
            IsDone = true;

            // loadSceneName씬을 비동기식으로 로딩
            asyncOperation = SceneManager.LoadSceneAsync(loadSceneName);

            // 씬전환 비활성화
            asyncOperation.allowSceneActivation = false;

            // 로딩이 100%(0.9) 미만이라면
            while (asyncOperation.progress < 0.9f) {
                // 로딩 진행 상황에 따라 이미지/텍스트 값 변경
                if (loadImage.fillAmount < asyncOperation.progress + 0.1f) {
                    loadImage.fillAmount = asyncOperation.progress + 0.1f;
                    loadText.text = "로딩.." + ((int)(loadImage.fillAmount * 100)).ToString() + "%";
                }
            }
        }

        // 로딩 완료
        loadImage.fillAmount = 1;
        loadText.text = "로딩..100%";

        // 1초 뒤에 아래에 있는 코드를 실행
        //yield return new WaitForSeconds(1);
        yield return 0;
    }
}
