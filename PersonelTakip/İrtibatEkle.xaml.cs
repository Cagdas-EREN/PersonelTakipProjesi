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
    /// Interaction logic for İrtibatEkle.xaml
    /// </summary>
    public partial class İrtibatEkle : MetroWindow
    {
        public İrtibatEkle()
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
                    Contact contact = grdContact.DataContext as Contact;
                    //var selectedEmployee = cbEmployee.SelectedItem as Employee;
                    contact.EmployeeId = EmployeeId;



                    db.Contacts.Add(contact); 
                    db.SaveChanges();
                    await this.ShowMessageAsync("İrtibat Ekleme", "Başarılı");
                    this.Close();
                }
                else if (FormTuru == "Güncelle")
                { 
                    Contact contact = grdContact.DataContext as Contact;

                    
                    Contact updatedContact = db.Contacts.SingleOrDefault(x => x.Id == contact.Id);

                    updatedContact.FirstName = contact.FirstName;
                    updatedContact.LastName = contact.LastName;
                    updatedContact.Phone = contact.Phone;
                    updatedContact.Relationship = contact.Relationship;

                    db.SaveChanges();
                    await this.ShowMessageAsync("İrtibat Güncelleme", "Başarılı");
                    this.Close();
                }
            }
        }

    }
}
