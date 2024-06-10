using System;
using CustomTools.Observer;

namespace CustomTools.ModifiableValue
{
    public interface IModifierValue<T>
    {
        public ObserverValue<T> ModObserver { get; }
    }

    [Serializable]
    public struct BaseModifierValue<T> : IModifierValue<T>
    {
        public ObserverValue<T> ModObserver { get; private set; }

        public BaseModifierValue(T value)
        {
            ModObserver = new ObserverValue<T>(value);
        }
    }
        
    public enum ModType
    {
        Flat = 100,
        Percentage = 200
    }

    [Serializable]
    public class BaseModifier<U, T> where T : class
    {
        public int Order { get; private set; }
        public IModifierValue<U> Modifier { get; private set; }
        public ModType Type { get; private set; }
        public T Source { get; private set; }
            
        public BaseModifier(IModifierValue<U> modifier, ModType type, T source = null) 
            : this(modifier, type, (int)type, source) { }
            
        public BaseModifier(IModifierValue<U> modifier, ModType type, int order, T source = null)
        {
            Modifier = modifier;
            Type = type;
            Order = order;
            Source = source;
        }
            
        public BaseModifier<U, T> Clone()
        {
            return new BaseModifier<U, T>(Modifier, Type, Order, Source);
        }
            
        public static int CompareModifierOrder(BaseModifier<U, T> a, BaseModifier<U, T> b)
        {
            return a.Order.CompareTo(b.Order);
        }
    }
}