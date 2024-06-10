using System;
using System.Collections.Generic;
using CustomTools.Observer;
using UnityEngine.Events;

namespace CustomTools.ModifiableValue
{
    [Serializable]
    public abstract class ValueWithMod<T, U> where U : class
    {
        public List<BaseModifier<T, U>> Modifiers { get; private set; }

        protected readonly ObserverValue<T> totalAmount;
        protected T baseValue;

        protected ValueWithMod(T baseValue = default)
        {
            totalAmount = new ObserverValue<T>(this.baseValue);
            Modifiers = new List<BaseModifier<T, U>>();
            SetBaseValue(baseValue);
        }

        public T GetBaseValue()
        {
            return baseValue;
        }

        public T GetTotalModValue()
        {
            T newValue = default;

            foreach (BaseModifier<T, U> modifier in Modifiers)
            {
                newValue = Add(newValue, modifier.Modifier.ModObserver.Value);
            }

            return newValue;
        }

        public void AddListener(UnityAction<T> callback) => totalAmount.AddListener(callback);
        public void RemoveListener(UnityAction<T> callback) => totalAmount.RemoveListener(callback);

        public void SetBaseValue(T baseValueToSet)
        {
            this.baseValue = baseValueToSet;
            UpdateAmount();
        }

        protected abstract T Add(object a, object b);

        private void UpdateAmount()
        {
            T newValue = Add(baseValue, GetTotalModValue());
            totalAmount.Value = newValue;
        }

        public bool HasModifier(BaseModifier<T, U> mod)
        {
            return Modifiers.Contains(mod);
        }

        public void AddModifier(BaseModifier<T, U> mod, bool allowStacking = false)
        {
            if (HasModifier(mod) && !allowStacking)
            {
                return;
            }

            Modifiers.Add(mod);
            Modifiers.Sort(BaseModifier<T, U>.CompareModifierOrder);
            UpdateAmount();
        }

        public void RemoveModifier(BaseModifier<T, U> mod)
        {
            Modifiers.Remove(mod);
            UpdateAmount();
        }

        public void RemoveAllModifiersFromSource(U source)
        {
            for (int i = Modifiers.Count - 1; i >= 0; i--)
            {
                if (Modifiers[i].Source != source)
                    continue;

                Modifiers.RemoveAt(i);
            }

            UpdateAmount();
        }

        public override string ToString()
        {
            return totalAmount.ToString();
        }

        protected void CopyModifiers(List<BaseModifier<T, U>> modifiers)
        {
            foreach (BaseModifier<T, U> modifier in modifiers)
            {
                AddModifier(modifier);
            }
        }
    }


    [Serializable]
    public class IntWithMods<T> : ValueWithMod<int, T> where T : class
    {
        public IntWithMods(int baseValue) : base(baseValue) { }

        public static implicit operator int(IntWithMods<T> mod)
        {
            return mod.totalAmount;
        }

        protected override int Add(object a, object b)
        {
            return (int)a + (int)b;
        }

        public IntWithMods<T> Clone()
        {
            IntWithMods<T> clone = new IntWithMods<T>(baseValue);

            clone.CopyModifiers(Modifiers);

            return clone;
        }
    }

    public class FloatWithMods<T> : ValueWithMod<float, T> where T : class
    {
        public FloatWithMods(float baseValue) : base(baseValue) { }

        public static implicit operator float(FloatWithMods<T> mod)
        {
            return mod.totalAmount;
        }

        protected override float Add(object a, object b)
        {
            return (float)a + (float)b;
        }

        public FloatWithMods<T> Clone()
        {
            FloatWithMods<T> clone = new FloatWithMods<T>(baseValue);
            clone.CopyModifiers(Modifiers);
            return clone;
        }
    }
}