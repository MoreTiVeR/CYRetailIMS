using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Cryptography;
public interface IEncryptionString
{
    string HashWithSHA256(string key);
}
