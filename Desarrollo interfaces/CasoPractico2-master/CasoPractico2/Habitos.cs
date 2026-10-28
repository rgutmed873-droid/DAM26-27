using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Habitos
{

    public class Habito
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
    }
    

}