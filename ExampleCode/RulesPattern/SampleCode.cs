using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleCode.RulesPattern
{

    public class Customer
    {
        public DateTime? DateOfFirstPurchase { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsVeteran { get; set; }
    }

    public static class CustomerExtensions
    {

        public static bool HasBeenLoyalForYears(this Customer customer, int numberOfYears, DateTime? date = null)
        {
            if (!customer.IsExisting())
                return false;

            numberOfYears = -1 * numberOfYears;
            return customer.DateOfFirstPurchase.Value < date.ToValueOrDefault().AddYears(numberOfYears);
        }


        public static bool IsExisting(this Customer customer)
        {
            return customer.DateOfFirstPurchase.HasValue;
        }

        public static bool IsSenior(this Customer customer, DateTime? date = null)
        {
            return customer.DateOfBirth < date.ToValueOrDefault().AddYears(-65);
        }

        public static bool IsBirthday(this Customer customer, DateTime? date = null)
        {
            date = date.ToValueOrDefault();
            return customer.DateOfBirth.Day == date.Value.Day && customer.DateOfBirth.Month == date.Value.Month;
        }
    }
    public static class DateTimeExtensions
    {
        public static DateTime ToValueOrDefault(this DateTime? dateTime, DateTime? defaultValue = null)
        {
            defaultValue = defaultValue.HasValue ? defaultValue.Value : DateTime.Now;
            return dateTime.HasValue ? dateTime.Value : defaultValue.Value;
        }
    }




    //The rules implement a simple interface
    public interface IDiscountRule
    {
        decimal CalculateCustomerDiscount(Customer customer);
    }

    //and the rules implementations just have a single responsibility that could be as simple or complex as necessary:
    public class BirthdayDiscountRule : IDiscountRule
    {
        public decimal CalculateCustomerDiscount(Customer customer)
        {
            return customer.IsBirthday() ? 0.10m : 0;
        }
    }

    public class NewCustomerRule : IDiscountRule
    {
        public decimal CalculateCustomerDiscount(Customer customer)
        {
            return !customer.IsExisting() ? 0.15m : 0;
        }
    }

    public class SeniorDiscountRule : IDiscountRule
    {
        public decimal CalculateCustomerDiscount(Customer customer)
        {
            return customer.IsSenior() ? 0.05m : 0;
        }
    }


    public class VeteranDiscountRule : IDiscountRule
    {
        public decimal CalculateCustomerDiscount(Customer customer)
        {
            return customer.IsVeteran ? 0.10m : 0;
        }
    }


    //And you can even reuse rules in other rules, such as the BirthdayDiscountRule being used in the LoyalCustomerRule here:
    public class LoyalCustomerRule : IDiscountRule
    {

        private readonly int _yearsAsCustomer;
        private readonly decimal _discount;
        private readonly DateTime _date;

        public LoyalCustomerRule(int yearsAsCustomer, decimal discount, DateTime? date = null)
        {
            _yearsAsCustomer = yearsAsCustomer;
            _discount = discount;
            _date = date.ToValueOrDefault();
        }
        public decimal CalculateCustomerDiscount(Customer customer)
        {
            if (customer.HasBeenLoyalForYears(_yearsAsCustomer, _date))
            {
                var birthdayRule = new BirthdayDiscountRule();
                return _discount + birthdayRule.CalculateCustomerDiscount(customer);
            }

            return 0;
        }
    }





    //Evaluator
    public interface IDiscountCalculator
    {
        decimal CalculateDiscountPercentage(Customer customer);
    }


    //It holds a collection of rules that calculate discounts and loops through them to find the greatest discount.
    //Rules are just added to the collection manually here for illustrative purposes, but in a real application you would more likely
    //load them dynamically with an IoC container or something similar without having to change RulesDiscountCalculator.
    public class RulesDiscountCalculator : IDiscountCalculator
    {
        private readonly List<IDiscountRule> _rules = new List<IDiscountRule>();

        public RulesDiscountCalculator()
        {
            _rules.Add(new BirthdayDiscountRule());
            _rules.Add(new SeniorDiscountRule());
            _rules.Add(new VeteranDiscountRule());
            _rules.Add(new LoyalCustomerRule(1, 0.10m));
            _rules.Add(new LoyalCustomerRule(5, 0.12m));
            _rules.Add(new LoyalCustomerRule(10, 0.20m));
            _rules.Add(new NewCustomerRule());

        }

        public decimal CalculateDiscountPercentage(Customer customer)
        {
            decimal discount = 0;
            foreach (var rule in _rules)
            {
                discount = Math.Max(rule.CalculateCustomerDiscount(customer), discount);
            }
            return discount;
        }
    }


    public class RulesDemo
    {
        public void Run()
        {
           // https://github.com/mwhelan/Blog_RulesPattern/blob/master/src/RulesPattern.Tests/StoreExample/DiscountCalculatorBaseTests.cs
        }
    }
}
