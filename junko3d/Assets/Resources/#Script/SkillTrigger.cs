using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTrigger : MonoBehaviour {

    public GameObject skill;

    private void OnTriggerEnter(Collider other)
    {

        switch (other.tag)
        {
            case "Monster":
                GameObject _monster;

                _monster = other.gameObject;
                if (_monster.GetComponent<MonsterAI>().monsterState == MonsterState.Alive)
                    skill.GetComponent<SkillDamage>().SkillTrigger(other);
                break;

            case "Boss":
                GameObject _boss;

                _boss = other.gameObject;
                if (_boss.GetComponent<BossAI>().bossState == BossState.Alive)
                    skill.GetComponent<SkillDamage>().SkillTrigger(other);
                break;
        }
    }
}
