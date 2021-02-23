using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for OdemeEkle.xaml
    /// </summary>
    public partial class OdemeEkle : MetroWindow
    {
        public OdemeEkle()
        {
            InitializeComponent();
        }

        public MainWindow formMain { get; set; }
        public int EmployeeId = 0;
        public string FormTuru { get; set; }

        private async void btnİnsert_Click(object sender, RoutedEventArgs e)
        {
            using(Context db = new Context())
            {

                if (FormTuru == "Yeni")
                {
                    Payment payment = grdPayment.DataContext as Payment;
                    payment.EmployeeId = EmployeeId;

                    db.Payments.Add(payment);
                    db.SaveChanges();
                    await this.ShowMessageAsync("Personel Ekleme", "Başarılı");
                    this.Close();
                }
                else if (FormTuru == "Güncelle")
                {
                    Payment payment = grdPayment.DataContext as Payment;

                    Payment updatePayment = db.Payments.SingleOrDefault(x => x.Id == payment.Id);

                    updatePayment.AgiId = payment.AgiId;
                    updatePayment.Year = payment.Year;
                    updatePayment.Month = payment.Month;
                    updatePayment.Amount = payment.Amount;
                    updatePayment.PaymentType = payment.PaymentType;

                    db.SaveChanges();
                    await this.ShowMessageAsync("Bilgi", "İzin Güncelleme Başarılı");
                    this.Close();
                }
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            int i = DateTime.Now.Year ;
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

            cbPaymentType.ItemsSource = Enum.GetValues(typeof(PaymentType)).Cast<PaymentType>();

            //using (Context db = new Context())
            //{
            //    List<Agy> agies = db.Agies.ToList();
            //    txtAgy.ItemsSource = agies;
            //}
        }
    }
}
