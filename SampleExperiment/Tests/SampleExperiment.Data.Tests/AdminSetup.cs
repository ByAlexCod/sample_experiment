using System.Threading.Tasks;
using NUnit.Framework;
using CK.Core;
using CK.SqlServer;
using CK.DB.Actor;
using CK.DB.Auth;
using CK.DB.User.UserPassword;

using static CK.Testing.DBSetupTestHelper;

namespace SampleExperiment.Data.Tests
{
    [TestFixture]
    public class AdminSetup
    {
        [Explicit]
        [Test]
        public async Task Create_admin_user()
        {
            var userTable = TestHelper.StObjMap.Default.Obtain<UserTable>();
            var userPasswordTable = TestHelper.StObjMap.Default.Obtain<UserPasswordTable>();
            var groupTable = TestHelper.StObjMap.Default.Obtain<GroupTable>();

            using( var ctx = new SqlStandardCallContext() )
            {
                var userId = await userTable.CreateUserAsync( ctx, 1, "admin" );
                Assert.Greater( userId, 0 );

                var passwordResponse = await userPasswordTable.CreateOrUpdatePasswordUserAsync( ctx, 1, userId, "admin", UCLMode.CreateOnly );
                Assert.AreEqual( passwordResponse.OperationResult, UCResult.Created );

                await groupTable.AddUserAsync( ctx, 1, 2, userId );
            }
        }
    }
}
