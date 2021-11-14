using System;
using System.Collections.Generic;
using TenmoClient.APIClients;
using TenmoClient.APIClients;
using TenmoClient.Data;
using System.Linq;

namespace TenmoClient
{
    public class UserInterface
    {
        private readonly ConsoleService consoleService = new ConsoleService();
        private readonly AuthService authService = new AuthService();

        private bool quitRequested = false;

        public void Start()
        {
            while (!quitRequested)
            {
                while (!UserService.IsLoggedIn)
                {
                    ShowLogInMenu();
                }

                // If we got here, then the user is logged in. Go ahead and show the main menu
                ShowMainMenu();
            }
        }

        private void ShowLogInMenu()
        {
            Console.WriteLine("Welcome to TEnmo!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.Write("Please choose an option: ");

            if (!int.TryParse(Console.ReadLine(), out int loginRegister))
                Console.WriteLine("Invalid input. Please enter only a number.");
            else if (loginRegister == 1)
                HandleUserLogin();
            else if (loginRegister == 2)
                HandleUserRegister();
            else
                Console.WriteLine("Invalid selection.");
        }

        private void ShowMainMenu()
        {
            AccountService accountService = new AccountService();

            int menuSelection;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests");
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                    Console.WriteLine("Invalid input. Please enter only a number.");
                else
                {
                    switch (menuSelection)
                    {
                        case 1: // View Balance
                            ViewBalance(accountService);
                            break;

                        case 2: // View Past Transfers
                            ViewTransfers(accountService);
                            break;

                        case 3: // View Pending Requests
                            ViewPendingRequests(accountService);
                            break;

                        // Case 4 & 5 both call InitializeTransfer using bool
                        // to flag a Send vs a Request.
                        case 4: // Send TE Bucks
                            InitializeTransfer(accountService, true);
                            break;

                        case 5: // Request TE Bucks
                            InitializeTransfer(accountService, false);
                            break;

                        case 6: // Log in as someone else
                            Console.WriteLine();
                            UserService.ClearLoggedInUser(); //wipe out previous login info
                            return; // Leaves the menu and should return as someone else

                        case 0: // Quit
                            Console.WriteLine("Goodbye!");
                            quitRequested = true;
                            return;

                        default:
                            Console.WriteLine("That doesn't seem like a valid choice.");
                            break;
                    }
                }
            } while (menuSelection != 0);
        }

        /// <summary>
        /// Display a list of pending transfer requests to this user
        /// Allow user to select a request and then approve/reject.
        /// </summary>
        public void ViewPendingRequests(AccountService accountService)
        {
            // Display a list of pending transfer requests to this user.
            IEnumerable<API_Transfer> pendingTransfers =
                accountService.GetTransfers().Where(t => t.Status == "Pending" && t.Sender.UserId == UserService.UserId);
            bool isValidSelection = false;
            int transferSelection = -1;
            Console.WriteLine();
            if (pendingTransfers.Count() == 0)
            {
                Console.WriteLine("No pending transfers exist for approval.");
                Console.Write("Enter to continue");
                Console.ReadLine();
                return;
            }
            while (!isValidSelection)
            {
                foreach (API_Transfer pendingTransfer in pendingTransfers)
                {
                    Console.WriteLine(pendingTransfer.Gist());
                }
                // Allow user to select a pending request to approve/reject
                Console.WriteLine();
                Console.Write("Please enter the Transfer ID to approve/reject (0 to cancel): ");
                bool validInt = int.TryParse(Console.ReadLine(), out transferSelection);
                if (validInt && transferSelection == 0)
                    return;
                if (!validInt || !pendingTransfers.Any(t => t.TransferId == transferSelection))
                {
                    Console.WriteLine();
                    Console.WriteLine("Sorry, that transfer ID is not recognized.");
                    Console.WriteLine();
                    continue;
                }
                isValidSelection = true;
            }

            // Make decision on chosen request
            API_Transfer transfer = pendingTransfers.First(t => t.TransferId == transferSelection);
            isValidSelection = false;
            int actionSelection = -1;
            while (!isValidSelection)
            {
                Console.WriteLine($"\n1: Approve \n2: Reject \n0: Cancel");
                Console.WriteLine(new string('-', 10));
                Console.Write("Please select an option: ");
                bool validInt = int.TryParse(Console.ReadLine(), out actionSelection);
                if (validInt && actionSelection == 0)
                    return;
                if (validInt && actionSelection == 1)
                {
                    isValidSelection = true;
                    transfer.Status = "Approved";
                    accountService.FinalizePendingTransfer(transfer);
                    return;
                }
                if (validInt && actionSelection == 2)
                {
                    isValidSelection = true;
                    transfer.Status = "Rejected";
                    accountService.FinalizePendingTransfer(transfer);
                    return;
                }
                Console.WriteLine();
                Console.WriteLine("Please select a valid option.");
            }
        }

        /// <summary>
        /// View balance for current user account
        /// </summary>
        /// <param name="service"></param>
        private void ViewBalance(AccountService service)
        {
            API_Account account = service.GetAccount();
            Console.WriteLine();
            Console.WriteLine($"Your current account balqance is: {account.Balance.ToString("C")}");
            Console.Write("Enter to continue");
            Console.ReadLine();
        }

