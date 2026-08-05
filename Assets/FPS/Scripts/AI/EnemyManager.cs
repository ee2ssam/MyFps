using UnityEngine;
using System.Collections.Generic;

namespace Unity.FPS.AI
{
    /// <summary>
    /// 적들을 관리하는 클래스
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {
        public List<EnemyController> Enemies {  get; private set; }
        public int NumberOfEnemiesTotal { get; private set; }           //생성한 모든 적의 숫자
        public int NumberOfEnemiesRamaining => Enemies.Count;           //현재 맵에 살아있는 적의 숫자

        private void Awake()
        {
            //적 리스트 생성
            Enemies = new List<EnemyController>();
        }

        //적 리스트 등록
        public void RegisterEnemy(EnemyController enemy)
        {
            Enemies.Add(enemy);

            //생성한 모든 적의 숫자 카운트
            NumberOfEnemiesTotal++;
        }

        //적 리스트에서 제거
        public void RemoveEnemy(EnemyController enemyKilled)
        {
            Enemies.Remove(enemyKilled);
        }
    }
}