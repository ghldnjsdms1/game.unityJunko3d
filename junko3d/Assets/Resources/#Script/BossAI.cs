using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BossState { Alive, Die };

public class BossAI : MonoBehaviour {

    private Animator animator;
    public GameObject target;

    public float BossHP = 1000;
    public float BossMaxHP = 1000;

    private float speed;
    private bool attack = false;

    private float attackTime = 0;
    private bool die = false;

    public float damage = 150;

    public Image BossHPImage;
    public Text BossHPText;

    public GameObject MissionClearUI;

    public BossState bossState = BossState.Alive;

    void DrawBossHP()
    {
        BossHPImage.fillAmount = BossHP / BossMaxHP;
        BossHPText.text = BossHP + " / " + BossMaxHP;
    }

    // Use this for initialization
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        DrawBossHP();
        MissionClearUI.SetActive(false);
    }

    IEnumerator BossDie()
    {
        yield return new WaitForSeconds(2f);
        MissionClearUI.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(4f);

        UserInfoManager._instance.OnDestroy();

        LoadingManager.loadSceneName = "WaitScene";

        // 로딩씬을 동기식으로 로딩
        SceneManager.LoadScene("Loading");
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        // 보스가 죽어있다면 아무행동도 하지 않습니다.
        if (die) return;

        // 보스가 유니티짱을 쳐다보도록 설정합니다.
        gameObject.transform.LookAt(target.transform);

        // 파라미터 초기화
        speed = 0;
        attack = false;

        // 유니티짱과 거리가 2보다 클 경우 유니티짱을 향해 달려옵니다.
        float _distance = Vector3.Distance(gameObject.transform.position, target.transform.position);

        if (_distance > 2)
        {
            animator.SetBool("Attack", attack);

            // 거리가 10보다 멀다면 달려옵니다.
            if (_distance > 10)
            {
                speed = 5;
            }

            // 거리가 10 이하라면 걸어옵니다.
            else
            {
                speed = 2;
            }

            animator.SetFloat("Speed", speed);
            gameObject.transform.position += transform.forward * speed * Time.deltaTime;
        }
        // 거리가 2보다 가까울경우
        else
        {
            // 공격 애니메이션
            if (Time.time - attackTime > 1f)
            {
                attackTime = Time.time;

                // 이펙트 출력

                // 유니티짱의 체력을 몬스터의 공격만큼 깎습니다.
                UserInfoManager._instance.UnityHP -= damage;

                attack = true;
                animator.SetBool("Attack", attack);
            }
        }
    }

    public void AttackedMonster(float damage)
    {
        BossHP -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject skill = other.gameObject;
        switch (other.tag)
        {
            case "Skill":
                // 몬스터가 이미 잠이 들었다면
                if (die) return;
                Debug.Log("1111111");

                // 몬스터 체력깎기 (잠이 들었다면)
                if (BossHP <= 0)
                {
                    // 보스 체력을 0으로 변경하고 UI 그리기
                    BossHP = 0;
                    DrawBossHP();

                    die = true;

                    animator.Play("Dead");

                    // 디졸브 셰이더 적용
                    StartCoroutine(BossDie());
                    return;
                }

                DrawBossHP();
                break;
        }
    }
}
