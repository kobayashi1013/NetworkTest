using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class UserInfo
    {
        public static UserInfo MyInfo;

        public string username; //���[�U�[��

        public UserInfo(string un)
        {
            username = un;
        }
    }
}
