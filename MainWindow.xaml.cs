using Microsoft.Win32;
using SQLGenerator.Classes;
using SQLGenerator.DataBaseNotes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using NewsAPI;
using NewsAPI.Models;
using NewsAPI.Constants;
using Newtonsoft.Json;

namespace SQLGenerator {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }


        int? noteId;


        private string toSqlFormat(string text) {
            return "'" + text + "', ";
        }
        private string toSqlFormat(string text, string text2, string text3) {
            return "'" + text + " " + text2 + " " + text3 + "' , ";
        }

        private void Button_Generate_Person_click(object sender, RoutedEventArgs e)
        {

            if (timesToGenerateUpDown.Value > 20 || timesToGenerateUpDown.Value <= 0) MessageBox.Show("Введите корректное число генерации", "Ошибка");
            if (textBoxTableName.Text == "" && (bool)checkBoxInsert.IsChecked || textBoxTableName.Text == null && (bool)checkBoxInsert.IsChecked)
            {
                MessageBox.Show("Введите корректное имя таблицы", "Ошибка");
                return;
            }
            for (int i = 0; i < timesToGenerateUpDown.Value; i++)
            {
                Person.Name person = GetterRandomPerson.getRandomPerson();
                if (person == null)
                {
                    MessageBox.Show("Проверьте подключение к интернету", "Ошибка");
                    return;
                }
                if ((bool)checkBoxInsert.IsChecked && (bool)checkBoxComma.IsChecked)
                {
                    textBoxGenerate.Text += "INSERT INTO " + textBoxTableName.Text + " VALUES (" + toSqlFormat(person.last) + toSqlFormat(person.first) + ")\n";
                }
                //else if ((bool)checkBoxInsert.IsChecked && !(bool)checkBoxComma.IsChecked)
                //{
                //    textBoxGenerate.Text += "INSERT INTO " + textBoxTableName.Text + " VALUES (" + toSqlFormat(person.lname, person.fname, person.patronymic) + toSqlFormat(person.login) + "'" + person.password + "'" + ")\n";
                //}
            }
        }



        //////////////////Events for Forms

        private void ButtonDecryptInFile_Click(object sender, RoutedEventArgs e) {
            if (textBoxEncryptSecTab.Text != "") {
                string cryptedFileText = Cryptor.Decryption(textBoxDectyptSecTab.Text.ToLower(), textBoxKeySecTab.Text.Length);
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
                Nullable<bool> result = saveFileDialog.ShowDialog();
                if (result == true) {
                    string fileName = saveFileDialog.FileName;
                    File.WriteAllText(fileName, cryptedFileText);
                }
            }
            else MessageBox.Show("В поле ничего нет.");
        }

        private void ButtonCryptInFile_Click(object sender, RoutedEventArgs e) {
            if (textBoxDecrypt.Text != "") {
                string cryptedFileText = Cryptor.Encryption(textBoxCrypt.Text.ToLower(), textBoxKey.Text.Length);
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
                Nullable<bool> result = saveFileDialog.ShowDialog();
                if (result == true) {
                    string fileName = saveFileDialog.FileName;
                    File.WriteAllText(fileName, cryptedFileText);
                }
            }
            else MessageBox.Show("В поле ничего нет.");


        }

        private void TextBoxKey_TextChanged(object sender, TextChangedEventArgs e) {
            Cryptor.CheckInputKey(textBoxKey);
        }

        private void TextBoxKeySecTab_TextChanged(object sender, TextChangedEventArgs e) {
            Cryptor.CheckInputKey(textBoxKeySecTab);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            textBoxGenerate.Clear();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e) {
            textBoxTableName.IsEnabled = true;

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e) {
            textBoxTableName.IsEnabled = false;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) {
            Why why = new Why();
            why.Show();
            this.Hide();
        }

        private void Window_Closed(object sender, EventArgs e) {
            Application.Current.Shutdown();
        }

        private void ButtonTranslate_Click(object sender, RoutedEventArgs e) {
            textBoxFromTranslate.Text = TranslateTask();
        }

        private void TextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            ProcessStartInfo info = new ProcessStartInfo() {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "https://translate.yandex.ru/"
            };
            Process.Start(info);
        }
        private void TextBoxToTranslate_TextChanged(object sender, TextChangedEventArgs e) {
            //if ((bool)isInstantlyTranslate.IsChecked) {
            //    if (e.Key == Key.Enter || e.Key == Key.Space || e.Key == Key.OemComma || e.Key == Key.OemQuestion || e.Key == Key.OemPeriod) {
            //        textBoxFromTranslate.Text = TranslateTask();
            //    }
            //}
        }

        private void CmbTranslateTo_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (cmbTranslateTo.Text != "" && cmbTranslateTo.Text != null)
                textBoxFromTranslate.Text = TranslateTask();
        }


        private void Image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) { //Swap languages and translations
            string translateFrom = textBoxFromTranslate.Text;
            string translateTo = textBoxToTranslate.Text;
            textBoxFromTranslate.Text = translateTo;
            textBoxToTranslate.Text = translateFrom;
            int indexFrom = cmbTranslateFrom.SelectedIndex;
            int indexTo = cmbTranslateTo.SelectedIndex;
            cmbTranslateFrom.SelectedIndex = indexTo;
            cmbTranslateTo.SelectedIndex = indexFrom;
            buttonTranslate.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void CmbTranslateFrom_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (cmbTranslateFrom.Text != "" && cmbTranslateFrom.Text != null)
                textBoxFromTranslate.Text = TranslateTask();
        }


        private void Button_crypt_Click(object sender, RoutedEventArgs e) { //CryptBtn
            if (textBoxKey.Text != "") {
                if (textBoxCrypt != null && textBoxCrypt.Text.Length != 0)
                    textBoxDecrypt.Text = Cryptor.Encryption(textBoxCrypt.Text.ToLower(), textBoxKey.Text.Length);
            }
            else MessageBox.Show("Введите двоичный ключ!");

        }

        private void ButtonDecrypt_Click(object sender, RoutedEventArgs e) {
            if (textBoxKeySecTab.Text != "") {
                if (textBoxDectyptSecTab != null && textBoxDectyptSecTab.Text.Length != 0)
                    textBoxEncryptSecTab.Text = Cryptor.Decryption(textBoxDectyptSecTab.Text.ToLower(), textBoxKeySecTab.Text.Length);
            }
            else MessageBox.Show("Введите двоичный ключ!");

        }

        private string TranslateTask() {
            string lang = Translator.GetLangTranslate(cmbTranslateTo.Text, cmbTranslateFrom.Text);
            string translate = Translator.Translate(textBoxToTranslate.Text, lang);
            return translate;
        }

        private void TextBoxToTranslate_KeyDown(object sender, KeyEventArgs e) {
            if ((bool)isInstantlyTranslate.IsChecked) {
                if (e.Key == Key.Enter || e.Key == Key.Space || e.Key == Key.OemComma || e.Key == Key.OemQuestion || e.Key == Key.OemPeriod) {
                    textBoxFromTranslate.Text = TranslateTask();
                }
            }
            if (e.Key == Key.Enter) textBoxFromTranslate.Text = TranslateTask();
        }

        private void TextBoxToTranslate_PreviewKeyDown(object sender, KeyEventArgs e) {
            if ((bool)isInstantlyTranslate.IsChecked) {
                if (e.Key == Key.Enter || e.Key == Key.Space || e.Key == Key.OemComma || e.Key == Key.OemQuestion || e.Key == Key.OemPeriod) {
                    textBoxFromTranslate.Text = TranslateTask();
                }
            }
            if (e.Key == Key.Enter) textBoxFromTranslate.Text = TranslateTask();

        }



        private void Window_Loaded(object sender, RoutedEventArgs e) {
            ListBoxNotes.Items.Clear();
            var notes = DbNoteHelper.getSortNotesDate(false);
            foreach (var note in notes) {
                ListBoxNotes.Items.Add(note.Note_title);
            }

        }

        private void ButtonAddNote_Click(object sender, RoutedEventArgs e) {
            if (textBoxNoteTitle.Text == "" || !NotePriorityUpDown.Value.HasValue) {
                MessageBox.Show("Ошибка при добавлении заметки.\nЗаметка должна иметь заголовок.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            using (DBContext db = new DBContext()) {
                if (noteId == null) {
                    Note note = new Note();
                    note.Note_title = textBoxNoteTitle.Text;
                    note.Note_text = textBoxNoteText.Text;
                    note.Note_tag = textBoxNoteTag.Text;
                    note.Note_priority = (int)NotePriorityUpDown.Value;
                    try {
                        db.Notes.Add(note);
                        db.SaveChanges();
                        sortNotes((bool)IsDescending.IsChecked, ListBoxNotes);
                    }
                    catch (Exception) {
                        MessageBox.Show("Произошла ошибка при добавлении. \nОбратите внимание на то, что заголовки заметок не могут быть одинаковыми.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else if (noteId != null) {
                    DbNoteHelper.AlterNote(textBoxNoteTitle.Text, textBoxNoteTag.Text, (int)NotePriorityUpDown.Value, textBoxNoteText.Text, noteId);
                    sortNotes((bool)IsDescending.IsChecked, ListBoxNotes);
                }

            }


        }

        private void ListBoxNotes_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            using (DBContext db = new DBContext()) {
                try {
                    var notes = db.Notes.ToList();
                    noteId = (from not in db.Notes where not.Note_title == ListBoxNotes.SelectedItem.ToString() select not.Note_id).SingleOrDefault(); //finnaly working!
                    Note note = (from n in db.Notes where n.Note_id == noteId select n).SingleOrDefault<Note>();
                    textBoxNoteTag.Text = note.Note_tag;
                    textBoxNoteText.Text = note.Note_text;
                    textBoxNoteTitle.Text = note.Note_title;
                    NotePriorityUpDown.Value = note.Note_priority;
                    textBlockNoteDate.Text = "Дата создания заметки: " + note.Note_date;
                    textBlockNoteEdit.Text = "Дата последнего редактирования заметки: " + note.Note_edit_date;

                }
                catch (Exception) {
                    return;
                }
            }

        }

        private void Image_PreviewMouseLeftButtonDown_1(object sender, MouseButtonEventArgs e) {
            sortNotes((bool)IsDescending.IsChecked, ListBoxNotes);
        }

        private void TextBoxSearchNote_TextChanged(object sender, TextChangedEventArgs e) {
            if (textBoxSearchNote.Text == "") {
                sortNotes((bool)IsDescending.IsChecked, ListBoxNotes);
                return;
            }
            if (textBoxSearchNote.Text == "Search...") return;
            ListBoxNotes.Items.Clear();
            using (DBContext db = new DBContext()) {
                var notes = db.Notes.Where(t => t.Note_title.Contains(textBoxSearchNote.Text));
                foreach (var note in notes) {
                    ListBoxNotes.Items.Add(note.Note_title);
                }
            }
        }
        #region


        private void TextBoxNoteText_LostFocus(object sender, RoutedEventArgs e) {
            if (textBoxNoteText.Text == "") textBoxNoteText.Text = "Enter new note...";
        }

        private void TextBoxNoteTitle_LostFocus(object sender, RoutedEventArgs e) {
            if (textBoxNoteTitle.Text == "") textBoxNoteTitle.Text = "Enter title...";
        }

        private void TextBoxNoteTag_LostFocus(object sender, RoutedEventArgs e) {
            if (textBoxNoteTag.Text == "") textBoxNoteTag.Text = "Enter tag...";

        }
        private void TextBoxSearchNote_LostFocus(object sender, RoutedEventArgs e) {
            if (textBoxSearchNote.Text == "") textBoxSearchNote.Text = "Search...";
        }

        #endregion

        private void TextBoxNoteText_TextChanged(object sender, TextChangedEventArgs e) {
            using (DBContext db = new DBContext()) {

            }
        }

        private void Image_PreviewMouseLeftButtonDown_2(object sender, MouseButtonEventArgs e) {
            SetNotesTeamplate();
            ListBoxNotes.SelectedItem = null;
            textBoxNoteTitle.Focus();
        }
        private void SetNotesTeamplate() {
            noteId = null;
            textBoxNoteTag.Text = "Enter tag...";
            textBoxNoteTitle.Text = "Enter title...";
            textBoxNoteText.Text = "Enter new note...";
            NotePriorityUpDown.Value = 1;
            textBlockNoteDate.Text = "";
            textBlockNoteEdit.Text = "";
        }
        private void Image_PreviewMouseLeftButtonDown_3(object sender, MouseButtonEventArgs e) {
            ListBoxNotes.SelectedIndex = ListBoxNotes.SelectedIndex - 1;
            DbNoteHelper.DeleteNote(noteId);
            sortNotes((bool)IsDescending.IsChecked, ListBoxNotes);
            SetNotesTeamplate();

        }

        private void TextBoxNoteText_GotFocus(object sender, RoutedEventArgs e) {
            if (textBoxNoteText.Text == "Enter new note...") textBoxNoteText.Clear();
        }

        private void TextBoxNoteTitle_GotFocus(object sender, RoutedEventArgs e) {
            if (textBoxNoteTitle.Text == "Enter title...") textBoxNoteTitle.Clear();
        }

        private void TextBoxNoteTag_GotFocus(object sender, RoutedEventArgs e) {
            if (textBoxNoteTag.Text == "Enter tag...") textBoxNoteTag.Clear();
        }

        private void TextBoxSearchNote_GotFocus(object sender, RoutedEventArgs e) {
            if (textBoxSearchNote.Text == "Search...") textBoxSearchNote.Clear();
        }


        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            try {
                sortNotes((bool)IsDescending.IsChecked, ListBoxNotes);
            }
            catch (Exception) {
                return;
            }
        }

        private void IsDescending_Checked(object sender, RoutedEventArgs e) {
            try {
                sortNotes((bool)IsDescending.IsChecked, ListBoxNotes);
            }
            catch (Exception) {
                return;
            }
        }

        private void IsDescending_Unchecked(object sender, RoutedEventArgs e) {
            try {
                sortNotes((bool)IsDescending.IsChecked, ListBoxNotes);
            }
            catch (Exception) {
                return;
            }
        }

        private void sortNotes(bool isDescending, ListBox listBox) {
            List<Note> notes;
            try {
                ListBoxNotes.Items.Clear();
                switch (comboBoxSort.SelectedIndex) {
                    case 0:
                        notes = DbNoteHelper.getSortNotesDate(!isDescending);
                        break;
                    case 1:
                        notes = DbNoteHelper.getSortNotesTitle(!isDescending);
                        break;
                    case 2:
                        notes = DbNoteHelper.getSortNotesPriority(!isDescending);
                        break;
                    default:
                        notes = DbNoteHelper.getSortNotesDate(true);
                        break;
                }
                foreach (var note in notes) {
                    listBox.Items.Add(note.Note_title);
                }
            }
            catch (Exception) {
                return;

            }
        }

        private void Image_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            ListBoxNotes.Focus();
            try {
                ListBoxNotes.SelectedIndex += 1;
            }
            catch (Exception) {
                return;
            } 
        }

        private void YandexImage_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            yandexRadio.IsChecked = true;
        }

        private void GoogleImage_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            googleRadio.IsChecked = true;
        }

        private void MailImage_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            mailRadio.IsChecked = true;
        }

        private void YahooImage_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            yahooRadio.IsChecked = true;
        }

        private void TextBoxInternetSearch_GotFocus(object sender, RoutedEventArgs e) {
            if (textBoxInternetSearch.Text == "Поиск...") {
                textBoxInternetSearch.Clear();
            }
            textBoxInternetSearch.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
        }

        private void TextBoxInternetSearch_LostFocus(object sender, RoutedEventArgs e) {
            if (textBoxInternetSearch.Text == "") {
                textBoxInternetSearch.Text = "Поиск...";
                textBoxInternetSearch.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F000000"));
            }
               

        }

        private void Image_PreviewMouseLeftButtonDown_4(object sender, MouseButtonEventArgs e) {
            if (textBoxInternetSearch.Text.Length == 0 || textBoxInternetSearch.Text == "" || textBoxInternetSearch.Text == "Поиск...") return;
            string googleQuery = "https://www.google.com/search?q=";
            string yandexQuery = "https://yandex.ru/search/?lr=217&text=";
            string yahooQuery = "https://search.yahoo.com/search?q=";
            string mailQuery = "https://go.mail.ru/search?q=";

            if ((bool)yandexRadio.IsChecked) Process.Start(yandexQuery + textBoxInternetSearch.Text);

            if ((bool)googleRadio.IsChecked) Process.Start(googleQuery + textBoxInternetSearch.Text);

            if ((bool)yahooRadio.IsChecked) Process.Start(yahooQuery + textBoxInternetSearch.Text);

            if ((bool)mailRadio.IsChecked) Process.Start(mailQuery + textBoxInternetSearch.Text);

        }

        private void Canvas_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                if (textBoxInternetSearch.Text.Length == 0 || textBoxInternetSearch.Text == "" || textBoxInternetSearch.Text == "Поиск...") return;
                string googleQuery = "https://www.google.com/search?q=";
                string yandexQuery = "https://yandex.ru/search/?lr=217&text=";
                string yahooQuery = "https://search.yahoo.com/search?q=";
                string mailQuery = "https://go.mail.ru/search?q=";

                if ((bool)yandexRadio.IsChecked) Process.Start(yandexQuery + textBoxInternetSearch.Text);

                if ((bool)googleRadio.IsChecked) Process.Start(googleQuery + textBoxInternetSearch.Text);

                if ((bool)yahooRadio.IsChecked) Process.Start(yahooQuery + textBoxInternetSearch.Text);

                if ((bool)mailRadio.IsChecked) Process.Start(mailQuery + textBoxInternetSearch.Text);
            }
            else return;
        }

        private void buttonNewsTest_Click(object sender, RoutedEventArgs e) {
            News.News news = News.NewsGetter.GetNews();
            if (news.status == "ok") {
                foreach (var article in news.articles) {
                    listBoxNews.Items.Add(article.title);
                }
            }
            
            
        }
    }


}
