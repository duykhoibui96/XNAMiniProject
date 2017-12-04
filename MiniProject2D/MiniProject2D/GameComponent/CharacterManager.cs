﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MiniProject2D.Config;
using MiniProject2D.Input;
using MiniProject2D.Model;
using MiniProject2D.Resource;
using MiniProject2D.Sound;

namespace MiniProject2D.GameComponent
{
    class CharacterManager
    {

        private Character[] characters;
        private Point targetPos;
        private int characterTrackerIndex;
        private bool isProcessing;
        private TerrainManager terrainManager;

        public Point CollisionPos { get; set; }


        public void Init(TerrainManager terrainManager, int numOfMummies, int numOfScorpions, int numOfZombies)
        {
            this.terrainManager = terrainManager;
            RandomCharacters(numOfMummies, numOfScorpions, numOfZombies);
            isProcessing = false;
            characterTrackerIndex = 0;
        }

        private void RandomCharacters(int numbersOfMummy, int numbersOfScorpion, int numbersOfZombie)
        {
            var unit = Configuration.Unit;
            var rand = Configuration.Rand;
            var entrancePosition = terrainManager.EntrancePos;

            characters = new Character[1 + numbersOfMummy + numbersOfScorpion + numbersOfZombie];
            characters[0] = ObjectInit(entrancePosition.X, entrancePosition.Y, Character.ObjectType.Player);
            var index = 1;
            for (int i = 0; i < numbersOfMummy; i++)
            {
                var positionX = rand.Next(2, 20) * unit;
                var positionY = rand.Next(2, 10) * unit;
                while (true)
                {
                    if (terrainManager.isValidPosition(new Point(positionX, positionY)))
                        break;
                    positionX = rand.Next(2, 20) * unit;
                    positionY = rand.Next(2, 10) * unit;
                }
                characters[index++] = ObjectInit(positionX, positionY, Character.ObjectType.Mummy);
            }
            for (int i = 0; i < numbersOfScorpion; i++)
            {
                var positionX = rand.Next(2, 20) * unit;
                var positionY = rand.Next(2, 10) * unit;
                while (true)
                {
                    if (terrainManager.isValidPosition(new Point(positionX, positionY)))
                        break;
                    positionX = rand.Next(2, 20) * unit;
                    positionY = rand.Next(2, 10) * unit;
                }
                characters[index++] = ObjectInit(positionX, positionY, Character.ObjectType.Scorpion);
            }
            for (int i = 0; i < numbersOfZombie; i++)
            {
                var positionX = rand.Next(2, 20) * unit;
                var positionY = rand.Next(2, 10) * unit;
                while (true)
                {
                    if (terrainManager.isValidPosition(new Point(positionX, positionY)))
                        break;
                    positionX = rand.Next(2, 20) * unit;
                    positionY = rand.Next(2, 10) * unit;
                }
                characters[index++] = ObjectInit(positionX, positionY, Character.ObjectType.Zombie);
            }
        }

        private Character ObjectInit(int startX, int startY, Character.ObjectType objectType)
        {
            var unit = Configuration.Unit;
            int numbersOfFrame = 0, defaultFrameIndex = 0;
            var sprite = ResManager.Instance.Player;
            var objType = Character.ObjectType.Player;
            var color = Color.White;

            switch (objectType)
            {
                case Character.ObjectType.Player:
                    numbersOfFrame = 4;
                    defaultFrameIndex = 2;
                    color = Setting.Instance.PlayerColor;
                    break;
                case Character.ObjectType.Mummy:
                    numbersOfFrame = 3;
                    defaultFrameIndex = 1;
                    sprite = ResManager.Instance.Mummy;
                    objType = Character.ObjectType.Mummy;
                    color = Setting.Instance.MummyColor;
                    break;
                case Character.ObjectType.Scorpion:
                    numbersOfFrame = 4;
                    defaultFrameIndex = 2;
                    sprite = ResManager.Instance.Scorpion;
                    objType = Character.ObjectType.Scorpion;
                    color = Setting.Instance.ScorpionColor;
                    break;
                case Character.ObjectType.Zombie:
                    numbersOfFrame = 6;
                    defaultFrameIndex = 0;
                    sprite = ResManager.Instance.Zombie;
                    objType = Character.ObjectType.Zombie;
                    color = Setting.Instance.ZombieColor;
                    break;
            }

            return new Character(new AnimationEntity(sprite, new Rectangle(startX, startY, unit, unit), color, numbersOfFrame, defaultFrameIndex), objType);
        }

