using System;
using System.Threading.Tasks;

namespace CustomTools.EventBusSystem
{
    public interface IEventBinding<T>
    {
        public string BindingID { get; }
        public int Priority { get; set; }
        public Action<T, Action> OnEventCallback { get; set; }
        public Action<T> OnEvent { get; set; }
        public Action OnEventNoArgs { get; set; }
        public Func<T, Task> OnEventAsync { get; set; }
    }
}