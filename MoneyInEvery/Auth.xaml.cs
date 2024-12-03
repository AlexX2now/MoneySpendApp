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
using Word = Microsoft.Office.Interop.Word;
using System.Windows.Shapes;

namespace MoneyInEvery
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        BankEntities db = new BankEntities();
        private double insumma, period, percents;
        public Auth(double insumma, double period, double percents)
        {
            InitializeComponent();
            this.insumma = insumma;
            this.period = period;
            this.percents = percents;
        }

        private void enterbtn_Click(object sender, RoutedEventArgs e)
        {
            if(inlogin.Text == string.Empty || inpassword.Text == string.Empty)
            {
                MessageBox.Show("Не оставляйте поля пустыми");
            }
            else { 

                var neededuser = db.User_.FirstOrDefault(x => x.Login.ToString() == inlogin.Text || x.Password == inpassword.Text);

                if (neededuser == null)
                {
                    MessageBox.Show("Вы ввели некорректные данные.");
                }
                else
                {
                    //Все нужное для заполнения бд
                    var LastBankAcc = db.BankAccount_.OrderByDescending(x => x.NumberAccount).FirstOrDefault();
                    
                    double idbankacc = (LastBankAcc.NumberAccount + 1);
                    
                    DateTime dateTime = DateTime.Now;
                    
                    //Добавление в базу данных
                    using (var context = new BankEntities())
                    {
                        var BankAccaunt = new BankAccount_
                        {
                            NumberAccount = idbankacc,
                            DateOpen = dateTime,
                            Balance = insumma,
                            Type = 1
                        };
                        context.BankAccount_.Add(BankAccaunt);
                        context.SaveChanges();

                        var Contract = new Contract_
                        {
                            NumberAccount = idbankacc,
                            IDUser = neededuser.IDUser,
                            Amount = insumma,
                            Period = Convert.ToInt32(period),
                            ExpirationDate = dateTime.AddDays(period),
                            Percet = percents
                        };
                        context.Contract_.Add(Contract);
                        context.SaveChanges();

                    }



                    //определение переменной для использования Word
                    var WordApp = new Word.Application();
                    WordApp.Visible = false;
                    // делаем диалог выбора папки, в которую будет сохранятся билет
                    var Worddoc = WordApp.Documents.Open(Environment.CurrentDirectory +
                    @"\Шаблон договора.docx");

                    //Определенние всего нужного
                    string num_ser = neededuser.Passport_.Number_Series.ToString();

                    //Заполнение
                    Repwo("Номер договора", idbankacc.ToString(), Worddoc);
                    Repwo("день", dateTime.Day.ToString(), Worddoc);
                    Repwo("месяц",dateTime.ToString("MMMM"), Worddoc);
                    Repwo("18", dateTime.Year.ToString(), Worddoc);
                    Repwo("________________________________!ФИО вкладчика", neededuser.Name + " " + neededuser.Surname + " " + neededuser.Patronymic,Worddoc);
                    Repwo("______________ !!ФИО вкладчика ", neededuser.Name + " " + neededuser.Surname + " " + neededuser.Patronymic, Worddoc);
                    Repwo("_________!Сумма вклада", insumma.ToString(), Worddoc);
                    Repwo("__________! Срок вклада ", period.ToString(), Worddoc);
                    Repwo("_______ !Дата окончания срока вклада ", dateTime.AddDays(period).ToString(), Worddoc);
                    Repwo("_________ !Процентная ставка по вкладу ", percents.ToString(), Worddoc);
                    Repwo("______________________________!Номер счета вклада", idbankacc.ToString(), Worddoc);
                    Repwo("______!Адрес регистрации", neededuser.Passport_.Adress, Worddoc);
                    Repwo("____________!Адрес электронной почты", neededuser.E_Mail, Worddoc);
                    Repwo("__!Серия", num_ser.Substring(0, 6), Worddoc);
                    Repwo("____!Номер", num_ser.Substring(6, 4), Worddoc);
                    Repwo("____!Кем и когда выдан", neededuser.Passport_.Issued, Worddoc);
                    Repwo("______!Дата рождения", neededuser.Passport_.DateOfBirth.ToString(), Worddoc);
                    Repwo("_______!Место рождения", neededuser.Passport_.PlaceOfBirth, Worddoc);

                    Worddoc.SaveAs2(Environment.CurrentDirectory + @"\Договор.docx");
                    MessageBox.Show("Договор сохранен успешно!");
                }
            }
        }
        private void Repwo(string subToReplace, string text, Word.Document worddoc)
        {
            var range = worddoc.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: subToReplace, ReplaceWith: text);
        }
    }
}
