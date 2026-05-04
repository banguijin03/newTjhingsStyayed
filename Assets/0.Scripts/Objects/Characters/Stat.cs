using UnityEngine;

public class Stat
{
        public FillValue Value;

        public string Name; 
        public float Percent => Value.Percent;

        public Stat(string name, int current, int max)
        {
            Name = name;
            Value = new FillValue(current, max);
        }

        public bool IsEmpty => Value.IsEmpty;
        public bool IsMax => Value.IsMax;

        public void Increase(int amount) => Value.IncreaseCurrent(amount);
        public void Decrease(int amount) => Value.DecreaseCurrent(amount);
}
