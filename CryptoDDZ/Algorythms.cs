﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Windows.Automation.Peers;

namespace CryptoDDZ
{
    class Algorythms
    {
        private Algorythm _algorythm;
        public event Action<string> WriteResult;
        public event Action<string> WriteText;

        public Algorythms(Action<string> writeAction, Action<string> writeTextAction)
        {
            _algorythm = null;
            WriteResult += writeAction;
            WriteText += writeTextAction;
        }

        public void DoAlg(string algName, Dictionary<string,string> parameters) //Функция выбирает нужный алгоритм и запускает его
        {
            switch (algName)//Сюда необходимо добавить свой алгоритм
            {
                case "Example":
                {
                    _algorythm = new PresentAlg();
                    break;
                }
                case "Shenks":
                {
                    _algorythm = new ShenksAlgorithm();                   
                    break;
                }
                case "Rsa":
                {
                    _algorythm = new RsaAlgorithm();
                    break;
                }
                case "DiffiHelman":
                {
                    _algorythm = new DiffiHelmanAlg();
                    break;
                }
                default: return;
            }
            if (_algorythm.Fill(parameters, WriteResult))
            {
                _algorythm.Do();
            }
        }

        public void DoCrypt(string algName, string text, Dictionary<string, string> parameters) //Функция выбирает нужный алгоритм и запускает его
        {           
            if (_algorythm?.Name != algName)
            {
                WriteResult?.Invoke("Сначала необходимо выполнить алгоритм");
                return;
            }
            WriteText?.Invoke(_algorythm?.Crypt(text, parameters));
        }

        public void DoDecrypt(string algName, string text, Dictionary<string, string> parameters) //Функция выбирает нужный алгоритм и запускает его
        {
            if (_algorythm?.Name != algName)
            {
                WriteResult?.Invoke("Сначала необходимо выполнить алгоритм.");
                return;
            }
            WriteText?.Invoke(_algorythm?.DeCrypt(text, parameters));
        }
    }

    public abstract class Algorythm //От этого класса наследуются алгоритмы
    {
        public string Name;
        public abstract event Action<string> WriteResult; //Event для вывода на экран
        public abstract bool Fill(Dictionary<string, string> parameters, Action<string> writeAction); //Функция заполнения и проверки всех параметров алгоритма
        public abstract void Do(); //Функция выполняет алгоритм и вываодит его в TextBox
        public abstract string Crypt(string text, Dictionary<string, string> parameters); //Функция шифрует текст заданным ключем
        public abstract string DeCrypt(string text, Dictionary<string, string> parameters); //Функция расшифровывает текст заданным ключем
    }

    /*Пример алгоритма*/
    public class PresentAlg : Algorythm
    {
        public override event Action<string> WriteResult;
        private int _x;
        private int _y;

        public override bool Fill(Dictionary<string, string> parameters, Action<string> writeAction)
        {
            Name = "Example";
            WriteResult += writeAction;
            if (parameters.Count != 2)
            {
                return false;
            }
            string a;
            parameters.TryGetValue("x", out a);
            if (a == null)
            {
                WriteResult?.Invoke("Параметр x не задан!");
                return false;
            }
            if (!int.TryParse(a, out _x))
            {
                WriteResult?.Invoke("Параметр x не удалось привести к int!");
                return false;
            }
            parameters.TryGetValue("y", out a);
            if (a == null)
            {
                WriteResult?.Invoke("Параметр y не задан!");
                return false;
            }
            if (!int.TryParse(a, out _y))
            {
                WriteResult?.Invoke("Параметр y не удалось привести к int!");
                return false;
            }
            return true;
        }

        public override void Do()
        {
            WriteResult?.Invoke("x=" + _x);
            WriteResult?.Invoke("y=" + _y);
            int rez = _x + _y;
            WriteResult?.Invoke("x+y=" + rez);

        }

        public override string Crypt(string text, Dictionary<string, string> parameters)
        {
            return "Не реализовано";
        }

        public override string DeCrypt(string text, Dictionary<string, string> parameters)
        {
            return "Не реализовано";
        }
    }

    /*Алгоритм Шенкса(Шаг младенца, шаг великана)*/
    public class ShenksAlgorithm : Algorythm
    {
        public override event Action<string> WriteResult;
        private int _p;
        private int _a;
        private int _b;
        private int _m;
        private int _k;

