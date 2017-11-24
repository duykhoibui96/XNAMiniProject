using System;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MiniProject2D.Config;
using MiniProject2D.EventHandler;
using MiniProject2D.Model;
using MiniProject2D.Resource;

namespace MiniProject2D.GameComponent
{
    class GameMatch : GameView
    {
        public enum BoundaryPositionType
        {
            Top = 0,
            Bottom = 1,
            Left = 2,
            Right = 3
        }

        public enum State
        {
            WaitingForUser = 0, //Chờ input từ người chơi
            MonsterMovementSetting = 1, //Thiết lập trạng thái di chuyển cho quái vật
            Moving = 2,//Người chơi và quái vật trong quá trình di chuyển
            PlayerDiscorved = 3,//Quái vật phát hiện người chơi
            PlayerDeath = 4,//Người chơi bị giết
            TreasureCollected = 5,//Người chơi thu thập được kho báu
            Lose = 6,//Thua -> bị quái vật giết
            Win = 7//Thắng -> ra được mê cung
        }

        private Random random;
        private CharacterManager[] characterInfoList;
        private BackgroundEntity background;
        private BackgroundEntity area;
        private BackgroundEntity boundary;
        private BackgroundEntity entrance;
        private BackgroundEntity[] obstacles;
        private BackgroundEntity exit;
        private AnimationEntity explosion;
        private BackgroundEntity loseGame;
        private ClickableEntity config;
        private int movingObjIndex = 0;
        private State state;
        private int endGameDelayTime;
        private int characterWidth = Configuration.Unit;
        private int characterHeight = Configuration.Unit;
        private BoundaryPositionType entrancePositionType;
        private BoundaryPositionType exitPositionType;
        private Texture2D doorArrow;
        private Texture2D visionSprite;

        public override void SetEnabled(bool isEnabled)
        {
            base.SetEnabled(isEnabled);
            var color = isEnabled ? Color.White : Color.Gray;
            foreach (var character in characterInfoList)
            {
                character.Object.MovementEntity.CurrentColor = color;
            }
            foreach (var obj in obstacles)
            {
                obj.CurrentColor = color;
            }
            entrance.CurrentColor = exit.CurrentColor = background.CurrentColor = config.CurrentColor = boundary.CurrentColor = area.CurrentColor = explosion.CurrentColor = loseGame.CurrentColor = color;
        }

        public State GameState
        {
            get { return state; }
            set { state = value; }
        }

        public GameMatch(Game game)
            : base(ViewType.Match)
        {
            var unit = Configuration.Unit;
            visionSprite = new Texture2D(game.GraphicsDevice, 1, 1);
            visionSprite.SetData(new Color[] { Color.AntiqueWhite });
            loseGame = new BackgroundEntity(ResManager.Instance.LoseGame, new Rectangle(300, 200, 500, 250), Color.White);
            explosion = new AnimationEntity(ResManager.Instance.Collision, new Rectangle(0, 0, 100, 100), Color.White, 4, 0);
            boundary = new BackgroundEntity(ResManager.Instance.Boundary, new Rectangle(unit, unit, unit * 20, unit * 10), Color.White);
            config = new ClickableEntity(EventBoard.Event.PauseGame, ResManager.Instance.Config,
                ResManager.Instance.ConfigHover,
                new Rectangle(0, 0, unit * 2, unit * 2), Color.White);
            background = new BackgroundEntity(ResManager.Instance.Ground, new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height), Color.White);
            area = new BackgroundEntity(ResManager.Instance.Ground, new Rectangle(boundary.Rect.X * 2, boundary.Rect.Y * 2, boundary.Rect.Width - 2 * unit, boundary.Rect.Height - 2 * unit), Color.White);
            entrance = new BackgroundEntity(ResManager.Instance.Ground, new Rectangle(unit * 2, unit * 2, unit, unit), Color.White);
            exit = new BackgroundEntity(ResManager.Instance.Ground, new Rectangle(unit * 2, unit * 2, unit, unit), Color.White);
        }

        public void Init()
        {
            var unit = Configuration.Unit;
            var numbersOfObstacles = 20;
            var numbersOfZombie = 1;
            var numbersOfScorpion = 2;
            var numbersOfMummy = 3;

            GameState = State.WaitingForUser;
            random = new Random();
            boundary.Rect.Width = Configuration.Unit * 20;
            boundary.Rect.Height = Configuration.Unit * 10;
            config.Rect.X = boundary.Rect.X + boundary.Rect.Width + unit;
            area.Rect.Width = 900;
            area.Rect.Height = 400;
            explosion.IsVisible = false;
            loseGame.IsVisible = false;
            endGameDelayTime = 1000;

            //Dinamic data----------------------------------------------------------------
            RandomDoors();
            RandomObstacles(numbersOfObstacles);
            RandomCharacters(numbersOfMummy, numbersOfScorpion, numbersOfZombie);
            //-----------------------------------------------------------------------------------------------

            SetEnabled(true);
        }

