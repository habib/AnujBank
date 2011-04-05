using System;
using AnujBank;
using NUnit.Framework;

namespace TestAnujBank
{
    [TestFixture]
    class TestAllocation
    {

        [Test]
        public void ShouldNotBeAbleToCreateATargetAccountWithAccountAndAllocationPercentage()
        {
            var account = new Account(new AccountId(12345678), new ClientId("ABC123"))
                                  {Balance = 100, LastUpdatedDate = DateTime.Now};
            var targetAccount = new Allocation(account, 2);
            Assert.AreEqual(targetAccount.GetAccountNumber(), account.GetAccountNumber());
        }
    }
}
