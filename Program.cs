
using System;
using System.IO;
using System.Threading;

class Program
{
    static int width = 40;
    static int height = 20;

    static int playerX = width / 2;
    static int playerY = height - 1;

    static int itemX;
    static int itemY;
    static char itemChar;

    static int score = 0;
    static bool gameOver = false;

    static Random rnd = new Random();
    static StreamWriter log;

    // --- LOG FONKSİYONLARI ---
    static void LogInput(string key)
    {
        log.WriteLine($"INPUT → key={key} playerX={playerX} playerY={playerY}");
        log.Flush();
    }

    static void LogMove()
    {
        log.WriteLine($"MOVE → playerX={playerX} playerY={playerY}");
        log.Flush();
    }

    static void LogUpdateSpawn()
    {
        log.WriteLine($"UPDATE → itemSpawned x={itemX} y={itemY}");
        log.Flush();
    }

    static void LogUpdateMove()
    {
        log.WriteLine($"UPDATE → itemMove x={itemX} y={itemY}");
        log.Flush();
    }

    static void LogCheck()
    {
        log.WriteLine($"CHECK → playerX={playerX} playerY={playerY} itemX={itemX} itemY={itemY}");
        log.Flush();
    }

    static void LogCollision()
    {
        log.WriteLine($"COLLISION → score={score}");
        log.Flush();
    }

    static void LogGameOver()
    {
        log.WriteLine($"GAMEOVER → score={score}");
        log.Flush();
    }

    // --- OYUN ---
    static void SpawnItem()
    {
        itemX = rnd.Next(0, width);
        itemY = 0;
        itemChar = rnd.Next(2) == 0 ? '*' : 'O';

        LogUpdateSpawn();
    }

    static void DrawChar(int x, int y, char c)
    {
        Console.SetCursorPosition(x, y);
        Console.Write(c);
    }

    static void DrawHUD()
    {
        Console.SetCursorPosition(0, height);
        Console.Write($"Score: {score}   ");
    }

    static void Input()
    {
        if (!Console.KeyAvailable) return;

        var key = Console.ReadKey(true).Key;

        if (key == ConsoleKey.LeftArrow && playerX > 0)
        {
            LogInput("LeftArrow");

            DrawChar(playerX, playerY, ' ');
            playerX--;
            DrawChar(playerX, playerY, '@');

            LogMove();
        }
        else if (key == ConsoleKey.RightArrow && playerX < width - 1)
        {
            LogInput("RightArrow");

            DrawChar(playerX, playerY, ' ');
            playerX++;
            DrawChar(playerX, playerY, '@');

            LogMove();
        }
    }

    static void Update()
    {
        DrawChar(itemX, itemY, ' ');

        itemY++;
        LogUpdateMove();

        LogCheck();

        if (playerX == itemX && playerY == itemY)
        {
            score++;
            LogCollision();
            SpawnItem();
        }

        if (itemY >= height)
            SpawnItem();

        DrawChar(itemX, itemY, itemChar);
        DrawHUD();

        if (score >= 10)
        {
            gameOver = true;
            LogGameOver();
        }
    }

    static void Main()
    {
        Console.CursorVisible = false;
        Console.SetWindowSize(width + 2, height + 2);
        Console.SetBufferSize(width + 2, height + 2);

        log = new StreamWriter("log.txt");

        Console.Clear();

        SpawnItem();

        DrawChar(playerX, playerY, '@');
        DrawChar(itemX, itemY, itemChar);
        DrawHUD();

        while (!gameOver)
        {
            Input();
            Update();
            Thread.Sleep(80);
        }

        log.Close();

        Console.SetCursorPosition(0, height + 1);
        Console.WriteLine("GAME OVER! Score: " + score);
        Console.ReadKey();
    }
}