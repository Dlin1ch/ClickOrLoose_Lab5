using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ClickOrLoose
{
    public abstract class CCollectable
    {
        // Позиция объекта в сцене
        protected Point position;

        // Размер объекта
        protected Size size;

        // Время жизни объекта (в секундах)
        protected double lifetime;

        // Визуальное отображение (кружок)
        protected Ellipse sprite;

        // Конструктор
        public CCollectable(Point position, double size, double lifetime)
        {
            this.position = position;
            this.size = new Size(size, size);
            this.lifetime = lifetime;

            // Создаём кружок
            sprite = new Ellipse();
            sprite.Fill = Brushes.BlueViolet;
            sprite.StrokeThickness = 2;
            sprite.Stroke = Brushes.Black;
            sprite.HorizontalAlignment = HorizontalAlignment.Center;
            sprite.VerticalAlignment = VerticalAlignment.Center;
            sprite.Width = this.size.Width;
            sprite.Height = this.size.Height;
            Canvas.SetLeft(sprite, position.X - size / 2);
            Canvas.SetTop(sprite, position.Y - size / 2);
        }

        // Проверка, находится ли курсор мыши над объектом
        public bool isMouseOnObject(Point mousePosition)
        {
            double left = position.X - size.Width / 2;
            double top = position.Y - size.Height / 2;
            double right = position.X + size.Width / 2;
            double bottom = position.Y + size.Height / 2;

            return mousePosition.X >= left && mousePosition.X <= right &&
                   mousePosition.Y >= top && mousePosition.Y <= bottom;
        }

        // Получить визуальный элемент (кружок)
        public Ellipse getSprite()
        {
            return sprite;
        }

        // Обновить время жизни. Возвращает true, если время истекло
        public bool updateLifetime(double delta)
        {
            lifetime -= delta;
            return lifetime <= 0;
        }

        // Абстрактный метод обработки клика
        // Каждый наследник реализует его по-своему
        public abstract bool onClick(CPlayer player, CController controller, Point mousePosition);
    }
}