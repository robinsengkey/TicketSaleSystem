using System;
using MySql.Data.MySqlClient;

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
		commands.Connect();
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
				case "load":
					commands.Load();
					break;
				case "find":
					commands.Find(command);
					break;
				case "refund":
					commands.Refund(command);
					break;
				case "sum":
					commands.Sum();
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
	MySqlConnection con;
	SystemCommands scomm = new SystemCommands();
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
            catch
            {
                scomm.WrongInput();
            }
        }
    }
	public void Connect()
	{
		string cs = @"server=localhost;userid=root;password=Robin789;database=ticketsales";
		con = new MySqlConnection(cs);
		con.Open();
	}
	public void Add(string name, string aTickets, string cTickets, string sTickets)
	{
		var sql = $"insert into purchases (name,aTickets,cTickets,sTickets) values ('{name}',{aTickets},{cTickets},{sTickets});";
		var cmd = new MySqlCommand(sql, con);
		cmd.ExecuteScalar();
	}
	public void Refund(string[] refund)
	{
		try
		{
			var sql = $"UPDATE `ticketsales`.`purchases` SET `refunded` = 1 WHERE(`NAME` = '{refund[1]}');";
			var cmd = new MySqlCommand(sql, con);
			cmd.ExecuteScalar();
			Console.WriteLine("Refunded");
		}
		catch { }
	}
	public void Load()
	{
		try
		{
			string sql = "SELECT * FROM purchases;";
			var cmd = new MySqlCommand(sql, con);
			MySqlDataReader rdr = cmd.ExecuteReader();
			while (rdr.Read())
			{
				Console.Write("ID: {0}, Name: {1}, Adult Tickets: {2}, Child Tickets: {3}, Senior Tickets {4}", rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4));
				if (rdr.GetInt32(5) == 1)
				{
					Console.Write(", !REFUNDED!");
				}
				Console.WriteLine("");

			}
			rdr.Close();
		}
		catch { }
	}
	public void Sum()
	{
		try
		{
			string sql = "select refunded, sum(aTickets), sum(cTickets), sum(sTickets) from purchases group by 1;";
			var cmd = new MySqlCommand(sql, con);
			MySqlDataReader rdr = cmd.ExecuteReader();
			while (rdr.Read())
			{
				var refunded = rdr.GetBoolean(0);
				if (refunded)
				{
					Console.WriteLine("refunded:");
				}
				else
				{
					Console.WriteLine("purchased:");
				}
				Console.WriteLine("Adult Tickets: {0}, Child Tickets: {1}, Senior Tickets {2}, Total {3}", rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(1) + rdr.GetInt32(2) + rdr.GetInt32(3));
			}
			rdr.Close();
		}
		catch { }
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
				string sql = $"SELECT* FROM purchases where name = '{search[1]}';";
				var cmd = new MySqlCommand(sql, con);
				MySqlDataReader rdr = cmd.ExecuteReader();
				bool found = false;
				while (rdr.Read())
				{
					Console.WriteLine("ID: {0}, Name: {1}, Adult Tickets: {2}, Child Tickets: {3}, Senior Tickets {4}", rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4));
					found = true;
				}
				if (!found)
				{
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine("There is no reciept with that name");
					Console.ResetColor();
				}
				rdr.Close();
			}
			catch
			{
				scomm.WrongInput();
			}
		}
	}
}
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