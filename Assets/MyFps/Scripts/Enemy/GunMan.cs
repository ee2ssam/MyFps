using UnityEngine;

namespace MyFps
{
    //적 공통 상태 정의
    public enum EnemyState
    {
        E_Idle,         //대기
        E_Walk,         //패트롤
        E_Chase,        //추격
        E_Attack,       //공격
        E_Death         //죽임
    }


    /// <summary>
    /// GunMan Enemy를 관리하는 클래스
    /// </summary>
    public class GunMan : MonoBehaviour
    {

    }
}
