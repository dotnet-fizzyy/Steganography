using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stegano.Interfaces
{
    public interface ICod
    {
        string Coding(string messageBin);
        string DeCoding(string messageBin);
    }
}
