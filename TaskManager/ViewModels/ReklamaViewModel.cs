using System;

namespace TaskManager.ViewModels
{
    public class ReklamaViewModel : ViewModel
    {
        public string TextReklama
        {
            get
            {
                string t;

                t = "     Програма розроблена для сповіщення та реєстрації " +
                     "виконаних завданнь. " + Environment.NewLine;
                t += "   Версія 1.0.2019 написана мною в листопаді 2019 року " +
                    "для особистого розвитку, та є інтелектуальним " +
                    "надбанням автора. " + Environment.NewLine;
                t += "   Для коректної роботи встановлено залежність ≧ " +
                    ".NET Framework 4.6" + Environment.NewLine;
                t += "   Всі права на впровадження нових функцій, " +
                    "вдосконалення та продаж даного програмного забезпечення " +
                    "залишаю за собою." + Environment.NewLine + Environment.NewLine;

                t += " © <Kuchinik & Co.>, 2019" + Environment.NewLine;
                t += " Кучінік Роман Вікторович" + Environment.NewLine;
                t += " тел. +38(068)104-18-24" + Environment.NewLine;
                return t;
            }
        }

        public string TitleWindow
        {
            get
            {
                return "(Про версію...)";
            }
        }
    }
}
