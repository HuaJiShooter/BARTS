using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using Unity.Netcode;

namespace CharactorMovement
{
    public class CharactorMovement : NetworkBehaviour
    {

        private GameObject Target = null;
        private Vector3 TargetPlace;
        private bool bAttacking = false;
        private bool bMoving = false;
        private bool bCommand = false;

        private Animator CharactorMoveAnimator = null;
        private Transform CharactorTransform = null;
        private NavMeshAgent Nav = null;



        // Start is called before the first frame update
        void Start()
        {
            //获取角色Transform组件
            CharactorTransform = GetComponent<Transform>();
            //获取动画组件
            CharactorMoveAnimator = CharactorTransform.GetChild(0).GetComponent<Animator>();
            //获取角色导航组件
            Nav = GetComponent<Transform>().GetComponent<NavMeshAgent>();

        }

        // Update is called once per frame
        void Update()
        {
            if (!IsOwner) return;

            //指定目标时

            //自动索敌

            //如果有攻击目标,且不在攻击
            if(Target != null && !bAttacking)
            {
                //更新TargetPlace
                TargetPlace = Target.GetComponent<Transform>().position;
                //检测是否在攻击范围
                //在攻击范围内则进行攻击
                if (true)
                {
                    //在攻击范围内则攻击
                    bAttacking = true;
                    CharactorMoveAnimator.SetBool("bAttacking", true);
                    //停止移动
                    Nav.isStopped = true;
                }
                //不在攻击范围内
                else
                {
                    //继续追击
                    Nav.SetDestination(TargetPlace);
                    bAttacking = false;
                    CharactorMoveAnimator.SetBool("bAttacking", false);
                    bMoving = false;
                }
            }
            //如果玩家执行移动命令
            else if(bCommand == true && Target == null)
            {
                Nav.SetDestination(TargetPlace);
                bMoving = true;
            }
            //如果已停止移动
            else if (Nav.pathEndPosition == CharactorTransform.position)
            {
                //到达导航位置
                if (bCommand == true)
                {
                    bCommand = false;
                    bMoving = false;
                }
                //未给命令的停止状态
                else
                {

                }
            }
            CharactorMoveAnimator.SetFloat("MoveSpeed", (float)(Nav.velocity.magnitude / 2.75));




        }

        public void MoveToPlace(GameObject Target)
        {
            this.Target = Target;
            bCommand = true;
        }
        public void MoveToPlace(Vector3 Target)
        {
            this.TargetPlace = Target;
            this.Target = null;
            bCommand = true;
        }
    }
}
