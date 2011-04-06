using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AnujBank;
using NHibernate;
using NUnit.Framework;

namespace TestAnujBank
{
    [TestFixture]
    public class TestClientPersistence : NHibernateInMemoryTestFixtureBase
    {

        private ISession session;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            //InitalizeSessionFactory(new FileInfo("src/Title.hbm.xml"), new FileInfo("src/CopyBidir.hbm.xml"));
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
        public void ShouldSaveClientToRepository()
        {
            
        }

    }
}
