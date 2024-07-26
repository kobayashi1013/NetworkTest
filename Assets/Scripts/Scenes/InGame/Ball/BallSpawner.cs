using UnityEngine;
using Scenes.InGame.Manager;
using UniRx;
using Fusion;

namespace Scenes.InGame.Ball
{
    public class BallSpawner : NetworkBehaviour
    {
        [SerializeField, Tooltip("Ball�̃v���t�@�u������")]
        GameObject _ballPrefab;

        [Header("�X�|�[���Ɋւ���p�����[�^")]
        [SerializeField, Tooltip("�X�e�B�b�N����y���ɃI�t�Z�b�g���鋗��")]
        private float _yOffsetDistance = 0.5f;

        void Start()
        {
            //�X�|�[�����X�g���[����
            InGameManager.Instance.OnSpawn
                .Subscribe(_ =>
                {
                    var Stick = GameObject.FindWithTag("Player");
                    BallSpawn(Stick.transform.position + new Vector3(0, _yOffsetDistance, 0));
                    //Instantiate(_ballPrefab, Stick.transform.position + new Vector3(0, _yOffsetDistance, 0), Quaternion.identity, transform.parent);
                }).AddTo(this);
        }

        private void BallSpawn(Vector3 Pos)
        {
            if (Runner.IsServer)
            {
                Runner.Spawn(_ballPrefab, Pos, Quaternion.identity, null);
            }
        }
    }
}