        private void RandomObstacles(int numbersOfObstacle)
        {
            var unit = Configuration.Unit;
            obstacles = new BackgroundEntity[numbersOfObstacle];
            for (int index = 0; index < obstacles.Length; index++)
            {
                var horizontalPosition = random.Next(2, 20);
                var verticalPosition = random.Next(2, 10);
                obstacles[index] = new BackgroundEntity(ResManager.Instance.Wall, new Rectangle(unit * horizontalPosition, unit * verticalPosition, unit, unit), Color.White);
            }
        }

        private void RandomCharacters(int numbersOfMummy, int numbersOfScorpion, int numbersOfZombie)
        {
            var unit = Configuration.Unit;
            characterInfoList = new CharacterManager[1 + numbersOfMummy + numbersOfScorpion + numbersOfZombie];
            characterInfoList[0] = ObjectInit(entrance.Rect.X, entrance.Rect.Y, Character.ObjectType.Player);
            var index = 1;
            for (int i = 0; i < numbersOfMummy; i++)
            {
                var positionX = random.Next(2, 20) * unit;
                var positionY = random.Next(2, 10) * unit;
                while (true)
                {
                    if (obstacles.All(obstacle => !obstacle.Rect.Contains(positionX, positionY)))
                        break;
                    positionX = random.Next(2, 20) * unit;
                    positionY = random.Next(2, 10) * unit;
                }
                characterInfoList[index++] = ObjectInit(positionX, positionY, Character.ObjectType.Mummy);
            }
            for (int i = 0; i < numbersOfScorpion; i++)
            {
                var positionX = random.Next(2, 20) * unit;
                var positionY = random.Next(2, 10) * unit;
                while (true)
                {
                    if (obstacles.All(obstacle => !obstacle.Rect.Contains(positionX, positionY)))
                        break;
                    positionX = random.Next(2, 20) * unit;
                    positionY = random.Next(2, 10) * unit;
                }
                characterInfoList[index++] = ObjectInit(positionX, positionY, Character.ObjectType.Scorpion);
            }
            for (int i = 0; i < numbersOfZombie; i++)
            {
                var positionX = random.Next(2, 20) * unit;
                var positionY = random.Next(2, 10) * unit;
                while (true)
                {
                    if (obstacles.All(obstacle => !obstacle.Rect.Contains(positionX, positionY)))
                        break;
                    positionX = random.Next(2, 20) * unit;
                    positionY = random.Next(2, 10) * unit;
                }
                characterInfoList[index++] = ObjectInit(positionX, positionY, Character.ObjectType.Zombie);
            }
        }

        private void RandomDoors()
        {

            //Entrance's position must be opposite to exit's

            entrancePositionType = (BoundaryPositionType)random.Next(4);//Random from 0 -> 3
            switch (entrancePositionType)
            {
                case BoundaryPositionType.Top:
                    exitPositionType = BoundaryPositionType.Bottom;
                    doorArrow = ResManager.Instance.ArrowDown;
                    break;
                case BoundaryPositionType.Bottom:
                    exitPositionType = BoundaryPositionType.Top;
                    doorArrow = ResManager.Instance.ArrowUp;
                    break;
                case BoundaryPositionType.Left: //LEFT
                    exitPositionType = BoundaryPositionType.Right;
                    doorArrow = ResManager.Instance.ArrowRight;
                    break;
                case BoundaryPositionType.Right: //RIGHT
                    exitPositionType = BoundaryPositionType.Left;
                    doorArrow = ResManager.Instance.ArrowLeft;
                    break;
            }

            var entrancePositionIndex = entrancePositionType > (BoundaryPositionType)1 ? random.Next(2, 10) : random.Next(2, 20);
            var exitPositionIndex = exitPositionType > (BoundaryPositionType)1 ? random.Next(2, 10) : random.Next(2, 20);

            entrance.Rect.Location = GetPosition(entrancePositionType, entrancePositionIndex);
            exit.Rect.Location = GetPosition(exitPositionType, exitPositionIndex);


        }

