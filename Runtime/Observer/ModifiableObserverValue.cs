using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace CustomTools.Observer
{
    
    [Serializable]
    public abstract class ModifiableObserverValue<T, U> : ObserverValue<T> where U : IModifierSource
    {
        public override T Value
        {
            get
            {
                T finalTotal = _baseValue;

                foreach (Modifier<T, U> modifier in _modifiers)
                {
                    finalTotal = AddToValue(finalTotal, modifier.amount);
                }
                
                return finalTotal;
            }
            set => UpdateBaseValue(value);
        }

        public T BaseValue => _baseValue;
        public List<Modifier<T, U>> Modifiers => _modifiers;

        private List<Modifier<T, U>> _modifiers = new();

        protected ModifiableObserverValue(T baseValue, UnityAction<T> callback = null) : base(baseValue, callback)
        {
            
        }

        protected abstract T AddToValue(T original, T amount);
        
        public void UpdateBaseValue(T newBaseValue)
        {
            _baseValue = newBaseValue;
        }

        public void AddModifier(Modifier<T, U> modifier)
        {
            _modifiers.Add(modifier);
        }

        public void RemoveModifier(Modifier<T, U> modifier)
        {
            _modifiers.Remove(modifier);
        }
    }
    
    public struct Modifier<T, U> where U : IModifierSource
    {
        public string id;
        public T amount;
        public U source;
    }

    public interface IModifierSource
    {
        
    }
    
    [Serializable]
    public class BaseModifiableFloat : ModifiableObserverValue<float, IModifierSource>
    {
        public BaseModifiableFloat(float baseValue, UnityAction<float> callback = null) : base(baseValue, callback)
        {
        }

        protected override float AddToValue(float original, float amount)
        {
            return original + amount;
        }
    }
    
    [Serializable]
    public class BaseModifiableInt : ModifiableObserverValue<int, IModifierSource>
    {
        public BaseModifiableInt(int baseValue, UnityAction<int> callback = null) : base(baseValue, callback)
        {
        }

        protected override int AddToValue(int original, int amount)
        {
            return original + amount;
        }
    }
}