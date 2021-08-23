using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consolaAim.Clases
{
    public class Card
    {
        public string Number { get; set; }
        public string Bank { get; set; }
        public string ExpirationDate { get; set; }
        public int Code { get; set; }

        public Card()
        {
            Code = 0;
            Number = "";
            Bank = "";
            ExpirationDate = "";
        }
        
    }
}
