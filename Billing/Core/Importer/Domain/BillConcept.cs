/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Empiria Plain Object                    *
*  Type     : BillConcept                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a bill concept.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Billing.Adapters;

namespace Empiria.Billing {

  /// <summary>Represents a bill concept.</summary>
  internal class BillConcept : BaseObject {

    #region Constructors and parsers

    private BillConcept() {
      // Required by Empiria Framework.
    }


    static public BillConcept Parse(int id) => ParseId<BillConcept>(id);

    static public BillConcept Parse(string uid) => ParseKey<BillConcept>(uid);

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


    public FixedList<BillTaxDto> Impuestos {
      get; set;
    } = new FixedList<BillTaxDto>();


    #endregion Public properties


  } // class BillConcept

} // namespace Empiria.Billing
