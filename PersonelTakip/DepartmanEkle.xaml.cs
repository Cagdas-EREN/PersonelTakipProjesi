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
    /// Interaction logic for DepartmanEkle.xaml
    /// </summary>
    public partial class DepartmanEkle : MetroWindow
    {
        public DepartmanEkle()
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
                    Departman departman = grdGorevler.DataContext as Departman;

                    db.Departmans.Add(departman);
                    db.SaveChanges();
                    await this.ShowMessageAsync("Bilgi", "Departman Güncelleme Başarılı");
                    this.Close();
                }
                else if (FormTuru == "Güncelle")
                {
                    Departman departman= grdGorevler.DataContext as Departman;

                    Departman updateDepartman = db.Departmans.SingleOrDefault(x => x.Id == departman.Id);

                    updateDepartman.DepartmanName = departman.DepartmanName;

                    db.SaveChanges();
                    await this.ShowMessageAsync("Bilgi", "Departman Güncelleme Başarılı");
                    this.Close();
                }
            }
        }
    }
}
