using System;
using System.Collections.Generic;

public class Program
{
    Commands commands = new Commands();

    static void Main()
    {
        Program p = new Program();
        p.Run();
    }

    void Run()
    {
        while (true)
        {
            string input = Console.ReadLine().ToLower();

            string[] command = input.Split(" ");

            switch (command[0])
            {
                case "exit":
                    Environment.Exit(0);
                    break;
                case "purchase":
                    commands.Purchase(command);
                    break;
                case "find":
                    commands.Find(command);
                    break;
                case "print":
                    commands.Print();
                    break;
                case "refund":
                    commands.Refund(command);
                    break;
                case "amount":
                    commands.AmountSold();
                    break;
                default:
                    Console.WriteLine("Unknown Command");
                    break;
            }
        }
    }

    public int Foo(int i)
    {
        return ++i;
    }
}

class Commands
{
    SystemCommands scomm = new SystemCommands();
    List<Reciept> reciept = new List<Reciept>();
    List<Reciept> refunds = new List<Reciept>(); 
    public void Purchase(string[] purchase)
    {
        if (purchase.Length != 5)
        {
            scomm.SyntaxError();
        }
        else if (purchase.Length > 1 && purchase[1].Length > 10)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Name must be less then 10 characters");
            Console.ResetColor();
        }
        else if (int.Parse(purchase[4]) < 0 | int.Parse(purchase[4]) < 0 | int.Parse(purchase[3]) < 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Cannot select a value smaler than 0");
            Console.ResetColor();
        }
        else
        {
            try
            {
                Add(purchase[1], purchase[2], purchase[3], purchase[4]);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Succesfully purchased");
                Console.ResetColor();
            }
            catch (RecieptAlreadyExistException)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("A reciept with that name already exists");
                Console.ResetColor();
            }
            catch (Exception)
            {
                scomm.WrongInput();
            }
        }
    }

    public void Refund(string[] refund)
    {
        if (refund.Length > 2)
        {
            scomm.SyntaxError();
        }
        else
        {
            try
            {
                for (int i = reciept.Count - 1; i >= 0; i--)
                {
                    if (reciept[i].GetName().ToLower() == refund[1].ToLower())
                    {
                        //refunds.AddRange(reciept.Select(reciept[i] => i));
                        Reciept tRefund = new Reciept(reciept[i].GetName(), reciept[i].GetAAmount(), reciept[i].GetCAmount(), reciept[i].GetSAmount());
                        refunds.Add(tRefund);
                        reciept.RemoveAt(i);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Succesfully refunded");
                        Console.ResetColor();
                    }
                }
            }
            catch
            {
                scomm.WrongInput();
            }
        }
    }

    public void Add(string name, string aTickets, string cTickets, string sTickets)
    {
        Reciept reciepts = new Reciept(name, int.Parse(aTickets), int.Parse(cTickets), int.Parse(sTickets));
        CheckDuplicate(reciepts);
        reciept.Add(reciepts);
    }

    public void Print()
    {
        bool found = false;
        if (reciept.Count != 0)
        {
            Console.WriteLine("Purchased:");
            
            for (int i = 0; i < reciept.Count; i++)
            {
                if (reciept[i] != null)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(reciept[i].GetName());
                    Console.ResetColor();
                    found = true;
                }
            }
        }
        if (refunds.Count != 0)
        {
            Console.WriteLine("Refunded:");
            for (int i = 0; i < refunds.Count; i++)
            {
                if (refunds[i] != null)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(refunds[i].GetName());
                    Console.ResetColor();
                    found = true;
                }
            }
        }
        if(!found)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("There are no purchases or refunds");
            Console.ResetColor();
        }
    }

    public void Find(string[] search)
    {
        if (search.Length > 2)
        {
            scomm.SyntaxError();
        }
        else
        {
            try
            {
                bool found = false;
                for (int i = reciept.Count - 1; i >= 0; i--)
                {
                    if (reciept[i].GetName().ToLower() == search[1].ToLower())
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(reciept[i].GetReciept());
                        Console.ResetColor();
                        found = true;
                    }
                }
                for (int i = refunds.Count - 1; i >= 0; i--)
                {
                    if (refunds[i].GetName().ToLower() == search[1].ToLower())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("!Refunded!");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(refunds[i].GetReciept());
                        Console.ResetColor();
                        found = true;
                    }
                }
                if (!found)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("There is no reciept with that name");
                    Console.ResetColor();
                }
            }
            catch
            {
                scomm.WrongInput();
            }
        }
    }

    private void CheckDuplicate(Reciept reciepts)
    {
        for (int i = 0; i < reciept.Count | i < refunds.Count; i++)
        {
            if (reciept[i] != null)
            {
                if (reciept[i].GetName().ToLower() == reciepts.GetName().ToLower())
                {
                    throw new RecieptAlreadyExistException();
                }
            }
        }
    }

    public void AmountSold()
    {
        for (int i = 0; i < reciept.Count | i < refunds.Count; i++)
        {
            if (reciept[i] != null)
            {
                Console.WriteLine($"Total Tickets sold: {reciept[i].GetAAmount() + reciept[i].GetCAmount() + reciept[i].GetSAmount()}");
                Console.WriteLine($"Adult Tickets sold: {reciept[i].GetAAmount()}");
                Console.WriteLine($"Child Tickets sold: {reciept[i].GetCAmount()}");
                Console.WriteLine($"Senior Tickets sold: {reciept[i].GetSAmount()}");
            }
        }
    }
}

class RecieptAlreadyExistException : Exception { }

class SystemCommands
{
    public void SyntaxError()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Syntax error");
        Console.ResetColor();
    }
    public void WrongInput()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Wrong input, try again");
        Console.ResetColor();
    }
}

class Reciept
{
    private string name;
    private int aTickets;
    private int cTickets;
    private int sTickets;

    private int aPrice = 100;
    private int cPrice = 25;
    private int sPrice = 75;

    private int tPrice;
    private string returnvalue;
    public Reciept(string name, int aTickets, int cTickets, int sTickets)
    {
        this.name = name;
        this.aTickets = aTickets;
        this.cTickets = cTickets;
        this.sTickets = sTickets;
    }
    public int GetAAmount()
    {
        return aTickets;
    }
    public int GetCAmount()
    {
        return cTickets;
    }
    public int GetSAmount()
    {
        return sTickets;
    }
    public string GetName()
    {
        return name;
    }
    public string GetReciept()
    {
        returnvalue = $"Name: {name} \n";
        if (aTickets > 0)
        {
            returnvalue = returnvalue + $"Adult: {aTickets} \n"; //string interpolation
            tPrice = tPrice + aTickets * aPrice;
        }
        if (cTickets > 0)
        {
            returnvalue = returnvalue + $"Child: {cTickets} \n";
            tPrice = tPrice + cTickets * cPrice;
        }
        if (sTickets > 0)
        {
            returnvalue = returnvalue + $"Senior: {sTickets} \n";
            tPrice = tPrice + sTickets * sPrice;
        }
        returnvalue = returnvalue + $"\nTotal Price: {tPrice}kr";
        return returnvalue;
    }
}