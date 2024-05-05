using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes.InGame.Stick
{
    public class StickStatus : MonoBehaviour
    {
        [Header("�X�e�B�b�N�̉σp�����[�^")]
        [SerializeField, Tooltip("�X�e�B�b�N���ړ����鑬�x")]
        private float _moveSpeed;//�X�e�B�b�N�̈ړ����x�����߂�p�����[�^�ł�

        public float MoveSpeed { get => _moveSpeed; }//���̃X�N���v�g����_moveSpeed�̒l���Q�Ƃ������ꍇ�͂��̊֐����g���܂�

        private bool _isMovable = true;//�X�e�B�b�N���ړ��ł��邩�ǂ����̃p�����[�^�ł�
        public bool IsMovable { get => _isMovable; }//���̃X�N���v�g����_isMovable�̒l���Q�Ƃ������ꍇ�͂��̊֐����g���܂�
        public void StopMove()
        {
            _isMovable = false;
        }
    }
}