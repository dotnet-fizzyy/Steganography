using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stegano.Model.Aditional_Coding
{
    public interface ICod
    {
        string Cod(string input);
        string DeCod(string input);
    }
}
