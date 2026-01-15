using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Beadando3.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        //automatikus propert nev felismeres beilleszti ide a gep annak propertynek a nevet ahonann meg lett hivva
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) //virtual tehat felulirhatja az oroklott osztaly
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // jelez a viewnak hogy frissiteni kell mert uj adat van
        }

        //beallitja a fieldet a valuera
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) // megelozni a fleslegese ertesiteseket referencia alapu
        {
            if (Equals(field, value)) return false; // megnezi hogy tenlyleg uj ertek e elkeruljuk a feesleges hivasokat

            field = value; // ha uj adat akkor berakja majd jelzi hogy prop change tortent
            OnPropertyChanged(propertyName); //property change hivasa
            return true; // jelzi hogy tortent e valtozas
        }
    }
}
