using System.Drawing;

namespace WebApplication1;

public static class CaptchaGenerator
{
    private static readonly int _width = 290;
    private static readonly int _height = 80;
    
    public static Bitmap Create(string text)
    {
        Random rnd = new Random();
        Bitmap result = new Bitmap(_width, _height);

        //Вычислим позицию текста
        int Xpos = rnd.Next(0, _width/2 - 50);
        int Ypos = rnd.Next(15, _height/2 - 15);

        //Добавим различные цвета
        Brush[] colors = { Brushes.Black,
            Brushes.Yellow,
            Brushes.RoyalBlue,
            Brushes.Green };
            
        //Укажем где рисовать
        Graphics g = Graphics.FromImage((Image)result);

        //Пусть фон картинки будет серым
        g.Clear(Color.LightGray);

        //Нарисуем сгенирируемый текст
        g.DrawString(text,
            new Font("Arial", 40),
            colors[rnd.Next(colors.Length)],
            new PointF(Xpos, Ypos));

        var rand = 0;
        //Добавим немного помех
        /////Линии из углов

        Pen pen = new Pen(Brushes.Black);
        pen.Width = 4.0F;
        g.DrawLine(pen,
            new Point(0, 0),
            new Point(_width - rand, _height));
        g.DrawLine(pen,
            new Point(0, _height),
            new Point(_width - rand, 0));
        ////Белые точки
        for (int i = 0; i < _width; ++i)
        {
            for (int j = 0; j < _height; ++j)
            {
                if (rnd.Next() % 20 == 0)
                    result.SetPixel(i, j, Color.White);
            }
        }

        return result;
    }
}