using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SQLGenerator.Classes {
    static class Cryptor {
        public static void CheckInputKey(TextBox textBox) {
            string str = textBox.Text;
            char[] strCharKey = str.ToCharArray();
            for (int i = 0; i < strCharKey.Length; i++) {
                if (strCharKey[i] != '0' && strCharKey[i] != '1') {
                    textBox.Text = str.Substring(0, str.Length - 1);
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }


        static string alph = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        static Random random = new Random();
        public static string Encryption(string text, int key) {
            var res = new StringBuilder();
            for (int i = 0; i < text.Length; i++) {
                for (int j = 0; j < alph.Length; j++) {
                    var ran = random.Next(0, 9);
                    if (text[i] == alph[j]) {
                        res.Append(alph[(j + key) % alph.Length]);
                    }
                    
                    res.Append(ran);
                }
                if (text[i] == '.') { res.Append('a'); }
                if (text[i] == '!') { res.Append('c'); }
                if (text[i] == ',') { res.Append('e'); }
                if (text[i] == ' ') { res.Append('p'); }
                if (text[i] == '?') { res.Append('x'); }
                Random random2 = new Random();
                for (int q = 0; q <= random.Next(5, 30); q++) {
                    Random random3 = new Random();
                    int ran = random3.Next(0, 9);
                    res.Append(ran);
                } 
            }
            return res.ToString();
        }
        public static string Decryption(string crypt, int key) {
            var res = new StringBuilder();
            for (int i = 0; i < crypt.Length; i++) {
                for (int j = 0; j < alph.Length; j++) {
                    if (crypt[i] == alph[j]) {
                        res.Append(alph[(j - key + alph.Length) % alph.Length]);
                    } 
                }
                if (crypt[i] == 'a') { res.Append('.'); }
                if (crypt[i] == 'c') { res.Append('!'); }
                if (crypt[i] == 'e') { res.Append(','); }
                if (crypt[i] == 'p') { res.Append(' '); }
                if (crypt[i] == 'x') { res.Append('?'); }
            }
            return res.ToString();
        }
    }
}
