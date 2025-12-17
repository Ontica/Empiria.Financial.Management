/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Data Transfer Object                    *
*  Type     : PaymentOrderDto, PaymentOrderDescriptor    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer objects used to return payment orders.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Documents;
using Empiria.History;
using Empiria.StateEnums;

using Empiria.Financial.Adapters;
using Empiria.Billing.Adapters;

using Empiria.Budgeting.Transactions.Adapters;

namespace Empiria.Payments.Adapters {

  /// <summary>Output DTO used to return a complete payment order.</summary>
  public class PaymentOrderHolderDto {

    public PaymentOrderDto PaymentOrder {
      get; internal set;
    }

    public PayableEntityDto PayableEntity {
      get; internal set;
    }

    public FixedList<PaymentOrderItemDto> Items {
      get; internal set;
    }

    public FixedList<BillDto> Bills {
      get; internal set;
    }

    public FixedList<BudgetTransactionDescriptorDto> BudgetTransactions {
      get; internal set;
    }

    public FixedList<PaymentInstructionDescriptor> PaymentInstructions {
      get; internal set;
    }

    public FixedList<DocumentDto> Documents {
      get; internal set;
    }


    public FixedList<HistoryEntryDto> History {
      get; internal set;
    }


    public PaymentOrderActions Actions {
      get; internal set;
    }

  }  // class PaymentOrderHolderDto



  public class PaymentOrderActions : BaseActions {


    public bool CanCancel {
      get; internal set;
    }

    public bool CanSuspend {
      get; internal set;
    }

    public bool CanReset {
      get; internal set;
    }

    [Newtonsoft.Json.JsonProperty(PropertyName = "CanApprovePayment")]
    public bool CanApproveBudget {
      get; internal set;
    }

    public bool CanGeneratePaymentInstruction {
      get; internal set;
    }

    public bool CanExerciseBudget {
      get; internal set;
    }

  }  // class PaymentOrderActionsDto



  /// <summary>Output DTO used to return a payment order.</summary>
  public class PaymentOrderDto {

    public string UID {
      get; internal set;
    }

    public NamedEntityDto PaymentOrderType {
      get; internal set;
    }

    public string PaymentOrderNo {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public NamedEntityDto BudgetType {
      get; internal set;
    }

    public NamedEntityDto Budget {
      get; internal set;
    }

    public NamedEntityDto PayTo {
      get; internal set;
    }

    public NamedEntityDto RequestedBy {
      get; internal set;
    }

    public DateTime RequestedDate {
      get; internal set;
    }

    public DateTime DueTime {
      get; internal set;
    }

    public Priority Priority {
      get; internal set;
    }

    public PaymentMethodDto PaymentMethod {
      get; internal set;
    }

    public PaymentAccountDto PaymentAccount {
      get; internal set;
    }

    public NamedEntityDto Currency {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

    public string ReferenceNumber {
      get; internal set;
    }

    public NamedEntityDto Status {
      get; internal set;
    }

  }  // class PaymentOrderDto



  /// <summary>Output DTO used to return minimal payment order's data for use in lists.</summary>
  public class PaymentOrderDescriptor {

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

    public string PriorityName {
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

    public string PayableNo {
      get; internal set;
    }

    public string PayableTypeName {
      get; internal set;
    }

    public string PayableName {
      get; internal set;
    }

    public string BudgetName {
      get; internal set;
    }

  } // class PaymentOrderDescriptor

}  // namespace Empiria.Payments.Adapters
