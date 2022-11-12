ComputerClub computerClub = new ComputerClub(8);
computerClub.Work();

class ComputerClub
{
    private int _money = 0;
    private List<Computer> _computers = new List<Computer>();
    private Queue<Schoolboy> _schoolboys = new Queue<Schoolboy>();

    public ComputerClub(int computerCount)
    {
        Random rand = new Random();

        for (int i = 0; i < computerCount; i++)
        {
            _computers.Add(new Computer(rand.Next(5, 15)));
        }

        CreateNewSchoolBoy(25);
    }

    public void CreateNewSchoolBoy(int count)
    {
        Random rand = new Random();

        for (int i = 0; i < count; i++)
        {
            _schoolboys.Enqueue(new Schoolboy(rand.Next(100, 250), rand));
        }
    }

    public void Work()
    {
        while (_schoolboys.Count > 0)
        {
            Console.WriteLine($"У компьютерного клуба сейчас {_money} рублей, ждем нового клиента.");

            Schoolboy schoolboy = _schoolboys.Dequeue();
            Console.WriteLine($"В очереди молодой человек, он хочет купить {schoolboy.DesiredMinutes} минут.");
            Console.WriteLine($"\nСписок компьютеров:");
            ShowAllComputers();

            Console.Write($"\nВы предлагаете ему ПК под номерои - ");

            int computerNumber;

            while (true)
            {
                var s = Console.ReadLine();
                if (int.TryParse(s, out int number))
                {
                    computerNumber = number;
                    break;
                }
                else
                {
                    Console.WriteLine("Не удалось распознать число, попробуйте еще раз.");
                }
            }
            

            if (computerNumber >= 0 && computerNumber < _computers.Count)
            {
                if (_computers[computerNumber].IsBusy)
                {
                    Console.WriteLine($"Вы предложили клиенту компьютер который уже занят. Клиент ушел.");
                }
                else
                {
                    if (schoolboy.CheckSolvency(_computers[computerNumber]))
                    {
                        Console.WriteLine($"Пересчитав деньги клиент оплатил нужное время и сел за компьютер.");
                        _money += schoolboy.ToPay();

                        _computers[computerNumber].TakeThePlace(schoolboy);
                    }
                    else
                    {
                        Console.WriteLine($"У клиента не хватило денег, он ушёл.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Вы сами не понимаете за какой ПК его посадить. Клиент ушёл.");
            }

            Console.WriteLine($"Для того, чтобы перейти к новому клиенту нажмите любую клавишу.");
            Console.ReadKey();
            Console.Clear();
            SkipMinute();
        }
    }

    public void SkipMinute()
    {
        foreach(var computer in _computers)
        {
            computer.SkipMinute();
        }
    }

    private void ShowAllComputers()
    {
        for (int i = 0; i < _computers.Count; i++)
        {
            Console.Write($"{i} -");
            _computers[i].ShowInfo();
        }
    }
}

class Computer
{
    private Schoolboy _schoolboy;
    private int _minutesLeft;

    public int PriceForMinutes { get; private set; }
    public bool IsBusy
    {
        get
        {
            return _minutesLeft > 0;
        }
    }

    public Computer(int priceForMinutes)
    {
        PriceForMinutes = priceForMinutes;
    }

    public void TakeThePlace(Schoolboy schoolboy)
    {
        _schoolboy = schoolboy;
        _minutesLeft = _schoolboy.DesiredMinutes;
    }

    public void FreeThePlace()
    {
        _schoolboy = null;
    }

    public void SkipMinute()
    {
        _minutesLeft--;
    }

    public void ShowInfo()
    {
        if (IsBusy)
            Console.WriteLine($"Компьютер занят. Осталось минут - {_minutesLeft}");
        else
            Console.WriteLine($"Компьютер свободен. Цена за минуту - {PriceForMinutes}");
    }
}

class Schoolboy
{
    private int _money;
    private int _moneyToPay;

    public int DesiredMinutes { get; private set; }

    public Schoolboy(int money, Random rand)
    {
        _money = money;
        DesiredMinutes = rand.Next(10, 30);
    }

    public bool CheckSolvency(Computer computer)
    {
        _moneyToPay = computer.PriceForMinutes * DesiredMinutes;
        if (_money >= _moneyToPay)
        {
            return true;
        }
        else
        {
            _moneyToPay = 0;
            return false;
        }
    }

    public int ToPay()
    {
        _money -= _moneyToPay;
        return _moneyToPay;
    }
}
