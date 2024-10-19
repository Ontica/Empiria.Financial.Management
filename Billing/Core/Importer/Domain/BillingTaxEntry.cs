/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Billing                          Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Empiria Plain Object                    *
*  Type     : BillingTaxEntry                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represent an entry for a billing tax.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/


using Empiria.Billing.Adapters;

namespace Empiria.Billing {

  /// <summary>Represent an entry for a billing tax.</summary>
  internal class BillingTaxEntry : BaseObject {

    #region Constructors and parsers

    private BillingTaxEntry() {
      // Required by Empiria Framework.
    }


    static public BillingTaxEntry Parse(int id) {
      return BaseObject.ParseId<BillingTaxEntry>(id);
    }


    static public BillingTaxEntry Parse(string uid) {
      return BaseObject.ParseKey<BillingTaxEntry>(uid);
    }


    #endregion Constructors and parsers

    #region Properties

    [DataField("TIPO_IMPUESTO")]
    public BillTaxApplicationType TipoImpuesto {
      get; set;
    }


    [DataField("BASE")]
    public decimal Base {
      get; set;
    }


    [DataField("IMPUESTO")]
    public string Impuesto {
      get; set;
    }


    [DataField("TIPO_FACTOR")]
    public string TipoFactor {
      get; set;
    }


    [DataField("TASA_O_CUOTA")]
    public decimal TasaOCuota {
      get; set;
    }


    [DataField("IMPORTE")]
    public decimal Importe {
      get; set;
    }

    #endregion Properties

  } // class BillingTaxEntry

}  // namespace Empiria.Billing
