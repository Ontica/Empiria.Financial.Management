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


    public decimal PayableTotal {
      get; internal set;
    }


    internal void EnsureIsValid(int payableId) {

      var issuedTo = Party.Parse(IssuedToUID);
      
      Assertion.Require(BillData.ValidateIfExistBill(BillNo).Count == 0,
                        "La factura/nota de crédito que se intenta guardar ya esta registrada.");
      
      //ToDo validar el importe de n facturas sea igual al del IPayable ligado
      //var billsByPayable = BillData.ValidateIfExistBillsByPayable(payableId);
      
      //if (billsByPayable.Count > 0) {
      //  Assertion.Require((billsByPayable.Sum(x=>x.Total) + Total) <= PayableTotal,
      //                  "El monto total de las facturas es mayor al monto total del contrato.");
      //}

      Assertion.Require(Party.Primary.Equals(issuedTo),
                        $"El receptor de la factura/nota de crédito no es {Party.Primary.Name}");

      Assertion.Require(IssueDate <= DateTime.Now,
                        "La fecha de la factura/nota de crédito no debe ser mayor a la fecha actual.");
    }


    internal void EnsureIsValidCreditNote() {

      var billsRelated = BillData.ValidateIfExistBill(CFDIRelated);

      Assertion.Require(billsRelated.Count == 1,
                        "El CFDI relacionado al que hace referencia no existe.");

      var creditNotesList = BillData.ValidateIfExistCreditNotesByBill(CFDIRelated);

      if (creditNotesList.Count > 0) {

        Assertion.Require((creditNotesList.Sum(x => x.Total) + Total) <= billsRelated.First().Total,
                        "El total de ésta nota de crédito y la suma de " +
                        "las notas de crédito registradas exceden el total de la factura.");
      }
    }

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

} // namespace Empiria.Billing.Adapters
