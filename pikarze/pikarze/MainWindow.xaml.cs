using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pikarze
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            textBoxWEPImie.SetFocus();
        }
        private string plikArchiwizacji = "archiwum.txt";

        private bool IsNotEmpty(TextBoxWithErrorProvider tb)
        {
            if (tb.Text.Trim() == "")
            {
                tb.SetError("Pole nie może być puste!");
                return false;
            }
            tb.SetError("");
            return true;
        }


        private void Clear()
        {

            textBoxWEPImie.Text = "";
            textBoxWEPNazwisko.Text = "";
            sliderWaga.Value = 75;
            sliderWiek.Value = 25;
            buttonEdytuj.IsEnabled = false;
            buttonUsun.IsEnabled = false;

            listBoxPilkarze.SelectedIndex = -1;
            textBoxWEPImie.SetFocus();
        }

        private void LoadPlayer(Pilkarz pilkarz)
        {
            textBoxWEPImie.Text = pilkarz.Imie;
            textBoxWEPNazwisko.Text = pilkarz.Nazwisko;
            sliderWaga.Value = pilkarz.Waga;
            sliderWiek.Value = pilkarz.Wiek;
            buttonEdytuj.IsEnabled = true;
            buttonUsun.IsEnabled = true;
            textBoxWEPImie.SetFocus();
        }

        private void buttonEdytuj_Click(object sender, RoutedEventArgs e)
        {

            if (IsNotEmpty(textBoxWEPImie) & IsNotEmpty(textBoxWEPNazwisko))
            {
                var biezacyPilkarz = new Pilkarz(textBoxWEPImie.Text.Trim(), textBoxWEPNazwisko.Text.Trim(), (uint)sliderWiek.Value, (uint)sliderWaga.Value);
                var czyJuzJestNaLiscie = false;
                foreach (var p in listBoxPilkarze.Items)
                {
                    var pilkarz = p as Pilkarz;
                    if (pilkarz.isTheSame(biezacyPilkarz))
                    {
                        czyJuzJestNaLiscie = true;
                        break;
                    }
                }
                if (!czyJuzJestNaLiscie)
                {
                    var dialogResult = MessageBox.Show($"Czy na pewno chcesz zmienić dane  {Environment.NewLine} {listBoxPilkarze.SelectedItem}?", "Edycja", MessageBoxButton.YesNo);

                    if (dialogResult == MessageBoxResult.Yes)
                    {
                       
                         listBoxPilkarze.Items[listBoxPilkarze.SelectedIndex] = biezacyPilkarz;
                        

                    }
                    Clear();
                    listBoxPilkarze.SelectedIndex = -1;

                }
                else
                    MessageBox.Show($"{biezacyPilkarz.ToString()} już jest na liście.", "Uwaga");



            }
        }

        private void buttonDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (IsNotEmpty(textBoxWEPImie) & IsNotEmpty(textBoxWEPNazwisko))
            {
                var biezacyPilkarz = new Pilkarz(textBoxWEPImie.Text.Trim(), textBoxWEPNazwisko.Text.Trim(), (uint)sliderWiek.Value, (uint)sliderWaga.Value);
                var czyJuzJestNaLiscie = false;
                foreach (var p in listBoxPilkarze.Items)
                {
                    var pilkarz = p as Pilkarz;
                    if (pilkarz.isTheSame(biezacyPilkarz))
                    {
                        czyJuzJestNaLiscie = true;
                        break;
                    }
                }
                if (!czyJuzJestNaLiscie)
                {
                    listBoxPilkarze.Items.Add(biezacyPilkarz);
                    Clear();
                }
                else
                {
                    var dialog = MessageBox.Show($"{biezacyPilkarz.ToString()} już jest na liście {Environment.NewLine} Czy wyczyścić formularz?", "Uwaga", MessageBoxButton.OKCancel);
                    if (dialog == MessageBoxResult.OK)
                    {
                        Clear();
                    }

                }
            }
        }

        private void buttonUsun_Click(object sender, RoutedEventArgs e)
        {

            if (IsNotEmpty(textBoxWEPImie) & IsNotEmpty(textBoxWEPNazwisko))
            {

                var dialog = MessageBox.Show($"Czy na pewno chcesz usunąć piłkarza  {Environment.NewLine} {listBoxPilkarze.SelectedItem}?", "Usunąć", MessageBoxButton.YesNo);
                if (dialog == MessageBoxResult.Yes)
                {
                    listBoxPilkarze.Items.RemoveAt(listBoxPilkarze.SelectedIndex);
                    Clear();
                }







            }



           
        }

        

        private void listBoxPilkarze_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            if (listBoxPilkarze.SelectedIndex > -1)
            {
                LoadPlayer((Pilkarz)listBoxPilkarze.SelectedItem);
            }

        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int n = listBoxPilkarze.Items.Count;
            Pilkarz[] pilkarze = null;
            if (n > 0)
            {
                pilkarze = new Pilkarz[n];
                int index = 0;
                foreach (var o in listBoxPilkarze.Items)
                {
                    pilkarze[index++] = o as Pilkarz;
                }
                Zapis.ZapisPilkarzyDoPliku(plikArchiwizacji, pilkarze);
            }


        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var pilkarze = Zapis.CzytajPilkarzyZPliku(plikArchiwizacji);
            if (pilkarze != null)
                foreach (var p in pilkarze)
                {
                    listBoxPilkarze.Items.Add(p);
                }

        }
    }

}



