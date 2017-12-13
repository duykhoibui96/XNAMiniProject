using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MiniProject2D.Config;
using MiniProject2D.Entity;
using MiniProject2D.EventHandler;
using MiniProject2D.Information;
using MiniProject2D.Input;
using MiniProject2D.Model;
using MiniProject2D.Resource;
using MiniProject2D.Sound;
using Effect = MiniProject2D.Entity.Effect;

namespace MiniProject2D.UI
{
    class MazeUI : GameUI
    {
        private enum State
        {
            Waiting = 0,
            PlayerMoving = 1,
            CalculateMonsterMovement = 2,
            MonsterMoving = 3,
            Collision = 4,
            End = 5
        }

        private int[,] matrix;
        private _2DModel[] mazeModels;
        private _2DModel playerSteps;

        private State state;
        private int mapWidth;
        private int mapHeight;
        private int numOfFreeSpace;
        private bool isTransitioning;

        private Character player;
        private int numOfPlayerSteps;
        private Character[] monsters;
        private int currentMonsterIndex;

        private Point exit;
        private Effect collision;
        private int endTime = 2000;

        public MazeUI()
        {
            var unit = Global.Instance.Unit;
            MazeObject.MazeStartPoint = new Point(unit * 11, unit * 2);
            mapWidth = Global.Instance.MapWidth;
            mapHeight = Global.Instance.MapHeight;
            collision = new Effect(Effect.EffectType.Collision);
            playerSteps = new _2DModel("Numbers of step: ", ResManager.Instance.SmallNotifyFont, new Vector2(unit * 11, unit), Color.Red);
            InitMap();
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            switch (state)
            {
                case State.Waiting:
                    if (PlayerRequestMoving)
                    {
                        SoundManager.Instance.PlaySound(ResManager.Instance.FootSteps);
                        player.NumOfSteps = 1;
                        state = State.PlayerMoving;
                    }
                    break;
                case State.CalculateMonsterMovement:
                    if (currentMonsterIndex == monsters.Length)
                    {
                        currentMonsterIndex = 0;
                        state = State.Waiting;
                    }
                    else
                    {
                        if (MonsterReadyToMove())
                        {
                            SoundManager.Instance.PlaySound(ResManager.Instance.FootSteps);
                            state = State.MonsterMoving;
                        }

                    }
                    break;
                case State.MonsterMoving:
                    {
                        var currentMonster = monsters[currentMonsterIndex];
                        if (currentMonster.NeedMoving)
                            currentMonster.Move(gameTime);
                        else
                        {
                            currentMonster.Stop();
                            if (CheckCollision())
                            {
                                SoundManager.Instance.PlaySound(ResManager.Instance.Explosion);
                                var pos = MazeObject.MapToRealPosition(currentMonster.LogicPos);
                                collision.Rect = new Rectangle(pos.X - 25, pos.Y - 25, 100, 100);
                                state = State.Collision;
                                return;
                            }
                            SoundManager.Instance.PlaySound(ResManager.Instance.FootSteps);

                            PlayerScanning();
                            if (currentMonster.NumOfSteps == 0)
                                currentMonsterIndex++;
                            state = State.CalculateMonsterMovement;
                        }
                    }
                    break;
                case State.PlayerMoving:
                    if (player.NeedMoving)
                        player.Move(gameTime);
                    else
                    {
                        player.Stop();
                        SoundManager.Instance.PlaySound(ResManager.Instance.FootSteps);
                        if (CheckCollision(true))
                        {
                            SoundManager.Instance.PlaySound(ResManager.Instance.Explosion);
                            var pos = MazeObject.MapToRealPosition(player.LogicPos);
                            collision.Rect = new Rectangle(pos.X - 25, pos.Y - 25, 100, 100);
                            state = State.Collision;
                            return;
                        }
                        if (GetOutOfMaze)
                        {
                            GameResult.Instance.IsWon = true;
                            GameResult.Instance.Score = 3 * numOfFreeSpace - numOfPlayerSteps;
                            EventBoard.Instance.AddEvent(EventBoard.Event.ShowResult);
                            state = State.End;
                            return;
                        }
                        for (int i = 0; i < monsters.Length; i++)
                        {
                            PlayerScanning(i);
                        }
                        numOfPlayerSteps++;
                        playerSteps.Text = "Numbers of step: " + numOfPlayerSteps;
                        state = State.CalculateMonsterMovement;
                    }
                    break;
                case State.Collision:
                    collision.Update(gameTime);
                    endTime -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (endTime <= 0)
                    {
                        GameResult.Instance.IsWon = false;
                        EventBoard.Instance.AddEvent(EventBoard.Event.ShowResult);
                        state = State.End;
                    }
                    break;
                case State.End:
                    break;
            }
        }

