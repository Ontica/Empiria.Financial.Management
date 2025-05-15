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
using Empiria.History.Services.Adapters;

using Empiria.Billing.Adapters;

using Empiria.Payments.Payables.Adapters;
using Empiria.Payments.Processor.Adapters;
using Empiria.Financial.Adapters;

namespace Empiria.Payments.Orders.Adapters {

  /// <summary>Output DTO used to return a complete payment order.</summary>
  public class PaymentOrderHolderDto {

    public PaymentOrderDto PaymentOrder {
      get; internal set;
    }

    public FixedList<PayableItemDto> Items {
      get; internal set;
    }

    public FixedList<BillDto> Bills {
      get; internal set;
    }


    public FixedList<DocumentDto> Documents {
      get; internal set;
    }


    public FixedList<HistoryEntryDto> History {
      get; internal set;
    }


    public FixedList<PaymentInstructionLogDescriptorDto> Log {
      get; internal set;
    }


    public PaymentOrderActionsDto Actions {
      get; internal set;
    }

  }  // class PaymentOrderHolderDto

  public class PaymentOrderActionsDto : BaseActions {

    public bool CanSendToPay {
      get; internal set;
    }


  }

  /// <summary>Output DTO used to return a payment order.</summary>
  public class PaymentOrderDto {

    public string UID {
      get; internal set;
    }

    public NamedEntityDto PaymentOrderType {
      get; internal set;
    }

    public string OrderNo {
      get; internal set;
    }

    public string ControlNo {
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

    public string Notes {
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

    public string RequestedBy {
      get; internal set;
    }

    public DateTime RequestedDate {
      get; internal set;
    }

    public DateTime DueTime {
      get; internal set;
    }

    public string PaymentMethod {
      get; internal set;
    }

    public string Currency {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

  } // class PaymentOrderDescriptor

}  // namespace Empiria.Payments.Orders.Adapters
