using Jalgpalli_mäng;
using System;
using System.Collections.Generic;

namespace Jalgpalli_mang
{
    public class Team
    {
        // Список игроков в команде
        public List<Player> Players { get; } = new List<Player>();

        // Название группы
        public string Name { get; private set; }

        // Ссылка на игру, которую в настоящее время играет команда
        public Game Game { get; set; }

        // Конструктор, который инициализирует команду с именем
        public Team(string name)
        {
            Name = name;
        }

        //Метод начала игры и присвоения случайных позиций игрокам на местах
        // 'width' и 'height' определяют размеры поля
        public void StartGame(int width, int height)
        {
            Random rnd = new Random();
            foreach (var player in Players)
            {
                // Случайно назначить каждому игроку место в поле.
                player.SetPosition(
                    rnd.NextDouble() * width,
                    rnd.NextDouble() * height
                );
            }
        }

        // Метод добавления игрока в команду
        // Если игрок уже принадлежит команде, их нельзя добавить
        public void AddPlayer(Player player)
        {
            // Проверь, есть ли у игрока команда.
            if (player.Team != null) return;

            // Добавить игрока в команду и назначить команду игрока в эту команду.
            Players.Add(player);
            player.Team = this;
        }

        // Метод определения нынешнего положения мяча для команды
        // Использует метод поиска позиции мяча
        public (double, double) GetBallPosition()
        {
            return Game.GetBallPositionForTeam(this);
        }

        // Метод определения скорости мяча с точки зрения команды
        // Использует метод игры для определения скорости шара
        public void SetBallSpeed(double vx, double vy)
        {
            Game.SetBallSpeedForTeam(this, vx, vy);
        }

        // Метод поиска игрока, ближе всего к мячу
        // Пропускает всех игроков и возвращает одного, близкого к мячу.
        public Player GetClosestPlayerToBall()
        {
            Player closestPlayer = Players[0]; // Начнем с первого игрока.
            double bestDistance = Double.MaxValue; // Инициировать лучшее расстояние до очень большого числа.

            foreach (var player in Players)
            {
                // Расчет расстояния игрока до мяча
                var distance = player.GetDistanceToBall();

                // Если этот игрок ближе, чем нынешний, обновите ближайший игрок.
                if (distance < bestDistance)
                {
                    closestPlayer = player;
                    bestDistance = distance;
                }
            }

            return closestPlayer; // Верни ближайшего игрока
        }

        // Метод перемещения игроков во время игры
        public void Move()
        {
            // Подвинь ближайшего игрока к мячу.
            GetClosestPlayerToBall().MoveTowardsBall();

            // Перевести всех игроков в команду на основе их индивидуальной логики
            Players.ForEach(player => player.Move());
        }
    }
}
