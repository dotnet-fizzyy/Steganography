using System;

namespace Stegano.Model
{
    public class FontInfo
    {
        public string Название_шрифта { get; set; }
        public int Количество_символов { get; set; }
        public string Процентное_Соотношение { get; set; }

        public FontInfo(string name, int count, int max)
        {
            Название_шрифта = name;
            Количество_символов = count;
            double p = (double) count / max * 100;

            Процентное_Соотношение = $"{Math.Round(p, 1)} %";
        }
    }
}
