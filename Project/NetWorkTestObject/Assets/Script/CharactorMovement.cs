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
            //��ȡ��ɫTransform���
            CharactorTransform = GetComponent<Transform>();
            //��ȡ�������
            CharactorMoveAnimator = CharactorTransform.GetChild(0).GetComponent<Animator>();
            //��ȡ��ɫ�������
            Nav = GetComponent<Transform>().GetComponent<NavMeshAgent>();

        }

        // Update is called once per frame
        void Update()
        {
            if (!IsOwner) return;

            //ָ��Ŀ��ʱ

            //�Զ�����

            //����й���Ŀ��,�Ҳ��ڹ���
            if(Target != null && !bAttacking)
            {
                //����TargetPlace
                TargetPlace = Target.GetComponent<Transform>().position;
                //����Ƿ��ڹ�����Χ
                //�ڹ�����Χ������й���
                if (true)
                {
                    //�ڹ�����Χ���򹥻�
                    bAttacking = true;
                    CharactorMoveAnimator.SetBool("bAttacking", true);
                    //ֹͣ�ƶ�
                    Nav.isStopped = true;
                }
                //���ڹ�����Χ��
                else
                {
                    //����׷��
                    Nav.SetDestination(TargetPlace);
                    bAttacking = false;
                    CharactorMoveAnimator.SetBool("bAttacking", false);
                    bMoving = false;
                }
            }
            //������ִ���ƶ�����
            else if(bCommand == true && Target == null)
            {
                Nav.SetDestination(TargetPlace);
                bMoving = true;
            }
            //�����ֹͣ�ƶ�
            else if (Nav.pathEndPosition == CharactorTransform.position)
            {
                //���ﵼ��λ��
                if (bCommand == true)
                {
                    bCommand = false;
                    bMoving = false;
                }
                //δ�������ֹͣ״̬
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