        public override void Render()
        {
            var modelList = new List<_2DModel>();

            modelList.Add(playerSteps);
            modelList.AddRange(mazeModels);

            modelList.AddRange(player.Models);

            foreach (var monster in monsters)
            {
                modelList.AddRange(monster.Models);
            }

            if (state == State.Collision)
                modelList.AddRange(collision.Models);

            _2DModel.Render(modelList.ToArray());
        }

        protected override void HandleEvent()
        {
            if (EventBoard.Instance.GetEvent() == EventBoard.Event.ResetGame)
            {
                InitMap();
                EventBoard.Instance.Finish();
            }

        }


        private void InitMap()
        {
            numOfPlayerSteps = 0;
            playerSteps.Text = "Numbers of step: " + numOfPlayerSteps;
            numOfFreeSpace = mapWidth * mapHeight - 4;

            matrix = new int[mapWidth, mapHeight];

            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    if (i == 0 || j == 0 || i == mapWidth - 1 || j == mapHeight - 1)
                    {
                        matrix[i, j] = 1;
                    }
                    else
                    {
                        matrix[i, j] = 0;
                    }
                }
            }
            endTime = 2000;
            numOfPlayerSteps = 0;
            currentMonsterIndex = 0;
            state = State.Waiting;
            InitDoors();
            InitObstacles();
            InitCharacters();
            RenderMap();
        }

        private void InitObstacles()
        {
            var rand = Global.Instance.Rand;
            var posList = new List<Point>();

            for (int i = 0; i < 20; i++)
            {
                while (true)
                {
                    var x = rand.Next(2, mapWidth - 2);
                    var y = rand.Next(2, mapHeight - 2);
                    var pos = new Point(x, y);
                    if (posList.Contains(pos))
                        continue;

                    matrix[x, y] = 1;
                    posList.Add(pos);
                    break;
                }

            }

        }

        private void RenderMap()
        {
            var modelList = new List<_2DModel>();
            var graphics = Global.Instance.Graphics;
            var unit = Global.Instance.Unit;
            var colors = new Color[]
            {
                new Color(246,215,152), 
                new Color(222,182,104)
                
            };
            int colorIndex = 0;


            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {

                    var realPosition = MazeObject.MapToRealPosition(i, j);
                    if (matrix[i, j] == 1)
                    {

                        modelList.Add(new _2DModel(ResManager.Instance.Wall,
                            new Rectangle(realPosition.X, realPosition.Y, unit, unit), Color.White));
                    }
                    else
                    {
                        var cell = new Texture2D(graphics, 1, 1);
                        cell.SetData(new Color[]
                        {
                            colors[colorIndex]
                        });

                        modelList.Add(new _2DModel(cell, new Rectangle(realPosition.X, realPosition.Y, unit, unit), Color.White));
                    }
                    colorIndex = 1 - colorIndex;

                }
            }

            mazeModels = modelList.ToArray();
        }

        private void InitDoors()
        {
            matrix[3, mapHeight - 1] = 0;
            exit = new Point(3, mapHeight - 1);
        }

        private void InitCharacters()
        {
            var rand = Global.Instance.Rand;
            matrix[5, 0] = 0;
            player = new Character(MazeObject.Type.Player, 5, 0);

            var monsterList = new List<Character>();
            int x, y;

            for (int i = 0; i < 2; i++)
            {
                while (true)
                {
                    x = rand.Next(1, mapWidth - 1);
                    y = rand.Next(1, mapHeight - 1);

                    if (!IsValidPosition(new Point(x, y))) continue;

                    monsterList.Add(new Character(MazeObject.Type.Mummy, x, y));
                    break;
                }

            }

            for (int i = 0; i < 1; i++)
            {
                while (true)
                {
                    x = rand.Next(1, mapWidth - 1);
                    y = rand.Next(1, mapHeight - 1);

                    if (!IsValidPosition(new Point(x, y))) continue;

                    monsterList.Add(new Character(MazeObject.Type.Scorpion, x, y));
                    break;
                }
            }

            for (int i = 0; i < 1; i++)
            {
                while (true)
                {
                    x = rand.Next(1, mapWidth - 1);
                    y = rand.Next(1, mapHeight - 1);

                    if (!IsValidPosition(new Point(x, y))) continue;

                    monsterList.Add(new Character(MazeObject.Type.Zombie, x, y));
                    break;
                }
            }

            monsters = monsterList.ToArray();

        }


        public bool GetOutOfMaze
        {
            get { return player.LogicPos.Equals(exit); }
        }

        private bool CheckCollision(bool checkAll = false)
        {
            return checkAll ? monsters.Any(monster => monster.LogicPos.Equals(player.LogicPos)) : player.LogicPos.Equals(monsters[currentMonsterIndex].LogicPos);
        }

        private void PlayerScanning(int monsterIndex = -1)
        {
            var currentMonster = monsters[monsterIndex > -1 ? monsterIndex : currentMonsterIndex];
            int startDirectionIndex = 0, endDirectionIndex = 0;

            switch (currentMonster.ObjectType)
            {
                case MazeObject.Type.Mummy:
                    startDirectionIndex = 0;
                    endDirectionIndex = 3;
                    break;
                case MazeObject.Type.Scorpion:
                    startDirectionIndex = 4;
                    endDirectionIndex = 7;
                    break;
                case MazeObject.Type.Zombie:
                    startDirectionIndex = 0;
                    endDirectionIndex = 7;
                    break;
            }

            for (int i = startDirectionIndex; i <= endDirectionIndex; i++)
            {
                if (LookingForPlayer(currentMonster.LogicPos, i))
                {
                    currentMonster.PreferredDirection = i;
                    currentMonster.IsAngry = true;
                    return;
                }
            }

            currentMonster.IsAngry = false;
            currentMonster.PreferredDirection = -1;

        }

        private bool MonsterReadyToMove()
        {
            var currentMonster = monsters[currentMonsterIndex];
            var newPos = currentMonster.LogicPos;
            var rand = Global.Instance.Rand;
            var direction = currentMonster.PreferredDirection;
            bool directionCheck = false;

            if (direction == -1)
            {
                switch (currentMonster.ObjectType)
                {
                    case MazeObject.Type.Mummy:
                        direction = rand.Next(0, 4);
                        break;
                    case MazeObject.Type.Scorpion:
                        direction = rand.Next(4, 8);
                        break;
                    case MazeObject.Type.Zombie:
                        direction = rand.Next(0, 8);
                        break;
                }
                currentMonster.NumOfSteps = 1;
            }
            else
            {
                directionCheck = true;
                direction = currentMonster.PreferredDirection;
                currentMonster.NumOfSteps = 2;
            }

            switch (direction)
            {
                case 0:
                    newPos.Y++;
                    break;
                case 1:
                    newPos.X--;
                    break;
                case 2:
                    newPos.X++;
                    break;
                case 3:
                    newPos.Y--;
                    break;
                case 4:
                    newPos.X--;
                    newPos.Y--;
                    break;
                case 5:
                    newPos.X--;
                    newPos.Y++;
                    break;
                case 6:
                    newPos.X++;
                    newPos.Y--;
                    break;
                case 7:
                    newPos.X++;
                    newPos.Y++;
                    break;
            }


            if (!directionCheck && !IsValidPosition(newPos)) return false;

            currentMonster.LogicPos = newPos;
            return true;
        }

        private bool LookingForPlayer(Point curPos, int direction)
        {
            int offsetX = 0, offsetY = 0;
            switch (direction)
            {
                case 0:
                    offsetY = 1;
                    break;
                case 1:
                    offsetX = -1;
                    break;
                case 2:
                    offsetX = 1;
                    break;
                case 3:
                    offsetY = -1;
                    break;
                case 4:
                    offsetX = -1;
                    offsetY = -1;
                    break;
                case 5:
                    offsetX = -1;
                    offsetY = 1;
                    break;
                case 6:
                    offsetX = 1;
                    offsetY = -1;
                    break;
                case 7:
                    offsetX = 1;
                    offsetY = 1;
                    break;
            }

            var pos = curPos;

            for (int i = 0; i < 4; i++)
            {
                pos.X += offsetX;
                pos.Y += offsetY;

                if (!IsValidPosition(pos)) return false;

                if (player.LogicPos.Equals(pos)) return true;

            }

            return false;

        }

        private bool PlayerRequestMoving
        {
            get
            {
                var key = KeyboardEvent.Instance.PressedKey;
                var newPos = player.LogicPos;
                switch (key)
                {
                    case Keys.Down:
                        newPos.Y++;
                        break;
                    case Keys.Left:
                        newPos.X--;
                        break;
                    case Keys.Right:
                        newPos.X++;
                        break;
                    case Keys.Up:
                        newPos.Y--;
                        break;
                }

                if (!newPos.Equals(player.LogicPos) && IsValidPosition(newPos))
                {
                    player.LogicPos = newPos;
                    return true;
                }

                return false;
            }
        }

        private bool IsValidPosition(Point newPos)
        {
            return (newPos.X >= 0 && newPos.Y >= 0 && newPos.X < mapWidth && newPos.Y < mapHeight) && matrix[newPos.X, newPos.Y] == 0;
        }
    }
}
