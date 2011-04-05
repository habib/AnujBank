using System;
using System.Collections.Generic;
using TestAnujBank;

namespace AnujBank
{
    public class Structure
    {
        private readonly ClientAccounts sourceClientAccounts;
        private readonly List<Allocation> allocations;
        private readonly InterestRates interestRates;

        public Structure(ClientAccounts sourceClientAccounts, List<Allocation> allocations, InterestRates interestRates)
        {
            if(sourceClientAccounts.Count < 2) throw new ArgumentException("A structure must have at least 2 source accounts.");
            
            float totalPercentage = 0;
            allocations.ForEach(al => totalPercentage += al.GetAllocationPercentage());

            if (totalPercentage != 100)
                throw new ArgumentException("A structure should have 100% allocation.");

            this.sourceClientAccounts = sourceClientAccounts;
            this.allocations = allocations;
            this.interestRates = interestRates;

        }

        public bool SharesASourceAccountWith(Structure newStructure)
        {
            return sourceClientAccounts.SharesAccountWith(newStructure.sourceClientAccounts);
        }

        public double NetBalance()
        {
            return sourceClientAccounts.NetBalance();
        }

        public double NetInterest()
        {
            double cumulativeBalance = NetBalance();
            if(cumulativeBalance > 0)
            {
                return interestRates.PositiveInterestRate()*cumulativeBalance/36500;
            }
            return interestRates.NegativeInterestRate()*cumulativeBalance/36500;
        }

        public Dictionary<Account, double> GetAllocation()
        {
            var interestAllocations = new Dictionary<Account, double >();

            var netInterest = NetInterest();
            allocations.ForEach(
                allocation =>
                interestAllocations.Add(allocation.GetAccount(),
                                        allocation.GetAllocationPercentage() * netInterest / 100));


            return interestAllocations;
        }
    }
}
