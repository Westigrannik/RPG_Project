void PrintSeparator()
{
    Console.WriteLine(new string('-', 40));
}

Random enemyRng = new Random();


void RegularPunch(Character attacker, Character defender)
{
    int damage = attacker.Attack();
    int gainedTp = Math.Max(2, damage / 3);
    attacker.AddTP(gainedTp);

    Console.WriteLine($"{attacker.Name} атакует ({damage} урона, +{gainedTp} TP) -> {defender.Name}");
    defender.TakeDamage(damage);
}


void ActivateBlock(Character character)
{
    character.Block = true;
    character.AddTP(10);
}


// !!!!!!!!!!!!!!!
void PlayerTurn(Character player, Character enemy)
{
    while (true)
    {
        Console.WriteLine($"\nХод игрока {player.Name} (TP: {player.TP}/{player.Maxtp}):");
        Console.WriteLine(">>> 1. Обычная атака");
        Console.WriteLine(">>> 2. Специальный приём");
        Console.WriteLine(">>> 3. Блок (50% защиты, +10 TP)");
        Console.Write("Выбор: ");

        string? choice = Console.ReadLine()?.Trim();

        if (choice == "1")
        {
            RegularPunch(player, enemy);
            return;
        }
        else if (choice == "2")
        {
            Console.WriteLine("\nСпециальные приёмы:");
            Console.WriteLine(player.SpecialMoves());
            Console.WriteLine("(0 — назад)");
            Console.Write("Выберите приём: ");

            if (int.TryParse(Console.ReadLine(), out int specChoice))
            {
                if (specChoice == 0) continue;

                int result = player.UseSpecial(specChoice);
                if (result == -1)
                {
                    Console.WriteLine("Недостаточно TP для этого приёма");
                }
                else if (result == -2)
                {
                    Console.WriteLine("Неверный номер приёма");
                }
                else if (result > 0)
                {
                    Console.WriteLine($"{player.Name} наносит {result} спец. урона -> {enemy.Name}");
                    enemy.TakeDamage(result);
                }
                if (result >= 0) return;
            }
        }
        else if (choice == "3")
        {
            ActivateBlock(player);
            return;
        }
        else
        {
            Console.WriteLine("Неверный выбор. Введите 1, 2 или 3.");
        }
    }
}

void EnemyTurn(Character enemy, Character player)
{
    Console.WriteLine($"\nХод противника {enemy.Name}:");
    int action = enemyRng.Next(1, 4); // Вот здесь
    if (action == 2 && enemy.TP >= 30)
    {   
        Console.WriteLine($"{enemy.Name} пробует использовать Специальный Приём");
        int randomSpecial = enemyRng.Next(1, enemy.SpecialCount + 1);
        int result = enemy.UseSpecial(randomSpecial);

            if (result > 0){
                Console.WriteLine($"  {enemy.Name} наносит {result} спец. урона -> {player.Name}");
                player.TakeDamage(result);
            }

            return;
        }
    

    if (action == 3) // Отключаю, а то слишком часто блокирует, бесит
    {
        ActivateBlock(enemy);
        Console.WriteLine("🛡️ Противник блокирует атаку! 🛡️");
    }
    else
    {
        RegularPunch(enemy, player);
        Console.WriteLine("Противник бьёт!");
    }
}
void Battle(Character player, Character enemy)
{
    Console.WriteLine();
    PrintSeparator();
    Console.WriteLine("⚔ БИТВА НАЧИНАЕТСЯ!");
    Console.WriteLine(player);
    Console.WriteLine("  против");
    Console.WriteLine(enemy);
    PrintSeparator();

    int round = 1;

    while (player.IsAlive() && enemy.IsAlive())
    {
        player.Block = false;
        enemy.Block = false;

        PlayerTurn(player, enemy);
        if (!enemy.IsAlive()) {
            player.Describe();
            enemy.Describe();
            break;
            };
        EnemyTurn(enemy, player);
        if (!player.IsAlive()) {
            player.Describe();
            enemy.Describe();
            break;
            }
        Console.WriteLine();
        player.UpdateBuffs();
        enemy.UpdateBuffs();
        player.Describe();
        enemy.Describe();
        round++;
        PrintSeparator();
    }

    if (player.IsAlive())
    {
        Console.WriteLine($"🏆 {player.Name} победил!");
    }
    else
    {
        Console.WriteLine($"💀 {enemy.Name} победил!");
    }
    PrintSeparator();
}

void PrintClasses()
{
    Console.WriteLine();
    Console.WriteLine("Выберите класс персонажа:");
    Console.WriteLine($"1 - {new Warrior("Особенности")}");
    Console.WriteLine($"2 - {new Mage("Особенности")}");
    Console.WriteLine($"3 - {new Archer("Особенности")}");
}

Character SelectCharacter(string prompt)
{
    Console.Write($"{prompt} = введите имя персонажа: ");
    string? name = Console.ReadLine();
    name = string.IsNullOrWhiteSpace(name) ? "Герой" : name.Trim();

    PrintClasses();

    while (true)
    {
        Console.Write("Выберите класс (1/2/3): ");
        string? choice = Console.ReadLine();

        switch (choice?.Trim())
        {
            case "1":
                return new Warrior(name);
            case "2":
                return new Mage(name);
            case "3":
                return new Archer(name);
            default:
                Console.WriteLine("Неверный выбор. Введите 1, 2 или 3.");
                break;
        }
    }
}

void RunGame()
{
    while (true)
    {
        Console.WriteLine("⚔ RPG БИТВА ⚔");
        Character player = SelectCharacter("Игрок");
        Character enemy = SelectCharacter("Противник");

        Battle(player, enemy);

        Console.Write("Сыграть ещё раз? (yes/нет): ");
        string? again = Console.ReadLine();

        if (again?.Trim().ToLower() == "yes")
        {
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("Спасибо за игру");
            break;
        }
    }
}

RunGame();