        private Vision.Direction GetUserMovementChoice()
        {
            var pressedKey = KeyboardEvent.Instance.PressedKey;
            var movementState = Vision.Direction.None;
            switch (pressedKey)
            {
                case Keys.Down:
                case Keys.S:
                    movementState = Vision.Direction.Bottom;
                    break;
                case Keys.Left:
                case Keys.A:
                    movementState = Vision.Direction.Left;
                    break;
                case Keys.Right:
                case Keys.D:
                    movementState = Vision.Direction.Right;
                    break;
                case Keys.Up:
                case Keys.W:
                    movementState = Vision.Direction.Top;
                    break;
            }

            return movementState;
        }

        private Vision.Direction GetMonsterMovementChoice()
        {
            var currentCharacter = characters[characterTrackerIndex];

            if (!currentCharacter.PreferableDirection.Equals(Vision.Direction.None))
                return currentCharacter.PreferableDirection;

            var rand = Configuration.Rand;

            switch (currentCharacter.ObjType)
            {
                case Character.ObjectType.Mummy:
                    return (Vision.Direction)rand.Next(4);
                case Character.ObjectType.Scorpion:
                    return (Vision.Direction)rand.Next(4, 8);
                case Character.ObjectType.Zombie:
                    return (Vision.Direction)rand.Next(8);
            }

            return Vision.Direction.None;
        }

        public void Update(GameTime gameTime)
        {
            var currentCharacter = characters[characterTrackerIndex];

            if (isProcessing)
            {
                if (targetPos.Equals(currentCharacter.MovementEntity.Rect.Location))
                {
                    currentCharacter.Stop();
                    Discover();
                    currentCharacter.NumOfSteps--;
                    SoundManager.Instance.PlaySound(ResManager.Instance.FootSteps);
                    if (currentCharacter.NumOfSteps == 0)
                    {
                        characterTrackerIndex++;
                        if (characterTrackerIndex >= characters.Length)
                        {
                            characterTrackerIndex = 0;
                        }
                    }
                    isProcessing = false;
                }
            }
            else
            {
                var movementDirection = Vision.Direction.None;
                var unit = Configuration.Unit;
                movementDirection = currentCharacter.ObjType.Equals(Character.ObjectType.Player) ? GetUserMovementChoice() : GetMonsterMovementChoice();
                if (!movementDirection.Equals(Vision.Direction.None))
                {
                    var newPosition = currentCharacter.MovementEntity.Rect.Location;
                    switch (movementDirection)
                    {
                        case Vision.Direction.Bottom:
                            newPosition.Y += unit;
                            break;
                        case Vision.Direction.Left:
                            newPosition.X -= unit;
                            break;
                        case Vision.Direction.Right:
                            newPosition.X += unit;
                            break;
                        case Vision.Direction.Top:
                            newPosition.Y -= unit;
                            break;
                        case Vision.Direction.TopLeft:
                            newPosition.X -= unit;
                            newPosition.Y -= unit;
                            break;
                        case Vision.Direction.BottomLeft:
                            newPosition.X -= unit;
                            newPosition.Y += unit;
                            break;
                        case Vision.Direction.TopRight:
                            newPosition.X += unit;
                            newPosition.Y -= unit;
                            break;
                        case Vision.Direction.BottomRight:
                            newPosition.X += unit;
                            newPosition.Y += unit;
                            break;
                    }
                    if (terrainManager.isValidPosition(newPosition))
                    {
                        targetPos = newPosition;
                        currentCharacter.StartMoving(movementDirection);
                        SoundManager.Instance.PlaySound(ResManager.Instance.FootSteps);
                        isProcessing = true;
                    }
                }

            }

            foreach (var character in characters)
            {
                character.Update(gameTime, terrainManager);
            }
        }

        public bool CheckCollision()
        {
            var player = characters[0];
            for (int i = 1; i < characters.Length; i++)
            {
                if (characters[i].MovementEntity.Rect.Equals(player.MovementEntity.Rect))
                {
                    CollisionPos = characters[i].MovementEntity.Rect.Location;
                    return true;
                }
            }
            return false;
        }

        public bool IsWon()
        {
            return characters[0].MovementEntity.Rect.Location.Equals(terrainManager.ExitPos);
        }

        private void Discover()
        {
            var player = characters[0];
            var currentCharacter = characters[characterTrackerIndex];

            if (currentCharacter.Equals(player))
            {
                for (int i = 1; i < characters.Length; i++)
                {
                    var direction = characters[i].Find(player);
                    characters[i].MaxPower = !direction.Equals(Vision.Direction.None);
                    characters[i].PreferableDirection = direction;
                }
            }
            else
            {
                var direction = currentCharacter.Find(player);
                currentCharacter.MaxPower = !direction.Equals(Vision.Direction.None);
                currentCharacter.PreferableDirection = direction;
            }


        }

        public void Draw(SpriteBatch spriteBatch, bool isDisabled = false)
        {
            foreach (var character in characters)
            {
                character.Draw(spriteBatch, isDisabled);
            }
        }

    }
}
