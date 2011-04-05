namespace AnujBank
{
    public class Allocation
    {
        private readonly Account account;    
        
        private readonly float allocatedPercentage;

        public Allocation(Account account, float allocatedPercentage)
        {
            this.account = account;
            this.allocatedPercentage = allocatedPercentage;
        }

        public int GetAccountNumber()
        {
            return account.GetAccountNumber();
        }

        public Account GetAccount()
        {
            return account;
        }

        public double GetAmount()
        {
            return account.Balance;
        }
        
        public float GetAllocationPercentage()
        {
            return allocatedPercentage;
        }
    }
}