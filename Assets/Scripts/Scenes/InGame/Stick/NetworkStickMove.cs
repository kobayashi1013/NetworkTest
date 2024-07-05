using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UniRx;
using Fusion.Addons.Physics;

namespace Scenes.InGame.Stick
{
    public class NetworkStickMove : NetworkBehaviour
    {
        private StickStatus _stickStatus;
        [SerializeField]
        private Rigidbody2D _rigidbody2D;
        private Vector2 _velocity;
        private Vector2 _moveVelocity;
        private const int CORRECTIONVALUE = 1;//数値を調整するための補正値です
        public override void Spawned()
        {
            if(HasStateAuthority)
            {
                _stickStatus = GetComponent<StickStatus>();
                _rigidbody2D = GetComponent<Rigidbody2D>();

                //値の変更をストリームで監視
                this.ObserveEveryValueChanged(x => x._stickStatus.IsMovable)
                    .Where(x => x == false)
                    .Subscribe(_ => _rigidbody2D.velocity = Vector2.zero);

                this.ObserveEveryValueChanged(x => x._velocity)
                    .Subscribe(_ => _moveVelocity = _velocity * _stickStatus.MoveSpeed);
            }
            
        }

        public override void FixedUpdateNetwork()
        {
            
                _velocity = Vector2.zero;
                if (GetInput(out NetworkInputData data))
                {
                    Debug.Log("Moving");
                    if (data.Buttons.IsSet(NetworkInputButtons.LeftArrow))
                    {
                        _velocity.x--;
                    }

                    if (data.Buttons.IsSet(NetworkInputButtons.RightArrow))
                    {
                        _velocity.x++;
                    }

                }

            //_rigidbody2D.velocity = _velocity;
                //Debug.Log(_moveVelocity);
            //Debug.Log(_stickStatus.IsMovable);
                _rigidbody2D.velocity = _moveVelocity * CORRECTIONVALUE;
            
                
            
        }
    }
}
