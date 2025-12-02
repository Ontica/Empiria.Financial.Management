/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Input Fields DTO                        *
*  Type     : FuelConsumptionBillFields                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used to create and update bill payment complement.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Billing.SATMexicoImporter;

namespace Empiria.Billing {

  /// <summary></summary>
  internal class FuelConsumptionBillFields : BillGeneralDataFields, IBillFields {

    public FixedList<BillConceptWithTaxFields> Concepts {
      get; set;
    } = new FixedList<BillConceptWithTaxFields>();


    public BillSchemaDataFields SchemaData {
      get; set;
    } = new BillSchemaDataFields();


    public BillSecurityDataFields SecurityData {
      get; internal set;
    } = new BillSecurityDataFields();


    public FixedList<FuelConsumptionComplementDataFields> ComplementData {
      get; internal set;
    } = new FixedList<FuelConsumptionComplementDataFields>();


    public FixedList<FuelConsumptionBillAddendaFields> Addenda {
      get; set;
    } = new FixedList<FuelConsumptionBillAddendaFields>();

  } // class FuelConsumptionBillFields


  public class FuelConsumptionBillAddendaFields {

    public FixedList<BillConceptWithTaxFields> Concepts {
      get; set;
    } = new FixedList<BillConceptWithTaxFields>();

  } // class BillAddendaFields


  public class FuelConsumptionComplementDataFields {

    public string Version {
      get; internal set;
    }


    public string TipoOperacion {
      get; internal set;
    }


    public string NumeroDeCuenta {
      get; internal set;
    }


    public decimal SubTotal {
      get; internal set;
    }


    public decimal Total {
      get; internal set;
    }


    public FixedList<FuelConseptionComplementConceptDataFields> ComplementConcepts {
      get; internal set;
    } = new FixedList<FuelConseptionComplementConceptDataFields>();

  } // class FuelConsumptionComplementDataFields


  public class FuelConseptionComplementConceptDataFields {

    public string Identificador {
      get; internal set;
    }


    public string Rfc {
      get; internal set;
    }


    public string ClaveEstacion {
      get; internal set;
    }


    public string TipoCombustible {
      get; internal set;
    }


    public string Unidad {
      get; internal set;
    }


    public string NombreCombustible {
      get; internal set;
    }


    public string FolioOperacion {
      get; internal set;
    }


    public DateTime Fecha {
      get; internal set;
    }


    public decimal Cantidad {
      get; internal set;
    }


    public decimal ValorUnitario {
      get; internal set;
    }


    public decimal Importe {
      get; internal set;
    }


    public FixedList<BillTaxEntryFields> TaxEntries {
      get; set;
    } = new FixedList<BillTaxEntryFields>();

  } // class FuelConseptionComplementConceptDataFields

} // namespace Empiria.Billing
