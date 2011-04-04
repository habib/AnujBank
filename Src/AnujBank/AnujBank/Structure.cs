using System;
using TestAnujBank;

namespace AnujBank
{
    public class Structure
    {
        private readonly ClientAccounts sourceClientAccounts;

        public Structure(ClientAccounts sourceClientAccounts)
        {
            if(sourceClientAccounts.Count < 2) throw new ArgumentException("A structure must have at least 2 source accounts.");
            this.sourceClientAccounts = sourceClientAccounts;
        }

        public bool SharesASourceAccountWith(Structure newStructure)
        {
            return sourceClientAccounts.SharesAccountWith(newStructure.sourceClientAccounts);
        }

        public double NetBalance()
        {
            return sourceClientAccounts.NetBalance();
        }

        public double NetInterest(InterestRateConfigurationManager interestRateConfigurationManager)
        {
            double cumulativeBalance = NetBalance();
            if(cumulativeBalance > 0)
            {
                return interestRateConfigurationManager.PositiveInterestRate()*cumulativeBalance/36500;
            }
            return interestRateConfigurationManager.NegativeInterestRate()*cumulativeBalance/36500;
        }
    }
}
