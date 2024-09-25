﻿

namespace Football
{
    public class Ball
    {
        // X и Y представляют собой нынешнее положение мяча на поле.
        public double X { get; private set; }
        public double Y { get; private set; }

        // _vx и _vy представляют скорость мяча в направлениях X и Y, соответственно
        private double _vx, _vy;

        // Ссылка на нынешнюю игру, используемую для доступа к стадиону и контрольных границ
        private Game _game;

        // Конструктор, который инициализирует положение мяча и связывает его с игрой.
        public Ball(double x, double y, Game game)
        {
            _game = game;
            X = x; // Начальное X положение мяча
            Y = y; // Начальное Y положение мяча
        }

        // Метод установки скорости (скорости) мяча в направлениях X и Y.
        public void SetSpeed(double vx, double vy)
        {
            _vx = vx; // Установить скорость в направлении X
            _vy = vy; // Установить скорость в направлении Y
        }

        // Метод перемещения мяча в зависимости от его скорости (_vx, _vy)
        public void Move()
        {
            // Рассчитайте новое положение мяча на основе его текущего положения и скорости.
            double newX = X + _vx;
            double newY = Y + _vy;

            // Проверьте, находится ли новая позиция в пределах стадиона.
            if (_game.Stadium.IsIn(newX, newY))
            {
                // Если мяч находится в пределах поля, обновите положение мяча.
                X = newX;
                Y = newY;
            }
            else
            {
                // Если мяч выходит за пределы поля, остановите его движение, установив скорость на 0.
                _vx = 0;
                _vy = 0;
            }
        }
    }
}
