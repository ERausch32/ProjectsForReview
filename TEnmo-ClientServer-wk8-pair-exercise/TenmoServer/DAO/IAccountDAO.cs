using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDAO
    {
        Account GetAccount(int userId);
        List<Member> GetMembers();
        Transfer CompleteTransfer(Transfer transfer);
        List<Transfer> GetAccountTransfers(Account account);
        Transfer GetSpecificTransfer(int transferId);
        void DenyRequest(Transfer transfer);
        void ApproveRequest(Transfer transfer);
        Transfer InitializeRequest(Transfer transfer);
    }
}

