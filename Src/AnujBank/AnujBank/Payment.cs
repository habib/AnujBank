﻿using System;

namespace AnujBank
{
    public class Payment
    {
        private readonly String account;
        private readonly decimal amount;
        private readonly DateTime date;

        public Payment(String account, decimal amount, DateTime date)
        {
            if(account==null) throw new ArgumentException("Account number is invalid");
            this.account = account;
            this.amount = amount;
            this.date = date;
        }

        public DateTime Date
        {
            get { return date; }
        }

        public decimal Amount
        {
            get { return amount; }
        }

        public string Account
        {
            get { return account; }
        }

        public String GetTransactionType()
        {
            if (amount > 0)
            {
                return "credit";
            }
            return "debit";
        }

        public bool Equals(Payment other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.amount == amount && Equals(other.account, account);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Payment)) return false;
            return Equals((Payment) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (amount.GetHashCode()*397) ^ account.GetHashCode();
            }
        }
    }
}