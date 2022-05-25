Account acc = new(100);
acc.Notify += DisplayMessage;
acc.Put(10);
acc.Take(20);
acc.Take(300);


void DisplayMessage(Account sender, AccountEventArgs e)
{
    Console.WriteLine($"Сумма транзакции: {e.Summa}");
    Console.WriteLine(e.Message);
    Console.WriteLine($"Текущая сумма на счете: {sender.Sum}");
    Console.WriteLine();
}


class AccountEventArgs
{
    public string Message { get; }
    public int Summa { get; }
    public AccountEventArgs(string message, int sum)
    {
        Message = message;
        Summa = sum;
    }
}

class Account
{
    public delegate void AccountHandler(Account sender, AccountEventArgs e);
    public event AccountHandler? Notify;
    public int Sum { get; private set; }
    public Account(int sum) => Sum = sum;

    internal void Put(int moneyPut)
    {
        Sum += moneyPut;
        Notify?.Invoke(this, new AccountEventArgs($"Пополнение на {moneyPut} руб.", moneyPut));
    }

    internal void Take(int moneyTake)
    {
        if (Sum > moneyTake)
        {
            Sum -= moneyTake;
            Notify?.Invoke(this, new AccountEventArgs($"Снято {moneyTake} руб.", moneyTake));
        }
        else
            Notify?.Invoke(this, new AccountEventArgs("Недостаточно средств", moneyTake));
    }
}