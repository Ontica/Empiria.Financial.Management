/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Integration interface                   *
*  Type     : PaymentInstructionDto                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO that holds information about a payment instruction to be sent to a payments broker. *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Payments.Adapters {

  /// <summary>Output DTO that holds information about a payment instruction
  /// to be sent to a payments broker.</summary>
  public class PaymentInstructionDto {

    public PaymentOrder PaymentOrder {
      get; internal set;
    }

    public string ReferenceNo {
      get; internal set;
    }

    public string RequestUniqueNo {
      get; internal set;
    }

    public DateTime RequestedTime {
      get; internal set;
    }

  }  // class PaymentInstructionDto


  /// <summary>Output DTO used to return minimal payment instruction's data for use in lists.</summary>
  public class PaymentInstructionDescriptor {

    public string UID {
      get; internal set;
    }

    public string PaymentOrderTypeName {
      get; internal set;
    }

    public string PaymentOrderNo {
      get; internal set;
    }

    public string PayTo {
      get; internal set;
    }

    public string PaymentMethod {
      get; internal set;
    }

    public string PaymentAccount {
      get; internal set;
    }

    public string CurrencyCode {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

    public DateTime DueTime {
      get; internal set;
    }

    public string PayableNo {
      get; internal set;
    }

    public string PayableTypeName {
      get; internal set;
    }

    public string ContractNo {
      get; internal set;
    }

    public string BudgetTypeName {
      get; internal set;
    }

    public string RequestedBy {
      get; internal set;
    }

    public DateTime RequestedTime {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

    public DateTime RequestedDate {
      get; internal set;
    }

  } // class PaymentInstructionDescriptor

}  // namespace Empiria.Payments.Adapters
