using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    public int _maxMonsterCount = 10;
    Coroutine _coUpdateSpawningPool;
    GameManager _game;

    public void StartSpawn()
    {
        _game = Managers.Game;
        if (_coUpdateSpawningPool == null)
            _coUpdateSpawningPool = StartCoroutine(CoUpdateSpawnPool());
    }

    // TODO 몬스터 스폰하는 방법 구현하기
    IEnumerator CoUpdateSpawnPool()
    {
        for (int i = 1; i <= 5; i++)
        {
            // TODO ID 찾아주기
            Managers.Object.Spawn<MonsterController>(new Vector3(Managers.Game.Player.transform.position.x + i * 2, 0, 0), 10000002);
        }
        yield return new WaitForSeconds(1);
    }
}
