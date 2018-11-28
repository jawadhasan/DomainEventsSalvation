using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleCode.Facade
{
    class Customer
    {
        public string Name { get; }

        public Customer(string name)
        {
            Name = name;
        }
    }


    //SubSystems: (Bank, Credit, Loan)
    //implement subsystem functionality.
    //handle work assigned by the Facade object.
    //have no knowledge of the facade and keep no reference to it.
    class Loan
    {
        public bool HasNoBadLoans(Customer customer)
        {
            Console.WriteLine($"Check loans for {customer.Name}");
            return true;
        }
    }

    class Credit
    {
        public bool HasGoodCredit(Customer customer)
        {
            Console.WriteLine($"Check credit for {customer.Name}");
            return true;
        }
    }

    class Bank
    {
        public bool HasSufficientSavings(Customer customer, int amount)
        {
            Console.WriteLine($"Check bank for {customer.Name}");
            return true;
        }
    }



    //facade: (MortgageApplication)
    //knows which subsystem classes are responsible for a request.
    //delegates client requests to appropriate subsystem objects.
    class Mortgage
    {
        private readonly Bank _bank = new Bank();
        private readonly Loan _loan = new Loan();
        private readonly Credit _credit = new Credit();

        public bool IsEligible(Customer customer, int amount)
        {
            Console.WriteLine($"{customer.Name} applies for {amount:C} loan");
            var eligible = true;

            //Check creditworthyness of applicant
            if (!_bank.HasSufficientSavings(customer, amount))
            {
                eligible = false;
            }else if (!_loan.HasNoBadLoans(customer))
            {
                eligible = false;
            }
            else if(!_credit.HasGoodCredit(customer))
            {
                eligible = false;
            }

            return eligible;
        }
    }






    public class FacadeDemo
    {
        public void Run()
        {
           var mortgage = new Mortgage();

            //Evaluate mortgage eligibility for customer
            var customer = new Customer("Ann Mckinsey");
            var eligible = mortgage.IsEligible(customer, 125000);
            var result = eligible ? "Approved" : "Rejected";

            Console.WriteLine($"{customer.Name} has been {result}");
        }
    }
}
