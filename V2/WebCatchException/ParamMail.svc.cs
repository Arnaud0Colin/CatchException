using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WebCatchException.Models;

namespace WebCatchException
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "ParamMail" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez ParamMail.svc ou ParamMail.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public class ParamMail : IParamMail
    {
        public ParamServerMail GetParamServerMail()
        {
            Models.Entities sql = new Models.Entities();
            CatchMe_ParamMail pm = sql.CatchMe_ParamMail.Select(x => x).FirstOrDefault();

            ParamServerMail psm = new ParamServerMail();
            if (pm != null)
            {
                psm.Smtp = pm.smtp;
                psm.Default_exp = pm.default_exp;
            }

            return psm;

        }
    }
}
