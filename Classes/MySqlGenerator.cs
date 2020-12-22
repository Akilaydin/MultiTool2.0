using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SQLGenerator {
    public static class MySqlGenerator { //Class for offline Generation
        private static List<string> GenerateFullName(int whoToGenerate) {
            List<string> names = new List<string>();
            List<string> secNames = new List<string>();
            List<string> midNames = new List<string>();

            if (whoToGenerate == 1) {
                names.Add("Ростислав ");
                names.Add("Денис ");
                names.Add("Сергей ");
                names.Add("Анатолий ");
                names.Add("Илья ");
                names.Add("Александр ");
                names.Add("Артем ");
                names.Add("Владислав ");

                secNames.Add("'Ермаков ");
                secNames.Add("'Лихачёв ");
                secNames.Add("'Молчанов ");
                secNames.Add("'Валенов ");
                secNames.Add("'Воробьев ");
                secNames.Add("'Панкратов ");
                secNames.Add("'Панов ");
                secNames.Add("'Селиверстов ");

                midNames.Add("Станиславович");
                midNames.Add("Матвеевич");
                midNames.Add("Митрофанович");
                midNames.Add("Андреевич");
                midNames.Add("Евгеньевич");
                midNames.Add("Викторович");
                midNames.Add("Владиславович");
                midNames.Add("Еремеевич");
            }
            else {
                names.Add("Елена ");
                names.Add("Валентина ");
                names.Add("Анастасия ");
                names.Add("Надежда ");
                names.Add("Эльвира ");
                names.Add("Валерия ");
                names.Add("Виктория ");
                names.Add("Татьяна ");

                secNames.Add("'Грязнова ");
                secNames.Add("'Овчинникова ");
                secNames.Add("'Полянова ");
                secNames.Add("'Дрессова ");
                secNames.Add("'Панина ");
                secNames.Add("'Бирюкова ");
                secNames.Add("'Холина ");
                secNames.Add("'Курамова ");

                midNames.Add("Станиславовна");
                midNames.Add("Андреевна");
                midNames.Add("Артемовна");
                midNames.Add("Александровна");
                midNames.Add("Максимовна");
                midNames.Add("Владимировна");
                midNames.Add("Сергеевна");
                midNames.Add("Анатольевна");
            }



            List<string> fullNames = new List<string>();
            fullNames.AddRange(secNames);
            fullNames.AddRange(names);
            fullNames.AddRange(midNames);
            return fullNames;
        }

        private static string GenerateNumber() {
            Random random = new Random();
            string number = "+7 (9";
            int[] numbers = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            for (int i = 0; i < 9; i++) {
                if (i == 2) {
                    number += ") ";
                }
                if (i == 5) {
                    number += "-";
                }
                if (i == 7) {
                    number += "-";
                }
                number += numbers[random.Next(0, 9)];
            }
            return number;

        }

        private static string GenerateAddress() {
            Random random = new Random();
            string address = " ";
            string[] streets = new string[] { "Бабаевская", "Баженова", "Измайловская", "Заводская", "Бойцовая", "Гончарная", "Горбунова", "Машиностроения", "Крамского", "Менжинского", "Потехова" };
            address += streets[random.Next(0, streets.Length)] + " ул." + " Дом №" + random.Next(1, 50) + " кв. №" + random.Next(1, 200);
            return address;


        }
        private static string DayGenerator() {
            Random random = new Random();
            int randomDay = random.Next(1, 28);
            string day = "";
            if (randomDay < 10) { day = "0" + randomDay; }
            else { day = randomDay.ToString(); }
            return day;

        }
        private static string MounthGenerator() {
            Random random = new Random();
            int randomMonth = random.Next(1, 12);
            string mounth = "";
            if (randomMonth < 10) { mounth = "0" + randomMonth; }
            else { mounth = randomMonth.ToString(); }
            return mounth;
        }

        private static string GenerateBirthDate() {
            string birthDate = "'";
            Random random = new Random();
            birthDate += DayGenerator() + "." + MounthGenerator() + "." + random.Next(1950, 2000) + "'";
            return birthDate;
        }

        public static void GenerateFull(int whoToGenerate,TextBox textBoxGenerate, bool isInsert, string tableName) {
          
            try {
                Person.Name person = GetterRandomPerson.getRandomPerson();
                textBoxGenerate.Text = person.last + person.first;
            } catch (Exception) {
                MessageBox.Show("Проверьте подключение к интернету. Будет использована базовая генерация.");
                Random random = new Random();

                string fullName = "";
                string[] fullNames = GenerateFullName(whoToGenerate).ToArray();
                for (int i = 0; i < fullNames.Length; i++) {
                    fullName = "";
                    fullName = fullNames[random.Next(0, 7)];
                    fullName += fullNames[random.Next(8, 15)];
                    fullName += fullNames[random.Next(16, 23)];
                    // textBoxGenerate.Text = "\n" + fullName ;

                }
                if (tableName == null && isInsert || tableName == "" && isInsert) return;
                if (isInsert) { textBoxGenerate.Text += "INSERT INTO " + tableName + " VALUES (" + fullName + "' , '" + GenerateNumber() + "'" + " , " + "'" + GenerateAddress() + "'" + " , " + GenerateBirthDate() + " , " + random.Next(100000, 999999) + ")\n"; }
                else { textBoxGenerate.Text += fullName + "' , '" + GenerateAddress() + "'" + " , " + "'" + GenerateNumber() + "'" + " , " + GenerateBirthDate() + " , " + random.Next(100000, 999999) + ")\n"; }
            }


        }
    }
}
