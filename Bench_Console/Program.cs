using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Bench_Console
{
    public class ContactModel
    {
        public DateTime Date { get; set; }
        public string Ledger { get; set; }
        public double Amount { get; set; }
        public string Company { get; set; }
        

    }

    class program
    {
        private const string URL = "https://localhost:44308/api/bench";
        

        static int Main (string[] args)
        {
            ;
            string input = string.Empty;
            if (args.Length == 0)
            {
                Func_Main();
                input = Console.ReadLine();
            }
            else
            {
                input = args[0];
            }
            switch (input.Split(new char[] { ' ' })[0])
            {
                case "all":
                    Func_All();
                    break;
                case "date":                 
                    Func_Date();
                    break;
                case "company":                   
                    Func_Company();
                    break;
                case "expenses":                    
                    Func_Expenses();
                    break;
                case "rb":
                    string RB = string.Empty;
                    Func_RB();
                    break;
                case "help":
                    string help = string.Empty;
                    Func_Help();
                    break;
                default:
                    Console.WriteLine("Incorrect Argument!");
                    break;
            }
            Console.WriteLine("Press Enter to exit!");
            Console.ReadLine();
            return 1;

        }
        //Func_All() is completed!
        //Returning all data inside json file
        private static void Func_All()
            {
            ContactModel[] contacts = Call_RestApi("bench");
                if (contacts.Length == 0)
                {
                    Console.WriteLine("empty result!");
                }
                foreach (var contact in contacts)
                {
                    string jsonString = JsonConvert.SerializeObject(contact);
                    Console.WriteLine(jsonString);
                }
            }
        //Returning all dates from JSON
        private static void Func_Date()
        {
            ContactModel[] contacts = Call_RestApi("bench");
            if (contacts.Length == 0)
            {
                Console.WriteLine("empty result!");
            }
            foreach (var contact in contacts)
            {
                //JsonConvert.DeserializeObject<ContactModel>(Date);
                string jsonString = JsonConvert.SerializeObject(contacts);
                Console.WriteLine(contact.Date);               
            }
        }
        //Returning name of the companies from JSON
        private static void Func_Company()
        {
            ContactModel[] contacts = Call_RestApi("bench");
            if (contacts.Length == 0)
            {
                Console.WriteLine("empty result!");
            }
            foreach (var contact in contacts)
            {
                //JsonConvert.DeserializeObject<ContactModel>(Company);
                string jsonString = JsonConvert.SerializeObject(contacts);
                Console.WriteLine(contact.Company);
            }
        }
        //Returning Dates with Amounts from JSON
        private static void Func_Expenses()
        {
            ContactModel[] contacts = Call_RestApi("bench");
            if (contacts.Length == 0)
            {
                Console.WriteLine("empty result!");
            }
            foreach (var contact in contacts)
            {
                //JsonConvert.DeserializeObject<ContactModel>(Amount);
                string jsonString = JsonConvert.SerializeObject(contacts);
                Console.WriteLine("Date: " + contact.Date + "| Amount: " + contact.Amount);
            }
        }
        //Returning Ruuning balances from dates in JSON
        private static void Func_RB()
        {
            ContactModel[] contacts = Call_RestApi("bench");
            if (contacts.Length == 0)
            {
                Console.WriteLine("empty result!");
                return;
            }
            var result = contacts
                .Select(x => new
                {
                    x.Date,
                    x.Amount
                })
                .GroupBy(x => x.Date, x => x.Amount,
                (Key, values) => new { Date = Key, Amount = values.Sum() });
            foreach (var contact in result)
            {
                Console.WriteLine($"Date: {contact.Date} | Daily Running Balances: {contact.Amount}");
            }
        }
        //Help section for incorrect arguments
        private static void Func_Help()
        {
            Console.WriteLine("Please indicate one of the above arguments to run the application.");
            Thread.Sleep(3000);
            Console.WriteLine("Press Enter to go back to main menu");
            Console.ReadLine();
            Console.Clear();
            Func_Main();          
        }
        //Calling RestAPI 
        private static ContactModel[] Call_RestApi(string urlParameters)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            ContactModel[] contactList = new ContactModel[] { };
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                contactList = JsonConvert.DeserializeObject<ContactModel[]>(jsonResponse);
            }
            //Dispose for memory management
            client.Dispose();
            return contactList;
        }
        private static void Func_Main()
        {
            Console.WriteLine("Please enter an argument:");
            Console.WriteLine(" 1.all                          - return all information in json file");
            Console.WriteLine(" 2.date                         - return all dates");
            Console.WriteLine(" 3.company                      - return company names");
            Console.WriteLine(" 5.expenses                     - return dates with amount of payment");
            Console.WriteLine(" 6.rb                           - return running balances (rb)");
            Console.WriteLine(" 7.help                         - show permitted arguments");
        }
    }
}