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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using PersonelTakip.DataAccess;
using PersonelTakip.Enums;
using PersonelTakip.Models;

namespace PersonelTakip
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            EmployeesList();
            DepartmanList();
            TitleList();
            AgyList();

        }
        // Employee
        public void EmployeesList()
        {
            using (Context db = new Context())
            {
                List<Employee> employees = db.Employees.Include("Departman").Include("Title").Include("District").Include("District.Province").Include("Agy").ToList();
                lbEmployee.ItemsSource = employees;

                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lbEmployee.ItemsSource);
                view.Filter = EmployeeFilter;
            }

        }
        private void btnAdded_Click(object sender, RoutedEventArgs e)
        {
            flyoutEmployee.Header = "Yeni";
            grdEmployeeAdd.DataContext = new Employee();
            flyoutEmployee.IsOpen = !flyoutEmployee.IsOpen;
        }
        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Employee selectemployee = lbEmployee.SelectedItem as Employee;

            if (selectemployee != null)
            {
                flyoutEmployee.Header = "Güncelle";

                //CalisanEkle calisanEkle = new CalisanEkle();
                //calisanEkle.FormTuru = "Güncelle";
                grdEmployeeAdd.DataContext = selectemployee;
                //calisanEkle.formMain = this;
                //calisanEkle.Show();

                flyoutEmployee.IsOpen = !flyoutEmployee.IsOpen;
            }
            else
            {
                await this.ShowMessageAsync("Hata", "Güncelleme işlemi için personel seçiniz.");
            }
        }
        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            using (Context db = new Context())
            {
                Employee select = lbEmployee.SelectedItem as Employee;

                //Employee selectDelete = db.Employees.Single(x => x.Id == @select.Id);

                if (select != null)
                {
                    Employee selectDelete = db.Employees.Single(x => x.Id == select.Id);
                    var dialogResult = MessageBox.Show("Silmek istediğizden emin misiniz?", "Uyarı", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        db.Employees.Remove(selectDelete);
                        db.SaveChanges();
                        await this.ShowMessageAsync("Bilgi", "Personel Silme Başarılı");
                    }
                    else
                    {
                        await this.ShowMessageAsync("Uyarı", "Silme işlemi iptal edildi.");
                    }
                }
                else
                {
                    await this.ShowMessageAsync("Uyarı", "Silme işlemi için personel seçiniz.");
                }


            }

            EmployeesList();
        }
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            EmployeesList();
            grdEmployees.DataContext = lbEmployee.SelectedItem;
            fgrdContact.DataContext = lbEmployee.SelectedItem;

        }
        private void lbEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PersonelBilgileriniDoldur();
        }
        private void tbControlPersoneller_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //PersonelBilgileriniDoldur();

        }
        private void PersonelBilgileriniDoldur()
        {
            if (lbEmployee.SelectedItem == null)
            {
                return;
            }

            Employee seciliCalisan = lbEmployee.SelectedItem as Employee;

            switch (tbControlPersoneller.SelectedIndex)
            {
                case 0:
                    dtgIrtibatlar.ItemsSource = new EmployeeService().GetEmplooyeeContacts(seciliCalisan.Id);

                    break;
                case 1:
                    dtgİzinler.ItemsSource = new EmployeeService().GetEmployeePermissions(seciliCalisan.Id);
                    break;
                case 2:
                    dtgOdemeler.ItemsSource = new EmployeeService().GetEmployeePayment(seciliCalisan.Id);
                    break;
            }
        }
        private async void btnİnsert_Click(object sender, RoutedEventArgs e)
        {
            using (Context db = new Context())
            {
                Employee employees = grdEmployeeAdd.DataContext as Employee;
                if (string.IsNullOrEmpty(employees.FirstName))
                {
                    await this.ShowMessageAsync("Uyarı", "Personel adı boş bırakılamaz.");
                    return;
                }

                if (flyoutEmployee.Header == "Yeni")
                {
                    db.Employees.Add(employees);
                    db.SaveChanges();
                    await this.ShowMessageAsync("Başarılı", "Personel kaydınız başarıyla eklendi");
                    flyoutEmployee.IsOpen = !flyoutEmployee.IsOpen;

                    EmployeesList();
                }
                else if (flyoutEmployee.Header == "Güncelle")
                {
                    //Employee employees = grdEmployeeAdd.DataContext as Employee;

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
                    await this.ShowMessageAsync("Başarılı", "Personel kaydınız başarıyla güncellendi.");
                    flyoutEmployee.IsOpen = !flyoutEmployee.IsOpen;

                    EmployeesList();
                }
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            flyoutEmployee.IsOpen = !flyoutEmployee.IsOpen;
        }
        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lbEmployee.ItemsSource).Refresh();
        }
        private bool EmployeeFilter(object item)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return ((item as Employee).FirstName.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        // Contact
        private async void btnContactAdd_Click(object sender, RoutedEventArgs e)
        {

            Employee selected = null;

            if (lbEmployee.SelectedItem == null)
            {
                await this.ShowMessageAsync("Bilgi", "İrtibat eklenebilmesi için önce personel seçilmelidir.");
                return;
            }
            else
            {
                selected = lbEmployee.SelectedItem as Employee;
                FlyoutContact.Header = "Yeni";
                fgrdContact.DataContext = new Contact();
                FlyoutContact.IsOpen = !FlyoutContact.IsOpen;
            }

            Contact c = new Contact();
            c.EmployeeId = selected.Id;
        }
        private async void btnContactUpdate_Click(object sender, RoutedEventArgs e)
        {
            Employee selected = null;
            Contact select = dtgIrtibatlar.SelectedItem as Contact;
            Contact contact = grdContact.DataContext as Contact;

            if (select != null)
            {
                if (lbEmployee.SelectedItem == null)
                {
                    await this.ShowMessageAsync("Bilgi", "İrtibat güncellenebilmesi için önce personel seçilmelidir.");
                    return;
                }
                else
                {
                    selected = lbEmployee.SelectedItem as Employee;
                }
                Contact selectedContact = dtgIrtibatlar.SelectedItem as Contact;

                FlyoutContact.Header = "Güncelle";
                fgrdContact.DataContext = selectedContact;
                FlyoutContact.IsOpen = !FlyoutContact.IsOpen;
                //İrtibatEkle irtibatEkle = new İrtibatEkle();
                //irtibatEkle.FormTuru = "Güncelle";
                //irtibatEkle.EmployeeId = selected.Id;
                //irtibatEkle.grdContact.DataContext = selectedContact;
                //irtibatEkle.formMain = this;
                //irtibatEkle.Show();
            }
            else
            {
                await this.ShowMessageAsync("Uyarı", "Güncelleme işlemi için lütfen irtibat seçiniz.");
            }

        }
        private async void btnContactDelete_Click(object sender, RoutedEventArgs e)
        {
            using (Context db = new Context())
            {
                Contact select = dtgIrtibatlar.SelectedItem as Contact;

                if (select != null)
                {
                    Contact selectDelete = db.Contacts.Single(x => x.Id == select.Id);
                    var dialogResult = MessageBox.Show("Silmek istediğizden emin misiniz?", "Uyarı", MessageBoxButton.YesNo, MessageBoxImage.Information);

                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        db.Contacts.Remove(selectDelete);
                        db.SaveChanges();
                        await this.ShowMessageAsync("bilgi", "Kayıt silme işlemi başarıyla tamamlanmıştır.");
                    }
                    else
                    {
                        await this.ShowMessageAsync("Uyarı", "İrtibat silme işlemi iptal edidi.");
                    }
                }
                else
                {
                    await this.ShowMessageAsync("Uyarı", "Silme işlemi için lütfen irtibat seçiniz.");
                }
            }
        }
        private async void btnContactİnsert_Click(object sender, RoutedEventArgs e)
        {

            using (Context db = new Context())
            {

                Contact contact = fgrdContact.DataContext as Contact;
                Employee seciliCalisan = lbEmployee.SelectedItem as Employee;
                if (string.IsNullOrEmpty(contact.FirstName))
                {
                    await this.ShowMessageAsync("Uyarı", "İritbat adı boş bırakılamaz.");
                    return;
                }

                if (FlyoutContact.Header == "Yeni")
                {
                    contact.EmployeeId = seciliCalisan.Id;
                    db.Contacts.Add(contact);
                    db.SaveChanges();
                    await this.ShowMessageAsync("Bilgi", "Kayıt ekleme başarı ile tamanlanmıştır.");
                    FlyoutContact.IsOpen = !FlyoutContact.IsOpen;

                }
                else if (FlyoutContact.Header == "Güncelle")
                {
                    Contact updatedContact = db.Contacts.SingleOrDefault(x => x.Id == contact.Id);


                    updatedContact.FirstName = contact.FirstName;
                    updatedContact.LastName = contact.LastName;
                    updatedContact.Phone = contact.Phone;
                    updatedContact.Relationship = contact.Relationship;

                    contact.EmployeeId = seciliCalisan.Id;

                    db.SaveChanges();
                    await this.ShowMessageAsync("Bilgi", "Kayıt güncelleme başarı ile tamanlanmıştır.");
                    FlyoutContact.IsOpen = !FlyoutContact.IsOpen;
                }
            }
        }
        private void btnContactCancel_Click(object sender, RoutedEventArgs e)
        {
            FlyoutContact.IsOpen = !FlyoutContact.IsOpen;
        }


        // Permisions
        private void btnPermissionAdd_Click(object sender, RoutedEventArgs e)
        {
            Employee selected = null;
            if (lbEmployee.SelectedItem == null)
            {
                this.ShowMessageAsync("Bilgi", "İzin eklenebilmesi için önce personel seçilmelidir.");
                return;
            }
            else
            {
                selected = lbEmployee.SelectedItem as Employee;
                FlyoutPermission.Header = "Yeni";
                fgrdPermission.DataContext = new Permission();
                FlyoutPermission.IsOpen = !FlyoutPermission.IsOpen;
            }
            Permission p = new Permission();
            p.EmployeeId = selected.Id;
        }
        private async void btnPermissionUpdate_Click(object sender, RoutedEventArgs e)
        {
            Employee selected = null;
            Permission select = dtgİzinler.SelectedItem as Permission;
            Permission permission = grdPermission.DataContext as Permission;

            if (select != null)
            {
                if (lbEmployee.SelectedItem == null)
                {
                    await this.ShowMessageAsync("Bilgi", "İzin güncellenebilmesi için önce personel seçilmelidir.");
                    return;
                }
                else
                {
                    selected = lbEmployee.SelectedItem as Employee;
                }
                Permission selectedPermission = dtgİzinler.SelectedItem as Permission;

                FlyoutPermission.Header = "Güncelle";
                fgrdPermission.DataContext = selectedPermission;
                FlyoutPermission.IsOpen = !FlyoutPermission.IsOpen;
            }
            else
            {
                await this.ShowMessageAsync("Uyarı", "Güncelleme işlemi için lütfen izin seçiniz.");
            }
        }
        private async void btnPermissionDelete_Click(object sender, RoutedEventArgs e)
        {
            using (Context db = new Context())
            {
                Permission select = dtgİzinler.SelectedItem as Permission;

                if (select != null)
                {
                    Permission selectDelete = db.Permissions.Single(x => x.Id == select.Id);
                    var dialogResult = MessageBox.Show("Silmek istediğizden emin misiniz?", "Uyarı", MessageBoxButton.YesNo, MessageBoxImage.Information);

                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        db.Permissions.Remove(selectDelete);
                        db.SaveChanges();
                        await this.ShowMessageAsync("bilgi", "Kayıt silme işlemi başarıyla tamamlanmıştır.");
                    }
                    else
                    {
                        await this.ShowMessageAsync("Uyarı", "İzin silme işlemi iptal edidi.");
                    }
                }
                else
                {
                    await this.ShowMessageAsync("Uyarı", "Silme işlemi için lütfen irtibat seçiniz.");
                }
            }
        }
        private async void btnPermissionİnsert_Click(object sender, RoutedEventArgs e)
        {
            using (Context db = new Context())
            {

                Permission permission = fgrdPermission.DataContext as Permission;
                Employee seciliCalisan = lbEmployee.SelectedItem as Employee;
                if (string.IsNullOrEmpty(permission.StarDate.ToString()))
                {
                    await this.ShowMessageAsync("Uyarı", "İzine Çıkış tarihi boş bırakılamaz.");
                    return;
                }

                if (FlyoutPermission.Header == "Yeni")
                {
                    permission.EmployeeId = seciliCalisan.Id;
                    db.Permissions.Add(permission);
                    db.SaveChanges();
                    await this.ShowMessageAsync("Bilgi", "Kayıt ekleme başarı ile tamanlanmıştır.");
                    FlyoutPermission.IsOpen = !FlyoutPermission.IsOpen;

                }
                else if (FlyoutPermission.Header == "Güncelle")
                {
                    Permission updatedPermission = db.Permissions.SingleOrDefault(x => x.Id == permission.Id);


                    updatedPermission.StarDate = permission.StarDate;
                    updatedPermission.EndDate = permission.EndDate;
                    updatedPermission.PermissionType = permission.PermissionType;
                    permission.EmployeeId = seciliCalisan.Id;

                    db.SaveChanges();
                    await this.ShowMessageAsync("Bilgi", "Kayıt güncelleme başarı ile tamanlanmıştır.");
                    FlyoutPermission.IsOpen = !FlyoutPermission.IsOpen;
                }
            }
        }
        private void btnPermissionCancel_Click(object sender, RoutedEventArgs e)
        {
            FlyoutPermission.IsOpen = !FlyoutPermission.IsOpen;
        }


        //Payment
        private void btnPaymentAdd_Click(object sender, RoutedEventArgs e)
        {
            Employee selected = null;
            if (lbEmployee.SelectedItem == null)
            {
                this.ShowMessageAsync("Bilgi", "Ödeme eklenebilmesi için önce personel seçilmelidir.");
                return;
            }
            else
            {
                selected = lbEmployee.SelectedItem as Employee;
                FlyoutPayments.Header = "Yeni";
                fgrdPayment.DataContext = new Payment();
                FlyoutPayments.IsOpen = !FlyoutPayments.IsOpen;
            }
            Payment payment = new Payment();
            payment.EmployeeId = selected.Id;


        }
        private async void btnPaymentUpdate_Click(object sender, RoutedEventArgs e)
        {
            Employee selected = null;
            Payment select = dtgOdemeler.SelectedItem as Payment;
            Payment payment = grdPAyment.DataContext as Payment;

            if (select != null)
            {
                if (lbEmployee.SelectedItem == null)
                {
                    await this.ShowMessageAsync("Bilgi", "ödeme güncellenebilmesi için önce personel seçilmelidir.");
                    return;
                }
                else
                {
                    selected = lbEmployee.SelectedItem as Employee;
                }
                Payment selectedPayment = dtgOdemeler.SelectedItem as Payment;

                FlyoutPayments.Header = "Güncelle";
                FlyoutPayments.DataContext = selectedPayment;
                FlyoutPayments.IsOpen = !FlyoutPayments.IsOpen;
            }
            else
            {
                await this.ShowMessageAsync("Uyarı", "Güncelleme işlemi için lütfen ödeme seçiniz.");
            }


        }
        private async void btnPaymentDelete_Click(object sender, RoutedEventArgs e)
        {
            using (Context db = new Context())
            {
                Payment select = dtgOdemeler.SelectedItem as Payment;

                if (select != null)
                {
                    Payment selectDelete = db.Payments.Single(x => x.Id == select.Id);
                    var dialogResult = MessageBox.Show("Silmek istediğizden emin misiniz?", "Uyarı", MessageBoxButton.YesNo, MessageBoxImage.Information);

                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        db.Payments.Remove(selectDelete);
                        db.SaveChanges();
                        await this.ShowMessageAsync("bilgi", "Kayıt silme işlemi başarıyla tamamlanmıştır.");
                    }
                    else
                    {
                        await this.ShowMessageAsync("Uyarı", "Ödeme silme işlemi iptal edidi.");
                    }
                }
                else
                {
                    await this.ShowMessageAsync("Uyarı", "Silme işlemi için lütfen ödeme seçiniz.");
                }
            }
        }
        private async void btnPaymentİnsert_Click(object sender, RoutedEventArgs e)
        {
            using (Context db = new Context())
            {

                Payment payment = fgrdPayment.DataContext as Payment;
                Employee seciliCalisan = lbEmployee.SelectedItem as Employee;
                if (string.IsNullOrEmpty(payment.Amount.ToString()))
                {
                    await this.ShowMessageAsync("Uyarı", "Ödeme tutarı boş bırakılamaz.");
                    return;
                }

                if (FlyoutPayments.Header == "Yeni")
                {
                    payment.EmployeeId = seciliCalisan.Id;
                    db.Payments.Add(payment);
                    db.SaveChanges();
                    await this.ShowMessageAsync("Bilgi", "Kayıt ekleme başarı ile tamanlanmıştır.");
                    FlyoutPayments.IsOpen = !FlyoutPayments.IsOpen;
                }
                else if (FlyoutPayments.Header == "Güncelle")
                {
                    Payment updatedPayment = db.Payments.SingleOrDefault(x => x.Id == payment.Id);


                    updatedPayment.Amount = payment.Amount;
                    updatedPayment.PaymentType = payment.PaymentType;
                    updatedPayment.Date = payment.Date;
                    updatedPayment.Year = payment.Year;
                    updatedPayment.Month = payment.Month;

                    db.SaveChanges();
                    await this.ShowMessageAsync("Bilgi", "Kayıt güncelleme başarı ile tamanlanmıştır.");
                    FlyoutPayments.IsOpen = !FlyoutPayments.IsOpen;
                }
            }
        }
        private void btnPaymentCancel_Click(object sender, RoutedEventArgs e)
        {
            FlyoutPayments.IsOpen = !FlyoutPayments.IsOpen;
        }



        // Departmans

        public void DepartmanList()
        {
            using (Context db = new Context())
            {
                List<Departman> departman = db.Departmans.Include("Employees").ToList();
                dtgDepartmans.ItemsSource = departman;
            }

        }
        private void btnDepartmanAdd_Click(object sender, RoutedEventArgs e)
        {
            flyoutDerpartman.Header = "Yeni";
            grdGorevler.DataContext = new Departman();
            flyoutDerpartman.IsOpen = !flyoutDerpartman.IsOpen;
        }
        private async void btnDepartmanUpdate_Click(object sender, RoutedEventArgs e)
        {
            Departman selectedDepartman = dtgDepartmans.SelectedItem as Departman;

            if (selectedDepartman != null)
            {
                flyoutDerpartman.Header = "Güncelle";
                grdGorevler.DataContext = selectedDepartman;
                flyoutDerpartman.IsOpen = !flyoutDerpartman.IsOpen;
            }
            else
            {
                await this.ShowMessageAsync("Hata", "Güncelleme işlemi için departman seçiniz.");
            }


        }
        private async void btnDepartmanDelete_Click(object sender, RoutedEventArgs e)
        {
            using (Context db = new Context())
            {
                Departman select = dtgDepartmans.SelectedItem as Departman;

                if (select != null)
                {
                    Departman selectDelete = db.Departmans.Single(x => x.Id == select.Id);
                    var dialogResult = MessageBox.Show("Silmek istediğizden emin misiniz?", "Uyarı", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        db.Departmans.Remove(selectDelete);
                        db.SaveChanges();
                        await this.ShowMessageAsync("Bilgi", "Departman silme işleme başarı ile tamamlanmıştır.");
                        DepartmanList();
                    }
                    else
                    {
                        await this.ShowMessageAsync("Uyarı", "Silme işlemi iptal edildi.");
                    }
                }
                else
                {
                    await this.ShowMessageAsync("Uyarı", "Silme işlemi için departman seçiniz.");
                }


            }
        }
        private async void btnDepartmanİnsert_Click(object sender, RoutedEventArgs e)
        {
            using (Context db = new Context())
            {
                Departman departman = grdGorevler.DataContext as Departman;

                if (string.IsNullOrEmpty(departman.DepartmanName))
                {
                    await this.ShowMessageAsync("Uyarı", "Departman adı boş bırakılamaz.");
                    return;
                }
                if (flyoutDerpartman.Header == "Yeni")
                {
                    db.Departmans.Add(departman);
                    db.SaveChanges();
                    await this.ShowMessageAsync("Bilgi", "Kayıt ekleme başarı ile tamamlanmıştır.");
                    flyoutDerpartman.IsOpen = !flyoutDerpartman.IsOpen;
                    DepartmanList();
                }
                else if (flyoutDerpartman.Header == "Güncelle")
                {
                    Departman updateDepartman = db.Departmans.SingleOrDefault(x => x.Id == departman.Id);

                    updateDepartman.DepartmanName = departman.DepartmanName;

                    db.SaveChanges();
                    await this.ShowMessageAsync("Bilgi", "Güncelleme işlemi başarı ile tamamlanmıştır.");
                    flyoutDerpartman.IsOpen = !flyoutDerpartman.IsOpen;
                    DepartmanList();
                }
            }
        }
        private void btnDepartmanCancel_Click(object sender, RoutedEventArgs e)
        {
            flyoutDerpartman.IsOpen = !flyoutDerpartman.IsOpen;
        }


        //Task
        private void btnTaskAdd_Click(object sender, RoutedEventArgs e)
        {
            flyoutTask.Header = "Yeni";
            grdBolümler.DataContext = new Title();
            flyoutTask.IsOpen = !flyoutTask.IsOpen;
        }
        private async void btnTaskUpdate_Click(object sender, RoutedEventArgs e)
        {
            Title selectedTitle = dtgTitles.SelectedItem as Title;

            if (selectedTitle != null)
            {
                flyoutTask.Header = "Güncelle";
                grdBolümler.DataContext = selectedTitle;
                flyoutTask.IsOpen = !flyoutTask.IsOpen;
            }
            else
            {
                await this.ShowMessageAsync("Hata", "Güncelleme işlemi için departman seçiniz.");
            }
        }
        private async void btnTaskDelete_Click(object sender, RoutedEventArgs e)
        {
            using (Context db = new Context())
            {
                Title select = dtgTitles.SelectedItem as Title;

                if (select != null)
                {
                    Title selectDelete = db.Titles.Single(x => x.Id == select.Id);
                    var dialogResult = MessageBox.Show("Silmek istediğizden emin misiniz?", "Uyarı", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        db.Titles.Remove(selectDelete);
                        db.SaveChanges();
                        await this.ShowMessageAsync("Bilgi", "Görev silme işleme başarı ile tamamlanmıştır.");
                        TitleList();
                    }
                    else
                    {
                        await this.ShowMessageAsync("Uyarı", "Silme işlemi iptal edildi.");
                    }
                }
                else
                {
                    await this.ShowMessageAsync("Uyarı", "Silme işlemi için departman seçiniz.");
                }


            }
        }
        public void TitleList()
        {
            using (Context db = new Context())
            {
                List<Title> titles = db.Titles.Include("Employees").ToList();
                dtgTitles.ItemsSource = titles;
            }

        }
        private async void btnTaskİnsert_Click(object sender, RoutedEventArgs e)
        {
            using (Context db = new Context())
            {
                Title title = grdBolümler.DataContext as Title;

                if (string.IsNullOrEmpty(title.TaskName))
                {
                    await this.ShowMessageAsync("Uyarı", "Bölüm adı boş bırakılamaz.");
                    return;
                }

                if (flyoutTask.Header == "Yeni")
                {
                    db.Titles.Add(title);
                    db.SaveChanges();
                    await this.ShowMessageAsync("Bilgi", "Kayıt ekleme başarı ile tamamlanmıştır.");
                    flyoutTask.IsOpen = !flyoutTask.IsOpen;
                    TitleList();
                }
                else if (flyoutTask.Header == "Güncelle")
                {
                    Title updateTitle = db.Titles.SingleOrDefault(x => x.Id == title.Id);

                    updateTitle.TaskName = title.TaskName;

                    db.SaveChanges();
                    await this.ShowMessageAsync("Bilgi", "Güncelleme işlemi başarı ile tamamlanmıştır.");
                    flyoutTask.IsOpen = !flyoutTask.IsOpen;
                    TitleList();
                }
            }
        }
        private void btnTaskCancel_Click(object sender, RoutedEventArgs e)
        {
            flyoutTask.IsOpen = !flyoutTask.IsOpen;
        }

        //Agy
        private void btnAgyAdd_Click(object sender, RoutedEventArgs e)
        {
            AgiEkle agiEkle = new AgiEkle();
            agiEkle.FormTuru = "Yeni";
            agiEkle.grdAgy.DataContext = new Agy();
            agiEkle.formMain = this;
            agiEkle.Show();
        }
        private void btnAgyUpdate_Click(object sender, RoutedEventArgs e)
        {
            Agy selectedList = lvAgy.SelectedItem as Agy;

            AgiEkle agiEkle = new AgiEkle();
            agiEkle.FormTuru = "Güncelle";
            agiEkle.grdAgy.DataContext = selectedList;
            agiEkle.formMain = this;
            agiEkle.Show();
        }
        private void btnAgyDelete_Click(object sender, RoutedEventArgs e)
        {
            using (Context db = new Context())
            {
                Agy select = lvAgy.SelectedItem as Agy;

                Agy DeletedAgy = db.Agies.Single(x => x.Id == select.Id);

                db.Agies.Remove(DeletedAgy);
                db.SaveChanges();
                this.ShowMessageAsync("Bilgi", "Agi Silme İşlemi Başarılı");

            }
        }
        public void AgyList()
        {
            using (Context db = new Context())
            {
                List<Agy> agies = db.Agies.ToList();
                lvAgy.ItemsSource = agies;
            }
        }

        //FlyoutControl
        private void flyoutsControl_Loaded(object sender, RoutedEventArgs e)
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

                cbEmployeeType.ItemsSource = Enum.GetNames(typeof(EmployeeType)); ;
                cbEmployeeStatus.ItemsSource = Enum.GetNames(typeof(EmployeeStatus));

                cbPermissionType.ItemsSource = Enum.GetNames(typeof(PermissionType));

                cbPaymentType.ItemsSource = Enum.GetNames(typeof(PaymentType));


                //Ödeme ekranı yıllar ve aylar
                int i = DateTime.Now.Year;
                for (i = i - 1; i <= DateTime.Now.Year + 9; i++)
                    txtYear.Items.Add(Convert.ToString(i));


                string[] month = new string[]
                {
                    "Ocak",
                    "Şubat",
                    "Mart",
                    "Nisan",
                    "Mayıs",
                    "Haziran",
                    "Temmuz",
                    "Ağustos",
                    "Eylül",
                    "Ekim",
                    "Kasım",
                    "Aralık"
                };

                txtMonth.ItemsSource = month;

                //using (Context db = new Context())
                //{
                //    List<Agy> agies = db.Agies.ToList();
                //    txtAgy.ItemsSource = agies;
                //}
            }

        }

       
    }
}
