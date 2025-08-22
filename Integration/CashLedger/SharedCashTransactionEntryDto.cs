/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                   Component : Integration Adapters Layer           *
*  Assembly : Empiria.Financial.Integration.dll             Pattern   : Output DTO                           *
*  Type     : SharedCashTransactionEntryDto                 License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Output DTO used to retrieve cash ledger transaction entries.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Financial.Integration.CashLedger {

  /// <summary>Output DTO used to retrieve cash ledger transaction entries.</summary>
  public class SharedCashTransactionEntryDto {

    public long Id {
      get; set;
    }

    public string AccountNumber {
      get; set;
    }

    public string AccountName {
      get; set;
    }

    public string ParentAccountFullName {
      get; set;
    }

    public string SectorCode {
      get; set;
    }

    public string SubledgerAccountNumber {
      get; set;
    }

    public string SubledgerAccountName {
      get; set;
    }

    public string VerificationNumber {
      get; set;
    }

    public string ResponsibilityAreaCode {
      get; set;
    }

    public string ResponsibilityAreaName {
      get; set;
    }

    public string BudgetCode {
      get; set;
    }

    public string Description {
      get; set;
    }

    public DateTime Date {
      get; set;
    }

    public int CurrencyId {
      get; set;
    }

    public string CurrencyName {
      get; set;
    }

    public decimal ExchangeRate {
      get; set;
    }

    public decimal Debit {
      get; set;
    }

    public decimal Credit {
      get; set;
    }

    public int CashAccountId {
      get; set;
    }

    public string CuentaSistemaLegado {
      get; set;
    }

  }  // class SharedCashTransactionEntryDto

}  // namespace Empiria.Financial.Integration.CashLedger
