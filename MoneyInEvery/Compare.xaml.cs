using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace MoneyInEvery
{
    /// <summary>
    /// Логика взаимодействия для Compare.xaml
    /// </summary>
    public partial class Compare : Window
    {
        private double insumma, resstable, resopti, resstand, period, optper = 5, standper = 6, stabper = 8;
        private void openopt_Click(object sender, RoutedEventArgs e)
        {
            Auth auth = new Auth(insumma, period, optper);
            auth.Show();
            this.Close();
        }

        private void openstand_Click(object sender, RoutedEventArgs e)
        {
            Auth auth = new Auth(insumma, period, standper);
            auth.Show();
            this.Close();
        }

        private void openstab_Click(object sender, RoutedEventArgs e)
        {
            Auth auth = new Auth(insumma, period, stabper);
            auth.Show();
            this.Close();
        }

        public Compare(double insumma, double resstable, double resopti, double resstand, double period)
        {
            InitializeComponent();
            this.insumma = insumma;
            this.resstable = resstable;
            this.resopti = resopti;
            this.resstand = resstand;
            this.period = period;

            dohstab.Text = resstable.ToString() + " Руб.";
            dohopt.Text = resopti.ToString() + " Руб.";
            dohstand.Text = resstand.ToString() + " Руб.";

            endopt.Text = (insumma + resopti).ToString() + " Руб.";
            endstab.Text = (insumma + resstable).ToString() + " Руб.";
            endstand.Text = (insumma + resstand).ToString() + " Руб.";

            if (period <= 180)
            {
                openopt.IsEnabled = false;
            }

            if (period <= 90)
            {
                openstab.IsEnabled = false;
                openstand.IsEnabled = false;
            }
        }

        private void createvip_Click(object sender, RoutedEventArgs e)
        {
            //Объект документа пдф
            iTextSharp.text.Document doc = new iTextSharp.text.Document();

            //Создаем объект записи пдф-документа в файл
            PdfWriter.GetInstance(doc, new FileStream("extract.pdf", FileMode.Create));


            doc.Open();

            PdfPTable table = new PdfPTable(4);

            //заголовки
            table.AddCell("Name");
            table.AddCell("Income");
            table.AddCell("Amount at the end of the term");
            table.AddCell("Bid");

            //стабильный
            table.AddCell("Stable");
            table.AddCell("" + resstable.ToString() + " Rub.");
            table.AddCell("" + (insumma + resstable).ToString() + " Rub.");
            table.AddCell("8% Rub.");

            //оптимальный
            table.AddCell("Optimal");
            table.AddCell(resopti.ToString() + " Rub.");
            table.AddCell((insumma + resopti).ToString() + " Rub.");
            table.AddCell("5% Rub.");

            //стандарт
            table.AddCell("Standard");
            table.AddCell(resstand.ToString() + " Rub.");
            table.AddCell((insumma + resstand).ToString() + " Rub.");
            table.AddCell("6% Rub.");

            doc.Add(table);
            doc.Close();

            MessageBox.Show("Pdf успешно сохранен!");
        }
    }
}
