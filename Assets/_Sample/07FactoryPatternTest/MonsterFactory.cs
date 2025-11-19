using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 심플 팩토리 : 몬스터를 생성하는 클래스
    /// 하나의 공장 하나의 생산라인(생성함수)에서 모든 몬스터를 생성한다
    /// </summary>
    public class MonsterFactory
    {

        public int count = 0;   //슬라임이 생성 갯수
        

        //몬스터 생성 메서드
        public Monster CreateMonster(MonsterType mType)
        {
            switch (mType)
            {
                case MonsterType.M_Slime:
                    return new Slime();

                case MonsterType.M_Zombie:
                    return new Zombie();

                case MonsterType.M_Goblin:
                    return new Goblin();

                case MonsterType.M_Skeleton:
                     return new Skeleton();
            }

            return null;
        }

        //좀비 어떤거
        public void AddSomething()
        {
            Debug.Log("Add Something");
        }
    }
}