        public static UInt64 MyPow(int x, int y, int p)
        {
            UInt64 result = (UInt64)BigInteger.ModPow(x, y, p);//(UInt64)x;
            //UInt64 uP = (UInt64)p;
            //for (int i = 0; i < y - 1; i++)
            //{
            //    result = (result * (UInt64)x) % uP;
            //}

            return result;
        }

        public override bool Fill(Dictionary<string, string> parameters, Action<string> writeAction)
        {
            Name = "Shenks";
            WriteResult += writeAction;
            if (parameters.Count != 5)
            {
                return false;
            }
            string helpStr;
            parameters.TryGetValue("p", out helpStr);
            if (helpStr == null)
            {
                WriteResult?.Invoke("Параметр p не задан!");
                return false;
            }
            if (!int.TryParse(helpStr, out _p))
            {
                WriteResult?.Invoke("Параметр p не удалось привести к int!");
                return false;
            }
            if (_p <= 0)
            {
                WriteResult?.Invoke("Параметр p задан не верно!");
            }
            parameters.TryGetValue("a", out helpStr);
            if (helpStr == null)
            {
                WriteResult?.Invoke("Параметр a не задан!");
                return false;
            }
            if (!int.TryParse(helpStr, out _a))
            {
                WriteResult?.Invoke("Параметр a не удалось привести к int!");
                return false;
            }
            if (_a <= 0)
            {
                WriteResult?.Invoke("Параметр a задан не верно!");
            }
            parameters.TryGetValue("b", out helpStr);
            if (helpStr == null)
            {
                WriteResult?.Invoke("Параметр b не задан!");
                return false;
            }
            if (!int.TryParse(helpStr, out _b))
            {
                WriteResult?.Invoke("Параметр b не удалось привести к int!");
                return false;
            }
            if (_b <= 0)
            {
                WriteResult?.Invoke("Параметр b задан не верно!");
            }
            parameters.TryGetValue("m", out helpStr);
            if (helpStr == null)
            {
                WriteResult?.Invoke("Параметр m не задан!");
                return false;
            }
            if (!int.TryParse(helpStr, out _m))
            {
                WriteResult?.Invoke("Параметр m не удалось привести к int!");
                return false;
            }
            parameters.TryGetValue("k", out helpStr);
            if (helpStr == null)
            {
                WriteResult?.Invoke("Параметр k не задан!");
                return false;
            }
            if (!int.TryParse(helpStr, out _k))
            {
                WriteResult?.Invoke("Параметр k не удалось привести к int!");
                return false;
            }
            if (_m * _k < _p)
            {
                WriteResult?.Invoke("Произвдение m*k меньше p!");
                return false;
            }
            return true;
        }

        public override void Do()
        {
            WriteResult?.Invoke("p=" + _p);
            WriteResult?.Invoke("a=" + _a);
            WriteResult?.Invoke("b=" + _b);
            WriteResult?.Invoke("m=" + _m);
            WriteResult?.Invoke("k=" + _k);
            
            int I = 0;
            // ReSharper disable once InconsistentNaming
            int J = 1;
            var mult = _k * _m;
            WriteResult?.Invoke("Произведение m и k равно " + mult);
            if (mult < _p)
            {
                return;
            }
            Dictionary<int, double> numer1 = new Dictionary<int, double>();
            Dictionary<int, double> numer2 = new Dictionary<int, double>();
            for (int i = 0; i < _m; i++)
            {
                var rez = ((int)MyPow(_a, i, _p) * _b) % _p;//(Math.Pow(_a, i) * _b) % _p;
                numer1.Add(i, rez);
            }
            for (int i = 1; i < _k + 1; i++)
            {
                var rez = MyPow(_a, i * _m, _p);
                numer2.Add(i, rez);
            }
            
            bool flag = false;
            for (int j = 1; j < _k + 1; j++)
            {
                for (int i = 1; i < _m; i++)
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (numer1[i] == numer2[j])
                    {
                        J = i;
                        I = j;
                        flag = true;
                        break;
                    }
                    if (flag)
                    {
                        break;
                    }
                }
            }
            var x = I * _m - J;
            if (x == -1)
            {
                WriteResult?.Invoke("Уравнение не имеет решений!");
                return;
            }
            WriteResult?.Invoke("Первый ряд:");
            var numerable1 = string.Empty;
            for (int i = 0; i < _m; i++)
            {
                if (i == J)
                {
                    numerable1 += "[" + numer1[i] + "] ";
                    continue;
                }
                numerable1 += numer1[i] + " ";
            }
            WriteResult?.Invoke(numerable1);
            WriteResult?.Invoke("Второй ряд:");
            var numerable2 = string.Empty;
            for (int i = 1; i < _k + 1; i++)
            {
                if (i == I)
                {
                    numerable2 += "[" + numer2[i] + "] ";
                    continue;
                }
                numerable2 += numer2[i] + " ";
            }
            WriteResult?.Invoke(numerable2);