        private Point GetPosition(BoundaryPositionType positionType, int positionIndex)
        {
            switch (positionType)
            {
                case BoundaryPositionType.Top: //TOP
                    return new Point(Configuration.Unit * positionIndex, Configuration.Unit);
                case BoundaryPositionType.Bottom: //BOTTOM
                    return new Point(Configuration.Unit * positionIndex, Configuration.Unit * 10);
                case BoundaryPositionType.Left: //LEFT
                    return new Point(Configuration.Unit, Configuration.Unit * positionIndex);
                case BoundaryPositionType.Right: //RIGHT
                    return new Point(Configuration.Unit * 20, Configuration.Unit * positionIndex);

            }

            return Point.Zero;
        }

        private CharacterManager ObjectInit(int startX, int startY, Character.ObjectType objectType)
        {
            int numbersOfFrame = 0, defaultFrameIndex = 0;
            var sprite = ResManager.Instance.Player;
            var objType = Character.ObjectType.Player;

            switch (objectType)
            {
                case Character.ObjectType.Player:
                    numbersOfFrame = 4;
                    defaultFrameIndex = 2;
                    break;
                case Character.ObjectType.Mummy:
                    numbersOfFrame = 3;
                    defaultFrameIndex = 1;
                    sprite = ResManager.Instance.Mummy;
                    objType = Character.ObjectType.Mummy;
                    break;
                case Character.ObjectType.Scorpion:
                    numbersOfFrame = 4;
                    defaultFrameIndex = 2;
                    sprite = ResManager.Instance.Scorpion;
                    objType = Character.ObjectType.Scorpion;
                    break;
                case Character.ObjectType.Zombie:
                    numbersOfFrame = 6;
                    defaultFrameIndex = 0;
                    sprite = ResManager.Instance.Zombie;
                    objType = Character.ObjectType.Zombie;
                    break;
            }

            var instance =
                new Character(new AnimationEntity(sprite, new Rectangle(startX, startY, characterWidth, characterHeight), Color.White, numbersOfFrame, defaultFrameIndex), objType);
            return new CharacterManager(instance);
        }

        public override void Update(GameTime gameTime)
        {
            if (!isEnabled || !IsVisible) return;
            config.Update(gameTime);
            if (UserInput.Instance.IsLeftClick && config.IsHover)
                config.LeftClick();

            switch (state)
            {
                case State.WaitingForUser:
                    var pressedKey = UserInput.Instance.PressedKey;
                    var movementState = Character.MovementState.Stand;
                    switch (pressedKey)
                    {
                        case Keys.Down:
                        case Keys.S:
                            movementState = Character.MovementState.MoveDown;
                            break;
                        case Keys.Left:
                        case Keys.A:
                            movementState = Character.MovementState.MoveLeft;
                            break;
                        case Keys.Right:
                        case Keys.D:
                            movementState = Character.MovementState.MoveRight;
                            break;
                        case Keys.Up:
                        case Keys.W:
                            movementState = Character.MovementState.MoveUp;
                            break;
                    }
                    if (movementState != Character.MovementState.Stand)
                    {
                        var newRect = GetDestination(characterInfoList[movingObjIndex].Object.MovementEntity.Rect, movementState);
                        if (newRect.Equals(Rectangle.Empty)) break;
                        characterInfoList[movingObjIndex].ApplyNewCoords(newRect, movementState);
                        characterInfoList[movingObjIndex].NumbersOfSteps = 1;
                        state = State.Moving;
                    }
                    break;
                case State.MonsterMovementSetting:
                    do
                    {
                        var direction = GetRandomDirection(characterInfoList[movingObjIndex].Object.ObjType);
                        var newRect = GetDestination(characterInfoList[movingObjIndex].Object.MovementEntity.Rect, direction);
                        if (newRect.Equals(Rectangle.Empty)) continue;
                        characterInfoList[movingObjIndex].ApplyNewCoords(newRect, direction);
                        if (characterInfoList[movingObjIndex].NumbersOfSteps == 0)
                            characterInfoList[movingObjIndex].NumbersOfSteps = 1;
                        break;

                    } while (true);
                    state = State.Moving;
                    break;
                case State.Moving:
                    var currentMovingObj = characterInfoList[movingObjIndex];
                    currentMovingObj.Move();
                    if (currentMovingObj.FinishMoving)
                    {
                        if (CheckCollision())
                        {
                            state = State.PlayerDeath;
                            return;
                        }
                        if (CheckWinner())
                        {
                            state = State.Win;
                            return;
                        }
                        currentMovingObj.StopMoving();
                        if (--currentMovingObj.NumbersOfSteps <= 0)
                            movingObjIndex++;
                        if (movingObjIndex >= characterInfoList.Length)
                        {
                            movingObjIndex = 0;
                            state = State.WaitingForUser;
                        }
                        else
                        {
                            state = State.MonsterMovementSetting;
                        }

                    }
                    break;
                case State.PlayerDiscorved:
                    break;
                case State.PlayerDeath:
                    if (!explosion.IsVisible)
                    {
                        var explosionCoord = characterInfoList[movingObjIndex].Object.MovementEntity.Rect.Location;
                        explosion.Rect.Location = explosionCoord;
                        explosion.Rect.Offset(-25, -25);
                        explosion.IsVisible = true;
                        explosion.AnimationMode = true;
                    }
                    explosion.Update(gameTime);
                    endGameDelayTime -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (endGameDelayTime <= 0)
                        state = State.Lose;
                    break;
                case State.TreasureCollected:
                    break;
                case State.Lose:
                    SetEnabled(false);
                    EventBoard.Instance.Ev = EventBoard.Event.ShowResultsWhenLose;
                    break;
                case State.Win:
                    SetEnabled(false);
                    EventBoard.Instance.Ev = EventBoard.Event.ShowResultsWhenWin;
                    break;
            }
            foreach (var obj in characterInfoList)
            {
                obj.Object.Update(gameTime);
            }
        }

