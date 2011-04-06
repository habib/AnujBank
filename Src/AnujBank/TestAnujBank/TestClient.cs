﻿using System.Collections.Generic;
using AnujBank;
using NUnit.Framework;

namespace TestAnujBank
{
    [TestFixture]
    public class TestClient
    {
        [Test]
        public void ShouldBeAbleAddAStructure()
        {
            var clientId = new ClientId("ABC123");
            var account1 = new Account(new AccountId(12341234), clientId);
            var account2 = new Account(new AccountId(12341235), clientId);
            var clientAccounts = new ClientAccounts();
            clientAccounts.Add(account1);
            clientAccounts.Add(account2);
            var structure = new Structure(clientAccounts, getAllocation(), null, null);
            var client = new Client(clientId, clientAccounts);
            client.AddStructure(structure);
            Assert.IsTrue(client.Contains(structure));
        }


        private List<Allocation> getAllocation()
        {
            var clientId = new ClientId("ABC123");
            var account1 = new Account(new AccountId(43214321), clientId);
            return new List<Allocation> { new Allocation(account1, 100) };
        }
    }

}
