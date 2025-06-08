using System;
using System.Threading.Tasks;

namespace CustomTools.EventBusSystem
{
    public interface IEventBinding<T>
    {
        public string BindingID { get; }
        public int Priority { get;  }
        public Action<T> OnEvent { get;  }
        public Action OnEventNoArgs { get;  }
        public Func<T, Task> OnEventAsync { get;  }
    }
}