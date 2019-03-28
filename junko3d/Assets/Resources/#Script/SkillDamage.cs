using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamage : MonoBehaviour {
    public static SkillDamage _instance;

    public GameObject skill = null;
    public GameObject attack = null;

    public bool delay = false;

    public float velocity = 1.0f;
    public float coolTime = 5f;
    public int damage = 350;
    public Vector3 destination = new Vector3(0, 0, 0);

    public float motionDelay;
    public bool motion;

    private void Awake()
    {
        //if (!_instance) _instance = this;
        //else if (_instance != this) Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);

        delay       = false;
        motion      = false;
        coolTime    = 5f;
        

        skill.SetActive(false);
        attack.SetActive(false);
    }

    public void Event()
    {
        skill.SetActive(true);
        Destination();
        StartCoroutine("CoolTime");
        StartCoroutine("MotionDelay");
    }

    public IEnumerator CoolTime()
    {
        delay   = true;
        yield return new WaitForSeconds(coolTime);
        delay   = false;
    }

    public IEnumerator MotionDelay()
    {
        motion  = true;
        yield return new WaitForSeconds(motionDelay);
        motion  = false;
    }

    void Update()
    {
        if (skill.activeSelf)
            gameObject.transform.position += destination * Time.deltaTime * velocity;
    }

    private void Destination()
    {
        gameObject.transform.position = JunkoControl._instance.transform.position + new Vector3(0, 1f, 0);
        gameObject.transform.forward = JunkoControl._instance.transform.forward;
        destination = gameObject.transform.forward;
        destination = Vector3.Normalize(destination);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject _monster;

        switch (other.tag)
        {
            case "Monster":
                _monster = other.gameObject;
                _monster.GetComponent<MonsterAI>().AttackedMonster(damage);
                SkillAttack();
                skill.SetActive(false);
                break;
        }
    }

    public void SkillTrigger(Collider other)
    {
        switch (other.tag) {
            case "Monster":
                GameObject _monster;

                _monster = other.gameObject;
                _monster.GetComponent<MonsterAI>().AttackedMonster(damage);
                StartCoroutine("SkillAttack");
                skill.SetActive(false);
                break;

            case "Boss":
                GameObject _boss;

                _boss = other.gameObject;
                _boss.GetComponent<BossAI>().AttackedMonster(damage);
                StartCoroutine("SkillAttack");
                skill.SetActive(false);
                break;
        }

    }

    private IEnumerator SkillAttack()
    {
        attack.SetActive(true);
        yield return new WaitForSeconds(2f);
        attack.SetActive(false);
    }
}
