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
    /// Interaction logic for GorevEkle.xaml
    /// </summary>
    public partial class GorevEkle : MetroWindow
    {
        public GorevEkle()
        {
            InitializeComponent();
        }

        public MainWindow formMain { get; set; }
        public int EmployeeId = 0;
        public string FormTuru { get; set; }

        private async void btnİnsert_Click(object sender, RoutedEventArgs e)
        {
            using (Context db = new Context())
            {
                if (FormTuru == "Yeni")
                {
                    Title title = grdBolümler.DataContext as Title;

                    db.Titles.Add(title);
                    db.SaveChanges();
                    await this.ShowMessageAsync("Bilgi", "Görev Ekleme Başarılı");
                    this.Close();
                }
                else if (FormTuru == "Güncelle")
                {
                    Title title = grdBolümler.DataContext as Title;

                    Title updateTitle = db.Titles.SingleOrDefault(x => x.Id == title.Id);

                    updateTitle.TaskName = title.TaskName;

                    db.SaveChanges();
                    await this.ShowMessageAsync("Bilgi", "Görev Güncelleme Başarılı");
                    this.Close();
                }
            }
        }
    }
}
