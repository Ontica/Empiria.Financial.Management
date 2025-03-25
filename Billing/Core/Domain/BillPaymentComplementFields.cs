/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Input Fields DTO                        *
*  Type     : BillPaymentComplementFields                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used to create and update bill payment complement.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Linq;
using Empiria.Billing.Data;
using Empiria.Billing.SATMexicoImporter;
using Empiria.Parties;

namespace Empiria.Billing {

  /// <summary>Input fields DTO used to create and update bill payment complement.</summary>
  internal class BillPaymentComplementFields : IBillFields {

    public string BillCategoryUID {
      get; set;
    } = string.Empty;


    public string BillNo {
      get; set;
    } = string.Empty;


    public string CertificationNo {
      get; set;
    } = string.Empty;


    public string CFDIRelated {
      get;
      internal set;
    } = string.Empty;


    public string IssuedByUID {
      get; set;
    } = string.Empty;


    public string IssuedToUID {
      get; set;
    } = string.Empty;


    public string ManagedByUID {
      get; set;
    } = string.Empty;


    public string Identificators {
      get; set;
    } = string.Empty;


    public string Tags {
      get; set;
    } = string.Empty;


    public string CurrencyUID {
      get; set;
    } = string.Empty;


    public decimal Subtotal {
      get; set;
    }


    public decimal Discount {
      get; set;
    }


    public decimal Total {
      get; set;
    }


    public string Version {
      get; internal set;
    }


    public FixedList<BillConceptFields> Concepts {
      get; set;
    } = new FixedList<BillConceptFields>();


    public BillSchemaDataFields SchemaData {
      get; set;
    } = new BillSchemaDataFields();


    public BillSecurityDataFields SecurityData {
      get; internal set;
    } = new BillSecurityDataFields();


    public FixedList<ComplementRelatedPayoutDataFields> ComplementRelatedPayoutData {
      get; internal set;
    } = new FixedList<ComplementRelatedPayoutDataFields>();

  } // class BillPaymentComplementFields


  public class ComplementRelatedPayoutDataFields {

    public DateTime FechaPago {
      get; internal set;
    }


    public string FormaDePagoP {
      get; internal set;
    }


    public string MonedaP {
      get; internal set;
    }


    public string TipoCambioP {
      get; internal set;
    }


    public decimal Monto {
      get; internal set;
    }


    public string NumOperacion {
      get; internal set;
    }


    public string IdDocumento {
      get; internal set;
    }


    public string MonedaDR {
      get; internal set;
    }


    public string EquivalenciaDR {
      get; internal set;
    }


    public string NumParcialidad {
      get; internal set;
    }


    public decimal ImpSaldoAnt {
      get; internal set;
    }


    public decimal ImpPagado {
      get; internal set;
    }


    public decimal ImpSaldoInsoluto {
      get; internal set;
    }


    public string ObjetoImpDR {
      get; internal set;
    }


    public FixedList<BillTaxEntryFields> Taxes {
      get; internal set;
    } = new FixedList<BillTaxEntryFields>();


    //public FixedList<ComplementRelatedDocumentDataFields> RelatedDocumentData {
    //  get; internal set;
    //} = new FixedList<ComplementRelatedDocumentDataFields>();

  }


  public class ComplementRelatedDocumentDataFields {

    public string IdDocumento {
      get; internal set;
    }


    public string MonedaDR {
      get; internal set;
    }


    public string EquivalenciaDR {
      get; internal set;
    }


    public string NumParcialidad {
      get; internal set;
    }


    public decimal ImpSaldoAnt {
      get; internal set;
    }


    public decimal ImpPagado {
      get; internal set;
    }


    public decimal ImpSaldoInsoluto {
      get; internal set;
    }


    public string ObjetoImpDR {
      get; internal set;
    }


    public FixedList<BillTaxEntryFields> Taxes {
      get; internal set;
    } = new FixedList<BillTaxEntryFields>();

  }

} // namespace Empiria.Billing
