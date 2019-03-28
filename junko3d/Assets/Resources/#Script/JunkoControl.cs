using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JunkoControl : MonoBehaviour {
    public static JunkoControl _instance;

    public GameObject portalOpen;
    public GameObject portalClose;

    public float _velocity = 3.0f;
    public LayerMask _layer;

    public CharacterController _controller;
    public bool _isMove = false;
    public Vector3 _destination = new Vector3(0, 0, 0);

    public Animator animator;
    public GameObject fireBallObject;
    public GameObject erikBallObject;

    public float recovery = 10;
    public float currentTime;

    void Awake()
    {
        if (!_instance) _instance = this;

        _controller = GetComponent<CharacterController>();
        animator    = GetComponent<Animator>();

        _isMove = false;
        currentTime = Time.time;
        portalOpen = GameObject.Find("portal_open").gameObject;
        portalClose = GameObject.Find("portal_close").gameObject;
        portalOpen.SetActive(false);
        portalClose.SetActive(true);
    }

    private void Start()
    {
        UIManager._instance.DrawUI();

        fireBallObject = Instantiate(UserInfoManager._instance.fireBallPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        erikBallObject = Instantiate(UserInfoManager._instance.erikBallPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        fireBallObject.GetComponent<SkillDamage>().coolTime = 1.5f;
        fireBallObject.GetComponent<SkillDamage>().damage = 350;
        fireBallObject.GetComponent<SkillDamage>().velocity = 11;
        erikBallObject.GetComponent<SkillDamage>().coolTime = 2.5f;
        erikBallObject.GetComponent<SkillDamage>().damage = 650;
        erikBallObject.GetComponent<SkillDamage>().velocity = 13;
    }

    void Update()
    {
        if (UserInfoManager._instance.unityState == PlayerState.Die) return;

        if (!fireBallObject.GetComponent<SkillDamage>().motion && !erikBallObject.GetComponent<SkillDamage>().motion)
        {
            KeyEvent();
            
            //땅바닥 좌표가 플레이어와 다르면 움직인다.
            if (_isMove)
            {
                Move();
            }
        }

        AnimationUpdate();

        SecondRecovery();
    }

    public void SecondRecovery()
    {
        if (UserInfoManager._instance.unityHP < UserInfoManager._instance.unityMaxHP)
        {
            if (Time.time - currentTime > 1f)
            {
                currentTime = Time.time;
                UserInfoManager._instance.unityHP += recovery;
                if (UserInfoManager._instance.unityMaxHP < UserInfoManager._instance.unityHP) {
                    UserInfoManager._instance.unityHP = UserInfoManager._instance.unityMaxHP;
                }
                UIManager._instance.DrawUI();
            }
        }
    }

    private void KeyEvent()
    {
        //화면을 클릭 하면 땅바닥 좌표 저장.
        if (Input.GetMouseButton(0))
        {
            MouseLeftEvent();
        }

        if (Input.GetKeyDown(KeyCode.A) && !fireBallObject.GetComponent<SkillDamage>().delay)
        {
            fireBallObject.GetComponent<SkillDamage>().Event();
        }
        else if (Input.GetKeyDown(KeyCode.S) && !erikBallObject.GetComponent<SkillDamage>().delay)
        {
            erikBallObject.GetComponent<SkillDamage>().Event();
        }
    }

    private void MouseLeftEvent()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layer))
        {
            _destination = hit.point;
            _isMove = true;
        }
    }

    //움직이는 함수
    private void Move()
    {
        //목적지와 거리가 같으면 안 움직임
        if (Vector3.Distance(transform.position, _destination) == 0.0F)
        {
            _isMove = false;
            return;
        }
        else if(Vector3.Distance(transform.position, _destination) < 0.3f)
        {
            _isMove = false;
            return;
        }

        Vector3 direction = _destination - transform.position;
        direction = Vector3.Normalize(direction);

        transform.LookAt(_destination);
        _controller.Move(direction * Time.deltaTime * _velocity);
    }

    void AnimationUpdate()
    {
        //if (Vector3.Distance(transform.position, _destination) == 0.0F)
        if(_isMove)
        {
            animator.SetBool("Move", true);
        }
        else
        {
            animator.SetBool("Move", false);
        }

        if (fireBallObject.GetComponent<SkillDamage>().motion)
        {
            animator.SetBool("Skill_1", true);
        }
        else
        {
            animator.SetBool("Skill_1", false);
        }

        if (erikBallObject.GetComponent<SkillDamage>().motion)
        {
            animator.SetBool("Skill_2", true);
        }
        else
        {
            animator.SetBool("Skill_2", false);
        }
    }

    public IEnumerator TriggerDelay(string tag) {
        yield return new WaitForSeconds(1f);
        string currentSceneName = Application.loadedLevelName;

        string startText, endText;

        startText = currentSceneName.Substring(0, currentSceneName.Length - 1);
        endText = currentSceneName.Substring(currentSceneName.Length - 1);

        int number = int.Parse(endText);
        number++;
        endText = number.ToString();

        string nextSceneName = startText + endText;

        SceneManager.LoadScene(nextSceneName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Portal")
            StartCoroutine(TriggerDelay(other.tag));
    }



    //http://lab.gamecodi.com/board/zboard.php?id=GAMECODILAB_Lecture_series&page=1&sn1=&divpage=1&sn=off&ss=on&sc=on&select_arrange=headnum&desc=asc&no=123
}
