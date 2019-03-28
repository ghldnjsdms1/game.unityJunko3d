using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MonsterState { Alive, Die };

public class MonsterAI : MonoBehaviour
{
    public static MonsterAI _instance = null;

    public GameObject player = null;
    public Animator animator = null;

    public float speed = 2f;
    public float speedReturn = 1f;
    private bool die = false;
    private bool attack = false;
    private Vector3 originalPosition = new Vector3(0, 0, 0);
    float attackTime = 0.0f;

    public float monsterHP = 1000;
    public float monsterMaxHP = 1000;
    public float monsterDamage = 300;

    public Image monsterHPImage = null;

    public MonsterState monsterState = MonsterState.Alive;


    void Awake()
    {
        if (!_instance) _instance = this;
        animator = GetComponent<Animator>();
        DrawMonsterHP();
        die = false;
        attack = false;
        originalPosition = transform.position;
        attackTime = -3f;
    }

    void DrawMonsterHP()
    {
        monsterHPImage.fillAmount = (float)monsterHP / monsterMaxHP;
        //UIManager._instance.skeletonHP.fillAmount = (float)monsterHP / monsterMaxHP;
        //UIManager._instance.skeletonHPText.text = monsterHP + " / " + monsterMaxHP;
    }

    void Update()
    {
        if (die) return;
        if (UserInfoManager._instance.unityState == PlayerState.Die) return;

        DistancePlayer();

        DrawMonsterHP();
    }

    public void AttackedMonster(float damage)
    {
        monsterHP -= damage;
        if (monsterHP <= 0)
        {
            StartCoroutine("MonsterDieAnimation");
        }
    }

    IEnumerator MonsterDieAnimation()
    {
        monsterState = MonsterState.Die;
        animator.Play("Die");
        yield return new WaitForSeconds(7f);
        gameObject.SetActive(false);
    }

    void DistancePlayer()
    {
        if (monsterState == MonsterState.Die) return;

        float _distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        attack = false;

        if(_distance > 2f && _distance < 6f)
        {
            animator.SetBool("Attack", attack);
            animator.SetBool("Move", true);
            //gameObject.transform.position += transform.forward * speed * Time.deltaTime;
            gameObject.transform.LookAt(player.transform);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else if(_distance <= 2f)
        {
            if (Time.time - attackTime > 3f)
            {
                attackTime = Time.time;

                // 이펙트 출력

                // 유니티짱의 체력을 몬스터의 공격만큼 깎습니다.
                //UserInfoManager._instance.UnityHP -= 600;
                StartCoroutine("Damage");

                attack = true;
                animator.SetBool("Attack", attack);
            }
        }
        else
        {
            animator.SetBool("Attack", attack);
            animator.SetBool("Move", false);
            _distance = Vector3.Distance(gameObject.transform.position, originalPosition);
            if (_distance != 0f)
            {
                //StartCoroutine("ReturnPosition");
                ReturnPosition();
            }
            else
            {

            }
        }
    }

    IEnumerator Damage()
    {
        yield return new WaitForSeconds(1f);
            UserInfoManager._instance.UnityHP -= monsterDamage;
    }

    void ReturnPosition()
    {
        //yield return new WaitForSeconds(1f);
        animator.SetBool("Move", true);
        Vector3 direction = originalPosition - transform.position;
        direction = Vector3.Normalize(direction);
        transform.LookAt(direction);
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, speedReturn * Time.deltaTime);
        //transform.position = Vector3.Lerp(transform.position, originalPosition, speed * Time.deltaTime);
    }
}