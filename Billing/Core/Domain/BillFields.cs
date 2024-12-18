﻿/* Empiria Financial *****************************************************************************************
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
    }


    public DateTime IssueDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public string IssuedByUID {
      get; set;
    } = string.Empty;


    public string IssuedToUID {
      get; set;
    } = string.Empty;


    public string ManagedByUID {
      get; set;
    } = string.Empty;


    public string SchemaVersion {
      get; set;
    } = string.Empty;


    public string TipoComprobante {
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


    public string SchemaExtData {
      get; set;
    } = string.Empty;


    public string SecurityExtData {
      get; set;
    } = string.Empty;


    public string PaymentExtData {
      get; set;
    } = string.Empty;


    public string ExtData {
      get; set;
    } = string.Empty;


    public FixedList<BillConceptFields> Concepts {
      get; set;
    } = new FixedList<BillConceptFields>();
    
  } // class BillFields


  /// <summary>Input fields DTO used to create and update bill concept.</summary>
  public class BillConceptFields {

    public string BillUID {
      get; set;
    } = string.Empty;


    public string ProductUID {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string Identificators {
      get; set;
    } = string.Empty;


    public string Tags {
      get; set;
    } = string.Empty;


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


    public string ExtData {
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


    public string ExtData {
      get; set;
    } = string.Empty;

  } // class BillTaxEntryFields


  /// <summary>Extension methods for BillFields type.</summary>
  static internal class BillFieldsExtensions {


    static internal void EnsureIsValid(this BillFields fields) {

      var issuedTo = Party.Parse(fields.IssuedToUID);

      Assertion.Require(BillData.ValidateIfExistBill(fields.BillNo).Count == 0,
                        "El documento que intenta guardar ya está registrado.");

      Assertion.Require(Party.Primary.Equals(issuedTo),
                        $"El receptor del CFDI no es {Party.Primary.Name}");

      Assertion.Require(fields.IssueDate <= DateTime.Now,
                        "La fecha del documento no debe de ser mayor a la fecha actual.");
    }


    static internal void EnsureIsValidBill(this BillFields fields, int payableId,
                                           decimal payableTotal, BillCategory billCategory) {

      if (billCategory == BillCategory.Factura) {

        var billsByPayable = BillData.ValidateIfExistBillsByPayable(payableId);

        Assertion.Require((billsByPayable.Sum(x => x.Total) + fields.Total) <= payableTotal,
                          "El monto total de las facturas registradas y/o " +
                          "la factura que intenta guardar es mayor al monto total del contrato.");
      }
    }


    static internal void EnsureIsValidCreditNote(this BillFields fields, BillCategory billCategory) {

      if (billCategory == BillCategory.NotaDeCredito) {

        Assertion.Require(fields.CFDIRelated != string.Empty,
                        "La nota de crédito que intenta guardar no tiene referencia a un CFDI relacionado.");

        var billsRelated = BillData.ValidateIfExistBill(fields.CFDIRelated);

        Assertion.Require(billsRelated.Count == 1,
                          "El CFDI relacionado al que hace referencia no existe.");

        var creditNotesList = BillData.ValidateIfExistCreditNotesByBill(fields.CFDIRelated);

        Assertion.Require((creditNotesList.Sum(x => x.Total) + fields.Total) <= billsRelated.First().Total,
                          "El total de ésta nota de crédito y la suma de las notas de crédito " +
                          "registradas exceden el total de la factura " +
                          $"(CFDI relacionado: {fields.CFDIRelated}).");
      }
    }


  } // class BillFieldsExtensions


} // namespace Empiria.Billing.Adapters
