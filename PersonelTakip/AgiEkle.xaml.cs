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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using PersonelTakip.Models;

namespace PersonelTakip
{
    /// <summary>
    /// Interaction logic for AgiEkle.xaml
    /// </summary>
    public partial class AgiEkle : MetroWindow
    {
        public AgiEkle()
        {
            InitializeComponent();
        }
        public MainWindow formMain { get; set; }
        public string FormTuru { get; set; }

        private async void btnİnsert_Click(object sender, RoutedEventArgs e)
        {
            using (Context db = new Context())
            {
                if (FormTuru == "Yeni")
                {
                    Agy agy = grdAgy.DataContext as Agy;

                    db.Agies.Add(agy);
                    db.SaveChanges();
                    await this.ShowMessageAsync("Bilgi", "Agi Ekleme Başarılı");
                    this.Close();
                }
                else if (FormTuru == "Güncelle")
                {
                    Agy agy = grdAgy.DataContext as Agy;

                    Agy updatedAgy = db.Agies.SingleOrDefault(x => x.Id == agy.Id);

                    updatedAgy.Name = agy.Name;
                    updatedAgy.Amount = agy.Amount;

                    db.SaveChanges();
                    await this.ShowMessageAsync("Bilgi", "Agi Düzenleme Başarılı");
                    this.Close();   
                }
            }
        }
    }
}
