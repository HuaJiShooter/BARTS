using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using Unity.Netcode;

namespace CharactorControllerBase
{
    public class CharactorControllerBase : NetworkBehaviour
    {

        protected GameObject Target = null;
        protected Vector3 TargetPlace;
        protected bool bAttacking = false;
        protected bool bMoving = false;
        protected bool bCommand = false;

        protected Animator charactorMoveAnimator = null;
        protected Transform charactorTransform = null;
        protected NavMeshAgent Nav = null;

        // Start is called before the first frame update
        void Start()
        {
            //获取角色Transform组件
            charactorTransform = GetComponent<Transform>();
            //获取动画组件
            charactorMoveAnimator = GetComponent<Animator>();
            //获取角色导航组件
            Nav = GetComponent<Transform>().GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsOwner) return;

            if (bCommand)
            {
                //指定目标时
                MoveAndAttack();
            }
            else
            {
                //自动索敌
            }

            //更新Animator变量
            UpdateAnimatorVar();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void MoveAndAttack()
        {

            //如果有攻击目标
            if (Target != null)
            {
                //更新TargetPlace
                TargetPlace = Target.GetComponent<Transform>().position;

                //指向目标的射线检测
                CapsuleCollider collider = this.GetComponent<CapsuleCollider>();
                Vector3 targetPosition = new Vector3(Target.transform.position.x, Target.transform.position.y + 0.3f, Target.transform.position.z);
                Vector3 direction = Target.transform.position - charactorTransform.position;
                direction.Normalize();
                Vector3 startRayPosition = new Vector3(charactorTransform.position.x, charactorTransform.position.y + 0.3f, charactorTransform.position.z) + direction * collider.radius * 75;
                Ray ray = new Ray(startRayPosition,direction);
                Debug.DrawLine(startRayPosition, new Vector3(Target.transform.position.x, Target.transform.position.y + 0.3f, Target.transform.position.z), Color.red);
                RaycastHit hitInfo = new RaycastHit();
                Physics.Raycast(ray, out hitInfo);

                //射线检测到
                if (hitInfo.collider != null)
                {
                    Debug.Log("碰撞物为：" + hitInfo.collider.gameObject.name.ToString());
                    Debug.Log("目标物为：" + Target.gameObject.name.ToString());

                    //检测是否在攻击范围
                    //在攻击范围内且没有障碍阻挡则攻击
                    if (hitInfo.collider.gameObject == Target.gameObject)
                    {
                        //攻击
                        bAttacking = true;
                        //TODO 使玩家面向攻击目标
                        charactorTransform.LookAt(Target.transform.position);
                    }
                    //不在攻击范围内
                    else
                    {
                        //继续追击
                        Nav.SetDestination(TargetPlace);
                        bAttacking = false;
                        bMoving = false;
                    }
                }
            }
            //如果玩家执行移动命令
            else if (Target == null)
            {
                bAttacking = false;

                Nav.SetDestination(TargetPlace);
                Debug.DrawLine(charactorTransform.position, TargetPlace);
                bMoving = true;
            }
            //如果已停止移动
            else if (Nav.pathEndPosition == charactorTransform.position)
            {
                //到达导航位置
                bCommand = false;
                bMoving = false;
            }

        }


        protected void UpdateAnimatorVar()
        {
            charactorMoveAnimator.SetBool("bAttacking", bAttacking);
            charactorMoveAnimator.SetBool("bMoving", bMoving);
            charactorMoveAnimator.SetFloat("MoveSpeed", (float)(Nav.velocity.magnitude / 2.75));
        }



        protected void MoveToPlace(GameObject Target)
        {
            this.Target = Target;
            bCommand = true;
        }
        protected void MoveToPlace(Vector3 Target)
        {
            this.TargetPlace = Target;
            this.Target = null;
            bCommand = true;
        }

        public bool Fire()
        {

            Ray ray = new Ray(charactorTransform.position, Target.transform.position);
            Debug.DrawLine(charactorTransform.position, Target.transform.position);
            RaycastHit hitInfo = new RaycastHit();

            if(Physics.Raycast(ray, out hitInfo))
            {
                return true;
            }
            return false;
        }

        public void CanMove()
        {
            Nav.isStopped = false;
        }
        public void CantMove()
        {
            Nav.isStopped = true;
        }
    }
}
