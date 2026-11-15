using System;
using System.Collections.Generic;
using System.Text;

namespace ClickOrLoose
{
    public class CCountdownTimer
    {
        private double targetTime;
        private double currentTime;

        public CCountdownTimer(double targetTime)
        {
            this.targetTime = targetTime;
            this.currentTime = 0;
        }

        // Получить оставшееся время
        public double getTime()
        {
            return targetTime - currentTime;
        }

        // Обновить таймер (delta - сколько секунд прошло)
        public void update(double delta)
        {
            if (currentTime < targetTime)
            {
                currentTime += delta;
                if (currentTime > targetTime) currentTime = targetTime;
            }
        }

        // Сбросить таймер (начать заново)
        public void reset()
        {
            currentTime = 0;
        }

        // Готов ли таймер (время вышло)
        public bool isReady()
        {
            return currentTime >= targetTime;
        }
    }
}
