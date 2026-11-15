public class Warrior : Character
{
    private const int BaseArmor = 5;
    public int Armor { get; private set; }
    public Warrior(string name) : base(name, 200)
    {
        Armor = BaseArmor;
    }

    public override int Attack()
    {
        return 15 + _random.Next(11);
    }

    public override int SpecialCount => 2;

    public override string SpecialMoves()
    {
        return "  1. Широкий замах (20 TP) # Наносит урон от 25 до 35\n" +
               "  2. Укрепление духа (30 TP) # Усилывает защиту от 5 до 15 на 3 хода";
    }


     public override int UseSpecial(int choice)
    {
        switch (choice)
        {
            case 1:
                if (!TrySpendTP(20)) return -1;
                int damage = 25 + _random.Next(11);
                Console.WriteLine($"Был использован приём @@ Широкий Замах @@");
                return damage;
                
            case 2:
                if (!TrySpendTP(30)) return -1;
                Armor = BaseArmor + 10;
                BuffTurns = 3;
                Console.WriteLine($"Был использован приём @@ Укрепление духа @@");
                return 0;

            default:
                return -2;
        }
    }

    protected override void OnBuffExpired()
    {
        Armor = BaseArmor;
        Console.WriteLine($"💧 Эффект {Name} закончился. 💧");
        Console.WriteLine();
    }
    
    public override void TakeDamage(int damage)
    {
        int reduced = Math.Clamp(damage - Armor, 0, damage);
        base.TakeDamage(reduced);
        Console.WriteLine();
    }

    public override string ToString()
    {
        return $"⚔ Воин {Name} (брони: {Armor})";
    }
}

public class Mage : Character
{
    public int Mana { get; private set; }
    public int MaxMana { get; private set; }
    private int bonusmagic = 0;

    public Mage(string name) : base(name, 150)
    {
        Mana = 60;
        MaxMana = 60;
    }

    public override int Attack()
    {
        if (Mana >= 20)
        {
            Mana -= 20;
            return 25 + _random.Next(16) + bonusmagic;
        }
        else
        {
            Console.WriteLine("ЗАКОНЧИЛАСЬ МАНА");
            return 5 + _random.Next(6);
        }
    }

    public override int SpecialCount => 3;

     public override string SpecialMoves()
    {
        return 
               "  1. Лечение (25 TP) # восстанавливает 50 HP\n" +
               "  2. Усиление магии (30 TP) # +15 к урону магии на 2 хода\n" +
               "  3. Пополнение энергией (15 TP) # восстанавливает 60 маны";
    }

    public override int UseSpecial(int choice)
    {
        switch (choice)
        {
            case 1:
                if (!TrySpendTP(25)) return -1;
                Heal(50);
                Console.WriteLine($"Был использован приём ~~ Лечение ~~");
                return 0;

            case 2:
                if (!TrySpendTP(30)) return -1;
                bonusmagic = 15;
                BuffTurns = 3;
                Console.WriteLine($"Был использован приём ~~ Усиление магии ~~");
                return 0;

            case 3:
                if (!TrySpendTP(15)) return -1;
                Mana = Math.Min(MaxMana, Mana + 60);
                Console.WriteLine($"Был использован приём ~~ Пополнение энергии ~~");
                return 0;

            default:
                return -2;
        }
    }

    protected override void OnBuffExpired()
    {
        bonusmagic = 0;
        Console.WriteLine($"💧 Эффект {Name} закончился. 💧");
        Console.WriteLine();
    }



    public override string ToString()
    {
        return $"🔮 Маг {Name} (мана: {Mana}/{MaxMana})";
    }
}

public class Archer : Character
{
    public double CritChance { get; private set; }
    public double DodgeChance {get; private set; }

    private const double BaseCrit = 0.15;
    private const double BaseDodge = 0.05;

    public Archer(string name) : base(name, 170)
    {
        CritChance = BaseCrit;
        DodgeChance = BaseDodge;
    }

    public override int Attack()
    {
        bool isCrit = _random.NextDouble() < CritChance;
        int baseDamage = 10 + _random.Next(11);

        if (isCrit)
        {
            Console.WriteLine(" 💥 Критический удар!");
            return (int)Math.Round(baseDamage * 2.5);
        }

        return baseDamage;
    }

    public override int SpecialCount => 2;

      public override string SpecialMoves()
    {
        return "  1. Зоркий глаз (30 TP) # Увеличивает шанс крит удара на 35% на 2 хода\n" +
               "  2. Камуфляж (30 TP) # Увеличивает шанс уворота на 35% на 2 хода";
    }

     public override int UseSpecial(int choice)
    {
        switch (choice)
        {
            case 1:
                if (!TrySpendTP(30)) return -1;
                CritChance = BaseCrit + 0.35;
                BuffTurns = 3;
                Console.WriteLine($"Был использован приём >> Зоркий глаз <<");
                return 0;

            case 2:
                if (!TrySpendTP(30)) return -1;
                DodgeChance = BaseDodge + 0.35;
                BuffTurns = 3;
                Console.WriteLine($"Был использован приём >> Камуфляж <<");
                return 0;

            default:
                return -2;
        }
    }

    protected override void OnBuffExpired()
    {
        CritChance = BaseCrit;
        DodgeChance = BaseDodge;
        Console.WriteLine($"💧 Эффект {Name} закончился. 💧");
        Console.WriteLine();
    }

    public override void TakeDamage(int damage)
    {
        if (_random.NextDouble() < DodgeChance)
        {
            Console.WriteLine($"  💨 {Name} уклонился от атаки! 💨");
            return;
        }
        base.TakeDamage(damage);
    }

    public override string ToString()
    {
        return $"🏹 Лучник {Name} (шанс крита: {Math.Round(CritChance * 100)}%)";
    }
}