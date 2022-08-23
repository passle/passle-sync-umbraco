using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassleDotCom.PasslePlugin.Core.Interfaces
{ 
    public interface ICommandQueue
    {
        event EventHandler OnCommandAdded;

        void Push(IBackgroundCommand command);
        bool TryGet(out IBackgroundCommand command);
    }

    public class CommandQueue : ICommandQueue
    {
        private readonly ConcurrentQueue<IBackgroundCommand> _commandQueue;

        public CommandQueue()
        {
            _commandQueue = new ConcurrentQueue<IBackgroundCommand>();
        }

        public void Push(IBackgroundCommand command)
        {
            _commandQueue.Enqueue(command);
            TriggerOnCommandAdded();
        }

        public bool TryGet(out IBackgroundCommand command)
        {
            return _commandQueue.TryDequeue(out command);
        }

        public event EventHandler OnCommandAdded;
        private void TriggerOnCommandAdded()
        {
            var handler = OnCommandAdded;
            handler?.Invoke(this, new EventArgs());
        }
    }
}