        private Character.MovementState GetRandomDirection(Character.ObjectType objType)
        {
            var movementState = Character.MovementState.Stand;

            switch (objType)
            {
                case Character.ObjectType.Mummy:
                    movementState = (Character.MovementState)random.Next(4);
                    break;
                case Character.ObjectType.Scorpion:
                    movementState = (Character.MovementState)random.Next(4, 8);
                    break;
                case Character.ObjectType.Zombie:
                    movementState = (Character.MovementState)random.Next(8);
                    break;
            }

            return movementState;
        }

        private Rectangle GetDestination(Rectangle oldRect, Character.MovementState movementState)
        {
            var newRect = oldRect;
            int offsetX = 0, offsetY = 0;
            switch (movementState)
            {
                case Character.MovementState.MoveDown:
                    offsetY = Configuration.Unit;
                    break;
                case Character.MovementState.MoveLeft:
                    offsetX = -Configuration.Unit;
                    break;
                case Character.MovementState.MoveRight:
                    offsetX = Configuration.Unit;
                    break;
                case Character.MovementState.MoveUp:
                    offsetY = -Configuration.Unit;
                    break;
                case Character.MovementState.MoveLeftUp:
                    offsetY = -Configuration.Unit;
                    offsetX = -Configuration.Unit;
                    break;
                case Character.MovementState.MoveLeftBottom:
                    offsetY = Configuration.Unit;
                    offsetX = -Configuration.Unit;
                    break;
                case Character.MovementState.MoveRightUp:
                    offsetY = -Configuration.Unit;
                    offsetX = Configuration.Unit;
                    break;
                case Character.MovementState.MoveRightBottom:
                    offsetY = Configuration.Unit;
                    offsetX = Configuration.Unit;
                    break;
            }
            newRect.Offset(offsetX, offsetY);
            var valid = true;
            valid = entrance.Rect.Contains(newRect) || exit.Rect.Contains(newRect) || area.Rect.Contains(newRect);
            if (!valid || obstacles.Any(obj => obj.Rect.Contains(newRect)))
                return Rectangle.Empty;
            return newRect;
        }

        private bool CheckCollision()
        {
            var playerRect = characterInfoList[0].Object.MovementEntity.Rect;
            for (int i = 1; i < characterInfoList.Length; i++)
            {
                if (characterInfoList[i].Object.MovementEntity.Rect.Equals(playerRect))
                    return true;
            }
            return false;
        }

        private bool CheckWinner()
        {
            return characterInfoList[0].Object.MovementEntity.Rect.Equals(exit.Rect);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;
            background.Draw(spriteBatch);
            config.Draw(spriteBatch);
            boundary.Draw(spriteBatch);
            area.Draw(spriteBatch);
            entrance.Draw(spriteBatch);
            spriteBatch.Draw(doorArrow, entrance.Rect, Color.White);
            exit.Draw(spriteBatch);
            spriteBatch.Draw(doorArrow, exit.Rect, Color.White);
            foreach (var obj in obstacles)
            {
                obj.Draw(spriteBatch);
            }
            foreach (var obj in characterInfoList)
            {
                obj.Object.Draw(spriteBatch);
            }
            explosion.Draw(spriteBatch);
            loseGame.Draw(spriteBatch);
        }

    }
}
