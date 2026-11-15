using System;
using System.Collections.Generic;
using System.Text;

namespace ClickOrLoose
{
    public class CPlayer
    {
        // Игрок имеет таймер перезарядки клика
        private CCountdownTimer countdownTimer;

        // Время перезарядки (в секундах)
        private double timeBeforeClick;

        public CPlayer(double timeBeforeClick)
        {
            this.timeBeforeClick = timeBeforeClick;
            this.countdownTimer = new CCountdownTimer(timeBeforeClick);
        }

        // Обновление таймера игрока
        public void update(double delta)
        {
            countdownTimer.update(delta);
        }

        // Можно ли кликать?
        public bool canClick()
        {
            return countdownTimer.isReady();
        }

        // Сбросить перезарядку (вызывается после клика)
        public void countdownEnded()
        {
            countdownTimer.reset();
        }

        // Уменьшить перезарядку (например, бонусной сферой)
        public void increaseSpeed(double speedModifier)
        {
            timeBeforeClick -= speedModifier;
            if (timeBeforeClick < 0.1) timeBeforeClick = 0.1; // Минимум 0.1 сек
            countdownTimer = new CCountdownTimer(timeBeforeClick);
        }
    }
}
