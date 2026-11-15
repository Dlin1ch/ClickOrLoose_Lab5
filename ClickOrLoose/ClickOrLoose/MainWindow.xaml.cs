using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ClickOrLoose
{
    public partial class MainWindow : Window
    {
        private CController? controller;
        private CPlayer? player;
        private DispatcherTimer timer;

        private double gameTimeLeft = 15.0; // Игра длится 15 секунд
        private bool isGameRunning = false;

        public MainWindow()
        {
            InitializeComponent();

            // Инициализация таймера
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 33); // ~30 FPS (33 мс)
            timer.Tick += Timer_Tick;
        }

        // Обработчик кнопки "Start"
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // Очищаем холст
            GameCanvas.Children.Clear();

            // Создаём контроллер и игрока
            Size sceneSize = new Size(GameCanvas.ActualWidth, GameCanvas.ActualHeight);
            controller = new CController(0.5, sceneSize);
            player = new CPlayer(0.2);

            // ПОДПИСЫВАЕМСЯ НА СОБЫТИЯ (ЭТО НОВОЕ!)
            controller.addObject += addObjectInScene;
            controller.removeObject += removeObjectFromScene;
            controller.removeObject += logMessage;

            gameTimeLeft = 15.0;
            isGameRunning = true;

            StartButton.IsEnabled = false;
            StatusText.Text = "Game!";
            StatusText.Foreground = System.Windows.Media.Brushes.Green;

            timer.Start();
        }

        // Таймер, который обновляет игру каждые 33 мс
        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (!isGameRunning) return;
            if (controller == null || player == null) return;

            double delta = 0.033;

            gameTimeLeft -= delta;
            if (gameTimeLeft <= 0)
            {
                gameTimeLeft = 0;
                isGameRunning = false;
                timer.Stop();
                StartButton.IsEnabled = true;
                StatusText.Text = "Finished!";
                StatusText.Foreground = System.Windows.Media.Brushes.Red;
            }

            controller.update(delta);
            player.update(delta);

            UpdateUI();
        }

        // Перерисовка интерфейса (ТЕПЕРЬ ТОЛЬКО ТЕКСТ!)
        private void UpdateUI()
        {
            ScoreText.Text = Math.Round(controller!.getPoints()).ToString();
            TimeText.Text = gameTimeLeft.ToString("F1");

            // УБРАЛИ ОЧИСТКУ ХОЛСТА И ЦИКЛ ДОБАВЛЕНИЯ СФЕР!
        }

        // Обработчик клика по холсту
        private void GameCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!isGameRunning) return;

            if (player!.canClick())
            {
                Point mousePos = e.GetPosition(GameCanvas);
                controller!.mouseClick(mousePos, player);
                player.countdownEnded();
            }
        }

        // ==================== ОБРАБОТЧИКИ СОБЫТИЙ ====================

        // Добавление сферы на холст
        public void addObjectInScene(object sender, CControllerEventArgs e)
        {
            GameCanvas.Children.Add(e.sprite);
        }

        // Удаление сферы с холста
        public void removeObjectFromScene(object sender, CControllerEventArgs e)
        {
            GameCanvas.Children.Remove(e.sprite);
        }

        // Логирование (пока для проверки)
        public void logMessage(object sender, CControllerEventArgs e)
        {
            MessageBox.Show(e.msg);
        }
    }
}