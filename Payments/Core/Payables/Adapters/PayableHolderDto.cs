/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Data Transfer Object                    *
*  Type     : PayableDataDto                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer objects used to return payable data structure.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents.Services.Adapters;
using Empiria.History.Services.Adapters;

using Empiria.Billing.Adapters;
using Empiria.Financial.Adapters;

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Data transfer objects used to return payable data structure.</summary>
  public class PayableHolderDto {

    public PayableDto Payable {
      get; internal set;
    }

    public PayableEntityDto PayableEntity {
      get; internal set;
    }

    public FixedList<PayableDataItemDto> Items {
      get; internal set;
    }

    public FixedList<BillDto> Bills {
      get; internal set;
    }

    public FixedList<DocumentDto> Documents {
      get; internal set;
    }

    public FixedList<HistoryDto> History {
      get; internal set;
    }

    public PayableActions Actions {
      get; internal set;
    }

  } // class PayableDataDto


  public class PayableActions : BaseActions {


    public bool CanGeneratePaymentOrder {
      get; internal set;
    }

  }



  // <summary>Output DTO used to return payable data item minimal information.</summary>
  public class PayableDataItemDto {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public NamedEntityDto BudgetAccount {
      get; internal set;
    }

    public string BillConcept {
      get; internal set;
    }

    public string PayableEntityItemUID {
      get; internal set;
    }

    public decimal Quantity {
      get; internal set;
    }

    public string Unit {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

  } // class PayableEntityItemDto


  /// <summary>Output DTO used to return payable type general information.</summary>
  public class PayableEntityDto {

    public string UID {
      get; internal set;
    }

    public NamedEntityDto Type {
      get; internal set;
    }

    public string EntityNo {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public NamedEntityDto Budget {
      get; internal set;
    }

    public NamedEntityDto Currency {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

    public NamedEntityDto PayTo {
      get; internal set;
    }

    public FixedList<PaymentAccountDto> PaymentAccounts {
      get; internal set;
    }

    public FixedList<PayableEntityItemDto> Items {
      get; internal set;
    }

  } // class PayableEntityDto


  // <summary>Output DTO used to return payable entity item minimal information.</summary>
  public class PayableEntityItemDto {

    public string UID {
      get; internal set;
    }

    public decimal Quantity {
      get; internal set;
    }

    public NamedEntityDto Unit {
      get; internal set;
    }

    public NamedEntityDto Product {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public decimal UnitPrice {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

    public NamedEntityDto BudgetAccount {
      get; internal set;
    }

  } // class PayableEntityItemDto

}  // namespace Empiria.Payments.Payables.Adapters
