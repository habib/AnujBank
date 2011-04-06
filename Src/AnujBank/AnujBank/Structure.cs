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
        private readonly PaymentInstructionService paymentInstructionService;

        public Structure(ClientAccounts sourceClientAccounts, List<Allocation> allocations, InterestRates interestRates, PaymentInstructionService paymentInstructionService)
        {
            if(sourceClientAccounts.Count < 2) throw new ArgumentException("A structure must have at least 2 source accounts.");
            
            float totalPercentage = 0;
            allocations.ForEach(al => totalPercentage += al.GetAllocationPercentage());

            if (totalPercentage != 100)
                throw new ArgumentException("A structure should have 100% allocation.");

            if (allocations.Count != 1 && allocations.Exists(al => !sourceClientAccounts.Contains(al.GetAccount())))
                    throw new ArgumentException("All allocations should be in pooled account or there should be only one allocation account.");

            this.sourceClientAccounts = sourceClientAccounts;
            this.allocations = allocations;
            this.interestRates = interestRates;
            this.paymentInstructionService = paymentInstructionService;
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

        public void GeneratePaymentInstruction()
        {
            Dictionary<Account, double> allocatedAmounts = GetAllocation();
            allocations.ForEach( al => 
                paymentInstructionService.Generate(new Payment(al.GetAccountNumber().ToString(),
                                                               new decimal(allocatedAmounts[al.GetAccount()]), 
                                                               DateTime.Now))
                );
        }
    }
}
