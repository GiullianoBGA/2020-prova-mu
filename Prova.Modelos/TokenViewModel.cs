using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Prova.Modelos
{
    public class TokenViewModel : INotifyPropertyChanged
    {
        private string login;
        public string Login
        {
            get { return login; }
            set
            {
                if (value != login)
                {
                    login = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Pwd { get; set; }
        private bool logged;
        public bool Logged
        {
            get { return logged; }
            set
            {
                logged = value;
                OnPropertyChanged();
            }
        }

        private string token;
        public string Token
        {
            get { return token; }
            set
            {
                token = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
