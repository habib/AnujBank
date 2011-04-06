using System;
using System.Collections.Generic;
using System.Linq;
using AnujBank;
using Moq;
using NUnit.Framework;
using TestAnujBank;

namespace PerformanceTest
{
    [TestFixture]
    public class PerformanceTest
    {
        [Test]
        public void PerformanceTestForStructureProcessing()
        {
            var repositoryStub = new RepositoryStub();
            var interestRate = new Mock<InterestRates>();
            interestRate.Setup(ir => ir.PositiveInterestRate()).Returns(2.0);
            interestRate.Setup(ir => ir.NegativeInterestRate()).Returns(3.0);
            var feedProcessor = new FeedProcessor(repositoryStub, "../../../Pickup/testdata/feed.csv");
            feedProcessor.Process();

            var paymentInstructionService = new PaymentInstructionService();
            var structures = repositoryStub.Accounts.GroupBy(a => a.GetClientId())
                                            .Select(g => new
                                                             {
                                                                 ClientAccount = new ClientAccounts(g), 
                                                                 Allocation=GetAllocation(g.ToList())
                                                             })
                                            .Select(cs=>new Structure(cs.ClientAccount, cs.Allocation, interestRate.Object, paymentInstructionService));
            var structureList = structures.ToList();

            var start = DateTime.Now.Ticks;
            structureList.ForEach(s=>s.GeneratePaymentInstruction());
            var stop = DateTime.Now.Ticks;
            Console.WriteLine((stop-start)/10000);
        }

        private List<Allocation> GetAllocation(List<Account> accounts)
        {
            var accPercent = 100/accounts.Count;
            var leftOverPercent = 100 - accPercent*(accounts.Count - 1);
            var allocations = new List<Allocation>();
            for (int i = 0; i < accounts.Count-1; i++)
            {
                allocations.Add(new Allocation(accounts[i], accPercent));
            }
            allocations.Add(new Allocation(accounts[accounts.Count-1], leftOverPercent));
            return allocations;
        }
    }

    public class RepositoryStub:IRepository
    {
        public readonly List<Account> Accounts = new List<Account>();
        public void Save(Account account)
        {
            Accounts.Add(account);
        }
    }
}
