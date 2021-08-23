using consolaAim.Clases;
using System;
using System.Collections;
using System.Linq;
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
        public static Regex cardRegex = new("^[0-9]{4}\\s[0-9]{4}\\s[0-9]{4}\\s[0-9]{4}$");
        public static Regex secNumRegex = new("^[0-9]{3}$");
        public static Regex expDateRegex = new("^[0-9]{2}/[0-9]{2}$");
        public static int currentCard = 1, MAX_CARDS = 31;
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
                    ParseCards();
                    break;
                }
                case 3:
                {
                    InsertTestCards();
                    break;
                }
                case 4:
                {
                    var tClose = new System.Timers.Timer(1000);
                    tClose.Elapsed += closeApp;
                    Console.WriteLine("===================================");
                    Console.WriteLine("HASTA LUEGO");
                    Console.WriteLine("===================================\n");
                    tClose.Start();
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

        static void ParseCards()
        {

            if (cards == null)
            {
                Console.WriteLine("No hay ninguna tarjeta cargada. Cargue tarjetas antes de consultar esta opción.");
                Main();
            }

            var validCards = 0;
            var todayMonth = DateTime.Now.ToString("MM");
            var todayYear = DateTime.Now.ToString("yyyy");

            var errors = new ArrayList[MAX_CARDS];

            Console.WriteLine("\n===================================================================\n");

            for (var i = 1; i < cards.Length; i++)
            {
                errors[i] = new ArrayList();

                Console.WriteLine("=Tarjeta {0}=\n" +
                    "Número: {1}\n" +
                    "Código de seguridad: {2}\n" +
                    "Banco: {3}\n" +
                    "Fecha de vencimiento: {4}\n", i, cards[i].Number, cards[i].Code, cards[i].Bank, cards[i].ExpirationDate);

                if (cards[i].Code > 800 || cards[i].Code < 100)
                {
                    errors[i].Add("Código inválido: " + cards[i].Code);
                }

                if (cards[i].Bank != "Macro")
                {
                    errors[i].Add("Banco inválido: " + cards[i].Bank);
                }

                if (Convert.ToInt32(cards[i].ExpirationDate.Split('/')[1]) < Convert.ToInt32(todayYear) || (Convert.ToInt32(cards[i].ExpirationDate.Split('/')[1]) == Convert.ToInt32(todayYear) && Convert.ToInt32(cards[i].ExpirationDate.Split('/')[0]) < Convert.ToInt32(todayMonth)))
                {
                    errors[i].Add("Tarjeta vencida (vencimiento: " + cards[i].ExpirationDate + ")");
                }

                if (Convert.ToInt32(cards[i].ExpirationDate.Split('/')[1]) < 2001 && Convert.ToInt32(cards[i].ExpirationDate.Split('/')[0]) < 5)
                {
                    errors[i].Add("Tarjeta emitida antes del 05/01 (emisión: " + cards[i].ExpirationDate + ")");
                }

                validCards++;

                if (errors[i].Count != 0)
                {

                    var totalErrors = "";

                    foreach (string error in errors[i])
                    {
                        totalErrors += "- " + error + "\n";
                    }

                    Console.WriteLine("TARJETA VÁLIDA: NO\n" +
                                        "Errores: \n{0}", totalErrors);

                }
                else
                {
                    Console.WriteLine("TARJETA VÁLIDA: SI\n");
                }

                Console.WriteLine("===================================================================\n");

            }
            Main();
        }

        static void InsertTestCards()
        {
            var random = new Random();

            for (var i = 1; i < MAX_CARDS; i++)
            {
                cards[i] = new Card
                {
                    Number = Convert.ToString(random.Next(1000, 9999) + " " + random.Next(1000, 9999) + " " + random.Next(1000, 9999) + " " + random.Next(1000, 9999)),
                    Code = random.Next(100, 999),
                    Bank = validBanks[random.Next(0, validBanks.Length)],
                    ExpirationDate = Convert.ToString(random.Next(01, 12) + "/" + random.Next(1995, 2025))
                };
            }

            Console.WriteLine("Valores de prueba insertados correctamente.\n\n");
            Main();

        }
    }
    
}
