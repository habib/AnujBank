﻿using System;

namespace AnujBank
{
    public class Account
    {
        public Account(AccountId id,ClientID clientId)
        {
            if (clientId == null)
                throw new NoClientException("You must provide client"); 
            AccountNo = id;
            ClientId = clientId;
        }
        public AccountId AccountNo { get; private set; }
        public ClientID ClientId { get; private set; }
        public double Balance { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        public int GetAccountNumber()
        {
            return AccountNo.Id;
        }
        public string GetClientId()
        {
            return ClientId.Id;
        }
    }

    public class NoClientException : Exception
    {
        public NoClientException(string message) : base(message)
        {
            
        }
    }

    
    
    public class ClientID
    {
      
        public ClientID(string id)
        {
            Id = id;
         }

        public string Id { get; set; }
     
    }
    public class AccountId
    {

        public AccountId(int id)
        {
            Id = id;
        }
        public int Id { get; set; }

    }
    
    

}
