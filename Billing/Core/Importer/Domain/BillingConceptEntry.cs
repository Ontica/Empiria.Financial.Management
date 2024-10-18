/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Billing                          Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Empiria Plain Object                    *
*  Type     : BillingConceptEntry                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represent an entry for billing concept.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Empiria.Financial.Management.Billing.Adapters;

namespace Empiria.Financial.Billing.Domain {

    /// <summary>Represent an entry for billing concept.</summary>
    internal class BillingConceptEntry :BaseObject{

        #region Constructors and parsers

        private BillingConceptEntry() {
            // Required by Empiria Framework.
        }


        static public BillingConceptEntry Parse(int id) {
            return BaseObject.ParseId<BillingConceptEntry>(id);
        }

        static public BillingConceptEntry Parse(string uid) {
            return BaseObject.ParseKey<BillingConceptEntry>(uid);
        }

        #endregion Constructors and parsers


        #region Public properties


        [DataField("CLAVE_PROD_SERV")]
        public string ClaveProdServ {
            get; set;
        }


        [DataField("CLAVE_UNIDAD")]
        public string ClaveUnidad {
            get; set;
        }


        [DataField("CANTIDAD")]
        public decimal Cantidad {
            get; set;
        }


        [DataField("UNIDAD")]
        public string Unidad {
            get; set;
        }


        [DataField("NO_IDENTIFICACION")]
        public string NoIdentificacion {
            get; set;
        }


        [DataField("DESCRIPCION")]
        public string Descripcion {
            get; set;
        }


        [DataField("VALOR_UNITARIO")]
        public decimal ValorUnitario {
            get; set;
        }


        [DataField("IMPORTE")]
        public decimal Importe {
            get; set;
        }


        [DataField("OBJETO_IMP")]
        public string ObjetoImp {
            get; set;
        }


        public FixedList<BillingTaxDto> Impuestos {
            get; set;
        } = new FixedList<BillingTaxDto>();


        #endregion Public properties


    } // class BillingConceptEntry

} // namespace Empiria.Financial.Billing.Domain
