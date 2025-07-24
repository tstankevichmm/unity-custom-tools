using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Events;

namespace CustomTools.Observer
{
    public enum ModifiableFloatType
    {
        Flat = 100,
        PercentageAdditive = 200,
        IncreasePercentage = 300,
        PercentageMultiplicative = 400,
        Override = int.MaxValue
    }
    
    [Serializable]
    public class ModifiableFloat<T> : ObserverValue<float>
    {
        public override float Value
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

        public readonly ReadOnlyCollection<FloatModifier> Modifiers;
        public float BaseValue => _baseValue;

        private bool _isDirty = true;
        private float _value;
        private readonly List<FloatModifier> _modifiers;
        
        public ModifiableFloat() : this(0, null) { }
        
        public ModifiableFloat(float baseValue, UnityAction<float> callback = null) 
            : base(baseValue, callback)
        {
            _modifiers = new List<FloatModifier>();
            Modifiers = _modifiers.AsReadOnly();
        }

        public void AddModifier(FloatModifier mod)
        {
            _modifiers.Add(mod);
            _modifiers.Sort(CompareModifierOrder);
            _isDirty = true;
            Invoke();
        }

        public bool RemoveModifier(FloatModifier mod)
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

        public int GetModifierAmount(string id = "")
        {
            if (id == "")
                return _modifiers.Count;
            
            return _modifiers.FindAll(mod => mod.ID == id).Count;
        }

        public void UpdateBaseValue(float value)
        {
            _baseValue = value;
            _isDirty = true;
            Invoke();
        }

        private float CalculateTotal()
        {
            float finalTotal = _baseValue;
            float sumPercentAdd = 0;

            for(var i = 0; i < _modifiers.Count; i++)
            {
                FloatModifier modifier = _modifiers[i];
                
                switch (modifier.Type)
                {
                    case ModifiableFloatType.Flat:
                        finalTotal += modifier.Value;
                        break;
                    case ModifiableFloatType.PercentageAdditive:
                        sumPercentAdd += modifier.Value;

                        if (i + 1 >= _modifiers.Count ||
                            _modifiers[i + 1].Type != ModifiableFloatType.PercentageAdditive)
                        {
                            finalTotal *= 1 + sumPercentAdd;
                            sumPercentAdd = 0;
                        }
                        break;
                    case ModifiableFloatType.IncreasePercentage:
                        finalTotal *= 1 + modifier.Value;
                        break;
                    case ModifiableFloatType.PercentageMultiplicative:
                        finalTotal *= modifier.Value;
                        break;
                    case ModifiableFloatType.Override:
                        finalTotal = modifier.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            return finalTotal;
        }

        private static int CompareModifierOrder(FloatModifier a, FloatModifier b)
        {
            if (a.Order < b.Order)
                return -1;

            if (a.Order > b.Order)
                return 1;

            return 0;
        }
    }
    
    public class FloatModifier
    {
        public readonly string ID;
        public readonly float Value;
        public readonly ModifiableFloatType Type;
        public readonly int Order;
        public readonly IModSource Source;
        
        public FloatModifier(string id, float value, ModifiableFloatType type, IModSource source, int order)
        {
            ID = id;
            Value = value;
            Type = type;
            Source = source;
            Order = order;
        }

        public FloatModifier(string id, float value, ModifiableFloatType type, IModSource source)
            : this(id, value, type, source, (int)type){ }
    }

    public interface IModSource
    {
        public string GetName();
    }
}