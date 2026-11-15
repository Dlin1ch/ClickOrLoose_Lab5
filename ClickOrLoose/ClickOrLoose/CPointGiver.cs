using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ClickOrLoose
{
    public class CPointGiver : CCollectable
    {
        private double pointsValue;

        // Конструктор вызывает конструктор родителя через base(...)
        public CPointGiver(Point position, double size, double lifetime)
            : base(position, size, lifetime)
        {
            // Перекрашиваем сферу в другой цвет
            sprite.Fill = Brushes.BlueViolet;

            // Вычисляем стоимость в очках
            pointsValue = ((1 / this.size.Width) / lifetime) * 1000;
        }

        // Переопределяем абстрактный метод
        public override bool onClick(CPlayer player, CController controller, Point mousePosition)
        {
            if (isMouseOnObject(mousePosition) == false)
                return false;

            // Добавляем очки контроллеру
            controller.pointsIncrease(pointsValue);
            return true; // Сфера собрана
        }
    }
}
