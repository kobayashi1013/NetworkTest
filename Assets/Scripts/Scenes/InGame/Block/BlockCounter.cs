using Scenes.InGame.Manager;
using UnityEngine;

namespace Scenes.InGame.Block
{
    /// <summary>
    /// ブロックの数をManagerに伝達する
    /// </summary>
    public class BlockCounter : MonoBehaviour
    {
        void Start()
        {
            InGameManager.Instance.BlockSize(transform.childCount);
        }
    }
}