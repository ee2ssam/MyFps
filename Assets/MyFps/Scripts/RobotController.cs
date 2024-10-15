using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFps
{
    //로봇 상태
    public enum RobotState
    {
        R_Idle,
        R_Walk,
        R_Attack,
        R_Death
    }


    //로봇 Enemy 관리 클레스
    public class RobotController : MonoBehaviour
    {
        #region Variables
        private Animator animator;

        //로봇 현재 상태
        private RobotState currentState;
        //로봇 이전 상태
        private RobotState beforeState;

        //체력
        [SerializeField] private float startHealth = 20;
        #endregion

        private void Start()
        {
            //참조
            animator = GetComponent<Animator>();
            SetState(RobotState.R_Idle);
        }

        //로봇의 상태 변경
        private void SetState(RobotState newState)
        {
            //현재 상태 체크
            if(currentState == newState)
                return;

            //이전 상태 저장
            beforeState = currentState;
            //상태 변경
            currentState = newState;

            //상태 변경에 따른 구현 내용
            animator.SetInteger("RobotState", (int)newState);
        }
    }
}