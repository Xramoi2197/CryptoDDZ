using System;
using System.Collections.Generic;

namespace CryptoDDZ
{
    class Algorythms
    {
        private Algorythm _algorythm;
        public event Action<string> WriteResult;

        public Algorythms(Action<string> writeAction)
        {
            WriteResult += writeAction;
        }

        public void DoAlg(string algName, Dictionary<string,string> parameters) //Функция выбирает нужный алгоритм и запускает его
        {
            switch (algName)//Сюда необходимо добавить свой алгоритм
            {
                case "Example":
                {
                    _algorythm = new PresentAlg();
                    if (_algorythm.Fill(parameters, WriteResult))
                    {
                        _algorythm.Do();
                    }
                    break;
                }
                case "Shennon":
                {
                    _algorythm = new ShennonAlgorithm();
                    if (_algorythm.Fill(parameters, WriteResult))
                    {
                        _algorythm.Do();
                    }
                    break;
                }
            }
        }
    }

    public abstract class Algorythm //От этого класса наследуются алгоритмы
    {
        public abstract event Action<string> WriteResult; //Event для вывода на экран
        public abstract bool Fill(Dictionary<string, string> parameters, Action<string> writeAction); //Функция заполнения и проверки всех параметров алгоритма
        public abstract void Do(); //Функция выполняет алгоритм и вываодит его в TextBox
    }

    /*Пример алгоритма*/
    public class PresentAlg : Algorythm
    {
        public override event Action<string> WriteResult;
        private int _x;
        private int _y;

        public override bool Fill(Dictionary<string, string> parameters, Action<string> writeAction)
        {
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
    }

    /*Алгоритм Шеннона(Шаг младенца, шаг великана)*/
    public class ShennonAlgorithm : Algorythm
    {
        public override event Action<string> WriteResult;
        private int _p;
        private int _a;
        private int _b;
        private int _m;
        private int _k;

        public static Int64 MyPow(int x, int y, int p)
        {
            Int64 result = x;
            for (int i = 0; i < y - 1; i++)
            {
                result = result % p * x;
            }
            return result % p;
        }

        public override bool Fill(Dictionary<string, string> parameters, Action<string> writeAction)
        {
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
                var rez = (Math.Pow(_a, i) * _b) % _p;
                numer1.Add(i, rez);
            }
            for (int i = 1; i < _k + 1; i++)
            {
                var rez = MyPow(_a, i * _m, _p);
                numer2.Add(i, rez);
            }
            WriteResult?.Invoke("Первый ряд:");
            string numerable = string.Empty;
            for (int i = 0; i < _m; i++)
            {
                numerable += numer1[i] + " ";
            }
            WriteResult?.Invoke(numerable);
            WriteResult?.Invoke("Второй ряд:");
            numerable = string.Empty;
            for (int i = 1; i < _k + 1; i++)
            {
                numerable += numer2[i] + " ";
            }
            WriteResult?.Invoke(numerable);
            for (int i = 0; i < _m; i++)
            {
                for (int j = 1; j < _k + 1; j++)
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (numer1[i] == numer2[j])
                    {
                        J = i;
                        I = j;
                    }
                }
            }
            WriteResult?.Invoke("i=" + I);
            WriteResult?.Invoke("j=" + J);
            var x = I * _m - J;
            WriteResult?.Invoke("x=i*m-j=" + x);
        }
    }
}