            WriteResult?.Invoke("i=" + I);
            WriteResult?.Invoke("j=" + J);
            
            WriteResult?.Invoke("x=i*m-j=" + x);
            UInt64 test = MyPow(_a, x, _p);
            if (test==(UInt64)_b)
            {
                WriteResult?.Invoke("true");
            }
            
        }

        public override string Crypt(string text, Dictionary<string, string> parameters)
        {
            return "Не реализовано";
        }

        public override string DeCrypt(string text, Dictionary<string, string> parameters)
        {
            return "Не реализовано";
        }
    }

    /*Алгоритм RSA*/
    class RsaAlgorithm : Algorythm
    {
        string _characters;
        public override event Action<string> WriteResult;
        // ReSharper disable once InconsistentNaming
        private long _N;
        private long _p;
        private long _q;
        private long _e;
        private long _d;

        private long GenEuclidAlgorithm(long a, long b)
        {
            long q, p = 0;
            long u1 = a, u2 = 1, u3 = 0;
            long v1 = b, v2 = 0, v3 = 1;
            long t1 = 1, t2, t3 = 1;
            while (t1 > 0)
            {
                q = u1 / v1;
                t1 = u1 % v1;
                t2 = u2 - v2 * q;
                p = t3;
                if (p < 0)
                {
                    p = p + a;
                }

                t3 = u3 - q * v3;
                u1 = v1;
                u2 = v2;
                u3 = v3;
                v1 = t1;
                v2 = t2;
                v3 = t3;
            }
            return p;
        }

        // ReSharper disable once InconsistentNaming
        private bool IsSimple(long N)
        {
            for (uint i = 2; i < (uint)(N / 2); i++)
            {
                if (N % 2 == 0)
                    return false;
            }

            return true;
        }

        // ReSharper disable once InconsistentNaming
        private void Factorization(long N, out long rez1, out long rez2)
        {
            long p, q = 0;
            for (p = 2; p <= (long)Math.Pow(N, 0.5); p++)
            {
                if (IsSimple(p) && N % p == 0 && IsSimple(N / p))
                {
                    q = N / p;
                    break;
                }
            }
            rez1 = p;
            rez2 = q;
            var msg = "После факторизации число N = " + p + " * " + q + ". ";
            WriteResult?.Invoke(msg);
        }

        private long EulerFunction(long p, long q)
        {
            var result = (p - 1) * (q - 1);
            var msg = "Функция Эйлера от N равна: ( " + p + " - 1 ) * ( " + q + " - 1 ) = " + result;
            WriteResult?.Invoke(msg);
            return result;
        }
        
        public override bool Fill(Dictionary<string, string> parameters, Action<string> writeAction)
        {
            _characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Name = "Rsa";
            _N = 0;
            _p = 0;
            _q = 0;
            _e = 0;
            _d = 0;
            if (parameters.Count != 2)
            {
                return false;
            }
            WriteResult += writeAction;
            string helpStr;
            parameters.TryGetValue("N", out helpStr);
            if (helpStr == null)
            {
                WriteResult?.Invoke("Параметр N не задан!");
                return false;
            }
            if (!long.TryParse(helpStr, out _N))
            {
                WriteResult?.Invoke("Параметр N не удалось привести к long!");
                return false;
            }
            if (_N <= 0)
            {
                WriteResult?.Invoke("Параметр N отрицателен.");
            }
            parameters.TryGetValue("e", out helpStr);
            if (helpStr == null)
            {
                WriteResult?.Invoke("Параметр e не задан!");
                return false;
            }
            if (!long.TryParse(helpStr, out _e))
            {
                WriteResult?.Invoke("Параметр e не удалось привести к long!");
                return false;
            }
            if (_e <= 0)
            {
                WriteResult?.Invoke("Параметр e отрицателен.");
            }
            return true;
        }

        public override void Do()
        {
            Factorization(_N, out _p, out _q);
            _d = GenEuclidAlgorithm(EulerFunction(_p, _q), _e);
            var msg = "Закрытая экспонента равна: " + _d;
            WriteResult?.Invoke(msg);
        }

        private string RSA_Endoce_Bigramm(string text, long e, long n)
        {
            string result = "";
            BigInteger bi;
            WriteResult?.Invoke("Длина текста " + text.Length);
            WriteResult?.Invoke("Для каждой пары символов считаем:");
            for (int i = 0; i < text.Length; i += 2)
            {
                int index1 = _characters.IndexOf(text[i]);
                if (i == text.Length - 1)
                {
                    WriteResult?.Invoke("\nШифруем один символ с индексом " + index1);
                    bi = BigInteger.Pow(index1, (int)e);
                    WriteResult?.Invoke("Возводим индекс " + index1 + " умноженный на длину алфавита " + _characters.Length + " в степень e " + e + " = " + bi);
                }
                else
                {
                    int index2 = _characters.IndexOf(text[i + 1]);
                    WriteResult?.Invoke("\nШифруем два символа: с индексами " + index1 + " " + index2);
                    bi = BigInteger.Pow(index2 + index1 * _characters.Length, (int)e);
                    WriteResult?.Invoke("Умножаем индекс первого символа на длину алфавита " + index1 * _characters.Length + " складываем с индексом второго символа " + index2 + " = " + (index1 * _characters.Length + index2));
                    WriteResult?.Invoke("Возводим сумму " + (index1 * _characters.Length + index2) + " в степень e " + e + " = " + bi);
                }
                BigInteger n_ = new BigInteger((int)n);
                bi = bi % n_;
                WriteResult?.Invoke("Находим остаток от деления полученного числа на N " + n_ + " = " + bi);
                result += bi.ToString();
                if (i != text.Length - 1)
                    result += ' ';
                WriteResult?.Invoke("Полученное число - зашифрованный биграмм записываем в результат: " + result);
            }

            return result;
        }

        private string RSA_Endoce_Symbol(string text, long e, long n)
        {
            string result = "";
            BigInteger bi;
            WriteResult?.Invoke("Длина текста " + text.Length);
            WriteResult?.Invoke("Для каждого символа считаем:");
            for (int i = 0; i < text.Length; ++i)
            {
                int index1 = _characters.IndexOf(text[i]);
                WriteResult?.Invoke("\nИндекс символа в алфавите " + index1);

                bi = BigInteger.Pow(index1 * _characters.Length, (int)e);
                WriteResult?.Invoke("Возводим индекс " + index1 + " умноженный на длину алфавита " + _characters.Length + " в степень e " + e + " = " + bi);
                BigInteger n_ = new BigInteger((int)n);
                bi = bi % n_;
                WriteResult?.Invoke("Находим остаток от деления полученного числа на N " + n_ + " = " + bi);

                result += bi.ToString();
                if (i != text.Length)
                    result += ' ';
                WriteResult?.Invoke("Полученное число - зашифрованный символ записываем в результат: " + result);

            }

            return result;
        }
        public override string Crypt(string text, Dictionary<string, string> parameters)
        {//N e - открытый ключ
            WriteResult?.Invoke("\n\nЗапустили шифрование:");
            if (parameters == null)
            {
                return "Не заданы параметры";
            }

            if (text.Length == 0)
                return "Нет текста!";
            text = text.ToUpper();
            string pattern = @"^[A-Z]+$";
            Regex reg = new Regex(pattern);
            if (!reg.IsMatch(text))
            {
                return "Текст не соответствует алфавиту A-Z!";
            }
            

            if (parameters["num"] == "1")
            {
                if (_N != 0)
                {
                    //var upper = text.ToUpper();

                    string result = "";
                    string help = RSA_Endoce_Symbol(text, _e, _N);
                    int num = help.Length;
                    for (int i = 0; i < num - 1; i++)
                    {
                        result += help[i];
                    }
                    return result;
                }
            }

            if (parameters["num"] == "2")
            {
                if (text.Length % 2 > 0)
                {
                    WriteResult?.Invoke("Так как символов нечетное количество допишем в конец строки Z.");
                    text +="Z";
                }

                if (_N != 0)
                {
                    //var upper = text.ToUpper();

                    string result = "";
                    string help = RSA_Endoce_Bigramm(text, _e, _N);
                    int num = help.Length;
                    for (int i = 0; i < num - 1; i++)
                    {
                        result += help[i];
                    }
                    return result;
                }
                return "N = 0";
            }
            return "Неправильно выбраны параметры";
        }

        private string RSA_Dedoce_Bigram(string input, long d, long n)
        {
            string result = "";
            List<string> helpList = new List<string>(input.Split(' '));
            WriteResult?.Invoke("Количество шифрованных биграм " + helpList.Count);
            WriteResult?.Invoke("Для каждого числа считаем:");
            foreach (string item in helpList)
            {
                try
                {
                    BigInteger bi = BigInteger.Pow(ulong.Parse(item), (int)d);
                    WriteResult?.Invoke("\nВозводим число " + item + " в степень d " + d);
                    BigInteger n_ = new BigInteger((int)n);

                    bi = bi % n_;
                    WriteResult?.Invoke("Считаем остаток от деления полученного числа на N " + n_ + " = " + bi);
                    int index2 = (int)(bi % _characters.Length);
                    int index1 = (int)(bi / _characters.Length);
                    WriteResult?.Invoke("Делим " + bi + " на размер алфавита " + _characters.Length + " получаем индекс алфавита первого символа " + index1 + " переводим в алфавит " + _characters[index1].ToString());
                    WriteResult?.Invoke("Берем остаток от деления " + bi + " на размер алфавита " + _characters.Length + " получаем индекс алфавита второго символа " + index2 + " переводим в алфавит " + _characters[index2].ToString());


                    result += _characters[index1].ToString() + _characters[index2].ToString();
                    WriteResult?.Invoke("Полученный биграмм записываем в результат " + result);
                }
                catch (Exception e)
                {
                    return "Неверно задан ключ";
                }
                
            }

            return result;
        }

        private string RSA_Dedoce_Symbol(string input, long d, long n)
        {
            string result = "";
            List<string> helpList = new List<string>(input.Split(' '));
            WriteResult?.Invoke("Количество шифрованных символов " + helpList.Count);
            WriteResult?.Invoke("Для каждого числа считаем:");
            foreach (string item in helpList)
            {
                try
                {
                    BigInteger bi = BigInteger.Pow(ulong.Parse(item), (int)d);
                    BigInteger n_ = new BigInteger((int)n);
                    WriteResult?.Invoke("\nВозводим число " + item + " в степень d " + d);
                    bi = bi % n_;
                    WriteResult?.Invoke("Считаем остаток от деления полученного числа на N " + n_ + " = " + bi);
                    int index1 = (int)(bi / _characters.Length);
                    WriteResult?.Invoke("Делим " + bi + " на размер алфавита " + _characters.Length + " получаем индекс алфавита символа " + index1 + " переводим в алфавит " + _characters[index1].ToString());
                    result += _characters[index1].ToString();
                    WriteResult?.Invoke("Полученный символ записываем в результат " + result);
                }
                catch (Exception e)
                {
                    return "Неверно задан ключ";
                }
                
            }

            return result;
        }
        public override string DeCrypt(string text, Dictionary<string, string> parameters)
        {//N d - Закрытый ключ
            WriteResult?.Invoke("\n\nЗапустили дешифрование:");
            if (parameters == null)
            {
                return "Не заданы параметры";
            }

            if (text.Length == 0)
                return "Нет текста!";

            string pattern = @"\s+";
            string target = " ";
            Regex regex = new Regex(pattern);
            text = regex.Replace(text, target);

            pattern = @"^\s";
            target = "";
            regex = new Regex(pattern);
            text = regex.Replace(text, target);

            pattern = @"\s$";
            target = "";
            regex = new Regex(pattern);
            text = regex.Replace(text, target);

            pattern = @"^(\d+ {1})+(\d)";
            regex = new Regex(pattern);
            if (!regex.IsMatch(text))
            {
                return "Текст не соответствует формату";
            }

            if (parameters["num"] == "1")
            {
                if (_N != 0)
                {
                    string result = RSA_Dedoce_Symbol(text, _d, _N);

                    return result;
                }
                return "N = 0";
            }

            if (parameters["num"] == "2")
            {
                if (_N != 0)
                {
                    string result = RSA_Dedoce_Bigram(text, _d, _N);

                    return result;
                }
                return "N = 0";
            }
            return "Неправильно выбраны параметры";
        }
    }

    class DiffiHelmanAlg : Algorythm
    {
        public override event Action<string> WriteResult;
        private UInt64 _g;
        private UInt64 _p;
        private UInt64 _a;
        private UInt64 _b;

        public static UInt64 MyPow(UInt64 x, UInt64 y, UInt64 p)
        {
            UInt64 result = (UInt64) BigInteger.ModPow(x, y, p);
            return result;
        }

        public override string Crypt(string text, Dictionary<string, string> parameters)
        {
            return "Не реализовано";
        }

        public override string DeCrypt(string text, Dictionary<string, string> parameters)
        {
            return "Не реализовано";
        }

        public override void Do()
        {
            var msg = "g=" + _g;
            WriteResult?.Invoke(msg);
            msg = "p=" + _p;
            WriteResult?.Invoke(msg);
            msg = "a=" + _a;
            WriteResult?.Invoke(msg);
            msg = "b=" + _b;
            WriteResult?.Invoke(msg);

            UInt64 A = MyPow(_g, _a, _p);
            msg = "1)\n(1)A=g^a modp=" + A;
            WriteResult?.Invoke(msg);
            UInt64 B = MyPow(_g, _b, _p);
            msg = "(2)B=g^b modp=" + B;
            WriteResult?.Invoke(msg);
            UInt64 B2 = MyPow(B, _a, _p);
            msg = "2)\n(3)B^a modp=g^ab modp=" + B2;
            WriteResult?.Invoke(msg);
            UInt64 A2 = MyPow(A, _b, _p);
            msg = "(4)A^b modp=g^ab modp=" + A2;
            WriteResult?.Invoke(msg);
            if (A2!=B2)
            {
                return;
            }
            ulong K = A2;
            msg = "3)\n(5)K=g^ab modp=" + K;
            WriteResult?.Invoke(msg);
        }

        public override bool Fill(Dictionary<string, string> parameters, Action<string> writeAction)
        {
            Name = "DiffiHelman";
            _g = 0;
            _p = 0;
            _a = 0;
            _b = 0;
            if (parameters.Count != 4)
            {
                return false;
            }
            WriteResult += writeAction;
            string helpStr;
            parameters.TryGetValue("g", out helpStr);
            if (helpStr == null)
            {
                WriteResult?.Invoke("Параметр g не задан!");
                return false;
            }
            if (!UInt64.TryParse(helpStr, out _g))
            {
                WriteResult?.Invoke("Параметр g не удалось привести к long!");
                return false;
            }
            if (_g <= 0)
            {
                WriteResult?.Invoke("Параметр g отрицателен.");
            }
            parameters.TryGetValue("p", out helpStr);
            if (helpStr == null)
            {
                WriteResult?.Invoke("Параметр p не задан!");
                return false;
            }
            if (!UInt64.TryParse(helpStr, out _p))
            {
                WriteResult?.Invoke("Параметр p не удалось привести к long!");
                return false;
            }
            if (_p <= 0)
            {
                WriteResult?.Invoke("Параметр p отрицателен.");
            }
            parameters.TryGetValue("a", out helpStr);
            if (helpStr == null)
            {
                WriteResult?.Invoke("Параметр a не задан!");
                return false;
            }
            if (!UInt64.TryParse(helpStr, out _a))
            {
                WriteResult?.Invoke("Параметр a не удалось привести к long!");
                return false;
            }
            if (_a <= 0)
            {
                WriteResult?.Invoke("Параметр a отрицателен.");
            }
            parameters.TryGetValue("b", out helpStr);
            if (helpStr == null)
            {
                WriteResult?.Invoke("Параметр b не задан!");
                return false;
            }
            if (!UInt64.TryParse(helpStr, out _b))
            {
                WriteResult?.Invoke("Параметр b не удалось привести к long!");
                return false;
            }
            if (_b <= 0)
            {
                WriteResult?.Invoke("Параметр b отрицателен.");
            }
            return true;
        }
    }
}
