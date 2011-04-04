using System;
using AnujBank;
using Moq;
using NUnit.Framework;

namespace TestAnujBank
{
    [TestFixture]
    public class TestStructure
    {
        [Test]
        public void ShouldNotBeAbleToCreateAStructureWithLessThanTwoSourceAccounts()
        {
            var clientId = new ClientId("ABC123");
            var account1 = new Account(new AccountId(12341234), clientId);
            var clientAccounts = new ClientAccounts();
            clientAccounts.Add(account1);
            Assert.Throws<ArgumentException>(() => new Structure(clientAccounts));
        }

        [Test]
        public void ShouldBeAbleToFindIfTwoStructuresHaveSharedAccounts()
        {
            var clientId = new ClientId("ABC123");
            var account1 = new Account(new AccountId(12341234), clientId);
            var account2 = new Account(new AccountId(12341235), clientId);
            var account3 = new Account(new AccountId(12341236), clientId);
            var account4 = new Account(new AccountId(12341237), clientId);

            var clientAccounts1 = new ClientAccounts();
            clientAccounts1.Add(account1);
            clientAccounts1.Add(account2);
            var structure1 = new Structure(clientAccounts1);

            var clientAccounts2 = new ClientAccounts();
            clientAccounts2.Add(account1);
            clientAccounts2.Add(account3);
            var structure2 = new Structure(clientAccounts2);
            
            var clientAccounts3 = new ClientAccounts();
            clientAccounts3.Add(account4);
            clientAccounts3.Add(account3);
            var structure3 = new Structure(clientAccounts3);

            Assert.IsTrue(structure1.SharesASourceAccountWith(structure2));
            Assert.IsFalse(structure1.SharesASourceAccountWith(structure3));

        }

        [Test]
        public void ShouldCalculateNetBalance()
        {
            Structure structure = GetTestStructure(1000.0, -500);
            Assert.AreEqual(500.0d, structure.NetBalance());
        }

        [Test]
        public void ShouldComputeInterestOnPositiveNetBalance()
        {
            string expected = (10.0 / 365).ToString().Substring(0, 5);

            var mock = new Mock<InterestRates>();

            mock.Setup(i => i.PositiveInterestRate()).Returns(2.0);

            Assert.AreEqual(expected, GetTestStructure(1000.0, -500.0).NetInterest(mock.Object).ToString().Substring(0, 5));

            mock.VerifyAll();
        }

        [Test]
        public void ShouldComputeInterestOnNegativeNetBalance()
        {
            string expected = (-15.0 / 365).ToString().Substring(0, 5);

            var mock = new Mock<InterestRates>();

            mock.Setup(i => i.NegativeInterestRate()).Returns(3.0);

            Assert.AreEqual(expected, GetTestStructure(-1000.0, 500.0).NetInterest(mock.Object).ToString().Substring(0, 5));

            mock.VerifyAll();
        }

        private Structure GetTestStructure(double balance1, double balance2)
        {
            var clientId = new ClientId("ABC123");
            var account1 = new Account(new AccountId(12341234), clientId);
            var account2 = new Account(new AccountId(12341235), clientId);
            account1.Balance = balance1;
            account2.Balance = balance2;

            var clientAccounts = new ClientAccounts();
            clientAccounts.Add(account1);
            clientAccounts.Add(account2);
            return new Structure(clientAccounts);
        }
    }
}
