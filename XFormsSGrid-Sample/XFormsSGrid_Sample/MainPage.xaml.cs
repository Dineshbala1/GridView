using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XFormsSGrid_Sample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new DataViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
        }
    }

    public class DataViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Person> UnderlyingCollection { get; set; }
        public ICommand ClickToSendCommand { get; set; }

        public DataViewModel()
        {
            UnderlyingCollection = new ObservableCollection<Person>();
            generateData();
            ClickToSendCommand = new Command<Person>(ClickCommandAction);
        }

        void ClickCommandAction(Person person)
        {
           person.IsBusy = true;
        }

        void generateData()
        {
            for (int i = 0; i < 250; i++)
            {
                var person = new Person() {Name = "Tile" + i};
                UnderlyingCollection.Add(person);
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Person: INotifyPropertyChanged
    {
        public string Name { get; set; }
        private bool isBusy = false;

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
