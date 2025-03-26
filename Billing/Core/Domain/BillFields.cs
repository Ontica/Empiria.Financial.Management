/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Input Fields DTO                        *
*  Type     : BillFields                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input fields DTO used to create and update bill.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Linq;

using Empiria.Parties;

using Empiria.Billing.Data;

namespace Empiria.Billing {


  internal interface IBillFields {

  }

  /// <summary>Input fields DTO used to create and update bill.</summary>
  internal class BillFields : IBillFields {

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


    public FixedList<BillConceptWithTaxFields> Concepts {
      get; set;
    } = new FixedList<BillConceptWithTaxFields>();


    public BillSchemaDataFields SchemaData {
      get; set;
    } = new BillSchemaDataFields();


    public BillSecurityDataFields SecurityData {
      get; internal set;
    } = new BillSecurityDataFields();


    public BillAddendaFields Addenda {
      get; set;
    }

  } // class BillFields


  public class BillOrganizationFields {

    public string RegimenFiscal {
      get; internal set;
    } = string.Empty;


    public string RFC {
      get; internal set;
    } = string.Empty;


    public string Nombre {
      get; internal set;
    } = string.Empty;


    public string DomicilioFiscal {
      get; internal set;
    } = string.Empty;


    public string UsoCFDI {
      get; internal set;
    } = string.Empty;

  }  // class BillOrganizationFields


  /// <summary>Input fields DTO used to create and update bill concept.</summary>
  public class BillConceptFields {

    public string BillUID {
      get; set;
    } = string.Empty;


    public string ProductUID {
      get; set;
    } = string.Empty;


    public string SATProductUID {
      get; set;
    } = string.Empty;


    public string SATProductCode {
      get; set;
    } = string.Empty;


    public string ClaveUnidad {
      get; set;
    } = string.Empty;


    public string Unidad {
      get; set;
    } = string.Empty;


    public string NoIdentificacion {
      get; set;
    } = string.Empty;


    public string ObjetoImp {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string[] Identificators {
      get; set;
    } = new string[0];


    public string[] Tags {
      get; set;
    } = new string[0];


    public decimal Quantity {
      get; set;
    }


    public decimal UnitPrice {
      get; set;
    }


    public decimal Subtotal {
      get; set;
    }


    public decimal Discount {
      get; set;
    }


    public string SchemaExtData {
      get; set;
    } = string.Empty;


    internal void EnsureIsValid() {
      // ToDo
    }

  } // class BillConceptFields


  /// <summary>Input fields DTO used to create and update bill concept with tax.</summary>
  public class BillConceptWithTaxFields : BillConceptFields {

    public FixedList<BillTaxEntryFields> TaxEntries {
      get; set;
    } = new FixedList<BillTaxEntryFields>();

  } // class BillConceptWithTaxFields


  /// <summary>Input fields DTO used to create and update bill tax entry.</summary>
  public class BillTaxEntryFields {

    public string BillUID {
      get; set;
    } = string.Empty;


    public string BillConceptUID {
      get; set;
    } = string.Empty;


    public BillTaxMethod TaxMethod {
      get; set;
    } = BillTaxMethod.Traslado;


    public BillTaxFactorType TaxFactorType {
      get; set;
    } = BillTaxFactorType.Tasa;


    public decimal Factor {
      get; set;
    }


    public decimal BaseAmount {
      get; set;
    }


    public decimal Total {
      get; set;
    }


    public string Impuesto {
      get; set;
    }


    static public BillTaxFactorType GetFactorTypeByTax(string tipoFactor) {

      switch (tipoFactor) {

        case "Cuota":
          return BillTaxFactorType.Cuota;

        case "Tasa":
          return BillTaxFactorType.Tasa;

        default:
          throw Assertion.EnsureNoReachThisCode($"Unhandled bill tax factor type for '{tipoFactor}'.");
      }
    }

  } // class BillTaxEntryFields



  public class BillSchemaDataFields {

