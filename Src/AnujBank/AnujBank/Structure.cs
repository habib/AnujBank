using System;
using System.Collections.Generic;
using TestAnujBank;

namespace AnujBank
{
    public class Structure
    {
        private readonly ClientAccounts sourceClientAccounts;
        private readonly List<Allocation> allocations;

        public Structure(ClientAccounts sourceClientAccounts, List<Allocation> allocations)
        {
            if(sourceClientAccounts.Count < 2) throw new ArgumentException("A structure must have at least 2 source accounts.");
            this.sourceClientAccounts = sourceClientAccounts;
            this.allocations = allocations;
        }

        public bool SharesASourceAccountWith(Structure newStructure)
        {
            return sourceClientAccounts.SharesAccountWith(newStructure.sourceClientAccounts);
        }

        public double NetBalance()
        {
            return sourceClientAccounts.NetBalance();
        }

        public double NetInterest(InterestRates interestRates)
        {
            double cumulativeBalance = NetBalance();
            if(cumulativeBalance > 0)
            {
                return interestRates.PositiveInterestRate()*cumulativeBalance/36500;
            }
            return interestRates.NegativeInterestRate()*cumulativeBalance/36500;
        }

        public Dictionary<Account, double> GetAllocation(InterestRates interestRates)
        {
            var interestAllocations = new Dictionary<Account, double >();

            allocations.ForEach(
                allocation =>
                interestAllocations.Add(allocation.GetAccount(),
                                        allocation.GetAllocationPercentage() * NetInterest(interestRates) / 100));


            return interestAllocations;
        }
    }
}
