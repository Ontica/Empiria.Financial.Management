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
using System.Collections.Generic;
using System.Linq;
using Empiria.Billing.Data;
using Empiria.Billing.SATMexicoImporter;
using Empiria.Parties;

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


    public string IssuedByUID {
      get; set;
    } = string.Empty;


    public string IssuedToUID {
      get; set;
    } = string.Empty;


    public string ManagedByUID {
      get; set;
    } = string.Empty;


    public string[] Tags {
      get; set;
    } = new string[0];


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


    public BillRelatedDocumentData BillRelatedDocument {
      get; internal set;
    } = new BillRelatedDocumentData();


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
      get; internal set;
    }


    public BillFiscalLegendFields FiscalLegendsData {
      get; internal set;
    }


    public FixedList<BillTaxEntryFields> BillTaxes {
      get; internal set;
    } = new FixedList<BillTaxEntryFields>();

  } // class BillFields


  internal class BillRelatedDocumentData {

    public string RelatedCFDI {
      get;
      internal set;
    } = string.Empty;


    public string TipoRelacion {
      get; internal set;
    } = string.Empty;


    public BillRelatedDocumentType TipoRelacionNombre {
      get; internal set;
    } = BillRelatedDocumentType.Ninguno;

  } // class BillRelatedDocumentData


  internal class BillGeneralDataFields {

    public string BillCategoryUID {
      get; set;
    } = string.Empty;


    public string BillNo {
      get; set;
    } = string.Empty;


    public string BillType {
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


    public string[] Tags {
      get; set;
    } = new string[0];


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

  }


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


    public string SATProductServiceCode {
      get; set;
    } = string.Empty;


    public string UnitKey {
      get; set;
    } = string.Empty;


    public string Unit {
      get; set;
    } = string.Empty;


    public string IdentificationNo {
      get; set;
    } = string.Empty;


    public string ObjectImp {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


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


    public bool IsBonusConcept {
      get; internal set;
    }


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


    public string Description {
      get; set;
    }


    public bool IsBonusTax {
      get; set;
    }

    static public BillTaxFactorType GetFactorTypeByTax(string tipoFactor) {

      switch (tipoFactor) {

        case "Cuota":
          return BillTaxFactorType.Cuota;

        case "Tasa":
          return BillTaxFactorType.Tasa;

        case "Exento":
          return BillTaxFactorType.Exento;

        case "None":
          return BillTaxFactorType.None;

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


    public string TipoCambio {
      get; internal set;
    } = string.Empty;


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


    public string CondicionesPago {
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


    public decimal Descuento {
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
      get; internal set;
    } = string.Empty;


    public string ClavePemex {
      get; internal set;
    } = string.Empty;


    public decimal TasaIEPS {
      get; internal set;
    }


    public decimal IEPS {
      get; internal set;
    }


    public decimal TasaIVA {
      get; internal set;
    }


    public decimal IVA {
      get; internal set;
    }


    public string NoIdentificacion {
      get; internal set;
    } = string.Empty;


    public decimal TasaAIEPS {
      get; set;
    }


    public decimal AIEPS {
      get; internal set;
    }

    public string Folio {
      get; internal set;
    }


    public string Serie {
      get; internal set;
    }


    public DateTime FechaEmision {
      get; internal set;
    }


    public FixedList<BillConceptFields> Concepts {
      get; internal set;
    } = new FixedList<BillConceptFields>();


    internal void MapEcoConceptFields(SATBillAddenda addenda) {

      if (addenda.EcoConcepts.Count > 0) {
        NoEstacion = addenda.NoEstacion;
        ClavePemex = addenda.ClavePemex;
        TasaIEPS = addenda.EcoConcepts[0].TasaIEPS;
        IEPS = addenda.EcoConcepts[0].IEPS;
        TasaIVA = addenda.EcoConcepts[0].TasaIVA;
        IVA = addenda.EcoConcepts[0].IVA;
        NoIdentificacion = addenda.EcoConcepts[0].NoIdentificacion;
        TasaAIEPS = addenda.EcoConcepts[0].TasaAIEPS;
        AIEPS = addenda.EcoConcepts[0].AIEPS;
      }
    }

  } // class BillAddendaFields


  public class BillFiscalLegendFields {

    public string DisposicionFiscal {
      get; internal set;
    } = string.Empty;


    public string Norma {
      get; internal set;
    } = string.Empty;


    public string TextoLeyenda {
      get; internal set;
    } = string.Empty;

  } // class BillFiscalLegendFields


  /// <summary>Extension methods for BillFields type.</summary>
  static internal class BillFieldsExtensions {

    static private void EnsureIsValid(string billNo, string issuedToUID, DateTime documentDate) {

      Assertion.Require(BillData.TryGetBillWithBillNo(billNo) == null,
                        "El documento que intenta guardar ya está registrado.");

      Assertion.Require(issuedToUID != string.Empty,
                        "El receptor del CFDI no se encuentra registrado.");

      var issuedTo = Party.Parse(issuedToUID);

      Assertion.Require(Organization.Primary.Equals(issuedTo),
                        $"El receptor del CFDI no es {Organization.Primary.Name}");

      Assertion.Require(documentDate <= DateTime.Now,
                        "La fecha del documento no debe de ser mayor a la fecha actual.");
    }


    static internal void EnsureIsValidNote(this BillFields fields, BillCategory billCategory) {

      if (fields.BillRelatedDocument.TipoRelacion == "02" ||
          fields.BillRelatedDocument.TipoRelacion == "04") {
        return;
      }

      if (billCategory == BillCategory.NotaDeCreditoProveedores) {
        
        Assertion.Require(fields.BillRelatedDocument.RelatedCFDI != string.Empty,
                        "La nota de crédito que intenta guardar " +
                        "no tiene referencia a un CFDI relacionado.");
      }

      if (fields.BillRelatedDocument.RelatedCFDI != string.Empty) {

        Bill relatedBill = BillData.TryGetBillWithBillNo(fields.BillRelatedDocument.RelatedCFDI);

        Assertion.Require(relatedBill,
                         $"El CFDI ({fields.BillRelatedDocument.RelatedCFDI}) al que hace referencia el documento, " +
                          "no ha sido registrado en el sistema.");
      }
    }


    static internal void EnsureIsValidDocument(this BillFields fields) {

      EnsureIsValid(fields.BillNo, fields.IssuedToUID, fields.SchemaData.Fecha);
    }


    static internal void EnsureIsValidDocument(this BillPaymentComplementFields fields) {

      EnsureIsValid(fields.BillNo, fields.IssuedToUID, fields.SchemaData.Fecha);
    }


    static internal void EnsureIsValidDocument(this FuelConsumptionBillFields fields) {

      EnsureIsValid(fields.BillNo, fields.IssuedToUID, fields.SchemaData.Fecha);
    }


    static internal void EnsureIsValidFuelConsumption(this FuelConsumptionBillFields fields,
                                                      BillCategory billCategory) {

      Assertion.Require(fields.BillType == "factura-consumo-combustible-sat",
                       "El documento que intenta guardar no es del tipo factura de consumo de combustible.");

      Assertion.Require(billCategory == BillCategory.FacturaConsumoCombustible,
                        "El documento que intenta guardar no es una factura de consumo de combustible.");
    }


    static internal void EnsureIsValidPaymentComplement(this BillPaymentComplementFields fields,
                                               BillCategory billCategory,
                                               int payableId,
                                               decimal payableTotal) {

      Assertion.Require(billCategory == BillCategory.ComplementoPagoProveedores,
                        "El documento que intenta guardar no es un complemento de pago.");

      Assertion.Require(fields.ComplementRelatedPayoutData.Count > 0,
                        $"El documento con folio fiscal {fields.BillNo} " +
                        $"no contiene información de un complemento de pago");

      decimal fieldsTotal = 0;

      foreach (var relatedDataFields in fields.ComplementRelatedPayoutData) {

        Assertion.Require(relatedDataFields.IdDocumento != string.Empty,
                          $"El documento con folio fiscal {fields.BillNo} " +
                          $"que intenta guardar no tiene referencia a un CFDI relacionado.");

        Bill relatedBill = BillData.TryGetBillWithBillNo(relatedDataFields.IdDocumento);

        Assertion.Require(relatedBill,
                          $"El documento con folio fiscal {fields.BillNo} " +
                          $"hace referencia a un CFDI que no ha sido registrado en el sistema.");
        fieldsTotal += relatedDataFields.ImpPagado;
      }

      var billsByPayable = BillData.GetBillsForPayable(payableId, billCategory);

      Assertion.Require((billsByPayable.Sum(x => x.Total) + fieldsTotal) <= payableTotal,
                          "El monto total de las facturas registradas y/o " +
                          "la factura que intenta guardar es mayor al monto total del contrato.");
    }

  } // class BillFieldsExtensions

} // namespace Empiria.Billing.Adapters
