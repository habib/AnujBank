﻿using System;
using System.Collections.Generic;
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
            Assert.Throws<ArgumentException>(() => new Structure(clientAccounts, null,null, null));
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

            var allocations = new List<Allocation> { new Allocation(new Account(new AccountId(54321439), clientId), 100) };

            var structure1 = new Structure(clientAccounts1, allocations, null, null);

            var clientAccounts2 = new ClientAccounts();
            clientAccounts2.Add(account1);
            clientAccounts2.Add(account3);
            var structure2 = new Structure(clientAccounts2, allocations, null, null);
            
            var clientAccounts3 = new ClientAccounts();
            clientAccounts3.Add(account4);
            clientAccounts3.Add(account3);
            var structure3 = new Structure(clientAccounts3, allocations, null, null);

            Assert.IsTrue(structure1.SharesASourceAccountWith(structure2));
            Assert.IsFalse(structure1.SharesASourceAccountWith(structure3));

        }

        [Test]
        public void ShouldCalculateNetBalance()
        {
            Structure structure = GetTestStructure(1000.0, -500, GetAllocation(), null, null);
            Assert.AreEqual(500.0d, structure.NetBalance());
        }

        [Test]
        public void ShouldComputeInterestOnPositiveNetBalance()
        {
            string expected = (10.0 / 365).ToString().Substring(0, 5);

            var mock = new Mock<InterestRates>();

            mock.Setup(i => i.PositiveInterestRate()).Returns(2.0);

            Assert.AreEqual(expected, GetTestStructure(1000.0, -500.0, GetAllocation(), mock.Object, null).NetInterest().ToString().Substring(0, 5));

            mock.VerifyAll();
        }

        [Test]
        public void ShouldComputeInterestOnNegativeNetBalance()
        {
            string expected = (-15.0 / 365).ToString().Substring(0, 5);

            var mock = new Mock<InterestRates>();

            mock.Setup(i => i.NegativeInterestRate()).Returns(3.0);

            Assert.AreEqual(expected, GetTestStructure(-1000.0, 500.0, GetAllocation(), mock.Object, null).NetInterest().ToString().Substring(0, 5));

            mock.VerifyAll();
        }

        [Test]
        public void ShouldAllocateInterestAmountToExternalAccount()
        {
            const double netInterest = (10.0 / 365);

            var mock = new Mock<InterestRates>();

            mock.Setup(i => i.PositiveInterestRate()).Returns(2.0);
            var clientId = new ClientId("ABC123");
            var account1 = new Account(new AccountId(43214321), clientId);
            var allocations = new List<Allocation> { new Allocation(account1, 100) };

            var expectedMap = new Dictionary<Account, double> { { account1, netInterest } };

            Assert.AreEqual(expectedMap[account1], GetTestStructure(1000.0, -500.0, allocations, mock.Object, null).GetAllocation()[account1]);

            mock.VerifyAll();
        }

        [Test]
        public void ShouldAllocateInterestAmountToPooledAccount()
        {
            const double netInterest = (10.0 / 365);

            var mock = new Mock<InterestRates>();

            mock.Setup(i => i.PositiveInterestRate()).Returns(2.0);
            var clientId = new ClientId("ABC123");
            var account1 = new Account(new AccountId(12341234), clientId);
            var account2 = new Account(new AccountId(12341235), clientId);
            account1.Balance = 1000.0;
            account2.Balance = -500.0;
            var allocations = new List<Allocation> { new Allocation(account1, 30) , new Allocation(account2, 70)};

            var expectedMap = new Dictionary<Account, double> { {account1, netInterest * 0.3} , {account2 , netInterest * 0.7}};

            Assert.AreEqual(expectedMap[account1], GetTestStructure(1000.0, -500.0, allocations, mock.Object, null).GetAllocation()[account1]);
            Assert.AreEqual(expectedMap[account2], GetTestStructure(1000.0, -500.0, allocations, mock.Object, null).GetAllocation()[account2]);

            mock.VerifyAll();
            
        }


        [Test]
        public void ShouldNotCreateStructureWithout100PercentAllocation()
        {
             var mock = new Mock<InterestRates>();

             mock.Setup(i => i.PositiveInterestRate()).Returns(2.0);
             var clientId = new ClientId("ABC123");
             var account1 = new Account(new AccountId(12341234), clientId);
             var account2 = new Account(new AccountId(12341235), clientId);
             var allocations = new List<Allocation> { new Allocation(account1, 30) , new Allocation(account2, 30)};
             
             try
             {
                 GetTestStructure(1000.0, -500.0, allocations, mock.Object, null);
                 Assert.Fail("Should Not create Structure with Allocation interest percent sum not equal to 100");
             }
             catch (ArgumentException)
             {
             }
           
        }

        [Test]
        public void ShouldNotHaveAllocationToPoolAndExternalAccount()
        {
            var mock = new Mock<InterestRates>();

            mock.Setup(i => i.PositiveInterestRate()).Returns(2.0);
            var clientId = new ClientId("ABC123");
            var account1 = new Account(new AccountId(12341234), clientId);
            var account2 = new Account(new AccountId(43121235), clientId);
            var allocations = new List<Allocation> { new Allocation(account1, 70), new Allocation(account2, 30) };
            try
            {
                GetTestStructure(1000.0, -500.0, allocations, mock.Object, null);
                Assert.Fail("Should not Allocate Interest to both Pool And External Account");
            }
            catch (ArgumentException)
            {
            }
             
        }

        [Test]
        public void ShouldGeneratePaymentInstructionForStructure()
        {
            var mock = new Mock<InterestRates>();
            mock.Setup(i => i.PositiveInterestRate()).Returns(2.0);

            var paymentInstructionService = new Mock<PaymentInstructionService>();
            var clientId = new ClientId("ABC123");
            var account1 = new Account(new AccountId(12341234), clientId);
            var account2 = new Account(new AccountId(12341235), clientId);
            account1.Balance = 1000.0;
            account2.Balance = -500.0;
            var allocations = new List<Allocation> { new Allocation(account1, 70), new Allocation(account2, 30) };

            Structure structure = GetTestStructure(1000.0, -500.0, allocations, mock.Object, paymentInstructionService.Object);

            const decimal amount1 = (decimal) (0.7*10/365);
            paymentInstructionService.Setup(
                pis => pis.Generate(new Payment("12341234", amount1, DateTime.Now)));

            const decimal amount2 = (decimal) (0.3*10/365);
            paymentInstructionService.Setup(
                pis => pis.Generate(new Payment("12341235", amount2, DateTime.Now)));

            structure.GeneratePaymentInstruction();

            paymentInstructionService.VerifyAll();
        }

        private static List<Allocation> GetAllocation()
        {
            var clientId = new ClientId("ABC123");
            var account1 = new Account(new AccountId(43214321), clientId);
            return new List<Allocation> { new Allocation(account1, 100) };
        }

        private static Structure GetTestStructure(double balance1, double balance2, List<Allocation> allocations, InterestRates interestRates, PaymentInstructionService paymentInstructionService)
        {
            var clientId = new ClientId("ABC123");
            var account1 = new Account(new AccountId(12341234), clientId);
            var account2 = new Account(new AccountId(12341235), clientId);
            account1.Balance = balance1;
            account2.Balance = balance2;

            var clientAccounts = new ClientAccounts();
            clientAccounts.Add(account1);
            clientAccounts.Add(account2);
            return new Structure(clientAccounts, allocations, interestRates, paymentInstructionService);
        }
    }
}
