using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebCatchException
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "ICatchException" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface ICatchException
    {
        [OperationContract]
        void CatchMe(MyException Exception);



        [OperationContract]
        void WriteInJournal(string Ordi, int App, string xml);

    }
}
