using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using PersonelTakip.Enums;
using PersonelTakip.Models;

namespace PersonelTakip
{
    /// <summary>
    /// Interaction logic for CalisanEkle.xaml
    /// </summary>
    public partial class CalisanEkle : MetroWindow
    {
        public CalisanEkle()
        {
            InitializeComponent();
        }

        public MainWindow formMain { get; set; }
        public string FormTuru { get; set; }


        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (Context db = new Context())
            {

                List<Province> provinces = db.Provinces.Include("Districts").ToList();
                cbProvince.ItemsSource = provinces;

                List<Departman> departmens = db.Departmans.ToList();
                cbDeparmant.ItemsSource = departmens;
                

                List<Title> titles = db.Titles.ToList();
                cbTask.ItemsSource = titles;

                List<Agy> agies = db.Agies.ToList();
                cbAgiId.ItemsSource = agies;


                cbGender.ItemsSource = Enum.GetNames(typeof(Gender));
                cbNationality.ItemsSource = Enum.GetNames(typeof(Nationality));

                cbEmployeeType.ItemsSource = Enum.GetNames(typeof(EmployeeType));;
                cbEmployeeStatus.ItemsSource = Enum.GetNames(typeof(EmployeeStatus));

            }
        }

        private async void btnİnsert_Click(object sender, RoutedEventArgs e)
        {
            using (Context db = new Context())
            {
                if (FormTuru == "Yeni")
                {
                    Employee employees = grdEmployeeAdd.DataContext as Employee;
                     
                    db.Employees.Add(employees);
                    db.SaveChanges();
                    await this.ShowMessageAsync("Personel Ekleme", "Başarılı");
                    formMain.EmployeesList();
                    this.Close();
                }
                else if (FormTuru == "Güncelle")
                {
                    Employee employees = grdEmployeeAdd.DataContext as Employee;

                    Employee updated = db.Employees.SingleOrDefault(x => x.Id == employees.Id);
                    updated.StaffCode = employees.StaffCode;
                    updated.IdNumber = employees.IdNumber;
                    updated.FirstName = employees.FirstName;
                    updated.LastName = employees.LastName;
                    updated.Email = employees.Email;
                    updated.CellPhone = employees.CellPhone;
                    updated.Birthdate = employees.Birthdate;
                    updated.Address = employees.Address;
                    updated.Gender = employees.Gender;
                    updated.Nationality = employees.Nationality;
                    updated.EmployeeType = employees.EmployeeType;
                    updated.EmployeeStatus = employees.EmployeeStatus;
                    updated.Amount = employees.Amount;
                    updated.StarDate = employees.StarDate;
                    updated.EndDate = employees.EndDate;
                    updated.TaskId = employees.TaskId;
                    updated.DeparmantId = employees.DeparmantId;
                    updated.DistrictId = employees.DistrictId;
                    updated.AgiId = employees.AgiId;

                    db.SaveChanges();
                    await this.ShowMessageAsync("Personel Güncelleme", "Başarılı");
                    formMain.EmployeesList();
                    this.Close();
                }
            }
        }

    }
}

    
