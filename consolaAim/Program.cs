using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;

/*
Sea V una lista de tarjetas de crédito de clientes, y sea T una única instancia 
de esa tarjeta de crédito la cual debe contener: número de tarjeta, banco asociado, 
fecha de vencimiento y código de seguridad. Los bancos disponibles son "Macro", "Bancor", 
"Nación" y "Santander Río". Se pide que un programa lea N tarjetas y nos diga cuál de las 
tarjetas introducidas está vencida al día de hoy, sabiendo que cada tarjeta no pudo haber sido 
emitida antes del 05/01 (MM/YY), su código de seguridad Cx debe ser 100 <= Cx <= 800 y debe 
pertenecer al banco Macro.
 
 */

namespace consolaAim
{
    class Program
    {

        public const string cardPattern = "^[0-9]{4}\\s[0-9]{4}\\s[0-9]{4}\\s[0-9]{4}$";
        public const string securityNumberPattern = "^[0-9]{3}$";
        public const string expDatePattern = "^[0-9]{2}/[0-9]{2}$";
        public static Regex cardRegex = new Regex(cardPattern);
        public static Regex secNumRegex = new Regex(securityNumberPattern);
        public static Regex expDateRegex = new Regex(expDatePattern);
        public static int currentCard = 1, MAX_CARDS = 30;
        public static string[] validBanks = { "Macro", "Nacion", "Santander", "Bancor" };
        public static Card[] cards = new Card[MAX_CARDS];

        static void Main()
        {
            Console.WriteLine("===================================");
            Console.WriteLine("SISTEMA DE TARJETAS");
            Console.WriteLine("===================================\n");

            Console.WriteLine("- Ingrese 1 para introducir una nueva tarjeta\n" +
                "- Ingrese 2 para ver todas las tarjetas\n" +
                "- Ingrese 3 para insertar una serie de tarjetas de prueba\n" +
                "- Ingrese 4 para salir del programa.");
            Console.Write("Su elección: ");

            switch (Convert.ToInt32(Console.ReadLine()))
            {
                case 1:
                    {
                        InsertNewCard();
                        break;
                    }
                case 2:
                    {
                        ProcessAllCards();
                        break;
                    }
                case 3:
                    {
                        InsertTestCards();
                        break;
                    }
                case 4:
                    {
                        Timer tClose = new Timer(1000);
                        tClose.Elapsed += closeApp;
                        Console.WriteLine("===================================");
                        Console.WriteLine("HASTA LUEGO");
                        Console.WriteLine("===================================\n");
                        break;
                    }
            }

            Console.ReadKey();

        }

        private static void closeApp(Object source, ElapsedEventArgs e)
        {
            Environment.Exit(0);
        }

        static void InsertNewCard()
        {
            Console.WriteLine("===================================");
            Console.WriteLine("INSERTANDO NUEVA TARJETA");
            Console.WriteLine("===================================\n");

            string inputCard, inputBank, inputExpDate, inputSecNum;
            cards[currentCard] = new Card();

            // Inserting Card Number

            do
            {
                Console.Write("Ingrese el número de la tarjeta {0} (4 grupos de 4 números): ", currentCard);
                inputCard = Console.ReadLine();
            } while (!cardRegex.IsMatch(inputCard));

            cards[currentCard].Number = inputCard;

            // Inserting Security Number

            do
            {
                Console.Write("Ingrese el número de seguridad de la tarjeta {0} (3 números): ", currentCard);
                inputSecNum = Console.ReadLine();
            } while (!secNumRegex.IsMatch(inputSecNum));

            cards[currentCard].Code = Convert.ToInt32(inputSecNum);

            // Inserting Bank

            do
            {
                Console.Write("Ingrese el banco de la tarjeta {0}.\nBancos aceptados: Macro, Santander, Nacion, Bancor: ", currentCard);
                inputBank = Console.ReadLine();
            } while (!validBanks.Contains(inputBank));

            cards[currentCard].Bank = inputBank;

            // Inserting Expiration Date

            do
            {
                Console.Write("Ingrese la fecha de vencimiento de la tarjeta {0}: ", currentCard);
                inputExpDate = Console.ReadLine();
            } while (!expDateRegex.IsMatch(inputExpDate));

            cards[currentCard].ExpirationDate = inputExpDate;

            currentCard++;
            Main();
        }

        static void ProcessAllCards()
        {

            if (cards.Length == 0)
            {
                Console.WriteLine("No hay ninguna tarjeta cargada. Cargue tarjetas antes de consultar esta opción.");
                Main();
            }

            int i = 1;
            foreach (Card card in cards)
            {
                
                Console.WriteLine("=Tarjeta {0}=\n\n" +
                    "Número: {1}\n" +
                    "Código de seguridad: {2}\n" +
                    "Banco: {3}\n" +
                    "Fecha de vencimiento: {4}\n", i, card.Number, card.Code, card.Bank, card.ExpirationDate);
                i++;
            }
        }

        static void InsertTestCards()
        {
            Card[] testCard = new Card[MAX_CARDS];
            Random random = new Random();

            for (int i = 0; i < MAX_CARDS; i++)
            {
                testCard[i] = new Card();
                testCard[i].Number = Convert.ToString(random.Next(1000, 9999) + " " + random.Next(1000, 9999) + " " + random.Next(1000, 9999) + " " + random.Next(1000, 9999));
                testCard[i].Code = random.Next(100, 999);
                testCard[i].Bank = validBanks[random.Next(0, validBanks.Length)];
                testCard[i].ExpirationDate = Convert.ToString(random.Next(01, 30) + "/" + random.Next(01, 12));
            }

            Console.WriteLine("Valores de prueba insertados correctamente.\n\n");
            Main();

        }


    }

    class Card
    {
        private int code;
        private string number, bank, expirationDate;

        public Card()
        {
            code = 0;
            number = "";
            bank = "";
            expirationDate = "";
        }

        public string Number
        {
            get { return number; }
            set { number = value; }
        }

        public string Bank
        {
            get { return bank; }
            set { bank = value; }
        }

        public string ExpirationDate
        {
            get { return expirationDate; }
            set { expirationDate = value; }
        }

        public int Code
        {
            get { return code; }
            set { code = value; }
        }

    }
}
