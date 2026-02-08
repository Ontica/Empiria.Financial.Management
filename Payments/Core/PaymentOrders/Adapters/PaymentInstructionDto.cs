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

using Empiria.Documents;
using Empiria.History;

using Empiria.Billing.Adapters;

namespace Empiria.Payments.Adapters {

  /// <summary>Output DTO with a payment instruction holder.</summary>
  public class PaymentInstructionHolderDto {

    public PaymentInstructionDto PaymentInstruction {
      get; internal set;
    }

    public FixedList<PaymentInstructionLogDescriptorDto> Log {
      get; internal set;
    }

    public BillsStructureDto Bills {
      get; internal set;
    }


    public FixedList<DocumentDto> Documents {
      get; internal set;
    }

    public FixedList<HistoryEntryDto> History {
      get; internal set;
    }

    public PaymentInstructionActions Actions {
      get; internal set;
    }

  } // class PaymentInstructionHolderDto



  /// <summary>Output DTO used to return available actions for a payment instruction.</summary>
  public class PaymentInstructionActions : BaseActions {

    public bool CanCancel {
      get; internal set;
    }

    public bool CanReset {
      get; internal set;
    }

    public bool CanSuspend {
      get; internal set;
    }

    public bool CanRequestPayment {
      get; internal set;
    }

    public bool CanCancelPaymentRequest {
      get; internal set;
    }

    [Newtonsoft.Json.JsonProperty(PropertyName = "canClosePayment")]
    public bool CanSetAsPayed {
      get; internal set;
    }

  }  // class PaymentInstructionActions



  /// <summary>Output DTO used to return payment instruction.</summary>
  public class PaymentInstructionDto {

    public string UID {
      get; internal set;
    }

    public string PaymentInstructionNo {
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

    public NamedEntityDto PayTo {
      get; internal set;
    }

    public PaymentMethodDto PaymentMethod {
      get; internal set;
    }

    public PaymentAccountDto PaymentAccount {
      get; internal set;
    }

    public string ReferenceNumber {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

    public NamedEntityDto Currency {
      get; internal set;
    }

    public DateTime DueTime {
      get; internal set;
    }

    public NamedEntityDto RequestedBy {
      get; internal set;
    }

    public DateTime RequestedTime {
      get; internal set;
    }

    public DateTime ProgrammedDate {
      get; internal set;
    }

    public string PayableNo {
      get; internal set;
    }

    public NamedEntityDto PayableType {
      get; internal set;
    }

    public NamedEntityDto Payable {
      get; internal set;
    }

    public DateTime LastUpdateTime {
      get; internal set;
    }

    public string OperationNo {
      get; internal set;
    }

    public NamedEntityDto RecordedBy {
      get; internal set;
    }

    public NamedEntityDto Priority {
      get; internal set;
    }

    public NamedEntityDto Status {
      get; internal set;
    }

  }  // class PaymentInstructionDto



  /// <summary>Output DTO used to return minimal payment instruction's data for use in lists.</summary>
  public class PaymentInstructionDescriptor {

    public string UID {
      get; internal set;
    }

    public string PaymentInstructionNo {
      get; internal set;
    }

    public string PaymentOrderType {
      get; internal set;
    }

    public string PaymentOrderNo {
      get; internal set;
    }

    public string PaymentInstructionTypeName {
      get; internal set;
    }

    public string Description {
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

    public string RequestedBy {
      get; internal set;
    }

    public DateTime RequestedTime {
      get; internal set;
    }

    public DateTime ProgrammedDate {
      get; internal set;
    }

    public DateTime LastUpdateTime {
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

    public string RecordedBy {
      get; internal set;
    }

    public string OperationNo {
      get; internal set;
    }

    public string PriorityName {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

  } // class PaymentInstructionDescriptor

}  // namespace Empiria.Payments.Adapters
