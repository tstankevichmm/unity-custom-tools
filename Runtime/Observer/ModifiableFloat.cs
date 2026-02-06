using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Events;

namespace CustomTools.Observer
{
    public enum ModifiableNumberType
    {
        Flat = 100,
        PercentageAdditive = 200,
        IncreasePercentage = 300,
        PercentageMultiplicative = 400,
        Override = int.MaxValue
    }

    [Serializable]
    public class ModifiableFloat<T> : ModifiableNumber<T, float> where T : IModSource
    {
        public ModifiableFloat() : this(0, null) { }

        public ModifiableFloat(float baseValue, UnityAction<float> callback = null)
            : base(baseValue, callback)
        {
            
        }
    }

    [Serializable]
    public class ModifiableInt<T> : ModifiableNumber<T, int> where T : IModSource
    {
        public ModifiableInt() : this(0, null) { }

        public ModifiableInt(int baseValue, UnityAction<int> callback = null)
            : base(baseValue, callback)
        {
            
        }
    }

    
    [Serializable]
    public abstract class ModifiableNumber<T, U> : ObserverValue<U> where T : IModSource where U : struct, IComparable<U>, IConvertible
    {
        public override U Value
        {
            get
            {
                if (_isDirty)
                {
                    _value = CalculateTotal();
                    _isDirty = false;
                }

                return _value;
            }
            set => UpdateBaseValue(value);
        }

        public readonly ReadOnlyCollection<NumberModifier<double>> Modifiers;
        public U BaseValue => _baseValue;

        private bool _isDirty = true;
        private U _value;
        private readonly List<NumberModifier<double>> _modifiers;
        
        public ModifiableNumber() : this(default, null) { }
        
        public ModifiableNumber(U baseValue, UnityAction<U> callback = null) 
            : base(baseValue, callback)
        {
            _modifiers = new List<NumberModifier<double>>();
            Modifiers = _modifiers.AsReadOnly();
        }

        public void AddModifier(NumberModifier<double> mod)
        {
            _modifiers.Add(mod);
            _modifiers.Sort(CompareModifierOrder);
            _isDirty = true;
            Invoke();
        }

        public void AddModifierRange(IEnumerable<NumberModifier<double>> mods)
        {
            _modifiers.AddRange(mods);
            _modifiers.Sort(CompareModifierOrder);
            _isDirty = true;
            Invoke();
        }

        public bool RemoveModifier(NumberModifier<double> mod)
        {
            bool wasRemoved = _modifiers.Remove(mod);

            if (wasRemoved)
            {
                _isDirty = true;
                Invoke();
            }

            return wasRemoved;
        }

        public int RemoveModifierBySource(T modSource)
        {
            int amountRemoved = _modifiers.RemoveAll(mod => mod.Source.Equals(modSource));

            if (amountRemoved > 0)
            {
                _isDirty = true;
                Invoke();
            }

            return amountRemoved;
        }

        public int RemoveModifierByID(string id)
        {
            int amountRemoved = _modifiers.RemoveAll(mod => mod.ID == id);
            
            if (amountRemoved > 0)
            {
                _isDirty = true;
                Invoke();
            }

            return amountRemoved;
        }

        public void ClearAllModifiers()
        {
            if (_modifiers.Count == 0)
                return;
            
            _modifiers.Clear();
            _isDirty = true;
            Invoke();
        }

        public int GetModifierAmount(string id = "")
        {
            if (id == "")
                return _modifiers.Count;
            
            return _modifiers.FindAll(mod => mod.ID == id).Count;
        }

        public void UpdateBaseValue(U value)
        {
            _baseValue = value;
            _isDirty = true;
            Invoke();
        }

        private U CalculateTotal()
        {
            double finalTotal = Convert.ToDouble(_baseValue);
            double sumPercentAdd = 0;

            for(var i = 0; i < _modifiers.Count; i++)
            {
                NumberModifier<double> modifier = _modifiers[i];
                double modValue = modifier.Value;
                
                switch (modifier.Type)
                {
                    case ModifiableNumberType.Flat:
                        finalTotal += modValue;
                        break;
                    case ModifiableNumberType.PercentageAdditive:
                        sumPercentAdd += modValue;

                        if (i + 1 >= _modifiers.Count ||
                            _modifiers[i + 1].Type != ModifiableNumberType.PercentageAdditive)
                        {
                            finalTotal *= 1 + sumPercentAdd;
                            sumPercentAdd = 0;
                        }
                        break;
                    case ModifiableNumberType.IncreasePercentage:
                        finalTotal *= 1 + modValue;
                        break;
                    case ModifiableNumberType.PercentageMultiplicative:
                        finalTotal *= modValue;
                        break;
                    case ModifiableNumberType.Override:
                        finalTotal = modValue;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            return (U)Convert.ChangeType(finalTotal, typeof(U));
        }

        private static int CompareModifierOrder(NumberModifier<double> a, NumberModifier<double> b)
        {
            if (a.Order < b.Order)
                return -1;

            if (a.Order > b.Order)
                return 1;

            return 0;
        }
    }
    
    
    public abstract class NumberModifier<T> where T : struct, IComparable<T>, IConvertible
    {
        public string ID { get; }
        public T Value { get; }
        public ModifiableNumberType Type { get; }
        public int Order { get; }
        public IModSource Source { get; }
        
        public NumberModifier(string id, T value, ModifiableNumberType type, IModSource source, int? order = null)
        {
            ID = id;
            Value = value;
            Type = type;
            Source = source;
            Order = order ?? (int)type;
        }

        public virtual string GetName() => Source.GetName();

        public virtual string GetDescription()
        {
            return $"{Source.GetName()}: {Value}";
        }
    }
    
    public class FloatModifier : NumberModifier<float>
    {
        public FloatModifier(string id, float value, ModifiableNumberType type, IModSource source, int? order = null) 
            : base(id, value, type, source, order)
        {
            
        }
    }
    
    public class IntModifier : NumberModifier<int>
    {
        public IntModifier(string id, int value, ModifiableNumberType type, IModSource source, int? order = null) 
            : base(id, value, type, source, order)
        {
            
        }
    }

    public interface IModSource
    {
        public string GetName();
    }
}