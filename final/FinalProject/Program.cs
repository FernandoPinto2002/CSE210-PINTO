using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonalFinanceTracker
{
    abstract class Transaction
    {
        public string Date { get; set; }
        public double Amount { get; set; }
        public string Category { get; set; }

        public Transaction(string date, double amount, string category)
        {
            Date = date;
            Amount = amount;
            Category = category;
        }

        public abstract string GetTransactionDetails();
    }


    class IncomeTransaction : Transaction
    {
        public IncomeTransaction(string date, double amount, string category)
            : base(date, amount, category) { }

        public override string GetTransactionDetails()
        {
            return $"Income | Date: {Date}, Amount: {Amount:C}, Category: {Category}";
        }
    }
    class ExpenseTransaction : Transaction
    {
        public ExpenseTransaction(string date, double amount, string category)
            : base(date, amount, category) { }

        public override string GetTransactionDetails()
        {
            return $"Expense | Date: {Date}, Amount: {Amount:C}, Category: {Category}";
        }
    }

    class Budget
    {
        public string Category { get; set; }
        public double MonthlyLimit { get; set; }

        public Budget(string category, double limit)
        {
            Category = category;
            MonthlyLimit = limit;
        }
    }

    class Report
    {
        public void GenerateSummary(List<Transaction> transactions, Dictionary<string, Budget> budgets)
        {
            Console.WriteLine("\n--- Financial Report ---");
            var groupedTransactions = transactions
                .GroupBy(t => t.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(t => t.Amount) });

            foreach (var group in groupedTransactions)
            {
                Console.WriteLine($"Category: {group.Category}, Total: {group.Total:C}");
            }

            foreach (var budget in budgets)
            {
                var spent = groupedTransactions.FirstOrDefault(g => g.Category == budget.Key)?.Total ?? 0;
                if (spent > budget.Value.MonthlyLimit)
                {
                    Console.WriteLine($"WARNING: Over budget in {budget.Key}! Spent: {spent:C}, Limit: {budget.Value.MonthlyLimit:C}");
                }
            }
        }
    }

    class FinanceTracker
    {
        private List<Transaction> transactions = new List<Transaction>();
        private Dictionary<string, Budget> budgets = new Dictionary<string, Budget>();
        private Report report = new Report();

        public void AddTransaction(Transaction transaction)
        {
            transactions.Add(transaction);
            Console.WriteLine("Transaction added successfully!");
        }

        public void SetBudget(string category, double limit)
        {
            if (budgets.ContainsKey(category))
            {
                budgets[category].MonthlyLimit = limit;
            }
            else
            {
                budgets[category] = new Budget(category, limit);
            }
            Console.WriteLine($"Budget set for {category}: {limit:C}");
        }

        public void GenerateReport()
        {
            report.GenerateSummary(transactions, budgets);
        }

        public void DisplayAllTransactions()
        {
            Console.WriteLine("\n--- All Transactions ---");
            if (transactions.Count == 0)
            {
                Console.WriteLine("No transactions recorded.");
                return;
            }

            foreach (var transaction in transactions)
            {
                Console.WriteLine(transaction.GetTransactionDetails());
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            FinanceTracker tracker = new FinanceTracker();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n--- Personal Finance Tracker ---");
                Console.WriteLine("1. Add Income");
                Console.WriteLine("2. Add Expense");
                Console.WriteLine("3. Set Budget");
                Console.WriteLine("4. Generate Report");
                Console.WriteLine("5. View All Transactions");
                Console.WriteLine("6. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter date (YYYY-MM-DD): ");
                        string incomeDate = Console.ReadLine();
                        Console.Write("Enter amount: ");
                        double incomeAmount = Convert.ToDouble(Console.ReadLine());
                        Console.Write("Enter category: ");
                        string incomeCategory = Console.ReadLine();

                        tracker.AddTransaction(new IncomeTransaction(incomeDate, incomeAmount, incomeCategory));
                        break;

                    case "2":
                        Console.Write("Enter date (YYYY-MM-DD): ");
                        string expenseDate = Console.ReadLine();
                        Console.Write("Enter amount: ");
                        double expenseAmount = Convert.ToDouble(Console.ReadLine());
                        Console.Write("Enter category: ");
                        string expenseCategory = Console.ReadLine();

                        tracker.AddTransaction(new ExpenseTransaction(expenseDate, expenseAmount, expenseCategory));
                        break;

                    case "3":
                        Console.Write("Enter category: ");
                        string budgetCategory = Console.ReadLine();
                        Console.Write("Enter monthly limit: ");
                        double budgetLimit = Convert.ToDouble(Console.ReadLine());

                        tracker.SetBudget(budgetCategory, budgetLimit);
                        break;

                    case "4":
                        tracker.GenerateReport();
                        break;

                    case "5":
                        tracker.DisplayAllTransactions();
                        break;

                    case "6":
                        exit = true;
                        Console.WriteLine("Exiting... Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}