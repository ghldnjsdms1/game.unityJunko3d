using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCount : MonoBehaviour {

    public int monsterCount;

    public void Update () {
        monsterCount = 0;

        for (int i = 0; i < transform.GetChildCount(); ++i)
        {
            if (transform.GetChild(i).GetComponent<MonsterAI>().monsterState == MonsterState.Alive)
                monsterCount++;
        }

        if (monsterCount == 0)
        {
            JunkoControl._instance.portalOpen.SetActive(true);
            JunkoControl._instance.portalClose.SetActive(false);
        }
    }
}
