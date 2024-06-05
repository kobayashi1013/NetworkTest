using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes
{
    public class UserInfo
    {
        public static UserInfo Instance;

        public string username; //ユーザー名

        public UserInfo(string un)
        {
            username = un;
        }
    }
}
