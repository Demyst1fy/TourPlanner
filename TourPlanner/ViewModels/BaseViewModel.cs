using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged 
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChangedEvent([CallerMemberName] string propertyName = "") 
        {
            ValidatePropertyName(propertyName);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void ValidatePropertyName(string propertyName) 
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null) 
            {
                throw new ArgumentException("Invalid propery name: " + propertyName);
            }
        }
    }
}
