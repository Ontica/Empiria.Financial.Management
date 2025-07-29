/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Output DTO                              *
*  Type     : CashTransactionDto                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output holder DTO used for a cash transaction.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Documents;
using Empiria.History;

namespace Empiria.CashFlow.CashLedger.Adapters {

  /// <summary>Output holder DTO used for a cash transaction.</summary>
  public class CashTransactionHolderDto {

    public CashTransactionDescriptor Transaction {
      get; internal set;
    }

    public FixedList<CashTransactionEntryDto> Entries {
      get; internal set;
    }

    public FixedList<DocumentDto> Documents {
      get; internal set;
    }

    public FixedList<HistoryEntryDto> History {
      get; internal set;
    }

    public CashTransactionActions Actions {
      get; internal set;
    }

  }  // class CashTransactionHolderDto



  /// <summary>Action flags for cash transactions.</summary>
  public class CashTransactionActions {

    public bool CanUpdate {
      get; internal set;
    }

  }  // class CashTransactionActions



  /// <summary>Output DTO used to retrieve cash ledger transaction entries.</summary>
  public class CashTransactionEntryDto {

    public long Id {
      get; internal set;
    }

    public string AccountNumber {
      get; internal set;
    } = string.Empty;


    public string AccountName {
      get; internal set;
    } = string.Empty;


    public string SectorCode {
      get; internal set;
    } = string.Empty;


    public string SubledgerAccountNumber {
      get; internal set;
    } = string.Empty;


    public string SubledgerAccountName {
      get; internal set;
    } = string.Empty;


    public string VerificationNumber {
      get; internal set;
    } = string.Empty;


    public string ResponsibilityAreaName {
      get; internal set;
    } = string.Empty;


    public string CurrencyName {
      get; internal set;
    } = string.Empty;


    public decimal ExchangeRate {
      get; internal set;
    }

    public decimal Debit {
      get; internal set;
    }

    public decimal Credit {
      get; internal set;
    }

    public NamedEntityDto CashAccount {
      get; internal set;
    }

  }  // class CashTransactionEntryDto

}  // namespace Empiria.CashFlow.CashLedger.Adapters
