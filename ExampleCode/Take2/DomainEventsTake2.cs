using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace ExampleCode.Take2
{

    public interface IGame
    {
    }

    public interface ICart
    {
    }

    //New Api
    //It looks like we’ve managed to bring down the complexity of defining an event.
    public static class DomainEvents
    {
        public static readonly DomainEvent<IGame> GameReportedLost = new DomainEvent<IGame>();
        public static readonly DomainEvent<ICart> CartIsFull = new DomainEvent<ICart>();
    }


    //New Service Layer
    //The advantage of having a disposable domain event allows us to use the "using" construct for cleanup.
    //I also want to mention that you dont necessarily have to have the same service layer object handle these events as that which calls the domain objects. In
    //other words, we can have singleton objects handling these events for things like sending emails, notifying external systems and auditing.
    public class AddGameToCartMessageHandler // : BaseMessageHandler<AddGameToCartMessage>
    {
        public void Handle(object addMGameToCardMessagem)
        {
            //using (ISession session = SessionFactory.OpenSession())
            //using (ITransaction tx = session.BeginTransaction())
            using (DomainEvents.GameReportedLost.Register(gameReportedLost))
            using (DomainEvents.CartIsFull.Register(cartIsFull))
            {
                //ICart cart = session.Get<ICart>(m.CartId);
                //IGame g = session.Get<IGame>(m.GameId);
                //cart.Add(g);
                //tx.Commit();
            }
        }

        private Action<IGame> gameReportedLost = delegate
        {
            //Bus.Return((int)ErrorCodes.GameReportedLost);
            Console.WriteLine($"Inside gameReportedLost callback function");
        };

        private Action<ICart> cartIsFull = delegate
        {
            //Bus.Return((int)ErrorCodes.CartIsFull);
            Console.WriteLine($"Inside gameReportedLost callback function");
        };

    }





    //Notice that the involcation list of domain event is thread static, meaning that each thread gets
    //its own copy - even though they're all working with the same instance of the domain event.
    public class DomainEvent<E>
    {
        [ThreadStatic]
        private static List<Action<E>> _actions;

        protected List<Action<E>> actions
        {
            get
            {
                if(_actions == null)
                    _actions = new List<Action<E>>();

                return _actions;
            }
        }

        public IDisposable Register(Action<E> callback)
        {
            actions.Add(callback);

            return new DomainEventRegistrationRemover(delegate
            {
                actions.Remove(callback);
            });
        }

        public void Raise(E args)
        {
            foreach (Action<E> action in actions)
            {
                action.Invoke(args);
            }
        }

    }


    public class DomainEventRegistrationRemover : IDisposable
    {
        private readonly Action CallOnDispose;

        public DomainEventRegistrationRemover(Action toCall)
        {
            this.CallOnDispose = toCall;
        }

        public void Dispose()
        {
            this.CallOnDispose.DynamicInvoke();
        }
    }
}
