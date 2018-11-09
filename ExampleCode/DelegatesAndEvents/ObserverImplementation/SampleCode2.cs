using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleCode.DelegatesAndEvents.ObserverImplementation
{
    //Subject
    public class StockTicker
    {
        public event EventHandler<StockChangeEventArgs> StockChange;

        private Stock _stock;
        public Stock Stock
        {
            get { return _stock;}
            set
            {
                _stock = value;
                OnStockChange(new StockChangeEventArgs(_stock));
            }
        }

        protected virtual void OnStockChange(StockChangeEventArgs e)
        {
            StockChange?.Invoke(this, e);
        }
    }



    public class StockChangeEventArgs : EventArgs
    {
        public Stock Stock { get; private set; }

        public StockChangeEventArgs(Stock stock)
        {
            Stock = stock;
        }
    }


    //Observer
    public class MicrosoftMinitor
    {
        public MicrosoftMinitor(StockTicker st)
        {
            st.StockChange += new EventHandler<StockChangeEventArgs>(st_StockChange);
        }

        private void st_StockChange(object sender, StockChangeEventArgs e)
        {
            CheckFilter(e.Stock);
        }

        private void CheckFilter(Stock stock)
        {
            if (stock.Symbol == "MSFT" && stock.Price > 10.00m)
            {
                Console.WriteLine($"Microsoft has reached the target price {stock.Price}");
            }
        }
    }

    //Observer
    public class GoogleMonitor
    {
        public GoogleMonitor(StockTicker st)
        {
            st.StockChange += new EventHandler<StockChangeEventArgs>(st_StockChange);
        }

        private void st_StockChange(object sender, StockChangeEventArgs e)
        {
            CheckFilter(e.Stock);
        }

        private void CheckFilter(Stock stock)
        {
            if (stock.Symbol == "GOOG")
            {
                Console.WriteLine($"Google's new price is: {stock.Price}");
            }
        }
    }
    
}
