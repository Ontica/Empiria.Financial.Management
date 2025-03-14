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
using Empiria.Billing.SATMexicoImporter;

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


    public FixedList<ComplementBalanceDataDto> SaldosTotales {
      get; internal set;
    } = new FixedList<ComplementBalanceDataDto>();


    public FixedList<ComplementRelatedPayoutDataDto> DatosComplementoPago {
      get; internal set;
    } = new FixedList<ComplementRelatedPayoutDataDto>();




  } // class BillPaymentComplementFields


  public class ComplementBalanceDataDto {

    public string PagosVersion {
      get; internal set;
    }


    public decimal TotalTrasladosBaseIVA16 {
      get; internal set;
    }


    public decimal TotalTrasladosImpuestoIVA16 {
      get; internal set;
    }


    public decimal MontoTotalPagos {
      get; internal set;
    }

  }


  public class ComplementRelatedPayoutDataDto {

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


    public FixedList<ComplementRelatedDocumentDataDto> RelatedDocumentData {
      get; internal set;
    } = new FixedList<ComplementRelatedDocumentDataDto>();

  }


  public class ComplementRelatedDocumentDataDto {

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


    public FixedList<SATBillTaxDto> Taxes {
      get; internal set;
    } = new FixedList<SATBillTaxDto>();

  }

} // namespace Empiria.Billing
