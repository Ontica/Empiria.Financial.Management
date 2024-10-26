/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Billing                          Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Empiria Plain Object                    *
*  Type     : BillTaxEntry                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represent an entry for a bill tax.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Billing {

  /// <summary>Represent an entry for a bill tax.</summary>
  internal class BillTaxEntry : BaseObject {

    #region Constructors and parsers

    private BillTaxEntry() {
      // Required by Empiria Framework.
    }


    static public BillTaxEntry Parse(int id) {
      return BaseObject.ParseId<BillTaxEntry>(id);
    }


    static public BillTaxEntry Parse(string uid) {
      return BaseObject.ParseKey<BillTaxEntry>(uid);
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

  } // class BillTaxEntry

}  // namespace Empiria.Billing
