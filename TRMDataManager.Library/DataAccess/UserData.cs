using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TRMDataManager.Library.SqlDataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library
{
    public class UserData
    {
        private readonly ISqlDataAccess sql;

        public UserData(ISqlDataAccess sql)
        {
            this.sql = sql;
        }

        public List<UserModel> GetUserById(string Id)
        {
            return sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", new { Id }, "TRMData");
        }

        public void CreateUser(UserModel user)
        {
            sql.SaveData("dbo.spUser_Insert", new { user.Id, user.FirstName, user.LastName, user.EmailAddress }, "TRMData");
        }
    }
}
