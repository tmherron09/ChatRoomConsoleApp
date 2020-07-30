using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Unsubscriber : IDisposable
    {
        private List<IObserver<Message>> observers;
        private IObserver<Message> observer;

        public Unsubscriber(List<IObserver<Message>> observers, IObserver<Message> observer)
        {
            this.observers = observers;
            this.observer = observer;
        }

        public void Dispose()
        {
            if (observer != null && observers.Contains(observer))
            {
                observers.Remove(observer);
            }
        }
    }
}
