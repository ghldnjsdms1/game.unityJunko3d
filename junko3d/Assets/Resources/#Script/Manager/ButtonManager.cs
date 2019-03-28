using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonManager : MonoBehaviour {
    public static ButtonManager _instance;
    private void Awake() {
        if (!_instance) _instance = this;
    }

    public void OnClickedStartButton() {
        Debug.Log("시작");
        // 로딩씬에서 로드할 씬 이름 설정
        LoadingManager.loadSceneName = "WaitScene";

        // 로딩씬을 동기식으로 로딩
        SceneManager.LoadScene("Loading");
    }

    public void OnClickedExitButton() {
        Debug.Log("종료");
        Application.Quit();
    }

    public void OnClickedStoryButton() {
        LoadingManager.loadSceneName = "StoryScene1-1-1";
        SceneManager.LoadScene("Loading");
    }

    public void OnClickedRestartButton()
    {
        // 죽었을 때 다시 시작하기
        UserInfoManager._instance.OnDestroy();

        LoadingManager.loadSceneName = "StoryScene1-1-1";
        //LoadingManager.loadSceneName = Application.loadedLevelName;
        SceneManager.LoadScene("Loading");
    }

    public void OnClickedWaitButton()
    {
        // 죽었을 때 대기씬 이동
        UserInfoManager._instance.OnDestroy();

        LoadingManager.loadSceneName = "WaitScene";
        SceneManager.LoadScene("Loading");
    }
}