        /// <summary>
        /// View a list of all transfers for this user
        /// </summary>
        private static void ViewTransfers(AccountService accountService)
        {
            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine(new string('-', 40));
            Console.WriteLine("Transfers");
            Console.WriteLine("ID         From/To                Amount");
            Console.WriteLine(new string('-', 40));


            List<API_Transfer> transfers = accountService.GetTransfers();
            int transferSelection = -1;
            bool isValidTransfer = false;
            while (!isValidTransfer)
            {
                foreach (API_Transfer transfer in transfers)
                {
                    Console.WriteLine(transfer.Gist());
                }
                Console.WriteLine(new string('-', 10));
                Console.WriteLine();
                Console.Write("Please enter transfer ID to view details (0 to cancel):");
                bool isInteger = int.TryParse(Console.ReadLine(), out transferSelection);
                if (isInteger && transferSelection == 0) // user chose to cancel
                    return;
                if (!isInteger || !transfers.Any(t => t.TransferId == transferSelection))
                {
                    Console.WriteLine("Not a valid Transfer ID.");
                    continue;
                }
                isValidTransfer = true;
            }
            API_Transfer selectedTransfer = transfers.Find(t => t.TransferId == transferSelection);
            Console.WriteLine(selectedTransfer);
            Console.ReadLine();
        }

        /// <summary>
        /// View a list of all members except user. Begin to build transfer request
        /// Add user as Sending member & selected member as Recipient member
        /// </summary>
        public API_Transfer SelectMember(AccountService service, bool isSend)
        {
            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine(new string('-', 40));
            Console.WriteLine("Account");
            Console.WriteLine("ID          Name");
            Console.WriteLine(new string('-', 40));

            List<API_Member> members = service.GetMembers();
            API_Transfer transfer = new API_Transfer();

            int selection = -1;
            API_Member userMember = new API_Member();
            bool isValidAccountId = false;
            while (!isValidAccountId)
            {
                foreach (API_Member member in members)
                {
                    if (member.UserId == UserService.UserId)
                        userMember = member;
                    else
                    {
                        Console.Write(member.AccountId.ToString().PadRight(12));
                        Console.WriteLine(member.Username);
                    }
                }
                Console.WriteLine(new string('-', 10));
                Console.Write("Enter ID of the account to transact with (0 to cancel): ");

                bool isInteger = int.TryParse(Console.ReadLine(), out selection);
                bool isAccountId = members.Any(m => m.AccountId == selection);
                bool isUser = userMember.AccountId == selection;
                if (isInteger && selection == 0) // User chose to cancel
                    return null;
                if (isInteger && isAccountId && !isUser)
                    isValidAccountId = true;
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Please input a valid account ID.");
                }
            }
            if (isSend)
            {
                transfer.Sender = userMember;
                transfer.Recipient = members.Find(m => m.AccountId == selection);
            }
            else
            {
                transfer.Sender = members.Find(m => m.AccountId == selection);
                transfer.Recipient = userMember;
            }
            return transfer;
        }

        /// <summary>
        /// Continue building transfer request by getting the amount to transfer
        /// </summary>
        private API_Transfer GetAmount(API_Transfer partialTransfer)
        {
            decimal amount = -1;
            bool isValidDecimal = false;
            while (!isValidDecimal)
            {
                Console.Write($"Enter amount to transfer (0 to cancel): ");
                bool isDecimal = decimal.TryParse(Console.ReadLine(), out amount);
                if (!isDecimal || amount < 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Transfer amount must be a valid amount.");
                    continue;
                }
                isValidDecimal = true;
            }
            partialTransfer.Amount = amount;
            return partialTransfer;
        }

        /// <summary>
        /// Initiate, validate, and submit a transfer request
        /// </summary>
        private void InitializeTransfer(AccountService service, bool isSend)
        {
            API_Transfer partialTransfer = SelectMember(service, isSend);
            if (partialTransfer == null) // User chose to cancel
                return;
            API_Transfer transferRequest = GetAmount(partialTransfer);
            if (transferRequest.Amount == 0) // user chose to cancel
                return;
            if (isSend)
            {
                transferRequest.Type = "Send";
                transferRequest.Status = "Approved";
            }
            else
            {
                transferRequest.Type = "Request";
                transferRequest.Status = "Pending";
            }
            API_Transfer completeTransfer = service.SubmitTransfer(transferRequest);
            if (completeTransfer == null)
            {
                Console.WriteLine("Failed to complete.");
                return;
            }
            Console.WriteLine();
            Console.WriteLine("Transfer Submitted");
            Console.WriteLine($"Transfer: id: {completeTransfer.TransferId}, recipient: {completeTransfer.Recipient.Username}, amount: {completeTransfer.Amount}");
            Console.WriteLine(new string('-', 10));
            Console.Write("Enter to continue");
            Console.ReadLine();
            return;
        }

        private void HandleUserRegister()
        {
            bool isRegistered = false;

            while (!isRegistered) //will keep looping until user is registered
            {
                LoginUser registerUser = consoleService.PromptForLogin();
                isRegistered = authService.Register(registerUser);
            }

            Console.WriteLine("");
            Console.WriteLine("Registration successful. You can now log in.");
        }

        private void HandleUserLogin()
        {
            while (!UserService.IsLoggedIn) //will keep looping until user is logged in
            {
                LoginUser loginUser = consoleService.PromptForLogin();
                authService.Login(loginUser);
            }
        }
    }
}
