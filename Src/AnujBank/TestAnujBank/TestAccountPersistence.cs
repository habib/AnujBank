using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AnujBank;
using NHibernate;
using NUnit.Framework;

namespace TestAnujBank
{
    class TestAccountPersistence:NHibernateInMemoryTestFixtureBase
    {

        private ISession session;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            var type = new Account().GetType();
            var assembly = Assembly.GetAssembly(type);
            InitalizeSessionFactory(assembly);
        }

        [SetUp]
        public void SetUp()
        {
            session = this.CreateSession();
        }

        [TearDown]
        public void TearDown()
        {
            session.Dispose();
        }

        [Test]
        public void ShouldSaveAccountToRepository()
        {

            var clientId = new ClientId("ABC123");
            var account = new Account(new AccountId(12341234), clientId) { Balance = 100.00, LastUpdatedDate = DateTime.Now };
            using (var tx = session.BeginTransaction())
            {
                session.Save(account);
                var query = session.CreateQuery("from Account");
                var accounts = query.List<Account>();
                tx.Commit();
                Assert.AreEqual(1, accounts.Count());
            }
           
        }
    }
}
