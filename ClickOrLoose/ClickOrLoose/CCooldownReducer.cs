using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ClickOrLoose
{
    public class CCooldownReducer : CCollectable
    {
        private double speedModifier;

        public CCooldownReducer(Point position, double size, double lifetime)
            : base(position, size, lifetime)
        {
            sprite.Fill = Brushes.HotPink; // Другой цвет
            speedModifier = 0.1; // Уменьшаем перезарядку на 0.1 сек
        }

        public override bool onClick(CPlayer player, CController controller, Point mousePosition)
        {
            if (isMouseOnObject(mousePosition) == false)
                return false;

            // Уменьшаем перезарядку игроку
            player.increaseSpeed(speedModifier);
            return true;
        }
    }
}
