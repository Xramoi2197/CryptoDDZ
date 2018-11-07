using System;
using System.Collections.Generic;

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
                default: return;
            }
            if (_algorythm.Fill(parameters, WriteResult))
            {
                _algorythm.Do();
            }
        }

        public void DoCrypt(string algName, string text) //Функция выбирает нужный алгоритм и запускает его
        {
            //switch (algName)//Сюда необходимо добавить свой алгоритм
            //{
            //    case "Example":
            //    {
            //        _algorythm = new PresentAlg();                    
            //        break;
            //    }
            //    case "Shenks":
            //    {
            //        _algorythm = new ShenksAlgorithm();
            //        break;
            //    }
            //    case "Rsa":
            //    {
            //        _algorythm = new RsaAlgorithm();
            //        break;
            //    }
            //    default: return;
            //}
            if (_algorythm?.Name != algName)
            {
                WriteResult?.Invoke("Сначала необходимо выполнить алгоритм");
                return;
            }
            WriteText?.Invoke(_algorythm?.Crypt(text));
        }

        public void DoDecrypt(string algName, string text) //Функция выбирает нужный алгоритм и запускает его
        {
            //switch (algName)//Сюда необходимо добавить свой алгоритм
            //{
            //    case "Example":
            //    {
            //        _algorythm = new PresentAlg();
            //        break;
            //    }
            //    case "Shenks":
            //    {
            //        _algorythm = new ShenksAlgorithm();
            //        break;
            //    }
            //    case "Rsa":
            //    {
            //        _algorythm = new RsaAlgorithm();                    
            //        break;
            //    }
            //    default: return;
            //}
            if (_algorythm?.Name != algName)
            {
                WriteResult?.Invoke("Сначала необходимо выполнить алгоритм.");
                return;
            }
            WriteText?.Invoke(_algorythm?.DeCrypt(text));
        }
    }

    public abstract class Algorythm //От этого класса наследуются алгоритмы
    {
        public string Name;
        public abstract event Action<string> WriteResult; //Event для вывода на экран
        public abstract bool Fill(Dictionary<string, string> parameters, Action<string> writeAction); //Функция заполнения и проверки всех параметров алгоритма
        public abstract void Do(); //Функция выполняет алгоритм и вываодит его в TextBox
        public abstract string Crypt(string text); //Функция шифрует текст заданным ключем
        public abstract string DeCrypt(string text); //Функция расшифровывает текст заданным ключем
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

        public override string Crypt(string text)
        {
            return "Зашифровали";
        }

        public override string DeCrypt(string text)
        {
            return "Расшифровали";
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
            UInt64 result = (UInt64)x;
            UInt64 uP = (UInt64)p;
            for (int i = 0; i < y - 1; i++)
            {
                result = (result * (UInt64)x) % uP;
            }
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

        public override string Crypt(string text)
        {
            return "Зашифровали";
        }

        public override string DeCrypt(string text)
        {
            return "Расшифровали";
        }
    }

    /*Алгоритм RSA*/
    class RsaAlgorithm : Algorythm
    {
        public char[] Characters = new char[] {  'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И',
            'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С',
            'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ',
            'Э', 'Ю', 'Я', ' ', '1', '2', '3', '4', '5', '6', '7',
            '8', '9', '0' };
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

        public override string Crypt(string text)
        {
            return "Зашифровали";
        }

        public override string DeCrypt(string text)
        {
            return "Расшифровали";
        }
    }
}
