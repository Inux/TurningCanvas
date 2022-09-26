using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace App.Lib.Settings
{
    public class Property<T> : INotifyPropertyChanged
    {
        private T internalValue = default;
        private readonly string name;

        public Property(T initialValue, [CallerMemberName] string name = null)
        {
            this.internalValue = initialValue;
            this.name = name;
        }

        public T Value
        {
            get
            {
                return this.internalValue;
            }
            set
            {
                if (!this.internalValue.Equals(value))
                {
                    this.internalValue = value;
                    OnPropertyChanged(this.name);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