    public BillOrganizationFields IssuedBy {
      get; internal set;
    } = new BillOrganizationFields();


    public BillOrganizationFields IssuedTo {
      get; internal set;
    } = new BillOrganizationFields();


    public string SchemaVersion {
      get; set;
    } = string.Empty;


    public DateTime Fecha {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public string TipoComprobante {
      get; set;
    } = string.Empty;


    public string Folio {
      get; set;
    } = string.Empty;


    public string Serie {
      get; set;
    } = string.Empty;


    public string MetodoPago {
      get; set;
    } = string.Empty;


    public string FormaPago {
      get; set;
    } = string.Empty;


    public string Exportacion {
      get; set;
    } = string.Empty;


    public string LugarExpedicion {
      get; set;
    } = string.Empty;


    public string Moneda {
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

  } // class BillSchemaDataFields


  public class BillSecurityDataFields {


    public string Xmlns_Tfd {
      get; set;
    } = string.Empty;


    public string Xmlns_Xsi {
      get; set;
    } = string.Empty;


    public string Xsi_SchemaLocation {
      get; set;
    } = string.Empty;


    public string Tfd_Version {
      get; set;
    } = string.Empty;


    public string UUID {
      get; internal set;
    } = string.Empty;


    public string Sello {
      get; internal set;
    } = string.Empty;


    public string NoCertificado {
      get; internal set;
    } = string.Empty;


    public string Certificado {
      get; internal set;
    } = string.Empty;


    public DateTime FechaTimbrado {
      get; internal set;
    }


    public string RfcProvCertif {
      get; internal set;
    } = string.Empty;


    public string SelloCFD {
      get; internal set;
    } = string.Empty;


    public string NoCertificadoSAT {
      get; internal set;
    } = string.Empty;


    public string SelloSAT {
      get; internal set;
    } = string.Empty;

  }  // class BillComplementFields


  public class BillAddendaFields {

    public string NoEstacion {
      get; set;
    } = string.Empty;


    public string ClavePemex {
      get; set;
    } = string.Empty;


    public decimal TasaIEPS {
      get; set;
    }


    public decimal IEPS {
      get; set;
    }


    public decimal TasaIVA {
      get; set;
    }


    public decimal IVA {
      get; set;
    }


    public decimal NoIdentificacion {
      get; set;
    }


    public decimal TasaAIEPS {
      get; set;
    }


    public decimal AIEPS {
      get; set;
    }

  } // class BillAddendaFields


  /// <summary>Extension methods for BillFields type.</summary>
  static internal class BillFieldsExtensions {

    static internal void EnsureIsValid(this BillFields fields) {

      Assertion.Require(fields.IssuedToUID != string.Empty,
                        "El receptor del CFDI no se encuentra registrado.");

      var issuedTo = Party.Parse(fields.IssuedToUID);

      Assertion.Require(BillData.TryGetBillWithBillNo(fields.BillNo) == null,
                        "El documento que intenta guardar ya está registrado.");

      // ToDo: REVISAR 2 REGISTROS QUE EXISTEN DE BANOBRAS EN PARTIES (PARTY_ID: 1, 5984)
      // ToDo: Revisar
      // Assertion.Require(Party.Primary.Code.Equals(issuedTo.Code),
      //                   $"El receptor del CFDI no es {Party.Primary.Name}");

      Assertion.Require(fields.SchemaData.Fecha <= DateTime.Now,
                        "La fecha del documento no debe de ser mayor a la fecha actual.");
    }


    static internal void EnsureIsValidBill(this BillFields fields, int payableId,
                                           decimal payableTotal, BillCategory billCategory) {

      var billsByPayable = BillData.GetBillsForPayable(payableId, billCategory);

      Assertion.Require((billsByPayable.Sum(x => x.Total) + fields.Total) <= payableTotal,
                        "El monto total de las facturas registradas y/o " +
                        "la factura que intenta guardar es mayor al monto total del contrato.");
    }


    static internal void EnsureIsValidCreditNote(this BillFields fields, BillCategory billCategory) {

      if (billCategory != BillCategory.NotaDeCreditoProveedores) {
        return;
      }

      Assertion.Require(fields.CFDIRelated != string.Empty,
                      "La nota de crédito que intenta guardar no tiene referencia a un CFDI relacionado.");

      Bill relatedBill = BillData.TryGetBillWithBillNo(fields.CFDIRelated);

      Assertion.Require(relatedBill,
                        "El CFDI al que hace referencia la nota de crédito, " +
                        "no ha sido registrado en el sistema.");

      var relatedDocuments = BillData.GetRelatedDocuments(fields.CFDIRelated);

      Assertion.Require((relatedDocuments.Sum(x => x.Total) + fields.Total) <= relatedBill.Total,
                        "El total de ésta nota de crédito y la suma de las notas de crédito " +
                        "registradas exceden el total de la factura " +
                        $"(CFDI relacionado: {fields.CFDIRelated}).");
    }


    static internal void EnsureIsValidPaymentComplement(this BillPaymentComplementFields fields,
                                                        BillCategory billCategory) {

      Assertion.Require(billCategory == BillCategory.ComplementoPagoProveedores,
                        "El documento que intenta guardar no es un complemento de pago.");

      Assertion.Require(BillData.TryGetBillWithBillNo(fields.BillNo) == null,
                        "El documento que intenta guardar ya está registrado.");

      Assertion.Require(fields.SchemaData.Fecha <= DateTime.Now,
                        "La fecha del documento no debe de ser mayor a la fecha actual.");

      Assertion.Require(fields.IssuedToUID != string.Empty,
                        "El receptor del CFDI no se encuentra registrado.");
    }


    static internal void EnsureIsValidDocument(this BillPaymentComplementFields fields,
                                               BillCategory billCategory,
                                               int payableId,
                                               decimal payableTotal) {

      Assertion.Require(fields.ComplementRelatedPayoutData.Count > 0,
                        $"El documento {billCategory.Name} " +
                        $"con folio fiscal {fields.BillNo} " +
                        $"no contiene información de un complemento de pago");
      decimal fieldsTotal = 0;
      foreach (var relatedDataFields in fields.ComplementRelatedPayoutData) {

        Assertion.Require(relatedDataFields.IdDocumento != string.Empty,
                          $"El documento {billCategory.Name} " +
                          $"con folio fiscal {fields.BillNo} " +
                          $"que intenta guardar no tiene referencia a un CFDI relacionado.");

        Bill relatedBill = BillData.TryGetBillWithBillNo(relatedDataFields.IdDocumento);

        Assertion.Require(relatedBill,
                          $"El documento {billCategory.Name} " +
                          $"con folio fiscal {fields.BillNo} " +
                          $"hace referencia a un CFDI que no ha sido registrado en el sistema.");
        fieldsTotal += relatedDataFields.ImpPagado;
      }

      var billsByPayable = BillData.GetBillsForPayable(payableId, billCategory);

      var totalsByBillCategory = GetTotalsByBillCategory(billsByPayable);

      //TODO RETORNAR bill.BillRelatedBills;
      Assertion.Require((totalsByBillCategory + fieldsTotal) <= payableTotal,
                          "El monto total de las facturas registradas y/o " +
                          "la factura que intenta guardar es mayor al monto total del contrato.");
    }


    static private decimal GetTotalsByBillCategory(FixedList<Bill> billsByPayable) {

      decimal totals = 0;
      foreach (var bill in billsByPayable) {

        if (bill.BillCategory == BillCategory.ComplementoPagoProveedores) {
          totals += bill.BillRelatedBills.Sum(x => x.BillRelatedSchemaExtData.ImpPagado);
        } else {
          totals += bill.Total;
        }
      }
      return totals;
    }
  } // class BillFieldsExtensions

} // namespace Empiria.Billing.Adapters
