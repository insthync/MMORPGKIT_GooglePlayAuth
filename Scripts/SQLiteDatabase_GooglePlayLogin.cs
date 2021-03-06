﻿#if UNITY_STANDALONE && !CLIENT_BUILD
using Cysharp.Threading.Tasks;
using Mono.Data.Sqlite;

namespace MultiplayerARPG.MMO
{
    public partial class SQLiteDatabase
    {
        public override async UniTask<string> GooglePlayLogin(string gpgId, string email)
        {
            await UniTask.Yield();
            string id = string.Empty;
            ExecuteReader((reader) =>
            {
                if (reader.Read())
                    id = reader.GetString(0);
            }, "SELECT id FROM userlogin WHERE username=@username AND password=@password AND authType=@authType LIMIT 1",
                new SqliteParameter("@username", "g_" + gpgId),
                new SqliteParameter("@password", GenericUtils.GetMD5(gpgId)),
                new SqliteParameter("@authType", AUTH_TYPE_GOOGLE_PLAY));

            if (string.IsNullOrEmpty(id))
            {
                id = GenericUtils.GetUniqueId();
                ExecuteNonQuery("INSERT INTO userlogin (id, username, password, email, authType) VALUES (@id, @username, @password, @email, @authType)",
                    new SqliteParameter("@id", id),
                    new SqliteParameter("@username", "g_" + gpgId),
                    new SqliteParameter("@password", GenericUtils.GetMD5(gpgId)),
                    new SqliteParameter("@email", email),
                    new SqliteParameter("@authType", AUTH_TYPE_GOOGLE_PLAY));
            }
            return id;
        }
    }
}
#endif