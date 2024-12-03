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

namespace MoneyInEvery
{
    /// <summary>
    /// Логика взаимодействия для Calculate.xaml
    /// </summary>
    public partial class Calculate : Window
    {
        private bool Avaible = false, getres = false;

        private double stableper = 0.08;
        private double optiper = 0.05;
        private double standper = 0.06;

        //Переменные для результатов
        private double resstable, resopti, resstand, insumma;

        private double sroknum;
        public Calculate()
        {
            InitializeComponent();

            this.ResizeMode = ResizeMode.NoResize;
        }

        private void summa_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            summatext.Text = (Math.Round(summa.Value) * 1000).ToString();

            insumma = (Math.Round(summa.Value) * 1000);

            if (summa != null)
            {
                changedoh();
            }
        }

        private void toCompare_Click(object sender, RoutedEventArgs e)
        {
            Compare compare = new Compare(insumma, resstable, resopti, resstand, sroknum);
            compare.Show();
            this.Close();
        }

        private void srok_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sroknum = Math.Round(srok.Value) * 30;
            sroltext.Text = sroknum.ToString();

            changedoh();
        }

        private void everymon_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            evertmontext.Text = (Math.Round(everymon.Value) * 1000).ToString();

            changedoh();
            getres = true;
        }

        private void changedoh()
        {
            if (sroknum >= 90)
            {
                //Просчет стабильного
                double stabres = Math.Round(((summa.Value * 1000) * stableper * sroknum) / (365 * 1));
                stabletext.Text = stabres.ToString() + " Руб.";
                resstable = stabres;

                //Просчет стандартного
                double standres = Math.Round((((summa.Value * 1000) + (everymon.Value * 1000)) * standper * sroknum) / 365);
                standarttext.Text = standres.ToString() + " Руб.";
                resstand = standres;
            }
            
            if (sroknum >= 180)
            {
                //Просчет оптимального
                double screw = 0;
                double mainsumm = (((summa.Value * 1000) * optiper) / 365) * 30;
                double monthsumm = 0;


                for (int i = 0; i < sroknum / 30; i++)
                {
                    screw += mainsumm;
                    mainsumm = (((screw + (everymon.Value * 1000) + monthsumm + (summa.Value * 1000)) * optiper) / 365) * 30;
                    monthsumm += (everymon.Value * 1000);
                }
                optimaltext.Text = Math.Round(screw).ToString() + " Руб.";
                resopti = Math.Round(screw);
            }
        }
    }
}
