
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football
{
    //Loome mängijale klassi. (Создаем класс для игрока)
    public class Player
    {
        public string Name { get; } //Mängija nimi (Имя игрока)
        public double X { get; private set; } //Koordinaadid (Координаты)
        public double Y { get; private set; } //Koordinaadid (Координаты)

        //Шаг игрока
        private double _vx, _vy;

        public Team? Team { get; set; } = null; //Meeskond (Команда)

        private const double MaxSpeed = 5; //Maksimaalne kiirus (Максимальная скорость)
        private const double MaxKickSpeed = 25; //Maksimaalne löögikiirus (Максимальная скорость удара)
        private const double BallKickDistance = 10; //Pallilöögi kaugus (Расстояние удара по мячу)

        private Random _random = new Random();

        //Võtab vastu ainult mängija nime (Принимает только имя игрока)
        public Player(string name)
        {
            Name = name;
        }

        //Võtab vastu mängija nime, koordinaadid, meeskonnad (Принимает имя игрока, координаты, команд)
        public Player(string name, double x, double y, Team team)
        {
            Name = name;
            X = x;
            Y = y;
            Team = team;
        }

        //Установка позиции игрока на поле 
        public void SetPosition(double x, double y)
        {
            X = x;
            Y = y;
        }

        //Установка позиции игрока на поле, когда на поле появилась команда
        public (double, double) GetAbsolutePosition()
        {
            return Team!.Game.GetPositionForTeam(Team, X, Y);
        }

        //растояние до мяча
        public double GetDistanceToBall()
        {
            var ballPosition = Team!.GetBallPosition();
            var dx = ballPosition.Item1 - X;
            var dy = ballPosition.Item2 - Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        //Движение игрока к мячу
        public void MoveTowardsBall()
        {
            var ballPosition = Team!.GetBallPosition();
            var dx = ballPosition.Item1 - X;
            var dy = ballPosition.Item2 - Y;
            //Arvutab, kui palju on vaja kiirust vähendada, et vastata maksimaalsele kiirusele
            //Рассчитывает, на сколько нужно уменьшить скорость, чтобы соответствовать максимальной скорости
            var ratio = Math.Sqrt(dx * dx + dy * dy) / MaxSpeed;
            //Uuendame kiirust (Обновляем скорость)
            _vx = dx / ratio;
            _vy = dy / ratio;
        }

        //перемещение игрока на поле 
        public void Move()
        {
            //Kui mängija ei ole pallile lähim, jääb ta seisma
            //Если игрок не является ближайшим к мячу, он останавливается
            if (Team.GetClosestPlayerToBall() != this)
            {
                _vx = 0; //tema kiirus on 0 (его скорость равняется 0)
                _vy = 0;
            }

            //Kui mängija on palli lähedal, annab ta löögi (Если игрок находится близко к мячу, он наносит удар)
            if (GetDistanceToBall() < BallKickDistance)
            {
                //juhuslik kiirus palli tabamiseks (случайная скорость для удара по мячу)
                Team.SetBallSpeed(
                    MaxKickSpeed * _random.NextDouble(),
                    MaxKickSpeed * (_random.NextDouble() - 0.5)
                    );
            }

            //mängija uus positsioon (новое положение игрока)
            var newX = X + _vx;
            var newY = Y + _vy;
            var newAbsolutePosition = Team.Game.GetPositionForTeam(Team, newX, newY);
            //Kontrolli, kas mängija jääb staadioni piiridesse (Проверка, находится ли игрок  в пределах стадиона)
            if (Team.Game.Stadium.IsIn(newAbsolutePosition.Item1, newAbsolutePosition.Item2))
            {
                //uuendame mängija koordinaate (обновляем координаты игрока)
                X = newX;
                Y = newY;
            }
            else
            {
                //peatame mängija, kui mängija väljub väljakult (останавливаем игрока, если игрок выходит за пределы поля)
                _vx = _vy = 0;
            }
        }
    }
}