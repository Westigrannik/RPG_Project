public abstract class Character
{
    public string Name { get; set; }
    public int HP { get; private set; }
    public int Maxhp { get; private set; }
    public int TP { get; private set; }
    public int Maxtp { get; private set; }
    public bool Block {get; set; }

    public int BuffTurns { get; protected set; }

    protected readonly Random _random = new Random();
    protected Random RNG => _random;

    public Character(string name, int hp, int tp = 0)
    {
        Name = name;
        Maxhp = hp;
        HP = hp;
        Maxtp = 50;
        TP = Math.Clamp(tp, 20, Maxtp);
        Block = false; 
    }

    public abstract int Attack();

// SPECIAL_SPECIAL_SPECIAL_SPECIAL_SPECIAL_SPECIAL_
    public abstract string SpecialMoves();

    public abstract int SpecialCount { get;}

    public abstract int UseSpecial(int choice);

    public virtual void UpdateBuffs()
    {
        if (BuffTurns > 0)
        {
            BuffTurns--;
            if (BuffTurns == 0)
            {
                OnBuffExpired();
            }
        }
    }

    protected virtual void OnBuffExpired() { }

// SPECIAL_SPECIAL_SPECIAL_SPECIAL_SPECIAL_SPECIAL_


    public virtual void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            return;
        }
        if (Block) {
            damage = (int)Math.Round(damage * 0.5);
            Console.WriteLine($"🛡️ {Name} блокирует половину урона 🛡️");

        }
            HP -= damage; 

        if (HP < 0)
        {
            HP = 0;
        }
        
        int gainedTp = Math.Max(1, damage / 3);
        AddTP(gainedTp);
    }

    public bool IsAlive()
    {
        return HP > 0;
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;
        HP = Math.Min(Maxhp, HP + amount);
    }

   public void AddTP(int amount)
    {
        if (amount <= 0) return;
        TP = Math.Min(Maxtp, TP + amount);
    }

    public bool TrySpendTP(int cost)
    {
        if (TP < cost) return false;
        TP -= cost;
        return true;
    }

    public string HPBar
    {
        get
        {
            int filled = (int)Math.Round((double)HP / Maxhp * 10);
            int empty = 10 - filled;
            return $"[{new string('█', filled)}{new string('░', empty)}] {HP}/{Maxhp}";
        }
    }

    public string TPBar
    {
        get
        {
            int filled = (int)Math.Round((double)TP / Maxtp * 10);
            int empty = 10 - filled;
            return $"[{new string('█', filled)}{new string('░', empty)}] {TP}/{Maxtp}";
        }
    }

    public void Describe()
    {
        Console.WriteLine($"{Name}\nHP: {HPBar} | TP: {TPBar}");
    }

    public abstract override string ToString();
}