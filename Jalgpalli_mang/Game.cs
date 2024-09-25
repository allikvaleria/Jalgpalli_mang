using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Football
{
    public class Game
    {
        public Team HomeTeam { get; }
        public Team AwayTeam { get; }
        public Stadium Stadium { get; }
        public Ball Ball { get; private set; }

        // Конструктор для начала игры с двумя командами и стадионом
        public Game(Team homeTeam, Team awayTeam, Stadium stadium)
        {
            HomeTeam = homeTeam;
            // Назначить нынешнюю игру для домашней команды
            homeTeam.Game = this;

            AwayTeam = awayTeam;
            // Назначить нынешнюю игру для другой команды
            awayTeam.Game = this;

            Stadium = stadium;
        }

        // Метод начала игры
        public void Start()
        {
            // Включить мяч в центр стадиона.
            Ball = new Ball(Stadium.Width / 2, Stadium.Height / 2, this);

            // Позиция игроков домашней команды на их половине поля
            HomeTeam.StartGame(Stadium.Width / 2, Stadium.Height);

            // Позиция игроков другой команды на их половину поля
            AwayTeam.StartGame(Stadium.Width / 2, Stadium.Height);
        }

        // Частный метод отражения позиции отряда на противоположной стороне поля
        // Это обеспечивает расположение другой команды на правильной стороне стадиона.
        private (double, double) GetPositionForAwayTeam(double x, double y)
        {
            // Для другой команды позиции x и y отражены по всей ширине и высоте стадиона.
            return (Stadium.Width - x, Stadium.Height - y);
        }

        // Метод получения регулируемой позиции для команды на местах.
        // Если команда является домашней командой, то положение остается прежним.
        // Если это команда отъезда, то положение зеркальное с помощью GetPositionForAwayTeam.
        public (double, double) GetPositionForTeam(Team team, double x, double y)
        {
            return team == HomeTeam ? (x, y) : GetPositionForAwayTeam(x, y);
        }

        // Метод определения позиции мяча по отношению к конкретной команде.
        // Для команды по отъезду она отражает положение мяча с помощью GetPositionForTeam
        public (double, double) GetBallPositionForTeam(Team team)
        {
            return GetPositionForTeam(team, Ball.X, Ball.Y);
        }

        // Метод определения скорости мяча на основе того, какая команда контролирует его.
        // Если это домашняя команда, то скорость устанавливается непосредственно.
        // Если это команда отъезда, скорость перевернута для того, чтобы соответствовать их перспективам
        public void SetBallSpeedForTeam(Team team, double vx, double vy)
        {
            if (team == HomeTeam)
            {
                // Установить скорость мяча непосредственно для домашней команды.
                Ball.SetSpeed(vx, vy);
            }
            else
            {
                // Для другой команды, скорость перевернута.
                Ball.SetSpeed(-vx, -vy);
            }
        }

        // Метод перемещения всех элементов игры
        public void Move()
        {
            // Перемещение игроков из домашней команды
            HomeTeam.Move();

            // Перемещение игроков из другой команды
            AwayTeam.Move();

            // Перемещение мяча в соответствии с его скоростью
            Ball.Move();
        }
    }
}