using System;
using System.Collections.Generic;
using System.IO;

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
		while(true)
		{
			Console.WriteLine("Fatal Error: 404");
			Console.WriteLine("-2147483647EE");


		}
		commands.SaveReciepts();
		commands.SaveRefunds();
		commands.LoadReciepts();
		commands.LoadRefunds();
        while (true)
        {
            string input = Console.ReadLine().ToLower();

            string[] command = input.Split(" ");

            switch (command[0])
            {
                case "exit":
					commands.SaveReciepts();
					commands.SaveRefunds();
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
				case "load":
					commands.LoadReciepts();
					break;
				default:
                    Console.WriteLine("Unknown Command");
                    break;
            }
        }
    }
}
class Commands
{
	string Recieptpath = @"C:..\..\..\Data\reciepts.txt";
	string Refundtpath = @"C:..\..\..\Data\refunds.txt";
    SystemCommands scomm = new SystemCommands();
    List<Reciept> reciepts = new List<Reciept>();
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
                for (int i = reciepts.Count - 1; i >= 0; i--)
                {
                    if (reciepts[i].GetName().ToLower() == refund[1].ToLower())
                    {
						//refunds.AddRange(reciept.Select(reciept[i] => i));
						// Reciept(reciepts[i].GetName(), reciepts[i].GetAAmount(), reciepts[i].GetCAmount(), reciepts[i].GetSAmount())

						Reciept tRefund = reciepts[i];
                        refunds.Add(tRefund);
                        reciepts.RemoveAt(i);
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
        Reciept reciept = new Reciept(name, int.Parse(aTickets), int.Parse(cTickets), int.Parse(sTickets));
        scomm.CheckDuplicate(reciept, reciepts, refunds);
        reciepts.Add(reciept);
    }
    public void Print()
    {
        bool found = false;
        if (reciepts.Count != 0)
        {
            Console.WriteLine("Purchased:");
            
            for (int i = 0; i < reciepts.Count; i++)
            {
                if (reciepts[i] != null)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(reciepts[i].GetName());
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
                for (int i = reciepts.Count - 1; i >= 0; i--)
                {
                    if (reciepts[i].GetName().ToLower() == search[1].ToLower())
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(reciepts[i].GetReciept());
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
    public void AmountSold()
    {
		int adultTickets = 0;
		int childTickets = 0;
		int seniorTickets = 0;
       foreach(Reciept r in reciepts)
		{
			adultTickets += r.GetAAmount();
			childTickets += r.GetCAmount();
			seniorTickets += r.GetSAmount(); 
		}
		Console.WriteLine($"Total Tickets Sold: {adultTickets + childTickets + seniorTickets}");
		Console.WriteLine($"Adult Tickets Sold: {adultTickets}");
		Console.WriteLine($"Child Tickets Sold: {childTickets}");
		Console.WriteLine($"Senior Tickets Sold: {seniorTickets}");
    }
	public void AmountRefunded()
    {
		int adultTickets = 0;
		int childTickets = 0;
		int seniorTickets = 0;
       foreach(Reciept r in refunds)
		{
			adultTickets += r.GetAAmount();
			childTickets += r.GetCAmount();
			seniorTickets += r.GetSAmount(); 
		}
		Console.WriteLine($"Total Tickets Refunded: {adultTickets + childTickets + seniorTickets}");
		Console.WriteLine($"Adult Tickets Refunded: {adultTickets}");
		Console.WriteLine($"Child Tickets Refunded: {childTickets}");
		Console.WriteLine($"Senior Tickets Refunded: {seniorTickets}");
    }
	public void SaveReciepts()
	{
		StreamWriter Recieptsw = new StreamWriter(Recieptpath, true);
		foreach (Reciept r in reciepts)
		{
			Recieptsw.WriteLine(r.Info()); ; 
		}
		Recieptsw.Close();
	}
	public void SaveRefunds()
	{
		StreamWriter Refundsw = new StreamWriter(Refundtpath, true);
		foreach (Reciept r in refunds)
		{
			Refundsw.WriteLine(r.Info()); ;
		}
		Refundsw.Close();
	}
	public void LoadReciepts()
	{
		StreamReader Recietsr = new StreamReader(Recieptpath);
		string text;
        while ((text = Recietsr.ReadLine()) != null)
        {
            string[] strings = text.Split(char.Parse(","));
			Reciept reciept = new Reciept(strings[0], int.Parse(strings[1]), int.Parse(strings[2]), int.Parse(strings[3]));
			reciepts.Add(reciept);
		}
        Recietsr.Close();
	}
	public void LoadRefunds()
	{
		StreamReader Refundsr = new StreamReader(Refundtpath);
		string text;
		while ((text = Refundsr.ReadLine()) != null)
		{
			string[] strings = text.Split(char.Parse(","));
			Reciept refund = new Reciept(strings[0], int.Parse(strings[1]), int.Parse(strings[2]), int.Parse(strings[3]));
			refunds.Add(refund);
		}
		Refundsr.Close();
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
	public void CheckDuplicate(Reciept reciept, List<Reciept> reciepts, List<Reciept> refunds)
	{
		for (int i = 0; i < reciepts.Count | i < refunds.Count; i++)
		{
			if (reciepts[i] != null)
			{
				if (reciepts[i].GetName().ToLower() == reciept.GetName().ToLower())
				{
					throw new RecieptAlreadyExistException();
				}
			}
		}
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
	public string Info()
	{
		return $"{name},{aTickets},{cTickets},{sTickets}";
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