/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Output DTO                              *
*  Type     : CashTransactionDto                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output holder DTO used for a cash transaction.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.History;

using Empiria.Financial.Integration.CashLedger;

namespace Empiria.CashFlow.CashLedger.Adapters {

  /// <summary>Output holder DTO used for a cash transaction.</summary>
  public class CashTransactionHolderDto {

    public CashTransactionDescriptor Transaction {
      get; set;
    }

    public FixedList<CashTransactionEntryDto> Entries {
      get; set;
    }

    public FixedList<DocumentDto> Documents {
      get; set;
    } = new FixedList<DocumentDto>();


    public FixedList<HistoryEntryDto> History {
      get; set;
    } = new FixedList<HistoryEntryDto>();


    public CashTransactionActions Actions {
      get; set;
    }

  }  // class CashTransactionHolderDto



  /// <summary>Action flags for cash transactions.</summary>
  public class CashTransactionActions {

    public bool CanAnalize {
      get; set;
    }

    public bool CanReview {
      get; set;
    }

    public bool CanUpdate {
      get; set;
    }

  }  // class CashTransactionActions



  /// <summary>Output DTO used to retrieve cash ledger transactions for use in lists.</summary>
  public class CashTransactionDescriptor : SharedCashTransactionDescriptor {

  }  // class CashTransactionDescriptor



  /// <summary>Output DTO used to retrieve cash ledger transaction entries.</summary>
  public class CashTransactionEntryDto : SharedCashTransactionEntryDto {

    public NamedEntityDto CashAccount {
      get; set;
    }

    internal bool Processed {
      get; set;
    }

  }  // class CashTransactionEntryDto

}  // namespace Empiria.CashFlow.CashLedger.Adapters
