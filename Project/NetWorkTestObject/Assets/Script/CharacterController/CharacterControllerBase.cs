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
            //��ȡ��ɫTransform���
            charactorTransform = GetComponent<Transform>();
            //��ȡ�������
            charactorMoveAnimator = GetComponent<Animator>();
            //��ȡ��ɫ�������
            Nav = GetComponent<Transform>().GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsOwner) return;

            if (bCommand)
            {
                //ָ��Ŀ��ʱ
                MoveAndAttack();
            }
            else
            {
                //�Զ�����
            }

            //����Animator����
            UpdateAnimatorVar();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void MoveAndAttack()
        {

            //����й���Ŀ��
            if (Target != null)
            {
                //����TargetPlace
                TargetPlace = Target.GetComponent<Transform>().position;

                //ָ��Ŀ������߼��
                CapsuleCollider collider = this.GetComponent<CapsuleCollider>();
                Vector3 targetPosition = new Vector3(Target.transform.position.x, Target.transform.position.y + 0.3f, Target.transform.position.z);
                Vector3 direction = Target.transform.position - charactorTransform.position;
                direction.Normalize();
                Vector3 startRayPosition = new Vector3(charactorTransform.position.x, charactorTransform.position.y + 0.3f, charactorTransform.position.z) + direction * collider.radius * 75;
                Ray ray = new Ray(startRayPosition,direction);
                Debug.DrawLine(startRayPosition, new Vector3(Target.transform.position.x, Target.transform.position.y + 0.3f, Target.transform.position.z), Color.red);
                RaycastHit hitInfo = new RaycastHit();
                Physics.Raycast(ray, out hitInfo);

                //���߼�⵽
                if (hitInfo.collider != null)
                {
                    Debug.Log("��ײ��Ϊ��" + hitInfo.collider.gameObject.name.ToString());
                    Debug.Log("Ŀ����Ϊ��" + Target.gameObject.name.ToString());

                    //����Ƿ��ڹ�����Χ
                    //�ڹ�����Χ����û���ϰ��赲�򹥻�
                    if (hitInfo.collider.gameObject == Target.gameObject)
                    {
                        //����
                        bAttacking = true;
                        //TODO ʹ������򹥻�Ŀ��
                        charactorTransform.LookAt(Target.transform.position);
                    }
                    //���ڹ�����Χ��
                    else
                    {
                        //����׷��
                        Nav.SetDestination(TargetPlace);
                        bAttacking = false;
                        bMoving = false;
                    }
                }
            }
            //������ִ���ƶ�����
            else if (Target == null)
            {
                bAttacking = false;

                Nav.SetDestination(TargetPlace);
                Debug.DrawLine(charactorTransform.position, TargetPlace);
                bMoving = true;
            }
            //�����ֹͣ�ƶ�
            else if (Nav.pathEndPosition == charactorTransform.position)
            {
                //���ﵼ��λ��
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
