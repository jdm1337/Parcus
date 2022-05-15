using Parcus.Domain.Invest.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Services.Extensions
{
    public static class TransactionEnumExtenstions
    {
        public static Transactions GetTransactType(this Transactions transactionEnum, string stringType)
        {
            try
            {
                Transactions transactionType = (Transactions)Enum.Parse(typeof(Transactions), stringType);
                return transactionType;
            }
            catch (Exception ex)
            {
                return (Transactions)transactionEnum;
            }
        }
        
    }
}
