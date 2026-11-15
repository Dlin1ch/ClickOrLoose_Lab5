using System;
using System.Collections.Generic;
using System.Windows;

namespace ClickOrLoose
{
    public class CController
    {
        // Делегат для событий
        public delegate void SceneEvent(object sender, CControllerEventArgs e);

        // События
        public event SceneEvent? addObject;
        public event SceneEvent? removeObject;

        // СПИСОК ОБЪЕКТОВ ОДНОГО ТИПА (базового класса)
        // В этом и есть полиморфизм!
        private List<CCollectable> objects;

        private double spawnRate;
        private double timeSinceLastSpawn;
        private Random rng;
        private double minLifetime;
        private double maxLifetime;
        private double minSpriteSize;
        private double maxSpriteSize;
        private Size sceneSize;

        private double points; // Набранные очки

        public CController(double spawnRate, Size sceneSize)
        {
            rng = new Random();
            objects = new List<CCollectable>();
            this.spawnRate = spawnRate;
            this.timeSinceLastSpawn = 0;
            this.sceneSize = sceneSize;
            this.points = 0;

            minLifetime = 1.0;
            maxLifetime = 4.0;
            minSpriteSize = 20;
            maxSpriteSize = 40;
        }

        // Получить очки
        public double getPoints() { return points; }

        // Добавить очки
        public void pointsIncrease(double amount) { points += amount; }

        // Получить список объектов (для отрисовки в окне)
        public List<CCollectable> getObjects() { return objects; }

        // Создание новой сферы (пока только обычной)
        public void spawnObject()
        {
            double x = rng.NextDouble() * (sceneSize.Width - maxSpriteSize) + maxSpriteSize / 2;
            double y = rng.NextDouble() * (sceneSize.Height - maxSpriteSize) + maxSpriteSize / 2;

            double size = rng.NextDouble() * (maxSpriteSize - minSpriteSize) + minSpriteSize;
            double lifetime = rng.NextDouble() * (maxLifetime - minLifetime) + minLifetime;

            // С шансом 20% создаём бонусную сферу, иначе обычную
            CCollectable newObj;
            if (rng.NextDouble() < 0.2)
            {
                newObj = new CCooldownReducer(new Point(x, y), size, lifetime);
            }
            else
            {
                newObj = new CPointGiver(new Point(x, y), size, lifetime);
            }

            objects.Add(newObj);

            // ВЫЗЫВАЕМ СОБЫТИЕ ДОБАВЛЕНИЯ (теперь окно само добавит спрайт на холст)
            addObject?.Invoke(this, new CControllerEventArgs(newObj.getSprite()));
        }

        // Обновление состояния (вызывается по таймеру из MainWindow)
        public void update(double delta)
        {
            // 1. Обновляем время жизни всех объектов
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (objects[i].updateLifetime(delta))
                {
                    // ВЫЗЫВАЕМ СОБЫТИЕ УДАЛЕНИЯ, чтобы окно убрало спрайт с холста
                    CControllerEventArgs e = new CControllerEventArgs(objects[i].getSprite());
                    e.msg = "Сфера исчезла!";
                    removeObject?.Invoke(this, e);

                    // Удаляем из списка, если время истекло
                    objects.RemoveAt(i);
                }
            }

            // 2. Создаём новые объекты
            timeSinceLastSpawn += delta;
            if (timeSinceLastSpawn >= spawnRate)
            {
                spawnObject();
                timeSinceLastSpawn = 0;
            }
        }

        // Обработка клика мышью
        public void mouseClick(Point mousePosition, CPlayer player)
        {
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (objects[i].onClick(player, this, mousePosition))
                {
                    // ВЫЗЫВАЕМ СОБЫТИЕ УДАЛЕНИЯ ПРИ КЛИКЕ
                    CControllerEventArgs e = new CControllerEventArgs(objects[i].getSprite());
                    e.msg = "Сфера собрана!";
                    removeObject?.Invoke(this, e);

                    objects.RemoveAt(i);
                    return;
                }
            }
        }
    }
}