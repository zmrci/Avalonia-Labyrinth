using System;
using System.Windows.Input;

namespace Beadando3.ViewModels
{// a xaml feluleten levo gombokat ennek a segitsegevel kotom ossze a viewModellal anelkul hogy esemenyeket raknek a code behinba
    public class DelegateCommand : ICommand //interfacet implemetalja kell a commandokhoz nme generikis!!
    {// ez olyan gombokhoz kell mint a pause stb ahol a gomb megnyomasa magaba hordozza az infot es nem kell plusz adat a viewbol

        private readonly Action<object?> _execute; // itt tarolom el a logikat amit majd tennie kjell ha megnyomjak vagy aktivaljak az Action miatt nem ad vissza semmit
        private readonly Func<object?, bool>? _canExecute; // azt a logikat tarolja hogy a parancs futtathato e boolt ad vissza a gom isenabled tulajdonsagat ez allitja

        public DelegateCommand(Action execute) : this(_ => execute(), null) { } //ugye a legtobb parancsnak nincsen logikai korlatozasa ezert van itt a null rogton

        public DelegateCommand(Action<object?> execute, Func<object?, bool>? canExecute = null) // azoknak maiknek kell a logikai korlatozas
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute; //privatba elpakolja
        }

        public event EventHandler? CanExecuteChanged; //szol hogy frissitse a gombok can exec allapotat

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true; // lehet e kattontani vagy sem eppen
        //mindig mukodik amig a parancsba null van megadva mert akkor true ad vissza vagy eppen a logikai kapu engedi a nyomkodast
        public void Execute(object? parameter) => _execute(parameter); // mi tortenjen ha rakantinak lefutatja a parancsot (atadja a viewmodelek ugye)
    }// wpf ezt hivja ha nyomkodjak meghivja az execute mezobe tarolt metodust
}
