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
using PersonelTakip.Enums;
using PersonelTakip.Models;

namespace PersonelTakip
{
    /// <summary>
    /// Interaction logic for İzinEkle.xaml
    /// </summary>
    public partial class İzinEkle : MetroWindow
    {
        public İzinEkle()
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
                    Permission permission = grdPermission.DataContext as Permission;
                    permission.EmployeeId = EmployeeId;

                    db.Permissions.Add(permission);
                    db.SaveChanges();
                    await this.ShowMessageAsync("Bİlgi", "İzin Ekleme Başarılı");
                    this.Close();
                }
                else if (FormTuru == "Güncelle")
                {
                    Permission permission = grdPermission.DataContext as Permission;

                    Permission updatePermission = db.Permissions.SingleOrDefault(x => x.Id == permission.Id);

                    updatePermission.StarDate = permission.StarDate;
                    updatePermission.EndDate = permission.EndDate;
                    updatePermission.Day = permission.Day;
                    updatePermission.PermissionType = permission.PermissionType;

                    db.SaveChanges();
                    await this.ShowMessageAsync("Bilgi", "İzin Güncelleme Başarılı");
                    this.Close();
                }
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            cbPermissionType.ItemsSource = Enum.GetValues(typeof(PermissionType)).Cast<PermissionType>();
        }
    }
}
