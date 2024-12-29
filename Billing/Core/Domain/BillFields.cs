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

  /// <summary>Input fields DTO used to create and update bill.</summary>
  internal class BillFields {

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


    public FixedList<BillConceptFields> Concepts {
      get; set;
    } = new FixedList<BillConceptFields>();


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


    public FixedList<BillTaxEntryFields> TaxEntries {
      get; set;
    } = new FixedList<BillTaxEntryFields>();


    internal void EnsureIsValid() {
      // ToDo
    }

  } // class BillConceptFields


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

      Assertion.Require(Party.Primary.Equals(issuedTo),
                        $"El receptor del CFDI no es {Party.Primary.Name}");

      Assertion.Require(fields.SchemaData.Fecha <= DateTime.Now,
                        "La fecha del documento no debe de ser mayor a la fecha actual.");
    }


    static internal void EnsureIsValidBill(this BillFields fields, int payableId,
                                           decimal payableTotal, BillCategory billCategory) {

      if (billCategory != BillCategory.FacturaProveedores) {
        return;
      }

      var billsByPayable = BillData.GetBillsForPayable(payableId);

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

      Assertion.Require(relatedBill != null,
                        "El CFDI al que hace referencia la nota de crédito, " +
                        "no ha sido registrado en el sistema.");

      var creditNotesList = BillData.GetBillCreditNotes(fields.CFDIRelated);

      Assertion.Require((creditNotesList.Sum(x => x.Total) + fields.Total) <= relatedBill.Total,
                        "El total de ésta nota de crédito y la suma de las notas de crédito " +
                        "registradas exceden el total de la factura " +
                        $"(CFDI relacionado: {fields.CFDIRelated}).");
    }

  } // class BillFieldsExtensions

} // namespace Empiria.Billing.Adapters
