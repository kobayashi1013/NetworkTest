using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class UserInfo
    {
        public static UserInfo MyInfo;

        public int userId; //ユーザーID
        public string userName; //ユーザー名
        public byte[] connectionToken; //接続トークン

        public UserInfo(int id, string name)
        {
            userId = id;
            userName = name;
            connectionToken = Guid.NewGuid().ToByteArray();
        }
    }
}
